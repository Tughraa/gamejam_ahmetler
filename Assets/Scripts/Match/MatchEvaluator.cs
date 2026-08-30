using UnityEngine;

public static class MatchEvaluator
{
    private const float MIN_MATCH_PERCENT = 0.6f;

    public static bool EvaluateMatch(HorseData targetHorse, HorseData playerProfile, out float matchPercent)
    {
        matchPercent = 0f;
        return true;

        // 1. Strict Gate: Is this horse matchable?
        if (targetHorse == null || !targetHorse.matchable)
        {
            return false;
        }

        // 2. If either profile lacks answer data, treat as an instant match
        if (playerProfile == null || playerProfile.answers == null ||
            targetHorse.answers == null || targetHorse.answers.Length == 0)
        {
            matchPercent = 1.0f;
            return true;
        }

        // 3. Compare question answers
        int totalQuestions = Mathf.Min(playerProfile.answers.Length, targetHorse.answers.Length);
        if (totalQuestions == 0) return true;

        int sharedAnswers = 0;
        for (int i = 0; i < totalQuestions; i++)
        {
            if (playerProfile.answers[i] == targetHorse.answers[i])
            {
                sharedAnswers++;
            }
        }

        matchPercent = (float)sharedAnswers / totalQuestions;
        return matchPercent >= MIN_MATCH_PERCENT;
    }
}