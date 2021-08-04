using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Image image;
    Text value;

    Sprite defaultSprite;
    Color idleColor;

    Item item;

    void Awake()
    {
        image = GetComponent<Image>();
        value = transform.Find("Value").GetComponent<Text>();

        defaultSprite = image.sprite;
        idleColor = image.color;
        
        item = null;
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        value.text = newItem.value.ToString();
        image.sprite = newItem.icon;
        idleColor = image.color;

        foreach (Transform button in transform)
            button.gameObject.SetActive(true);
        value.gameObject.SetActive(false);
    }

    public void RemoveItem()
    {
        item = null;
        image.sprite = defaultSprite;
        image.color = idleColor;

        foreach (Transform button in transform)
            button.gameObject.SetActive(false);
    }

    public void Use()
    {
        if (item == null)
            return;

        item.Activate();
    }

    public void Drop()
    {
        if (item == null)
            return;

        item.Drop();
        InventoryManager.instance.Remove(item);
        //RemoveItem();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;

        value.gameObject.SetActive(true);
        Color tempColor = idleColor;
        tempColor.a = 100;
        image.color = tempColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
            return;

        value.gameObject.SetActive(false);
        image.color = idleColor;
    }
}
