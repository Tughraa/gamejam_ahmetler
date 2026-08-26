using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteButton : MonoBehaviour
{
    HorseCustomization horseCustom;
    public Sprite sprite;
    public int layer;
    void Start()
    {
        //Might have to change based on the object parent/child structure
        horseCustom = this.transform.parent.parent.GetComponent<HorseCustomization>();
    }
    public void Clicked()
    {
        horseCustom.ChangeSprite(layer,sprite);
    }
}
