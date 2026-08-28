using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeEffect : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private Image _image;
    private Vector2 _initialPosition;
    private float _distanceMoved;
    private bool _swipeLeft;

    [Header("Details Scroll Settings")]
    public RectTransform contentHolder;
    public float maxScrollUpDistance = 340f;
    private float _currentScrollY = 0f;
    private bool _isDraggingVertical = false;

    public event Action cardMoved;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        transform.localScale = Vector3.one;

        if (contentHolder == null)
        {
            Transform t = transform.Find("ContentHolder");
            if (t != null) contentHolder = t.GetComponent<RectTransform>();
        }
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        _initialPosition = _rectTransform.anchoredPosition;
        _isDraggingVertical = false;
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        Vector2 totalDelta = pointerEventData.position - pointerEventData.pressPosition;

        // Determine if the player wants to scroll details or swipe the card
        if (!_isDraggingVertical && Mathf.Abs(totalDelta.y) > Mathf.Abs(totalDelta.x) && Mathf.Abs(totalDelta.y) > 20f)
        {
            _isDraggingVertical = true;
        }

        // 1. VERTICAL SCROLL (Details View)
        if (_isDraggingVertical && contentHolder != null)
        {
            _currentScrollY = Mathf.Clamp(_currentScrollY + pointerEventData.delta.y, 0f, maxScrollUpDistance);
            contentHolder.anchoredPosition = new Vector2(contentHolder.anchoredPosition.x, _currentScrollY);
            return;
        }

        // 2. HORIZONTAL SWIPE
        _rectTransform.anchoredPosition += new Vector2(pointerEventData.delta.x, 0);

        float dragOffset = _rectTransform.anchoredPosition.x - _initialPosition.x;
        float progress = Mathf.Clamp01(Mathf.Abs(dragOffset) / (Screen.width / 2f));

        if (dragOffset > 0)
            _rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, -30f, progress));
        else
            _rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, 30f, progress));
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        if (_isDraggingVertical)
        {
            _isDraggingVertical = false;
            return;
        }

        _distanceMoved = Mathf.Abs(_rectTransform.anchoredPosition.x - _initialPosition.x);

        if (_distanceMoved < 0.2f * Screen.width)
        {
            // Snap back horizontally
            _rectTransform.anchoredPosition = _initialPosition;
            _rectTransform.localEulerAngles = Vector3.zero;
        }
        else
        {
            _swipeLeft = (_rectTransform.anchoredPosition.x < _initialPosition.x);
            cardMoved?.Invoke();

            if (!_swipeLeft)
            {
                HandleLikeSwipe();
            }

            StartCoroutine(MovedCard());
        }
    }

    private void HandleLikeSwipe()
    {
        CardDisplay display = GetComponent<CardDisplay>();
        if (display != null && display.horseData != null && PlayerData.Instance != null)
        {
            bool[] playerAnswers = PlayerData.Instance.playerAnswers;
            bool isMatch = MatchEvaluator.EvaluateMatch(display.horseData, playerAnswers, out float matchPercent);

            if (isMatch)
            {
                ScreenManager.Instance?.OpenMatchScreen();
            }
        }
    }

    private IEnumerator MovedCard()
    {
        float time = 0f;
        float duration = 0.35f;

        Vector2 startPos = _rectTransform.anchoredPosition;
        float targetX = _swipeLeft ? startPos.x - Screen.width : startPos.x + Screen.width;
        Vector2 targetPos = new Vector2(targetX, startPos.y);

        Color startColor = _image != null ? _image.color : Color.white;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            _rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, smoothT);

            if (_image != null)
                _image.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(1f, 0f, smoothT));

            yield return null;
        }

        Destroy(gameObject);
    }
}