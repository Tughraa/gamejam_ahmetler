using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueOpt : ScriptableObject
{
    public string answer;
    public string question;
    public DialogueOpt opt1;
    public DialogueOpt opt2;
    public bool endInteraction;
}
