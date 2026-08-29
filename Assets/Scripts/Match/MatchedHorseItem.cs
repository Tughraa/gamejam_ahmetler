using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchedHorseItem : MonoBehaviour
{
    [Header("UI Slots")]
    public Image avatarImage;
    public TextMeshProUGUI nameText;
    public Button button;

    private HorseData _horseData;

    [Header("Sound Effects")]
    public AudioClip clickSound;

    public void Setup(HorseData data)
    {
        _horseData = data;

        if (data == null) return;

        // 1. Set horse avatar
        if (avatarImage != null && data.horseSprite != null)
        {
            avatarImage.sprite = data.horseSprite;
        }

        // 2. Set horse name
        if (nameText != null)
        {
            nameText.text = data.horseName;
        }

        // 3. Setup click event
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClickItem);
        }
    }

    private void OnClickItem()
    {
        SoundManager.instance.PlaySoundEffect(clickSound,0.7f);
        ScreenManager.Instance?.OpenMessengerScreenWithHorse(_horseData);
    }
}