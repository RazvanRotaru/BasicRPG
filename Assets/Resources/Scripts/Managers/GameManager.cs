using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject inventoryCanvas;
    public GameObject questsCanvas;

    static bool menuActive;

    public static bool MenuActive { get => menuActive; }

    void Start()
    {
        GameObject.FindGameObjectWithTag("InventoryGrid")
                        .GetComponent<InventoryGrid>().Init();
        inventoryCanvas.SetActive(false);

        GameObject.FindGameObjectWithTag("QuestsGrid")
                        .GetComponent<QuestsGrid>().Init();
        questsCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            inventoryCanvas.SetActive(!inventoryCanvas.activeSelf);
        
        if (Input.GetKeyDown(KeyCode.Q))
            questsCanvas.SetActive(!questsCanvas.activeSelf);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            questsCanvas.SetActive(false);
            inventoryCanvas.SetActive(false);
        }

        menuActive = (questsCanvas.activeSelf || inventoryCanvas.activeSelf);
    }
}
