using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : CharacterController
{
    public enum InitialState
    {
        Walking,
        Idle
    }

    public float activeRange = 100.0f;
    public float attackRange = 10.0f;

    public float offset = 0.5f;
    public float rotationSpeed = 10f;

    public Transform target;
    public string walkingType;

    Vector3 offsetVec3;
    Transform player;
    Vector3 random_target;

    public InitialState initialState = InitialState.Idle;

    new void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        offsetVec3 = new Vector3(0, 0, offset);

        bool walking = (Random.Range(0, 100) < 80);
        walkingType = walking ? "walk" : "crawl";

        StartCoroutine("InactiveCoroutine", initialState);
    }

    IEnumerator InactiveCoroutine(InitialState state)
    {
        switch (state)
        {
            case InitialState.Idle:
                foreach (var param in animator.parameters)
                    animator.SetBool(param.name, false);
                break;
            case InitialState.Walking:
                StartCoroutine("RandomWalk");
                break;
            default:
                break;
        }
        yield return null;
    }

    IEnumerator RandomWalk()
    {
        StopCoroutine("InactiveCoroutine");
        animator.SetBool("random_walk", true);

        List<Vector3> positions = new List<Vector3>();
        positions.Add(transform.position);
        positions.Add(transform.position + new Vector3(10, 0, 0));
        positions.Add(transform.position + new Vector3(10, 0, 10));

        int index = 1;
        int size = positions.Count;

        while (true)
        {
            random_target = positions[index];
            yield return new WaitUntil(() => Distance(random_target, 3.3f));
            index = (index + 1) % size;
        }
    }

    void Update()
    {
        agent.acceleration = 0;
        agent.speed = 0;

        if (animator.GetBool("random_walk"))
            MoveEnemy(random_target);


        if (Distance(player.position, attackRange))
        {
            animator.SetBool("attack", true);
            animator.SetBool("walk", false);
        }
        else
        {
            animator.SetBool("attack", false);

            if (Distance(player.position, activeRange))
            {
                StopCoroutine("RandomWalk");

                animator.SetBool("aware", true);
                MoveEnemy(player.position);
            }
            else
                animator.SetBool("aware", false);
        }
    }

    public Vector3 GetPosition()
    {
        return target.position;
    }

    bool Distance(Vector3 target, float size)
    {
        float distance = Vector3.SqrMagnitude(transform.position - target);
        return distance < size;
    }

    private void MoveEnemy(Vector3 targetPosition)
    {
        if (animator.GetBool("active"))
        {
            animator.SetBool(walkingType, true);

            Vector3 destination = offsetVec3 + targetPosition;
            agent.SetDestination(destination);

            Quaternion newRot = Quaternion.LookRotation((destination
                                            - transform.position).normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                        newRot, Time.deltaTime * rotationSpeed);
        }
    }
}
