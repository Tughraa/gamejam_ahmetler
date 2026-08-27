using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [Header("UI References")]
    public Image cardImage;
    public TextMeshProUGUI nameText;

    [HideInInspector]
    public HorseData horseData;

    // Call this right after instantiating to bind data
    public void SetupCard(HorseData data)
    {
        horseData = data;

        if (horseData != null)
        {
            if (cardImage != null && horseData.horseSprite != null)
                cardImage.sprite = horseData.horseSprite;

            if (nameText != null)
                nameText.text = horseData.horseName;
        }
    }
}