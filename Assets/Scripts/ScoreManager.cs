using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int timeMultiplier = 10;
    public int currentScore { get; private set; }
    [field: SerializeField] public int startingScore { get; private set; } = 0;

    public Action<int> OnScoreChanged;

    void Awake()
    {
        currentScore = startingScore;
    }

    private void OnEnable()
    {
        HitscanDetector.OnEnemyHit += AddEnemyScore;
    }

    private void OnDisable()
    {
        HitscanDetector.OnEnemyHit -= AddEnemyScore;
    }

    //When stage is completed, remaining time is mulitplied and added into score.
    // OR SHOULD IT BE CALLED WHEN LEVEL IS COMPLETED??
    public void AddTimeScore(float timeLeft)
    {
        int timerScore = Mathf.RoundToInt(timeLeft) * timeMultiplier;
        currentScore += timerScore;
        
        OnScoreChanged?.Invoke(currentScore);
    }

    //When enemies take Damage/Die call this function to add enemiesValue to score
    public void AddEnemyScore(int enemyValue)
    {
        currentScore += enemyValue;
        OnScoreChanged?.Invoke(currentScore);
    }


}
