using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance;

    [Header("Screen Panels")]
    public GameObject swipeCardContainer;
    public GameObject matchPanel;
    public GameObject messagingPanel;
    public GameObject datePanel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        ShowOnlySwipeCards();
    }

    // Opens the "It's a Match!" popup over the cards
    public void OpenMatchScreen()
    {
        matchPanel.SetActive(true);
    }

    // Called when clicking "Send a Message" from the match screen
    public void OpenMessagingScreen()
    {
        matchPanel.SetActive(false);
        swipeCardContainer.SetActive(false);
        datePanel.SetActive(false);
        messagingPanel.SetActive(true);
    }

    // Called when pressing "Ask out on a Date" from chat
    public void OpenDateScreen()
    {
        messagingPanel.SetActive(false);
        datePanel.SetActive(true);
    }

    // Closes any active screen and returns to swiping
    public void ShowOnlySwipeCards()
    {
        if (matchPanel != null) matchPanel.SetActive(false);
        if (messagingPanel != null) messagingPanel.SetActive(false);
        if (datePanel != null) datePanel.SetActive(false);
        if (swipeCardContainer != null) swipeCardContainer.SetActive(true);
    }
}