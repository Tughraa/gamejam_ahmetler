using UnityEngine;

public class SecondCard : MonoBehaviour
{
    private SwipeEffect _frontSwipeEffect;
    private RectTransform _frontCardRect;
    private readonly Vector3 _targetBaseScale = Vector3.one;

    void Start()
    {
        transform.localScale = _targetBaseScale * 0.8f;
        TryBindFrontCard();
    }

    void Update()
    {
        // Continuously try to bind if the previous front card was destroyed
        if (_frontSwipeEffect == null || _frontCardRect == null)
        {
            TryBindFrontCard();
            return;
        }

        // Follow front card drag progress
        float distanceMoved = Mathf.Abs(_frontCardRect.anchoredPosition.x);
        float maxSwipeDistance = Screen.width / 2f;
        float progress = Mathf.Clamp01(distanceMoved / maxSwipeDistance);

        float scaleFactor = Mathf.Lerp(0.8f, 1.0f, progress);
        transform.localScale = _targetBaseScale * scaleFactor;
    }

    private void TryBindFrontCard()
    {
        SwipeEffect currentFront = FindObjectOfType<SwipeEffect>();
        if (currentFront != null && currentFront.gameObject != this.gameObject)
        {
            _frontSwipeEffect = currentFront;
            _frontCardRect = currentFront.GetComponent<RectTransform>();
            _frontSwipeEffect.cardMoved += PromoteToFront;
        }
    }

    void PromoteToFront()
    {
        if (_frontSwipeEffect != null)
        {
            _frontSwipeEffect.cardMoved -= PromoteToFront;
        }

        transform.localScale = _targetBaseScale;
        gameObject.AddComponent<SwipeEffect>();
        Destroy(this);
    }

    void OnDestroy()
    {
        if (_frontSwipeEffect != null)
        {
            _frontSwipeEffect.cardMoved -= PromoteToFront;
        }
    }
}