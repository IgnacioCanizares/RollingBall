using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Para crear un singleton
    private Vector3 lastCheckpoint; // Para guardar la última posición de un checkpoint
 
    void Awake() // Singleton
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

    public void SaveCheckpoint(Vector3 checkpointPosition) // Guardamos la posicion del checkpoint
    {
        lastCheckpoint = checkpointPosition;
    }

    public Vector3 GetLastCheckpoint()
    { 
        return lastCheckpoint; // Devolvemos la última posición del checkpoint
    }

    public void ReloadLevel() // Recargar el nivel
    {
        if (!ScoreManager.instance.getHasCheckpoint()) // Si no hay checkpoint
        {
            ScoreManager.instance.SetTimer(false); // Paramos el tiempo
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recargamos la escena
    }

    public void FullyReloadLevel() // Recargar el nivel completamente
    {
        lastCheckpoint = Vector3.zero; // Reseteamos el checkpoint
        ScoreManager.instance.SetTimer(false); // Paramos el tiempo
        ScoreManager.instance.setHasCheckpoint(false); // No hay checkpoint
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recargamos la escena
    }

    public void EndLevel() // Finalizar el nivel
    {
        UIManager.instance.DisableCurrentUI(); // Desactivamos la UI actual
        UIManager.instance.EnableEndLevelUI(); // Activamos la UI de fin de nivel
        UIManager.instance.UpdateEndLevelUI(); // Actualizamos la UI de fin de nivel
    }

    public void LoadNextLevel(int currentLevel) // Cargar el siguiente nivel
    {
        Debug.Log("Loading next level");
        if (currentLevel < SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.Log("Loading next level" + currentLevel + 1);
            SceneManager.LoadScene(currentLevel + 1); // Cargamos el siguiente nivel
        }
        else
        {
            Debug.Log("Se acabó el juego"); // Si no hay más niveles
        }
    }
}
