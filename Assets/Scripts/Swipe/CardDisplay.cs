using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [Header("Main Card UI")]
    public Image mainImage;
    public TextMeshProUGUI nameText;

    [Header("Details Section UI")]
    public TextMeshProUGUI bioText;
    public Image photo2Image;
    public Image photo3Image;
    public TextMeshProUGUI funnyAnswer1Text;
    public TextMeshProUGUI funnyAnswer2Text;

    [HideInInspector]
    public HorseData horseData;

    public void SetupCard(HorseData data)
    {
        horseData = data;
        if (horseData == null) return;

        // 1. Main Header info
        if (mainImage != null && horseData.horseSprite != null)
            mainImage.sprite = horseData.horseSprite;

        if (nameText != null)
            nameText.text = horseData.horseName;

        // 2. Bio text
        if (bioText != null)
            bioText.text = horseData.bio;

        // 3. Extra Photos from Array
        if (horseData.horsePictures != null)
        {
            if (photo2Image != null && horseData.horsePictures.Length > 0 && horseData.horsePictures[0] != null)
                photo2Image.sprite = horseData.horsePictures[0];

            if (photo3Image != null && horseData.horsePictures.Length > 1 && horseData.horsePictures[1] != null)
                photo3Image.sprite = horseData.horsePictures[1];
        }

        // 4. Funny Q&A Text
        if (funnyAnswer1Text != null)
            funnyAnswer1Text.text = horseData.funnyAnswer1;

        if (funnyAnswer2Text != null)
            funnyAnswer2Text.text = horseData.funnyAnswer2;
    }
}