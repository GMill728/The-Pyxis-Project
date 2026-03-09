using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class GUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth PlayerHealthScript;
    public TMP_Text healthText;
    [SerializeField] private ScoreManager ScoreManagerScript;
    public TMP_Text scoreText;
    [SerializeField] private TimeManager TimeManagerScript;
    public TMP_Text timerText;
    
    void Start()
    {
        if (PlayerHealthScript == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                PlayerHealthScript = playerObj.GetComponent<PlayerHealth>();
            }
        }

        PlayerHealthScript.OnHealthChanged += UpdateHealthText;
        ScoreManagerScript.OnScoreChanged += UpdateScoreText;
        TimeManagerScript.OnTimerChanged += UpdateTimerText;



        UpdateHealthText(PlayerHealthScript.currentHealth);
        UpdateScoreText(ScoreManagerScript.currentScore);

    }


    private void UpdateHealthText(int healthValue)
    {
        healthText.text = $"Health: {healthValue}";
    }

    private void UpdateScoreText(int scoreValue)
    {
        scoreText.text = $"Score: {scoreValue}";
    }

    private void UpdateTimerText(float timerValue)
    {
        timerText.text = $"{Mathf.CeilToInt(timerValue)}";
    }
}
