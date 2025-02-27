using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Scores")]
    [SerializeField] private int coinsCollected = 0;
    [SerializeField] private float timeScore = 0;
    [SerializeField] private int coinValue = 100;
    [SerializeField] private float maxTimerScore = 1000;

    private float timer = 0;
    private bool activateTimer = false;

    private int totalScore = 0;

    public event Action OnScoreChange; // Evento que se dispara cuando cambia el score
    public event Action OnTimerChange; // Evento que se dispara cuando cambia el tiempo


    void Awake()
    {
        if (instance == null) // Para hacer un singleton que se comparta entre escenas
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(activateTimer)
        {
            timer += Time.deltaTime;
            OnTimerChange?.Invoke();
        }
    }

    public void AddCoins(int amount)
    {
        coinsCollected += amount;
        OnScoreChange?.Invoke();
    }

    public int getCoinsCollected()
    {
        return coinsCollected;
    }

    public float getTimer()
    {
        return timer;
    }

    public void ResetScores()
    {
        coinsCollected = 0;
        timer = 0;
        SetTimer(false);
        OnScoreChange?.Invoke();
        OnTimerChange?.Invoke();
    }

    public void SetTimer(bool value)
    {
        activateTimer = value;
    }

    public void CalculateTotalScore()
    {
        totalScore = coinsCollected * coinValue + (int)(maxTimerScore - timer*10);
        Debug.Log("Total Score: " + totalScore);
    }

    public int getTotalScore()
    {
        return totalScore;
    }
}
