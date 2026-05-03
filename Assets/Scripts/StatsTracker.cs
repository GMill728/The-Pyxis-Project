using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatsTracker : MonoBehaviour
{
    public static StatsTracker Instance;
    private ScoreManager scoreManager;
    private PuzzleManager puzzleManager;

    public int score { get; private set; } = 0;
    private int stagesCleared = 0;

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

            if (scoreManager == null)
                Debug.LogError("ScoreManager component missing!");
        }
        else
        {
            Debug.LogError("GameManager object not found!");
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
            score = scoreManager.currentScore;

        stagesCleared++;
        Debug.Log("Stages Cleared: " + stagesCleared);
    }

    public int GetTotalScore()
    {
        return scoreManager != null ? scoreManager.currentScore : score;
    }
}