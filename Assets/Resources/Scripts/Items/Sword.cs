using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item
{
    GameObject sword;
    
    private void OnEnable()
    {
        sword = Resources.Load<GameObject>("Models/Sword/Model");
        sword.name = "Sword";
    }

    public override void Activate()
    {
        //sword.GetComponent<DamageController>().SetDamage(value);
        player.SetWeapon(sword);
    }

    public override void Drop()
    {
        player.SetWeapon(null);

        Quaternion swordRotation = Quaternion.Euler(90, 90, 0);
        ItemController.Type swordType = ItemController.Type.Sword;
        CreateItem(sword, swordRotation, swordType);
    }
}
