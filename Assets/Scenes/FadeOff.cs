using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOff : MonoBehaviour
{
    public Image image;
    public float duration = 2f;
    public AudioClip startSound;

    public void Start()
    {
        SoundManager.instance.PlaySoundEffect(startSound,1f);
        StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        float elapsed = 0f;
        Color c = image.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = 1f - Mathf.Clamp01(elapsed / duration);
            image.color = c;
            yield return null;
        }

        c.a = 0f;
        image.color = c;
        SoundManager.instance.musicSource.Play();
    }
}
