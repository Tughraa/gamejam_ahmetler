using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    public GameObject cardPrefab;

    [Header("Deck of Horses")]
    public List<HorseData> horseDeck = new List<HorseData>();
    private int _currentCardIndex = 0;

    void InstantiateCard()
    {
        if (cardPrefab == null || _currentCardIndex >= horseDeck.Count) return;

        GameObject newCard = Instantiate(cardPrefab, transform, false);

        // Reset scale and position
        RectTransform rt = newCard.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;
        }

        // Assign horse data to this card
        CardDisplay display = newCard.GetComponent<CardDisplay>();
        if (display != null)
        {
            display.SetupCard(horseDeck[_currentCardIndex]);
            _currentCardIndex++;
        }

        newCard.transform.SetAsFirstSibling();
    }

    void Update()
    {
        // Maintain 2 cards in the UI stack as long as we have horses left
        if (transform.childCount < 2 && _currentCardIndex < horseDeck.Count)
        {
            InstantiateCard();
        }
    }
}