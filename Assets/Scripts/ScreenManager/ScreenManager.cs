using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance;

    [Header("Screen Panels")]
    public GameObject swipeCardContainer;
    public GameObject matchPanel;
    public GameObject messagingPanel;
    public GameObject datePanel;
    public MatchScreenUI matchScreenUI;



    [Header("Player Reference")]
    public HorseData playerProfile;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        ShowOnlySwipeCards();
    }

    // Opens the "It's a Match!" popup over the cards
    public void OpenMatchScreen(HorseData matchedHorse)
    {
        if (matchScreenUI != null)
        {
            matchScreenUI.SetupMatchScreen(matchedHorse, playerProfile);
        }
    }

    // Called when clicking "Send a Message" from the match screen
    public void OpenMessengerScreen()
    {
        // Transition to messenger UI
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

    public void OpenMessengerScreenWithHorse(HorseData horse)
    {
        if (horse == null) return;

        Debug.Log($"Opening messenger with {horse.horseName}!");

        // Open your messaging panel
        if (messagingPanel != null)
        {
            messagingPanel.SetActive(true);
        }

        // Pass the horse data into your dialogue/chat manager when ready
    }
}