using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreen : MonoBehaviour
{
    TMP_Text endText;
    public float typingSpeed = 0.05f;
    public Image[] toAppear;
    public float appearTime = 3f;
    void Awake()
    {
        endText = this.transform.GetChild(0).GetComponent<TMP_Text>();
    }
    void Start()
    {
        foreach (Image img in toAppear)
        {
            Color c = img.color;
            c.a = 0f;
            img.color = c;
        }
    }

    public void TriggerEnding(string inEndText) //maybe input music aswell
    {
        StartCoroutine(TypeText(inEndText));
    }
    private IEnumerator TypeText(string text)
    {
        endText.text = "";
        foreach (char c in text)
        {
            endText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        StartCoroutine(FadeInImages(toAppear,appearTime));
    }
    public IEnumerator FadeInImages(Image[] images, float duration)
    {
        float elapsed = 0f;

        // Make sure all images start fully transparent
        foreach (Image img in images)
        {
            Color c = img.color;
            c.a = 0f;
            img.color = c;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / duration);

            foreach (Image img in images)
            {
                Color c = img.color;
                c.a = alpha;
                img.color = c;
            }

            yield return null;
        }

        // Ensure they end fully opaque
        foreach (Image img in images)
        {
            Color c = img.color;
            c.a = 1f;
            img.color = c;
        }
    }
}
