using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int timeMultiplier = 10;
    public int currentScore { get; private set; }
    [field: SerializeField] public int startingScore { get; private set; } = 0;

    public Action<int> OnScoreChanged;

    private void OnEnable()
    {
        HitscanDetector.OnEnemyHit += AddEnemyScore;
        Pickup_Handler.OnScorePickup += AddItemScore;
    }

    private void OnDisable()
    {
        HitscanDetector.OnEnemyHit -= AddEnemyScore;
        Pickup_Handler.OnScorePickup -= AddItemScore;
    }

    public void AddTimeScore(float timeLeft)
    {
        int timerScore = Mathf.RoundToInt(timeLeft) * timeMultiplier;
        currentScore += timerScore;
        OnScoreChanged?.Invoke(currentScore);
    }

    public void AddEnemyScore(int enemyValue)
    {
        currentScore += enemyValue;
        OnScoreChanged?.Invoke(currentScore);
    }

    public void AddItemScore(int itemValue)
    {
        currentScore += itemValue;
        OnScoreChanged?.Invoke(currentScore);
    }

    public void SetScore(int value)
    {
        currentScore = value;
        OnScoreChanged?.Invoke(currentScore);
    }

    public void ResetScore()
    {
        currentScore = startingScore;
        OnScoreChanged?.Invoke(currentScore);
    }
}