﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    void Awake()
    {
        instance = this;
    }

    string NPCName;
    List<string> sentences;

    Transform canvas;
    Text dialogueText;
    Text nameText;

    Text yesButton;
    Text noButton;

    bool questDialogue;
    int index;

    public delegate void dialogueDoneEvent(string name);
    public static event dialogueDoneEvent onDialogueDone;
    public static event dialogueDoneEvent onQuestAccepted;

    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("DialogueCanvas").transform;
        canvas.gameObject.SetActive(false);

        dialogueText = canvas.Find("Text").GetComponent<Text>();
        nameText = canvas.Find("Name").GetComponent<Text>();

        yesButton = canvas.Find("Button").Find("Text").GetComponent<Text>();
        noButton = canvas.Find("DenyButton").Find("Text").GetComponent<Text>();
    }

    public void AddDialogue(string[] lines, string NPCname, bool quest)
    {
        index = 0;
        questDialogue = quest;

        sentences = new List<string>(lines);
        NPCName = NPCname;

        ShowDialogue();
    }

    public void ShowDialogue()
    {
        if (index == sentences.Count)
        {
            yesButton.text = "Continue";
            noButton.text = "Close";
            index = 0;

            if (questDialogue)
                onQuestAccepted.Invoke(NPCName);
            onDialogueDone.Invoke(NPCName);

            canvas.gameObject.SetActive(false);
            return;
        }

        dialogueText.text = sentences[index++];
        nameText.text = NPCName;

        if (questDialogue && index == sentences.Count)
        {
            yesButton.text = "Accept";
            noButton.text = "Deny";
        }

        canvas.gameObject.SetActive(true);
    }

    public void CancelDialogue()
    {
        index = 0;

        yesButton.text = "Continue";
        noButton.text = "Close";

        canvas.gameObject.SetActive(false);
    }

}
