using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Item
{
    GameObject gold;

    private void OnEnable()
    {
        //gold = Resources.Load<GameObject>("Models/Gold/Model");
        name = "Gold";

        icon = Resources.Load<Sprite>("Sprites/Coins");
    }
    
    public void UpdateValue(int delta)
    {
        value += delta;
    }
}
