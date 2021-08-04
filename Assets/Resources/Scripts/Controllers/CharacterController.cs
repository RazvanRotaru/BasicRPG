using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    public float health = 100.0f;
   
    public Animator animator;
    public NavMeshAgent agent;
    public Slider healthSlider;

    public delegate void onCharacterDeath(string name);
    public static event onCharacterDeath onDeath;

    public void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        healthSlider = transform.Find("Canvas").Find("HealthSlider")
                                    .gameObject.GetComponent<Slider>();
        healthSlider.value = healthSlider.maxValue;
    }

    public void TakeDamage(float damage)
    {
        animator.SetFloat("damage", damage);

        health -= damage;
        healthSlider.value = health;
        if (health <= 0 && !animator.GetBool("dead"))
            StartCoroutine(DestroyCoroutine());
    }

    public IEnumerator DestroyCoroutine()
    {
        Dies();
        yield return new WaitForSeconds(10.0f);
        Destroy(gameObject);

        // [TODO] fade effect
    }

    public void Dies()
    {
        animator.SetTrigger("die");
        onDeath.Invoke(gameObject.name);
    }

    public bool IsDead()
    {
        return animator.GetBool("dead");
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == null)
            return;

        if (gameObject.CompareTag("Player") && other.gameObject.CompareTag("EnemyWeapon")
                                                    || other.gameObject.CompareTag("Weapon"))
        {
            float damage = other.gameObject
                            .GetComponent<DamageController>().GetDamage();
            TakeDamage(damage);
        }
    }
}
