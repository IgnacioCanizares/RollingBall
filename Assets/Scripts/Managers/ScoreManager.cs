using UnityEngine;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Singleton

    [Header("Scores")]
    [SerializeField] private int coinsCollected = 0;
    [SerializeField] private float timeScore = 0;
    [SerializeField] private int coinValue = 100;
    [SerializeField] private float maxTimerScore = 1000;

    private float timer = 0; // Variable para el tiempo
    private bool activateTimer = false; // Variable para activar el tiempo

    private int totalScore = 0; // Variable para el total de puntos

    public event Action OnScoreChange; // Evento que se dispara cuando cambia el score
    public event Action OnTimerChange; // Evento que se dispara cuando cambia el tiempo

    [SerializeField] private bool hasCheckpoint = false; // Variable para saber si se ha llegado a un checkpoint

    public bool getHasCheckpoint() // Getter para saber si se ha llegado a un checkpoint
    {
        return hasCheckpoint;
    }

    public void setHasCheckpoint(bool value) // Setter para saber si se ha llegado a un checkpoint
    {
        hasCheckpoint = value;
    }
    void Awake() // Singleton
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);

        }
    }
    void Update()
    {
        if(activateTimer) // Si el tiempo está activado
        {
            timer += Time.deltaTime; // Aumentamos el tiempo
            OnTimerChange?.Invoke(); // Disparamos el evento de cambio de tiempo
        }
    }

    public void AddCoins(int amount) // Añadir monedas
    {
        coinsCollected += amount; // Añadimos la cantidad de monedas
        OnScoreChange?.Invoke(); // Disparamos el evento de cambio de score
    }

    public int getCoinsCollected() // Getter para las monedas
    {
        return coinsCollected;
    }

    public float getTimer() // Getter para el tiempo
    {
        return timer;
    }

    public void ResetScores() // Reseteamos los scores
    {
        coinsCollected = 0; // Reseteamos las monedas 
        Debug.Log("Reset Scores"); // Debug
        if(!hasCheckpoint){ // Si no hay checkpoint
            Debug.Log("No checkpoint"); 
            timer = 0; // Reseteamos el tiempo
            SetTimer(false);
            OnTimerChange?.Invoke();  // Disparamos el evento de cambio de tiempo
        }
        OnScoreChange?.Invoke(); // Disparamos el evento de cambio de score
        
    }

    public void SetTimer(bool value) // Setter para el tiempo
    {
        activateTimer = value;
    }

    public void CalculateTotalScore()
    {
        totalScore = coinsCollected * coinValue + (int)(maxTimerScore - timer*10); // Calculamos el total de puntos
        Debug.Log("Total Score: " + totalScore); 
    }

    public int getTotalScore()
    {
        return totalScore; // Getter para el total de puntos
    } 
}
