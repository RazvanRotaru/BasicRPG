using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private void Awake()
    {
        instance = this;
    }

    public delegate void OnInventoryChanged();
    public event OnInventoryChanged inventoryChangedCallback;

    public List<Item> items = new List<Item>();
    public int capacity = 30;

    Gold gold = null;

    public void Add(Item item)
    {
        if (items.Count > capacity)
            return;

        items.Add(item);
        inventoryChangedCallback.Invoke();
    }

    public void Add(Gold item)
    {
        if (items.Count > capacity && gold == null)
            return;

        if (gold == null)
        {
            gold = item;
            items.Add(gold);
        }
        else
        {
            int goldRef = items.FindIndex(x => x.name == "Gold");
            gold.UpdateValue(item.value);
            items.Insert(goldRef, gold);
            items.RemoveAt(goldRef + 1);
        }

        inventoryChangedCallback.Invoke();
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        inventoryChangedCallback.Invoke();
    }

    public void RemoveItems(string itemType, int amount)
    {
        while (amount-- > 0)
        {
            int index = items.FindIndex(x => x.name.Contains(itemType));
            items.RemoveAt(index);
        }

        inventoryChangedCallback.Invoke();
    }

    public void Render(bool type)
    {
        if (type == false)
            return;

        inventoryChangedCallback.Invoke();
    }
}
