using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPopUp : MonoBehaviour
{
    public float animLength;
    public AnimationCurve sizeAnim;

    float timer = 0f;
    Vector3 ogSize;

    void Start()
    {
        ogSize = this.transform.localScale;
    }

    void Update()
    {
        if (timer < animLength)
        {
            timer += Time.deltaTime;
            this.transform.localScale = ogSize*sizeAnim.Evaluate(timer/animLength);
        }
        else
        {
            timer = animLength;
            this.transform.localScale = ogSize;
        }

    }
}
