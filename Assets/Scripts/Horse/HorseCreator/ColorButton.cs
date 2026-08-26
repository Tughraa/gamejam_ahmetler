using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButton : MonoBehaviour
{
    HorseCustomization horseCustom;
    public Color color;
    public int layer;
    void Start()
    {
        //Might have to change based on the object parent/child structure
        horseCustom = this.transform.parent.parent.GetComponent<HorseCustomization>();
    }
    public void Clicked()
    {
        horseCustom.ChangeColor(layer,color);
    }
}
