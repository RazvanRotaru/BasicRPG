using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillDrakeQuest : Quest
{
    public int killAmmount = 20;
    public int rewardAmmount = 100;

    void Start()
    {
        completed = false;
        this.name = "Kill Drakes";

        goals.Add(new KillGoal(this, "Drake", killAmmount));
        foreach (Goal goal in goals)
            goal.Init();

        rewards.Add(new GoldReward(rewardAmmount));
    }
}
