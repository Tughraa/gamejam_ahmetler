using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeEffect : MonoBehaviour, IDragHandler , IBeginDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private Vector2 _initialPosition;
    private float _distanceMoved;
    private bool _swipeLeft;

    public event Action cardMoved;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        _initialPosition = _rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        //throw new System.NotImplementedException();
        _rectTransform.anchoredPosition += new Vector2(pointerEventData.delta.x, 0);

        if(_rectTransform.localPosition.x - _initialPosition.x > 0)
        {
            _rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, -30, 
                (_initialPosition.x + _rectTransform.localPosition.x) / (Screen.width / 2)));
        }
        else
        {
            _rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, 30,
                (_initialPosition.x - _rectTransform.localPosition.x) / (Screen.width / 2)));
        }
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        _distanceMoved = Mathf.Abs(_rectTransform.anchoredPosition.x - _initialPosition.x);

        if(_distanceMoved < 0.2 * Screen.width)
        {
            _rectTransform.localPosition = _initialPosition;
            _rectTransform.localEulerAngles = Vector3.zero;
        }
        else
        {
            if(_rectTransform.position.x > _initialPosition.x)
            {
                _swipeLeft = false;
            }
            else
            {
                _swipeLeft = true;
            }
            cardMoved?.Invoke();
            StartCoroutine(MovedCard());
        }
    }

    private IEnumerator MovedCard()
    {
        float time = 0;
        while (GetComponent<Image>().color != new Color(1, 1, 1))
        {
            time += Time.deltaTime;

            if (_swipeLeft)
            {
                _rectTransform.position = new Vector3(Mathf.SmoothStep(_rectTransform.anchoredPosition.x, _rectTransform.anchoredPosition.x - Screen.width, 4 * time),
                     _rectTransform.anchoredPosition.y, 0);
            }
            else
            {
                _rectTransform.position = new Vector3(Mathf.SmoothStep(_rectTransform.anchoredPosition.x, _rectTransform.anchoredPosition.x + Screen.width, 4 * time),
                    _rectTransform.anchoredPosition.y, 0);
            }
            GetComponent<Image>().color = new Color(1, 1, 1, Mathf.SmoothStep(1, 0, 4 * time));
            yield return null;
        }

        Destroy(gameObject);
    }
}
