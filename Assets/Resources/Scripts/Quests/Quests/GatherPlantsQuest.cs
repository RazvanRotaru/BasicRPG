using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherPlantsQuest : Quest
{
    public int gatherAmount = 20;
    public int rewardAmmount = 50;

    void Start()
    {
        completed = false;
        this.name = "Gather Plants";

        goals.Add(new GatherGoal(this, "Plant", gatherAmount));
        foreach (Goal goal in goals)
            goal.Init();

        rewards.Add(new GoldReward(rewardAmmount));
    }
}
