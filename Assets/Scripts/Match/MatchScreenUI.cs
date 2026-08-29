using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchScreenUI : MonoBehaviour
{
    [Header("Match UI")]
    public Image playerAvatarImage;
    public Image matchedHorseImage;
    public TextMeshProUGUI matchTitleText;

    [Header("Buttons")]
    public Button keepSwipingButton;
    public Button goToMessagesButton;

    private HorseData _currentMatchedHorse;

    void Awake()
    {
        if (keepSwipingButton != null)
            keepSwipingButton.onClick.AddListener(OnKeepSwipingClicked);

        if (goToMessagesButton != null)
            goToMessagesButton.onClick.AddListener(OnGoToMessagesClicked);
    }

    public void SetupMatchScreen(HorseData matchedHorse, HorseData playerProfile)
    {
        _currentMatchedHorse = matchedHorse;

        // 1. Immediately save this match into our permanent list
        MatchesListManager.Instance?.AddMatch(matchedHorse);

        // 2. Setup display avatars
        if (playerAvatarImage != null && playerProfile != null)
        {
            playerAvatarImage.sprite = playerProfile.horseSprite;
        }

        if (matchedHorse != null)
        {
            if (matchedHorseImage != null) matchedHorseImage.sprite = matchedHorse.horseSprite;
            if (matchTitleText != null) matchTitleText.text = $"You and {matchedHorse.horseName} liked each other!";
        }

        gameObject.SetActive(true);
    }

    private void OnKeepSwipingClicked()
    {
        // Just hide the modal so the player can keep swiping cards
        gameObject.SetActive(false);
    }

    private void OnGoToMessagesClicked()
    {
        gameObject.SetActive(false);
        ScreenManager.Instance?.OpenMessengerScreenWithHorse(_currentMatchedHorse);
    }
}