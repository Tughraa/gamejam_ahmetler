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

    void Awake()
    {
        // Auto-find references inside ContentHolder if left empty in Inspector
        if (mainImage == null)
        {
            Transform t = transform.Find("ContentHolder/ProfilePhoto") ?? transform.Find("ProfilePhoto");
            if (t != null) mainImage = t.GetComponent<Image>();
        }

        if (photo2Image == null)
        {
            Transform t = transform.Find("ContentHolder/Photo2") ?? transform.Find("Photo2");
            if (t != null) photo2Image = t.GetComponent<Image>();
        }

        if (photo3Image == null)
        {
            Transform t = transform.Find("ContentHolder/Photo3") ?? transform.Find("Photo3");
            if (t != null) photo3Image = t.GetComponent<Image>();
        }
    }

    public void SetupCard(HorseData data)
    {
        horseData = data;
        if (horseData == null) return;

        // 1. Main Header info
        if (mainImage != null && horseData.horseSprite != null)
        {
            mainImage.sprite = horseData.horseSprite;
        }

        if (nameText != null)
            nameText.text = horseData.horseName;

        // 2. Bio text
        if (bioText != null)
            bioText.text = horseData.bio;

        // 3. Extra Photos from horsePictures Array
        if (horseData.horsePictures != null && horseData.horsePictures.Length > 0)
        {
            if (photo2Image != null && horseData.horsePictures.Length > 0 && horseData.horsePictures[0] != null)
            {
                photo2Image.sprite = horseData.horsePictures[0];
                photo2Image.gameObject.SetActive(true);
            }
            else if (photo2Image == null)
            {
                Debug.LogWarning($"[CardDisplay] photo2Image UI slot is NOT assigned on {gameObject.name}!");
            }

            if (photo3Image != null && horseData.horsePictures.Length > 1 && horseData.horsePictures[1] != null)
            {
                photo3Image.sprite = horseData.horsePictures[1];
                photo3Image.gameObject.SetActive(true);
            }
            else if (photo3Image == null)
            {
                Debug.LogWarning($"[CardDisplay] photo3Image UI slot is NOT assigned on {gameObject.name}!");
            }
        }
        else
        {
            Debug.LogWarning($"[CardDisplay] horsePictures array on {horseData.horseName} is empty!");
        }

        // 4. Funny Q&A Text
        if (funnyAnswer1Text != null)
            funnyAnswer1Text.text = horseData.funnyAnswer1;

        if (funnyAnswer2Text != null)
            funnyAnswer2Text.text = horseData.funnyAnswer2;
    }
}