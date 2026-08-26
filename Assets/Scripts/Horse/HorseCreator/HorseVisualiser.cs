using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseVisualiser : MonoBehaviour
{
    public SpriteRenderer[] layers;
    public Sprite[] sprites;
    public Color[] colors;
    void Start()
    {
        UpdateVisuals();
    }
    public void UpdateVisuals()
    {
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].sprite = sprites[i];
            layers[i].color = colors[i];
        }
    }
}
