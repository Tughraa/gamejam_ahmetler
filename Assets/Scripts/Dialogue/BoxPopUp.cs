using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoxPopUp : MonoBehaviour
{
    public float animLength;
    public AnimationCurve sizeAnim;

    float timer = 0f;
    Vector3 ogSize;
    RectTransform rect;

    private TMP_Text tmp;

    public float maxWidth = 600f;
    public float padding = 20f;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        tmp = GetComponentInChildren<TMP_Text>();
    }
    void Start()
    {
        //rect.localScale = new Vector3(rect.localScale.x+Random.Range(-1f,1f),rect.localScale.y,rect.localScale.z);
        //this.transform.position = new Vector3(this.transform.position.x+(rect.lossyScale.x/2f*Mathf.Sign(this.transform.localPosition.x)),this.transform.position.y,this.transform.position.z);
        ogSize = this.transform.localScale;
    }

    public void Setup(string text, float xAnchor)
{
    tmp.text = text;
    tmp.enableWordWrapping = true;

    // First pass: unconstrained to get natural width
    Vector2 preferred = tmp.GetPreferredValues(text, Mathf.Infinity, Mathf.Infinity);

    float targetWidth = Mathf.Min(preferred.x + padding, maxWidth);

    // Set TMP child width FIRST so it wraps correctly
    RectTransform tmpRect = tmp.GetComponent<RectTransform>();
    tmpRect.sizeDelta = new Vector2(targetWidth - padding, 0f);

    // Second pass: now get height with the constrained width
    float targetHeight = tmp.GetPreferredValues(text, targetWidth - padding, Mathf.Infinity).y + padding;

    // Now set both rects with final values
    rect.sizeDelta = new Vector2(targetWidth, targetHeight);
    tmpRect.sizeDelta = new Vector2(targetWidth - padding, targetHeight - padding);

    // Offset from anchor inward by half width
    float halfWidth = targetWidth / 2f;
    float xPos = xAnchor - halfWidth * Mathf.Sign(xAnchor);
    rect.localPosition = new Vector3(xPos+700f, rect.localPosition.y, 0f);
}

    void Update()
    {
        if (timer < animLength)
        {
            timer += Time.deltaTime;
            this.transform.localScale = ogSize*sizeAnim.Evaluate(timer/animLength);
        }
        else
        {
            timer = animLength;
            this.transform.localScale = ogSize;
        }

    }
}
