using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(300)]
public class DialogueNode : Node
{
    [Input]  public DialogueConnection entry;
    [Output] public DialogueConnection opt1;
    [Output] public DialogueConnection opt2;
    
    public string flirtLine;
    public string ourLine;
    public bool goodEnd;
}