using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerTrigger : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1; // Asegurarse de que el tiempo no esté pausado
    }
    void OnTriggerEnter(Collider other)
    {
        if(gameObject.tag == "StartTrigger"){ // Si el trigger es de inicio
            Debug.Log("Start Trigger"); 
            ScoreManager.instance.SetTimer(true); // Activamos el tiempo
        }
        else if(gameObject.tag == "EndTrigger"){ // Si el trigger es de fin
            Debug.Log("End Trigger");
            ScoreManager.instance.SetTimer(false); // Paramos el tiempo
            ScoreManager.instance.CalculateTotalScore(); // Calculamos el total de puntos 
            GameManager.instance.EndLevel(); // Finalizamos el nivel
            Time.timeScale = 0; // Pausamos el tiempo
        }
    }
}
