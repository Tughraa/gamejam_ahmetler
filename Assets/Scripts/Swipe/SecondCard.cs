using System.Collections;
using UnityEngine;

public class SecondCard : MonoBehaviour
{
    private FirstCard _frontCard;
    private readonly Vector3 _minScale = Vector3.one * 0.8f;
    private readonly Vector3 _maxScale = Vector3.one;

    [Header("Details Scroll Settings to Pass to FirstCard")]
    [Tooltip("Keep this equal to the maxScrollUpDistance on FirstCard")]
    public float maxScrollUpDistance = 480f; // Set this high enough for Photo3

    void Start()
    {
        transform.localScale = _minScale;
        TryBindFrontCard();
    }

    void Update()
    {
        if (_frontCard == null)
        {
            TryBindFrontCard();
            return;
        }

        transform.localScale = Vector3.Lerp(_minScale, _maxScale, _frontCard.NormalizedDragProgress);
    }

    private void TryBindFrontCard()
    {
        FirstCard[] activeCards = FindObjectsOfType<FirstCard>();
        foreach (FirstCard card in activeCards)
        {
            if (card.gameObject != this.gameObject)
            {
                _frontCard = card;
                _frontCard.cardMoved += OnFrontCardLeft;
                break;
            }
        }
    }

    private void OnFrontCardLeft()
    {
        if (_frontCard != null)
        {
            _frontCard.cardMoved -= OnFrontCardLeft;
            _frontCard = null;
        }

        StartCoroutine(SmoothScaleUpAndPromote());
    }

    private IEnumerator SmoothScaleUpAndPromote()
    {
        Vector3 startScale = transform.localScale;
        float time = 0f;
        float duration = 0.25f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, time / duration);
            transform.localScale = Vector3.Lerp(startScale, _maxScale, t);
            yield return null;
        }

        transform.localScale = _maxScale;

        // Upgrade to FirstCard and pass the scroll distance
        FirstCard newFirst = GetComponent<FirstCard>();
        if (newFirst == null)
        {
            newFirst = gameObject.AddComponent<FirstCard>();
        }
        newFirst.maxScrollUpDistance = maxScrollUpDistance;

        Destroy(this);
    }

    void OnDestroy()
    {
        if (_frontCard != null)
        {
            _frontCard.cardMoved -= OnFrontCardLeft;
        }
    }
}