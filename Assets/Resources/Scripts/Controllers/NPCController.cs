﻿using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    Outline outline;
    List<Quest> quests;

    GameObject exclamationMark;

    Quest activeQuest;
    bool questCompleted;
    bool questDialogue;

    public string name;
    public string[] sentences;

    void Start()
    {
        outline = gameObject.GetComponent<Outline>();
        outline.enabled = false;

        quests = new List<Quest>(gameObject.GetComponents<Quest>());

        exclamationMark = transform.Find("Canvas").gameObject;
        if (quests.Count == 0)
            exclamationMark.SetActive(false);

        activeQuest = null;
        questCompleted = false;
        questDialogue = false;

        DialogueManager.onDialogueDone += DialogueDone;
        DialogueManager.onQuestAccepted += AcceptQuest;
    }

    public void Interact()
    {
        if (questCompleted)
        {
            GiveReward();
            return;
        }

        if (quests.Count <= 0)
            return;

        DialogueManager.instance.AddDialogue(sentences, name, questDialogue);
    }

    public void AcceptQuest(string name)
    {
        if (quests.Count <= 0 && name != gameObject.name)
            return;

        Quest quest = quests[0];

        if (!QuestManager.instance.Add(quest))
            return;

        activeQuest = quest;
        questCompleted = false;
        exclamationMark.SetActive(false);

        quests.RemoveAt(0);
    }

    public void QuestDone()
    {
        questCompleted = true;
        // render ! over npc
        exclamationMark.SetActive(true);
    }

    public void DialogueDone(string name)
    {
        if (quests.Count > 0 && name == gameObject.name)
        {
            sentences = quests[0].sentences;
            questDialogue = true;
        }
    }

    public void GiveReward()
    {
        activeQuest.GiveRewards();

        if (quests.Count == 0)
        {
            exclamationMark.SetActive(false);
            sentences = null;
        }

        //QuestManager.instance.Remove(activeQuest);
        questCompleted = false;
        activeQuest = null;
    }
}
