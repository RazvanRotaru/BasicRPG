using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Item
{
    private void OnEnable()
    {
        
    }

    public override void Activate()
    {
        player.GainHealth(value);
        InventoryManager.instance.Remove(this);
    }

    public override void Drop()
    {
        InventoryManager.instance.Remove(this);
    }
}
