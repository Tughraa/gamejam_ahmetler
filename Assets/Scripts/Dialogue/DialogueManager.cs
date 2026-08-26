using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public DialogueGraph dialogueGraph;
    public Button optButton1;
    public Button optButton2;
    public Transform contentRect;
    public GameObject optBoxFab;
    public float boxOffset = 20f;
    public float yOrigin = 0f;
    public float rBoxPos = 200f;
    public float lBoxPos = -200f;
    public float responseTime = 1.5f;

    private DialogueNode currentNode;

    void Start()
    {
        // Find the entry node (the one with nothing connected to its input)
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
        NewBox(node.flirtLine, true);

        XNode.NodePort port1 = node.GetOutputPort("opt1");
        XNode.NodePort port2 = node.GetOutputPort("opt2");

        bool hasOpt1 = port1 != null && port1.IsConnected;
        bool hasOpt2 = port2 != null && port2.IsConnected;

        optButton1.interactable = hasOpt1;
        optButton2.interactable = hasOpt2;

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
    }

    private void SelectOption(DialogueNode next)
    {
        StartCoroutine(OptionRoutine(next));
    }

    public void NewBox(string inText, bool right)
    {
        GameObject newBox = Instantiate(optBoxFab, contentRect);
        newBox.transform.GetChild(0).GetComponent<TMP_Text>().text = inText;

        float xOffs = right ? rBoxPos : lBoxPos;
        newBox.transform.localPosition = new Vector3(xOffs+700f, 0f, 0f);

        int ite = contentRect.childCount;
        foreach (Transform boxChild in contentRect)
        {
            float offPos = yOrigin + boxOffset * ite;
            boxChild.localPosition = new Vector3(boxChild.localPosition.x, offPos, 0f);
            ite--;
        }
    }
    IEnumerator OptionRoutine(DialogueNode next)
    {
        NewBox(next.ourLine, false);
        yield return new WaitForSeconds(responseTime);
        LoadNode(next);
    }
    /*
    IEnumerator RoutineOpt1()
    {
        DialogueOpt selected = opt1;
        NewBox(selected.ourLine, false);

        yield return new WaitForSeconds(ourLineTime);

        NewBox(selected.flirtLine, true);
        opt1 = selected.opt1;
        opt2 = selected.opt2;
    
        UpdateButtons();
    }
    IEnumerator RoutineOpt2()
    {
        DialogueOpt selected = opt1;
        NewBox(selected.ourLine, false);

        yield return new WaitForSeconds(ourLineTime);

        NewBox(selected.flirtLine, true);
        opt1 = selected.opt1;
        opt2 = selected.opt2;
    
        UpdateButtons();
    }*/
}
