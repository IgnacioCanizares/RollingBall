using UnityEngine;

public class DashPowerUp : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) // Si el jugador toca el power up
        {
            other.GetComponent<PlayerController>().AddDash(); // Añadimos el dash al jugador
            Destroy(gameObject); // Destruimos el power up
        }
    }
}
