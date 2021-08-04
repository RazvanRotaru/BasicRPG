using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public BoxCollider weapon;
    public float damage;

    public void SetWeapon(GameObject newWeapon)
    {
        if (newWeapon == null)
            return;
        
        weapon = newWeapon.GetComponent<BoxCollider>();
        damage = newWeapon.GetComponent<DamageController>().GetDamage();
    }

    public void ActivateWeapon(bool mode)
    {
        weapon.enabled = mode;
    }

    public float GetDamage()
    {
        return damage;
    }
}
