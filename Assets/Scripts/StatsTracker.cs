using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatsTracker : MonoBehaviour
{
    public static StatsTracker Instance;
    private ScoreManager scoreManager;
    private PuzzleManager puzzleManager;

    public int score { get; private set; } = 0;
    private int stagesCleared = -1; // accounts for tutorial level

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnEnable()
    {
        InteractionHandler.onLevelComplete += UpdateStats;
    }

    void OnDisable()
    {
        InteractionHandler.onLevelComplete -= UpdateStats;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPuzzleManager();

        if (SceneManager.GetActiveScene().name == "GameOver")
            GameObject.FindGameObjectWithTag("Score Display").GetComponent<TextMeshProUGUI>().text = "Score: " + score;
            ResetStats();

        if (puzzleManager != null && stagesCleared >= 3)
            puzzleManager.isFinalStage = true;

        StartCoroutine(InitScoreManager());
    }

    private IEnumerator InitScoreManager()
    {
        yield return null; // wait one frame for scene objects to initialize

        FindScoreManager();

        if (scoreManager != null)
            scoreManager.SetScore(score);
    }

    private void FindScoreManager()
    {
        GameObject obj = GameObject.Find("GameManager");

        if (obj != null)
        {
            scoreManager = obj.GetComponent<ScoreManager>();
        }

    }

    private void FindPuzzleManager()
    {
        GameObject obj = GameObject.Find("Puzzle Board");

        if (obj != null)
        {
            puzzleManager = obj.GetComponent<PuzzleManager>();

            if (puzzleManager == null)
                Debug.LogError("PuzzleManager component missing!");
        }
    }

    private void ResetStats()
    {
        score = 0;
        stagesCleared = 0;

        if (scoreManager != null)
            scoreManager.ResetScore();
    }

    public void UpdateStats()
    {
        if (scoreManager != null)
            score = scoreManager.currentScore; // bank the full running total

        stagesCleared++;
        Debug.Log("Stages Cleared: " + stagesCleared);
    }

    public int GetTotalScore()
    {
        return scoreManager != null ? scoreManager.currentScore : score;
    }
}