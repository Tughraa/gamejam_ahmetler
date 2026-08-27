using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessagesManager : MonoBehaviour
{
    public void ChangeTab(GameObject tabToOpen)
    {
        foreach (Transform textTab in this.transform)
        {
            textTab.gameObject.SetActive(false);
        }
        tabToOpen.SetActive(true);
    }
}
