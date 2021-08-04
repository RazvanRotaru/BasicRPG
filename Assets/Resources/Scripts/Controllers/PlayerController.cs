using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : CharacterController
{
    Transform weaponHandle;
    Transform magicHandle;

    WeaponHandler weaponController;
    
    GameObject projectilePrefab;
    GameObject defaultWeapon;

    EnemyController target;
    float stoppingDistance = 5f;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        weaponController = GetComponent<WeaponHandler>();

        weaponHandle = GameObject.FindGameObjectWithTag("WeaponHandle").transform;
        magicHandle = GameObject.FindGameObjectWithTag("MagicHandle").transform;

        projectilePrefab = Resources.Load<GameObject>("Models/Projectile/Model");
        defaultWeapon = Resources.Load<GameObject>("Models/Fists/Model");

        SetWeapon(defaultWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        SetTarget();
        DebugActions();
        Walk();
        Attack();
    }

    private void DebugActions()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            animator.SetBool("melee", !animator.GetBool("melee"));
        if (Input.GetKeyDown(KeyCode.Keypad2))
            animator.SetBool("ranged", !animator.GetBool("ranged"));
        if (Input.GetKeyDown(KeyCode.Keypad3))
            animator.SetBool("hit", !animator.GetBool("hit"));
    }

    private void Walk()
    {
        animator.SetBool("walk", agent.remainingDistance > 0.33f);
        //transform.position = agent.nextPosition;
    }


    private void SetTarget()
    {
        bool rightClick = Input.GetMouseButtonDown(1);
        bool leftClick = Input.GetMouseButtonDown(0);
        
        if (rightClick || leftClick)
        {
            Ray mouseClickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(mouseClickRay, out hit/*, LayerMask.GetMask("Player")*/))
            {
                if (leftClick)
                {
                    string layer = LayerMask.LayerToName(hit.collider.gameObject.layer);
                    if (layer == "Enemy" || layer == "Item" || layer == "NPC")
                    {
                        ResetPath(hit);
                        SelectTarget(hit);
                    }
                }
                else
                    ResetPath(hit);
            }
        }
    }

    private void SelectTarget(RaycastHit hit)
    {
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Item"))
            StartCoroutine("PickingUpCoroutine", hit.collider.gameObject);

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("NPC"))
            StartCoroutine("InteractCoroutine", hit.collider.gameObject);
        
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            target = hit.collider.gameObject.GetComponent<EnemyController>();
    }

    private void ResetPath(RaycastHit hit)
    {
        StopCoroutine("PickingUpCoroutine");
        StopCoroutine("InteractCoroutine");
        target = null;
   
        agent.SetDestination(hit.point);
    }

    IEnumerator PickingUpCoroutine(GameObject obj)
    {
        Vector3 targetPos = obj.transform.position;
        float targetSize = obj.GetComponent<BoxCollider>().size.x
                            * obj.transform.localScale.x;
        yield return new WaitUntil(() => Distance(targetPos, targetSize));

        obj.GetComponent<ItemController>().PickUp();
        yield return null;
    }
    
    IEnumerator InteractCoroutine(GameObject obj)
    {
        Vector3 targetPos = obj.transform.position;
        float targetSize = obj.GetComponent<BoxCollider>().size.x
                            * obj.transform.localScale.x;
        yield return new WaitUntil(() => Distance(targetPos, targetSize * 5));

        agent.ResetPath();
        obj.GetComponent<NPCController>().Interact();
        yield return null;
    }

    private float SetStoppingDistance(string type)
    {
        if (type.Contains("Staff"))
            return 50.0f;
        
        return 5.0f;
    }

    bool Distance(Vector3 target, float size)
    {
        float distance = Vector3.SqrMagnitude(transform.position - target);
        return distance < size;
    }










    public void SetWeapon(GameObject newWeapon)
    {
        if (weaponHandle.childCount != 0)
            foreach (Transform child in weaponHandle)
                Destroy(child.gameObject);

        if (newWeapon == null)
            newWeapon = defaultWeapon;

        GameObject weaponInstance = 
                        Instantiate(newWeapon, weaponHandle);
        weaponInstance.layer = LayerMask.NameToLayer("Weapon");
        weaponController.SetWeapon(weaponInstance);
        stoppingDistance = SetStoppingDistance(weaponInstance.name);
    }

    public void Attack()
    {
        if (target == null || target.IsDead())
        {
            StopAttacking();
            return;
        }

        RotateAfter();
        MoveTowards();
        HitOnce();
    }

    private void HitOnce()
    {
        if (!Distance(target.GetPosition(), stoppingDistance))
        {
            StopAttacking();
            return;
        }

        if (weaponHandle.childCount > 0 && 
                weaponHandle.GetChild(0).name.Contains("Staff"))
            animator.SetBool("ranged", true);
        else
            animator.SetBool("melee", true);
    }

    private void StopAttacking()
    {
        animator.SetBool("melee", false);
        animator.SetBool("ranged", false);
    }

    private void MoveTowards()
    {
        if (!Distance(target.GetPosition(), stoppingDistance))
            agent.SetDestination(target.GetPosition());
        else
            agent.ResetPath();
    }

    public void RotateAfter()
    {
        Quaternion newRot = Quaternion.LookRotation((target.GetPosition()
                                            - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation,
                                                newRot, Time.deltaTime * 10.0f);
    }

    public void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.GetComponent<DamageController>()
                        .SetDamage(weaponController.GetDamage() / 10);
        projectile.GetComponent<ProjectileController>()
                            .Launch(magicHandle, target.GetPosition());
    }

    public void GainHealth(float amount)
    {
        health += amount;
        healthSlider.value = health;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.collider.gameObject.name);
    }

}
