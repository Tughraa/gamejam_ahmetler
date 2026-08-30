using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagesManager : MonoBehaviour
{
    public GameObject textingTab;
    public GameObject textingButton;
    public Transform buttonParent;
    public float tabSpacing = 80f;
    public HorseData[] matchedHorses;
    public Color[] tabColors;
    void Start()
    {
        CloseTabs();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            CreateChat(matchedHorses[Random.Range(0,matchedHorses.Length)]);
        }
    }
    public void CloseTabs()
    {
        foreach (Transform textTab in this.transform)
        {
            textTab.GetComponent<DialogueManager>().messagesManager = this;
            textTab.gameObject.SetActive(false);
        }
    }
    public void ChangeTab(GameObject tabToOpen)
    {
        CloseTabs();
        tabToOpen.SetActive(true);
    }
    public GameObject CreateChat(HorseData horse)
    {
        GameObject newTab = Instantiate(textingTab,this.transform);
        GameObject newButton = Instantiate(textingButton,buttonParent);

        DialogueManager newTabDM = newTab.GetComponent<DialogueManager>();
        newTabDM.myButton = newButton.GetComponent<Button>();
        newTabDM.UpdateHorseData(horse);
        CloseTabs();
        
        
        newTab.GetComponent<DialogueManager>().UpdateHorseData(horse);
        OrderButtons();
        return newTab;
    }
    public void OrderButtons()
    {
        float y0 = buttonParent.GetChild(0).position.y;
        int i = 0;
        foreach (Transform messageButton in buttonParent)
        {
            messageButton.GetComponent<Image>().color = tabColors[i%tabColors.Length];
            messageButton.GetChild(0).GetComponent<TMP_Text>().color = tabColors[(i+1)%tabColors.Length];
            RectTransform rect = messageButton.GetComponent<RectTransform>();
            rect.position = new Vector3(rect.position.x,y0-i*tabSpacing,0f);
            i++;
        }
    }
}
