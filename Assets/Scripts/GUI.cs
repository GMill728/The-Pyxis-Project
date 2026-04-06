using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class GUI : MonoBehaviour
{
    [SerializeField] private Animator GUIAnimator;
    private static readonly int GUITimerID = Animator.StringToHash("GUITimer");

    private float animPOS = 0f;
    private float currentAnimPOS = 0f;
    [SerializeField] private float smoothSpeed= 10f;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Gradient healthGrad;
    [SerializeField] private Image fill;

    [SerializeField] private Animator ObjAnimator;

    [SerializeField] private PlayerHealth PlayerHealthScript;
    public TMP_Text healthText;
    [SerializeField] private ScoreManager ScoreManagerScript;
    public TMP_Text scoreText;
    [SerializeField] private TimeManager TimeManagerScript;
    public TMP_Text timerText;
    public TMP_Text pickUpText;
    bool objPickedUp = false;




    void Start()
    {
        animPOS = TimeManagerScript.timeLeft;
        healthSlider.maxValue = PlayerHealthScript.maxHealth;
        healthSlider.value = PlayerHealthScript.currentHealth;
        fill.color = healthGrad.Evaluate(1f);

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
        UpdatePickUpMsg();

    }

    private void Update()
    {
        currentAnimPOS = Mathf.MoveTowards(currentAnimPOS, animPOS, Time.deltaTime * smoothSpeed);
        GUIAnimator.SetFloat(GUITimerID, Mathf.Clamp01(currentAnimPOS));
    }
    private void OnEnable()
    {
        Pickup_Handler.OnIntelPickup += IntelRetrieved;
    }

    private void OnDisable()
    {
        Pickup_Handler.OnIntelPickup -= IntelRetrieved;
    }


    private void UpdateHealthText(int healthValue)
    {
        healthSlider.value = healthValue;
        fill.color = healthGrad.Evaluate(healthSlider.normalizedValue);
        healthText.text = $"Health: {healthValue}";
    }

    private void UpdateScoreText(int scoreValue)
    {
        scoreText.text = $"{scoreValue}";
    }

    private void UpdateTimerText(float timerValue)
    {
        int minutes = Mathf.FloorToInt(timerValue / 60f);
        int seconds = Mathf.FloorToInt(timerValue % 60f);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timerValue >= 66f) 
        {
            animPOS = 0.0f;
        }
        else if (timerValue <= 65f && timerValue > 60f)
        {
            float t = Mathf.InverseLerp(65f, 60f, timerValue);
            animPOS = Mathf.Lerp(0.0f, 0.7f, t);
        }
        else if (timerValue <= 60f && timerValue > 33f)
        {
            animPOS = 0.7f;
        }
        else if (timerValue <= 33 && timerValue > 30f)
        {
            float t = Mathf.InverseLerp(33f, 30f, timerValue);
            animPOS = Mathf.Lerp(0.7f, 1.0f, t);
        }
        else if (timerValue <= 30f)
        {
            animPOS = 1.0f;
        }
    }

    private void IntelRetrieved()
    {
        objPickedUp = true;
        
        UpdatePickUpMsg();
    }
    private void UpdatePickUpMsg()
    {

        if (!objPickedUp)
            //pickUpText.text = $"OBJECTIVE: Collect Intel";
            ObjAnimator.SetBool("pickedUp", false) ;
        else
            //pickUpText.text = $"Completed";
            ObjAnimator.SetBool("pickedUp", true);
    }

    private void OnDestroy()
    {
        if ((PlayerHealthScript != null))
        {
           PlayerHealthScript.OnHealthChanged -= UpdateHealthText;
        }
        if ((ScoreManagerScript != null))
        {
            ScoreManagerScript.OnScoreChanged -= UpdateScoreText;
        }
        if ((TimeManagerScript != null))
        {
            TimeManagerScript.OnTimerChanged -= UpdateTimerText;
        }
    }
}
