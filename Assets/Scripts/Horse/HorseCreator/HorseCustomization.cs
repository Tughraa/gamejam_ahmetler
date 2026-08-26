using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseCustomization : MonoBehaviour
{
    public HorseVisualiser horseVis;
    public GameObject[] layerButtons;
    int currentLayer = 0;
    public void SwapLayer(bool right)
    {
        foreach (GameObject but in layerButtons)
        {
            but.SetActive(false);
        }
        if (right)
        {currentLayer++;}
        else
        {currentLayer--;}
        if (currentLayer < 0)
        {currentLayer = layerButtons.Length-1;}
        if (currentLayer > layerButtons.Length-1)
        {currentLayer = 0;}

        layerButtons[currentLayer].SetActive(true);
        //change name too dont forgetti
    }
    public void ChangeColor(int layer, Color inColor)
    {
        horseVis.colors[layer] = inColor;
        horseVis.UpdateVisuals();
    }
    public void ChangeSprite(int layer, Sprite inSprite)
    {
        horseVis.sprites[layer] = inSprite;
        horseVis.UpdateVisuals();
    }
}
