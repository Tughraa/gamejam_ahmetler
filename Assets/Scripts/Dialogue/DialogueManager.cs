using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public HorseData horse;
    public Button myButton;
    public MessagesManager messagesManager;
    public DialogueGraph dialogueGraph;
    public Button optButton1;
    public Button optButton2;
    public Button askOutButton;
    public Transform contentRect;
    public GameObject optBoxFab;
    public float boxOffset = 20f;
    public float yOrigin = 0f;
    public float rBoxPos = 200f;
    public float lBoxPos = -200f;
    public float responseTime = 1.5f;

    private DialogueNode currentNode;

    public AudioClip clickSound;
    public AudioClip popSound;

    void Start()
    {
        messagesManager = this.transform.parent.GetComponent<MessagesManager>();
        UpdateHorseData(horse);

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
    public void UpdateHorseData(HorseData data)
    {
        horse = data;
        dialogueGraph = data.textDialogue;
        myButton.transform.GetChild(0).GetComponent<TMP_Text>().text = data.horseName;
        myButton.transform.GetChild(1).GetComponent<Image>().sprite = data.horseSprite;

        
            myButton.onClick.RemoveAllListeners();
            myButton.onClick.AddListener(() => OpenMessageTab());
    }
    public void OpenMessageTab()
    {
        SoundManager.instance.PlaySoundEffect(clickSound,0.7f);
        messagesManager.ChangeTab(this.gameObject);
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
        if (!hasOpt1 && !hasOpt2)
        {
            EndDialogue(node.goodEnd);
        }
    }
    public void CloseDialogues()
    {
        messagesManager.CloseTabs();
    }
    public void EndDialogue(bool good)
    {
        if (good)
        {
            askOutButton.gameObject.SetActive(true);
            optButton1.gameObject.SetActive(false);
            optButton2.gameObject.SetActive(false);
        }
        else
        {
            myButton.interactable = false;
        }
    }

    private void SelectOption(DialogueNode next)
    {
        SoundManager.instance.PlaySoundEffect(clickSound,0.7f);
        StartCoroutine(OptionRoutine(next));
    }

    public void NewBox(string inText, bool right)
    {
        GameObject newBox = Instantiate(optBoxFab, contentRect);
        SoundManager.instance.PlaySoundEffect(popSound,0.85f);

        float anchor = right ? rBoxPos : lBoxPos;
        newBox.GetComponent<BoxPopUp>().Setup(inText, anchor);

        // Stack boxes by accumulating real heights
        float currentY = yOrigin;
        foreach (Transform boxChild in contentRect)
        {
            RectTransform childRect = boxChild.GetComponent<RectTransform>();
            currentY += childRect.sizeDelta.y + boxOffset;
        }
        foreach (Transform boxChild in contentRect)
        {
            RectTransform childRect = boxChild.GetComponent<RectTransform>();
            boxChild.localPosition = new Vector3(boxChild.localPosition.x, currentY, 0f);
            currentY -= childRect.sizeDelta.y + boxOffset;
        }
    }
    IEnumerator OptionRoutine(DialogueNode next)
    {
        NewBox(next.ourLine, false);

        optButton1.interactable = false;
        optButton2.interactable = false;

        yield return new WaitForSeconds(responseTime);

        optButton1.interactable = true;
        optButton2.interactable = true;

        LoadNode(next);
    }
}
