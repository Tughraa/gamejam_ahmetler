using System.Collections.Generic;
using UnityEngine;

public class MatchesListManager : MonoBehaviour
{
    public static MatchesListManager Instance { get; private set; }

    [Header("Match Cap Settings")]
    public int maxMatches = 6;

    [Header("UI references")]
    public Transform leftScreenContainer;
    public GameObject matchItemPrefab;

    private readonly List<HorseData> _matchedHorses = new List<HorseData>();

    // Helper to check if maximum limit has been reached
    public bool HasReachedMaxMatches => _matchedHorses.Count >= maxMatches;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddMatch(HorseData horse)
    {
        if (horse == null) return;

        // Strict limit: reject if already at 6 matches or already in the list
        if (HasReachedMaxMatches || _matchedHorses.Contains(horse)) return;

        _matchedHorses.Add(horse);

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