using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    public GameObject cardPrefab;

    [Header("Deck of Horses")]
    public List<HorseData> horseDeck = new List<HorseData>();
    private int _currentCardIndex = 0;

    void Start()
    {
        CardDisplay[] startingCards = GetComponentsInChildren<CardDisplay>();

        for (int i = startingCards.Length - 1; i >= 0; i--)
        {
            if (_currentCardIndex < horseDeck.Count)
            {
                startingCards[i].SetupCard(horseDeck[_currentCardIndex]);
                _currentCardIndex++;
            }
        }
    }

    public void InstantiateCard()
    {
        if (cardPrefab == null || _currentCardIndex >= horseDeck.Count) return;

        GameObject newCard = Instantiate(cardPrefab, transform, false);

        RectTransform rt = newCard.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchoredPosition = new Vector2(39.3f, -18.5f);
            rt.localScale = Vector3.one;
        }

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
        if (transform.childCount < 2 && _currentCardIndex < horseDeck.Count)
        {
            InstantiateCard();
        }
    }
}