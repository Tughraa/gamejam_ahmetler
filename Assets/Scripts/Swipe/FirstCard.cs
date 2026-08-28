using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FirstCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private Image _image;
    private Canvas _rootCanvas;
    private Vector2 _initialPosition;
    private float _distanceMoved;
    private bool _swipeLeft;

    [Header("Details Scroll Settings")]
    public RectTransform contentHolder;
    public float maxScrollUpDistance = 340f;
    [Tooltip("Adjust scroll sensitivity for low-res pixel canvases (0.5 - 1.0 is standard)")]
    public float scrollSensitivity = 1.0f;
    [Tooltip("Smoothing speed for details scrolling")]
    public float scrollSmoothSpeed = 15f;

    private float _targetScrollY = 0f;
    private float _currentScrollY = 0f;
    private bool _isDraggingVertical = false;

    public float NormalizedDragProgress { get; private set; }

    public event Action cardMoved;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _rootCanvas = GetComponentInParent<Canvas>();
        _initialPosition = _rectTransform.anchoredPosition;

        if (contentHolder == null)
        {
            Transform t = transform.Find("ContentHolder");
            if (t != null) contentHolder = t.GetComponent<RectTransform>();
        }

        if (contentHolder != null)
        {
            _targetScrollY = contentHolder.anchoredPosition.y;
            _currentScrollY = _targetScrollY;
        }
    }

    void Update()
    {
        // Smoothly interpolate vertical scrolling
        if (contentHolder != null)
        {
            _currentScrollY = Mathf.Lerp(_currentScrollY, _targetScrollY, Time.deltaTime * scrollSmoothSpeed);
            contentHolder.anchoredPosition = new Vector2(contentHolder.anchoredPosition.x, _currentScrollY);
        }
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        // If we are currently scrolled down viewing details, default directly to vertical scrolling
        if (_targetScrollY > 5f)
        {
            _isDraggingVertical = true;
        }
        else
        {
            _isDraggingVertical = false;
        }

        if (contentHolder != null)
        {
            _targetScrollY = contentHolder.anchoredPosition.y;
            _currentScrollY = _targetScrollY;
        }
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        Vector2 totalDelta = pointerEventData.position - pointerEventData.pressPosition;

        // Detect vertical intent if not already locked in
        if (!_isDraggingVertical)
        {
            // If dragging vertically more than horizontally, switch to vertical scroll
            if (Mathf.Abs(totalDelta.y) > Mathf.Abs(totalDelta.x) && Mathf.Abs(totalDelta.y) > 10f)
            {
                _isDraggingVertical = true;
            }
        }

        // 1. VERTICAL SCROLL (Works both UP to view details and DOWN to return to profile photo)
        if (_isDraggingVertical && contentHolder != null)
        {
            float canvasScale = _rootCanvas != null ? _rootCanvas.scaleFactor : 1f;
            float scaledDeltaY = (pointerEventData.delta.y / canvasScale) * scrollSensitivity;

            // Clamps smoothly between 0 (Top / Profile Photo) and maxScrollUpDistance (Bottom / Details)
            _targetScrollY = Mathf.Clamp(_targetScrollY + scaledDeltaY, 0f, maxScrollUpDistance);
            return;
        }

        // 2. HORIZONTAL SWIPE (Only allowed when viewing the top profile photo)
        if (_targetScrollY <= 5f)
        {
            _rectTransform.anchoredPosition += new Vector2(pointerEventData.delta.x, 0);

            float dragOffset = _rectTransform.anchoredPosition.x - _initialPosition.x;
            NormalizedDragProgress = Mathf.Clamp01(Mathf.Abs(dragOffset) / (Screen.width / 2f));

            if (dragOffset > 0)
                _rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, -30f, NormalizedDragProgress));
            else
                _rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, 30f, NormalizedDragProgress));
        }
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        if (_isDraggingVertical)
        {
            _isDraggingVertical = false;
            return;
        }

        _distanceMoved = Mathf.Abs(_rectTransform.anchoredPosition.x - _initialPosition.x);

        if (_distanceMoved < 0.1f * Screen.width)
        {
            _rectTransform.anchoredPosition = _initialPosition;
            _rectTransform.localEulerAngles = Vector3.zero;
            NormalizedDragProgress = 0f;
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
            if (!display.horseData.matchable) return;

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
        if (_image != null) _image.raycastTarget = false;

        float time = 0f;
        float duration = 0.3f;

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