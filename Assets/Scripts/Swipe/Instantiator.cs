using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    public GameObject cardPrefab;

    void InstantiateCard()
    {
        if (cardPrefab == null) return;

        // Instantiate inside the canvas container
        GameObject newCard = Instantiate(cardPrefab, transform, false);

        // Reset local position and scale to clean defaults
        RectTransform rt = newCard.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;
        }

        // Send to back of the UI stack
        newCard.transform.SetAsFirstSibling();
    }

    void Update()
    {
        if (transform.childCount < 2)
        {
            InstantiateCard();
        }
    }
}