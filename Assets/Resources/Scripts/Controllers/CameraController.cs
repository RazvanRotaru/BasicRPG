using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target;
    public Vector3 offset;
    public float height = 2f;
    public float zoom = 5f;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (GameManager.MenuActive)
            return;

        zoom -= Input.mouseScrollDelta.y * 0.1f;
        zoom = Mathf.Max(zoom, 2f);
    }

    void LateUpdate()
    {
        transform.position = target.position + offset * zoom;
        transform.LookAt(target.position + Vector3.up * height);
    }
}
