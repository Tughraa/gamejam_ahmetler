using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    [Header("Player Answers (True = Yes, False = No)")]
    public bool[] playerAnswers = new bool[42]; // Set size to your question count

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetAnswer(int questionIndex, bool answer)
    {
        if (questionIndex >= 0 && questionIndex < playerAnswers.Length)
        {
            playerAnswers[questionIndex] = answer;
        }
    }
}