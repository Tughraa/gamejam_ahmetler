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
        // Find all CardDisplay components on the starting cards in the container
        CardDisplay[] startingCards = GetComponentsInChildren<CardDisplay>();
        ShuffleDeck();
        // Unity UI order: Last child is the front card (index 0 in deck), first child is back card (index 1)
        for (int i = startingCards.Length - 1; i >= 0; i--)
        {
            if (_currentCardIndex < horseDeck.Count)
            {
                startingCards[i].SetupCard(horseDeck[_currentCardIndex]);
                _currentCardIndex++;
            }
        }
    }
    public void ShuffleDeck()
{
    for (int i = horseDeck.Count - 1; i > 0; i--)
    {
        int randomIndex = Random.Range(0, i + 1);
        HorseData temp = horseDeck[i];
        horseDeck[i] = horseDeck[randomIndex];
        horseDeck[randomIndex] = temp;
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