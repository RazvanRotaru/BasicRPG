using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "UI/Item", order = 1)]
public class Item : ScriptableObject
{
    new public string name = "New MyScriptableObject";
    public string objectName = "New MyScriptableObject";

    public int value;

    public Sprite icon;
    public Vector3[] spawnPoints;

    public PlayerController player;
    Transform ground;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")
                                .GetComponent<PlayerController>();
        ground = GameObject.FindGameObjectWithTag("Ground").transform;
    }

    public virtual void Activate() { }
    public virtual void Drop() { }

    public virtual void SetParameters(string objectName)
    {
        this.objectName = objectName;
        name = objectName;

        if (objectName != "None")
            icon = Resources.Load<Sprite>("Sprites/" + objectName);

        value = Random.Range(20, 50);
    }

    public GameObject CreateItem(GameObject prefab, Quaternion rotation, ItemController.Type type)
    {
        GameObject newItem = GameObject.Instantiate(prefab, ground);

        Vector3 swordPosition = ground.InverseTransformPoint(player.transform.position)
                                    * ground.localScale.x;
        swordPosition.y = 0.05f;

        newItem.transform.position = swordPosition;
        newItem.transform.rotation = rotation;
        newItem.transform.localScale = prefab.transform.localScale / ground.localScale.x;

        newItem.AddComponent<ItemController>().type = type;
        newItem.AddComponent<Outline>().color = 1;
        newItem.AddComponent<BoxCollider>();
        newItem.layer = LayerMask.NameToLayer("Item");

        return newItem;
    }
}
