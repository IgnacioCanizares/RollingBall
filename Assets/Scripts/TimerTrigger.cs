using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerTrigger : MonoBehaviour
{



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(gameObject.tag == "StartTrigger"){
            Debug.Log("Start Trigger");
            ScoreManager.instance.SetTimer(true);
        }
        else if(gameObject.tag == "EndTrigger"){
            Debug.Log("End Trigger");
            ScoreManager.instance.SetTimer(false);
            ScoreManager.instance.CalculateTotalScore();
            GameManager.instance.EndLevel(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 0;
        }
    }
}
