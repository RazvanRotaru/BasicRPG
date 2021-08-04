using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward : ScriptableObject
{
    public int ammount;
    public PlayerController player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")
                            .GetComponent<PlayerController>();
    }

    public abstract void GiveReward();
}
