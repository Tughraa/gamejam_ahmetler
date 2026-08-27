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

    [Header("Details View")]
    public CanvasGroup detailsCanvasGroup; // Assign DetailsPanel's CanvasGroup
    public float swipeDownThreshold = 150f;

    private bool _isShowingDetails = false;
    private bool _isDraggingDown = false;

    public event Action cardMoved;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        transform.localScale = Vector3.one;

        // Auto-find CanvasGroup on details panel if not assigned manually
        if (detailsCanvasGroup == null)
        {
            Transform panel = transform.Find("DetailsPanel");
            if (panel != null)
            {
                detailsCanvasGroup = panel.GetComponent<CanvasGroup>();
            }
        }

        SetDetailsState(false);
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        _initialPosition = _rectTransform.anchoredPosition;
        _isDraggingDown = false;
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        Vector2 totalDragDelta = pointerEventData.position - pointerEventData.pressPosition;

        // If dragging downward and vertical movement is dominant
        if (!_isShowingDetails && totalDragDelta.y < -30f && Mathf.Abs(totalDragDelta.y) > Mathf.Abs(totalDragDelta.x))
        {
            _isDraggingDown = true;
        }

        if (_isDraggingDown)
        {
            // Only allow pulling downward
            float pullDistance = Mathf.Clamp(pointerEventData.position.y - pointerEventData.pressPosition.y, -swipeDownThreshold * 1.5f, 0f);

            // Move the card slightly down as visual feedback
            _rectTransform.anchoredPosition = new Vector2(_initialPosition.x, _initialPosition.y + (pullDistance * 0.2f));

            // Fade in details panel smoothly according to drag progress
            if (detailsCanvasGroup != null)
            {
                float progress = Mathf.Clamp01(Mathf.Abs(pullDistance) / swipeDownThreshold);
                detailsCanvasGroup.alpha = progress;
            }
            return;
        }

        // Standard Horizontal Swipe Logic
        if (!_isShowingDetails)
        {
            _rectTransform.anchoredPosition += new Vector2(pointerEventData.delta.x, 0);

            float dragOffset = _rectTransform.anchoredPosition.x - _initialPosition.x;
            float progress = Mathf.Clamp01(Mathf.Abs(dragOffset) / (Screen.width / 2f));

            if (dragOffset > 0)
            {
                _rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, -30f, progress));
            }
            else
            {
                _rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, 30f, progress));
            }
        }
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        // Handling end of downward drag
        if (_isDraggingDown)
        {
            float verticalDragDistance = pointerEventData.pressPosition.y - pointerEventData.position.y;

            if (verticalDragDistance >= swipeDownThreshold)
            {
                OpenDetails();
            }
            else
            {
                CloseDetails();
            }

            _rectTransform.anchoredPosition = _initialPosition;
            _isDraggingDown = false;
            return;
        }

        // Handling end of horizontal drag
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
            StartCoroutine(MovedCard());
        }
    }

    public void OpenDetails()
    {
        _isShowingDetails = true;
        SetDetailsState(true);
    }

    public void CloseDetails()
    {
        _isShowingDetails = false;
        SetDetailsState(false);
    }

    private void SetDetailsState(bool isOpen)
    {
        if (detailsCanvasGroup != null)
        {
            detailsCanvasGroup.alpha = isOpen ? 1f : 0f;
            detailsCanvasGroup.interactable = isOpen;
            detailsCanvasGroup.blocksRaycasts = isOpen;
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
            {
                _image.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(1f, 0f, smoothT));
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}