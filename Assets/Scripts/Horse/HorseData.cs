using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHorse", menuName = "Game/Horse Data")]
public class HorseData : ScriptableObject
{
    [Header("Profile Info")]
    public string horseName;
    public Sprite horseSprite; // Main portrait photo
    public Sprite[] horsePictures; // Extra photos for details (e.g. Photo 2 & Photo 3)
    [TextArea(3, 5)] public string bio;

    [Header("Funny Prompts / Q&A")]
    [TextArea(2, 3)] public string funnyAnswer1;
    [TextArea(2, 3)] public string funnyAnswer2;

    [Header("Matching & Dialogue Settings")]
    public bool matchable = true;
    public DialogueGraph textDialogue;
    public DialogueGraph dateDialogue;

    [Header("Question Answers (True = Yes, False = No)")]
    public bool[] answers;
}