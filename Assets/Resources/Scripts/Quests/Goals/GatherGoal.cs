using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherGoal : Goal
{
    string itemName;
    Quest quest;

    public GatherGoal(Quest quest, string itemName, int ammount)
    {
        this.quest = quest;

        this.itemName = itemName;
        this.text = "Gather " + ammount.ToString() + " " + itemName + "s";

        this.ammount = ammount;
        this.currAmount = 0;

        this.completed = false;
    }

    public override void Finish()
    {
        InventoryManager.instance.RemoveItems(itemName, ammount);
    }

    public override void Init()
    {
        ItemController.onPickUp += ItemGathered;
    }

    void ItemGathered(string name)
    {
        if (name.Contains(itemName))
        {
            if (++currAmount >= ammount)
            {
                completed = true;
                quest.CheckGoals();
            }

            QuestManager.instance.UpdateStatus();
        }
    }

    private void OnDisable()
    {
        ItemController.onPickUp -= ItemGathered;
    }
}
