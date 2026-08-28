using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FirstCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private Canvas _rootCanvas;
    private Vector2 _initialPosition;
    private float _distanceMoved;
    private bool _swipeLeft;

    [Header("Details Scroll Settings")]
    public RectTransform contentHolder;
    public float maxScrollUpDistance = 350f;
    public float scrollSensitivity = 1.0f;
    public float scrollSmoothSpeed = 20f;

    private float _initialContentY = 0f;
    private float _targetScrollY = 0f;
    private float _currentScrollY = 0f;
    private bool _isDraggingVertical = false;

    public float NormalizedDragProgress { get; private set; }
    public event Action cardMoved;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rootCanvas = GetComponentInParent<Canvas>();
        _initialPosition = _rectTransform.anchoredPosition;

        if (contentHolder == null)
        {
            Transform t = transform.Find("ContentHolder");
            if (t != null) contentHolder = t.GetComponent<RectTransform>();
        }

        if (contentHolder != null)
        {
            // Record where ContentHolder sits in the prefab editor as its absolute top resting place
            _initialContentY = contentHolder.anchoredPosition.y;
            _targetScrollY = _initialContentY;
            _currentScrollY = _initialContentY;
        }
    }

    void Update()
    {
        if (contentHolder != null)
        {
            _currentScrollY = Mathf.Lerp(_currentScrollY, _targetScrollY, Time.deltaTime * scrollSmoothSpeed);
            contentHolder.anchoredPosition = new Vector2(contentHolder.anchoredPosition.x, _currentScrollY);
        }
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        _isDraggingVertical = (_targetScrollY > _initialContentY + 5f);

        if (contentHolder != null)
        {
            _currentScrollY = contentHolder.anchoredPosition.y;
            _targetScrollY = _currentScrollY;
        }
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        Vector2 totalDelta = pointerEventData.position - pointerEventData.pressPosition;

        if (!_isDraggingVertical)
        {
            if (Mathf.Abs(totalDelta.y) > Mathf.Abs(totalDelta.x) && Mathf.Abs(totalDelta.y) > 10f)
            {
                _isDraggingVertical = true;
            }
        }

        // 1. VERTICAL SCROLL
        if (_isDraggingVertical && contentHolder != null)
        {
            float canvasScale = _rootCanvas != null ? _rootCanvas.scaleFactor : 1f;
            float step = (pointerEventData.delta.y / canvasScale) * scrollSensitivity;

            // Clamps between its initial top resting position and max scroll distance
            _targetScrollY = Mathf.Clamp(_targetScrollY + step, _initialContentY, _initialContentY + maxScrollUpDistance);
            return;
        }

        // 2. HORIZONTAL SWIPE
        if (_targetScrollY <= _initialContentY + 5f)
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

            if (_targetScrollY < _initialContentY + 25f)
            {
                _targetScrollY = _initialContentY;
            }
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
        Image rootImage = GetComponent<Image>();
        if (rootImage != null) rootImage.raycastTarget = false;

        float time = 0f;
        float duration = 0.3f;

        Vector2 startPos = _rectTransform.anchoredPosition;
        float targetX = _swipeLeft ? startPos.x - Screen.width : startPos.x + Screen.width;
        Vector2 targetPos = new Vector2(targetX, startPos.y);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, time / duration);
            _rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        Destroy(gameObject);
    }
}