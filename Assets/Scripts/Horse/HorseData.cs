using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHorse", menuName = "Game/Horse Data")]
public class HorseData : ScriptableObject
{
    public string horseName;
    public Sprite horseSprite;
    public Boolean matchable;
    [TextArea] public string bio;

    [Header("Question Answers (True = Yes, False = No)")]
    public bool[] answers;
}