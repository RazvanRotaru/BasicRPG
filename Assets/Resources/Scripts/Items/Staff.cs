using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : Item
{
    GameObject staff;
    
    private void OnEnable()
    {
        staff = Resources.Load<GameObject>("Models/Staff/Model");
        staff.name = "Staff";
    }

    public override void Activate()
    {
        staff.GetComponent<DamageController>().SetDamage(value);
        player.SetWeapon(staff);
    }

    public override void Drop()
    {
        player.SetWeapon(null);

        Quaternion staffRotation = Quaternion.Euler(0, 90, 0);
        ItemController.Type staffType = ItemController.Type.Staff;
        CreateItem(staff, staffRotation, staffType);
    }
}
