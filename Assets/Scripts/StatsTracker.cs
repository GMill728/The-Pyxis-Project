using UnityEngine;
using UnityEngine.SceneManagement;

public class StatsTracker : MonoBehaviour
{
    public static StatsTracker Instance;
    private ScoreManager scoreManager;
    private PuzzleManager puzzleManager;

    private int score = 0;
    private int stagesCleared = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            FindScoreManager();
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
        FindScoreManager();
        FindPuzzleManager();

        // player died
        if(SceneManager.GetActiveScene().name == "GameOver")
            ResetStats();

        if (puzzleManager != null && stagesCleared >= 3) 
            puzzleManager.isFinalStage = true;
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
    }

    public void UpdateStats()
    {
        if (scoreManager == null)
        {
            Debug.LogWarning("ScoreManager is null, trying to find it...");
            FindScoreManager();
        }

        if (scoreManager != null)
        {
            score += scoreManager.currentScore;
            Debug.Log("Total Score: " + score);

            stagesCleared++; 
            Debug.Log("Stages Cleared: " + stagesCleared);
        }
    }
}