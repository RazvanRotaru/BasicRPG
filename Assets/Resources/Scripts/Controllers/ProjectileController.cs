using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    float speed = 5.0f;
    Vector3 direction;

    bool launched = false;

    private void Update()
    {
        if (!launched)
            return;

        transform.position += direction.normalized * speed * Time.deltaTime;
    }

    public void Launch(Transform origin, Vector3 dest)
    {
        transform.position = origin.position;
        direction = (dest - transform.position).normalized;

        StartCoroutine(DestroyCoroutine());

        launched = true;
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
