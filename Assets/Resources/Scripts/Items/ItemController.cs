using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public delegate void onItemPickUp(string name);
    public static event onItemPickUp onPickUp;
    public enum Type
    {
        None,
        Sword,
        Staff,
        Plant
    };

    Outline outline;
    public Type type;

    void Start()
    {
        outline = gameObject.GetComponent<Outline>();
        outline.enabled = false;
    }

    public void PickUp()
    {
        Item item = CreateItem();
        item.SetParameters(type.ToString());

        InventoryManager.instance.Add(item);
        onPickUp.Invoke(type.ToString());
        
        Destroy(gameObject);
    }

    Item CreateItem()
    {
        switch (type)
        {
            case Type.Sword:
                return ScriptableObject.CreateInstance<Sword>();
            case Type.Staff:
                return ScriptableObject.CreateInstance<Staff>();
            case Type.Plant:
                return ScriptableObject.CreateInstance<Plant>();
            default:
                return ScriptableObject.CreateInstance<Item>();
        }
    }
}
