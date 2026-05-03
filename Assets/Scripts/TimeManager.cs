using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth PlayerHealthScript;
    [SerializeField] private ScoreManager ScoreManagerScript;

    [field: SerializeField] public float timeLeft { get; private set; } = 60f;
    private bool timerActive = true;
    public Action<float> OnTimerChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");

        if(playerObj != null)
        PlayerHealthScript = playerObj.GetComponent<PlayerHealth>();


    }

    // Update is called once per frame
    //CountDownTimer
    void Update()
    {
        if (timerActive)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                OnTimerChanged?.Invoke(timeLeft);
            }
            else
            {
                timeLeft = 0;
                timerActive = false;
                PlayerHealthScript.TakeDamage(100);
                //or GameOver can be handled by an event in a GameManager Script
            }
        }
    }

    //When stage is completed, time is added to remaining time in timer.
    public void CompletedStage(float time)
    {
        //Call CompletedStage when stage is completed through a trigger or some other way to detect stage was completed (When player is the next stage)
        timerActive = false;
        timeLeft += time;
        OnTimerChanged?.Invoke(timeLeft);
        ScoreManagerScript.AddTimeScore(timeLeft); //Wait until Level Completion?
        
    }

    //Can be called when Loading, changing scenes
    public void StopTimer()
    {
        timerActive = false;
    }

    //Can be called to activate timer when done
    public void StartTimer()
    {
        timerActive = true;
    }

   
}
