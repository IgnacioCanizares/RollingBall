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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScoreManager.instance.OnScoreChange += UpdateCoinText;
        ScoreManager.instance.OnTimerChange += UpdateTimerText;
        
    }

  
    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateCoinText()
    {
        // Update the coin text
        coinText.text = "Coins: " + ScoreManager.instance.getCoinsCollected().ToString();
    }
    private void UpdateTimerText()
    {
        // Update the timer text
        timerText.text = "Time: " + ScoreManager.instance.getTimer().ToString("F2");
    }

    public void DisableCurrentUI()
    {
        // Disable the current UI, not the ui components. All the ui
        currentUI.SetActive(false);
    }

    public void EnableEndLevelUI()
    {
        // Enable the end level UI
        endLevelUI.SetActive(true);
    }

    public void UpdateEndLevelUI()
    {
        // Update the end level UI
        coinScoreText.text = "Coins: " + ScoreManager.instance.getCoinsCollected().ToString();
        timerScoreText.text = "Time: " + ScoreManager.instance.getTimer().ToString("F2");
        totalScoreText.text = "Total Score: " + ScoreManager.instance.getTotalScore().ToString();
    }



}
