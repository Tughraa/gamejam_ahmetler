using UnityEngine;

public static class MatchEvaluator
{
    // Minimum percentage (e.g., 0.6f = 60% agreement) to trigger a match
    public const float DEFAULT_THRESHOLD = 0.1f;

    public static bool EvaluateMatch(HorseData horse, bool[] playerAnswers, out float matchPercentage, float threshold = DEFAULT_THRESHOLD)
    {
        if (horse == null || horse.answers == null || playerAnswers == null || horse.answers.Length == 0)
        {
            matchPercentage = 0f;
            return false;
        }

        int totalQuestions = Mathf.Min(horse.answers.Length, playerAnswers.Length);
        int agreementCount = 0;

        for (int i = 0; i < totalQuestions; i++)
        {
            // Both answered True or both answered False
            if (horse.answers[i] == playerAnswers[i])
            {
                agreementCount++;
            }
        }

        matchPercentage = (float)agreementCount / totalQuestions;
        return matchPercentage >= threshold;
    }
}