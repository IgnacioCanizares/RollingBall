using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI coinScoreText;
    [SerializeField] private TextMeshProUGUI timerScoreText;
    [SerializeField] private TextMeshProUGUI totalScoreText;

    public static UIManager instance;
    [SerializeField] private GameObject endLevelUI;
    [SerializeField] private GameObject currentUI;


    public void Awake()
    {
        if (instance == null) // Para hacer un singleton que se comparta entre escenas
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    void Start()
    {
        ScoreManager.instance.OnScoreChange += UpdateCoinText; // Nos subscribimos al evento de cambio de score
        ScoreManager.instance.OnTimerChange += UpdateTimerText; // Nos subscribimos al evento de cambio de tiempo
    }

    void UpdateCoinText()
    {
        coinText.text = "Coins: " + ScoreManager.instance.getCoinsCollected().ToString(); // Actualizamos el texto de las monedas
    }
    private void UpdateTimerText()
    {
        timerText.text = "Time: " + ScoreManager.instance.getTimer().ToString("F2"); // Actualizamos el texto del tiempo
    }

    public void DisableCurrentUI()
    {
        currentUI.SetActive(false); // Desactivamos la UI actual
    }

    public void EnableEndLevelUI()
    {
        endLevelUI.SetActive(true); // Activamos la UI de fin de nivel
    }

    public void UpdateEndLevelUI()
    {
        coinScoreText.text = "Coins: " + ScoreManager.instance.getCoinsCollected().ToString(); // Actualizamos el texto de las monedas
        timerScoreText.text = "Time: " + ScoreManager.instance.getTimer().ToString("F2"); // Actualizamos el texto del tiempo
        totalScoreText.text = "Total Score: " + ScoreManager.instance.getTotalScore().ToString(); // Actualizamos el texto del total de puntos
    }

}
