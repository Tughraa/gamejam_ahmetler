using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DateManager : MonoBehaviour
{
    public HorseData horse;
    
    public Button optButton1;
    public Button optButton2;
    public DialogueGraph dialogueGraph;
    private DialogueNode currentNode;
    public TMP_Text talkText;

    
    private Coroutine typingCoroutine;
    public float typingSpeed = 0.05f; // seconds per character

    void Start()
    {
        foreach (XNode.Node node in dialogueGraph.nodes)
        {
            DialogueNode dNode = node as DialogueNode;
            if (dNode != null && !dNode.GetInputPort("entry").IsConnected)
            {
                currentNode = dNode;
                break;
            }
        }

        LoadNode(currentNode);
    }

    private void LoadNode(DialogueNode node)
    {
        currentNode = node;

        // Stop any ongoing typing before starting new one
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(node.flirtLine));

        // Hide buttons while typing
        optButton1.interactable = false;
        optButton2.interactable = false;

        XNode.NodePort port1 = node.GetOutputPort("opt1");
        XNode.NodePort port2 = node.GetOutputPort("opt2");

        bool hasOpt1 = port1 != null && port1.IsConnected;
        bool hasOpt2 = port2 != null && port2.IsConnected;

        if (hasOpt1)
        {
            DialogueNode next1 = port1.Connection.node as DialogueNode;
            optButton1.transform.GetChild(0).GetComponent<TMP_Text>().text = next1.ourLine;
            optButton1.onClick.RemoveAllListeners();
            optButton1.onClick.AddListener(() => SelectOption(next1));
        }

        if (hasOpt2)
        {
            DialogueNode next2 = port2.Connection.node as DialogueNode;
            optButton2.transform.GetChild(0).GetComponent<TMP_Text>().text = next2.ourLine;
            optButton2.onClick.RemoveAllListeners();
            optButton2.onClick.AddListener(() => SelectOption(next2));
        }

        // Pass these into the coroutine so buttons unlock after typing finishes
        StartCoroutine(UnlockButtonsAfterTyping(hasOpt1, hasOpt2, node));
    }

    private IEnumerator TypeText(string text)
    {
        talkText.text = "";
        foreach (char c in text)
        {
            talkText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        typingCoroutine = null;
    }

    private IEnumerator UnlockButtonsAfterTyping(bool hasOpt1, bool hasOpt2, DialogueNode node)
    {
        // Wait until typing is done
        yield return new WaitUntil(() => typingCoroutine == null);

        optButton1.interactable = hasOpt1;
        optButton2.interactable = hasOpt2;

        if (!hasOpt1 && !hasOpt2)
            EndDialogue(node.goodEnd);
    }
    public void EndDialogue(bool good)
    {
        optButton1.gameObject.SetActive(false);
        optButton2.gameObject.SetActive(false);
        if (good)
        {

        }
        else
        {
            
        }
    }

    private void SelectOption(DialogueNode next)
    {
        //StartCoroutine(OptionRoutine(next));
        LoadNode(next);
        //Debug.Log("selected");
    }
}
