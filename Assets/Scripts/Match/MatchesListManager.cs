using System.Collections.Generic;
using UnityEngine;

public class MatchesListManager : MonoBehaviour
{
    public static MatchesListManager Instance { get; private set; }

    [Header("UI references")]
    public Transform leftScreenContainer; // Drag Canvas/ApplicationUI/LeftScreen here
    public GameObject matchItemPrefab;   // Drag MatchedHorseItemPrefab here

    // Saved list of matches
    private readonly List<HorseData> _matchedHorses = new List<HorseData>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddMatch(HorseData horse)
    {
        if (horse == null) return;

        // Prevent duplicate entries
        if (_matchedHorses.Contains(horse)) return;

        _matchedHorses.Add(horse);

        // Spawn a banner icon in LeftScreen
        if (matchItemPrefab != null && leftScreenContainer != null)
        {
            GameObject newItem = Instantiate(matchItemPrefab, leftScreenContainer);
            MatchedHorseItem itemScript = newItem.GetComponent<MatchedHorseItem>();
            if (itemScript != null)
            {
                itemScript.Setup(horse);
            }
        }
    }

    public List<HorseData> GetMatchedHorses()
    {
        return _matchedHorses;
    }
}