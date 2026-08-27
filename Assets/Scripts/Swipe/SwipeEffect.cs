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

    public event Action cardMoved;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        transform.localScale = Vector3.one;
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        _initialPosition = _rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
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
        _distanceMoved = Mathf.Abs(_rectTransform.anchoredPosition.x - _initialPosition.x);

        if (_distanceMoved < 0.2f * Screen.width)
        {
            _rectTransform.anchoredPosition = _initialPosition;
            _rectTransform.localEulerAngles = Vector3.zero;
        }
        else
        {
            _swipeLeft = (_rectTransform.anchoredPosition.x < _initialPosition.x);
            cardMoved?.Invoke();

            // Right swipe = test match trigger
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
                // Open match overlay and pass data
                ScreenManager.Instance?.OpenMatchScreen();
                Debug.Log($"Matched with {display.horseData.horseName}! ({matchPercent * 100}%)");
            }
            else
            {
                Debug.Log($"Passed threshold check on {display.horseData.horseName}: No match ({matchPercent * 100}%).");
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