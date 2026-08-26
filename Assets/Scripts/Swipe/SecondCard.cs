using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class SecondCard : MonoBehaviour
{
    private SwipeEffect _swipeEffect;
    private GameObject _firstCard;
    private Vector3 _originalScale;
    private bool _isSwipeCompleted = false;
    private float _step = 0.8f;

    // Start is called before the first frame update
    void Awake()
    {
        _originalScale = transform.localScale;
    }

    void Start()
    {
        _swipeEffect = FindObjectOfType<SwipeEffect>();
        _firstCard = _swipeEffect.gameObject;
        _swipeEffect.cardMoved += CardMovedFront;

        transform.localScale =  new Vector3(36,36,36);

    }

    // Update is called once per frame
    void Update()
    {
        if (_isSwipeCompleted)
        {
            _step = Mathf.MoveTowards(_step, 1.0f, Time.deltaTime * 5f);
            transform.localScale = _originalScale * _step;
            return;
        }
        float distanceMoved = Mathf.Abs(_firstCard.transform.localPosition.x);
        float maxSwipeDistance = Screen.width / 2f;

        float dragPercentage = Mathf.Clamp01(distanceMoved / maxSwipeDistance);


        if(Mathf.Abs(distanceMoved) > 0)
        {
            float step = Mathf.Lerp(0.8f, 1.0f, dragPercentage);
            transform.localScale = _originalScale * step;
        }
    }

    void CardMovedFront()
    {
        gameObject.AddComponent<SwipeEffect>();
        Destroy(this);

    }

    public void OnSwipeCompleted()
    {
        _isSwipeCompleted = true;
    }
}
