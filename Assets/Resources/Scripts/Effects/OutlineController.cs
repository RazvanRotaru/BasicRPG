using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    Outline outline;

    void Start()
    {
        outline = gameObject.GetComponent<Outline>();
        outline.enabled = false;
    }

    public void Highlight(bool type)
    {
        outline.enabled = type;
    }

    private void OnMouseOver()
    {
        Highlight(true);
    }

    private void OnMouseExit()
    {
        Highlight(false);
    }
}
