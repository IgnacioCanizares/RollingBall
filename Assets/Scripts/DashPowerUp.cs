using UnityEngine;

public class DashPowerUp : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().AddDash();
            Destroy(gameObject);
        }
    }
}
