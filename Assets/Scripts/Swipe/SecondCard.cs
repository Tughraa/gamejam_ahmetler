using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCard : MonoBehaviour
{
    private SwipeEffect _swipeEffect;
    private GameObject _firstCard;
    private Vector3 _originalScale = Vector3.one;

    void Awake()
    {
        // Cache initial scale (or default to 1, 1, 1)
        _originalScale = transform.localScale == Vector3.zero ? Vector3.one : transform.localScale;
    }

    void Start()
    {
        // Set visual start size to 80%
        transform.localScale = _originalScale * 0.8f;
        FindFrontCard();
    }

    void FindFrontCard()
    {
        _swipeEffect = FindObjectOfType<SwipeEffect>();
        if (_swipeEffect != null)
        {
            _firstCard = _swipeEffect.gameObject;
            _swipeEffect.cardMoved += CardMovedFront;
        }
    }

    void Update()
    {
        if (_firstCard == null || _swipeEffect == null)
        {
            FindFrontCard();
            return;
        }

        float distanceMoved = Mathf.Abs(_firstCard.transform.localPosition.x);
        float maxSwipeDistance = Screen.width / 2f;
        float dragPercentage = Mathf.Clamp01(distanceMoved / maxSwipeDistance);

        // Zoom from 80% to 100% based on swipe progress
        float step = Mathf.Lerp(0.8f, 1.0f, dragPercentage);
        transform.localScale = _originalScale * step;
    }

    void CardMovedFront()
    {
        if (_swipeEffect != null)
        {
            _swipeEffect.cardMoved -= CardMovedFront;
        }

        // Snap to full size before taking over
        transform.localScale = _originalScale;

        // Upgrade to active swipe card
        gameObject.AddComponent<SwipeEffect>();
        Destroy(this);
    }

    void OnDestroy()
    {
        if (_swipeEffect != null)
        {
            _swipeEffect.cardMoved -= CardMovedFront;
        }
    }
}