using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGrid : MonoBehaviour
{
    InventoryManager inventory;
    List<InventorySlot> slots;


    public GameObject prefab;

    public void Init()
    {
        inventory = InventoryManager.instance;
        inventory.inventoryChangedCallback += UpdateInventory;

        for (int i = 0; i < inventory.capacity; ++i)
            GameObject.Instantiate(prefab, transform);

        slots = new List<InventorySlot>(GetComponentsInChildren<InventorySlot>());
    }    

    void UpdateInventory()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < inventory.items.Count) 
                slots[i].AddItem(inventory.items[i]);
            else 
                slots[i].RemoveItem();
        }
    }
}
