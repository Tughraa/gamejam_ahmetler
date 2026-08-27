using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagesManager : MonoBehaviour
{
    public GameObject textingTab;
    public GameObject textingButton;
    public Transform buttonParent;
    void Start()
    {
        foreach (Transform textTab in this.transform)
        {
            textTab.GetComponent<DialogueManager>().messagesManager = this;
            textTab.gameObject.SetActive(false);
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
        
    }
}
