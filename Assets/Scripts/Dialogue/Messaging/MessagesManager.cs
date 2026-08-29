using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagesManager : MonoBehaviour
{
    public GameObject textingTab;
    public GameObject textingButton;
    public Transform buttonParent;
    public float tabSpacing = 80f;
    public HorseData[] matchedHorses;
    void Start()
    {
        foreach (Transform textTab in this.transform)
        {
            textTab.GetComponent<DialogueManager>().messagesManager = this;
            textTab.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            CreateChat(matchedHorses[Random.Range(0,matchedHorses.Length)]);
        }
    }
    public void ChangeTab(GameObject tabToOpen)
    {
        foreach (Transform textTab in this.transform)
        {
            textTab.gameObject.SetActive(false);
        }
        tabToOpen.SetActive(true);
    }
    public void CreateChat(HorseData horse)
    {
        GameObject newTab = Instantiate(textingTab,this.transform);
        GameObject newButton = Instantiate(textingButton,buttonParent);

        DialogueManager newTabDM = newTab.GetComponent<DialogueManager>();
        newTabDM.myButton = newButton.GetComponent<Button>();
        newTabDM.UpdateHorseData(horse);
        
        
        newTab.GetComponent<DialogueManager>().UpdateHorseData(horse);
        OrderButtons();
    }
    public void OrderButtons()
    {
        float y0 = buttonParent.GetChild(0).position.y;
        int i = 0;
        foreach (Transform messageButton in buttonParent)
        {
            RectTransform rect = messageButton.GetComponent<RectTransform>();
            rect.position = new Vector3(rect.position.x,y0-i*tabSpacing,0f);
            i++;
        }
    }
}
