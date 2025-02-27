using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndLevel(int currentLevel)
    {
        // Disable current ui and enable the endlevel ui
        UIManager.instance.DisableCurrentUI();
        UIManager.instance.EnableEndLevelUI();
        // Update the end level ui
        UIManager.instance.UpdateEndLevelUI();
        
    }

    public void LoadNextLevel(int currentLevel)
    {
        if(currentLevel < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(currentLevel + 1);
        }
        else
        {
            Debug.Log("Se acabó el juego");
        }
    }
}
