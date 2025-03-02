using TMPro;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField] private bool isCollected = false; // No implementado al final, pero se deja para futuras mejoras
    [SerializeField] private int coinValue = 1; // Valor de la moneda inicial, se puede cambiar en el inspector para distintas monedas
    void Awake()
    {
        if (isCollected)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.Rotate(1, 0, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) // Si el jugador toca la moneda
        {
            ScoreManager.instance.AddCoins(coinValue); // Añadimos el valor de la moneda al score
            isCollected = true; // No implementado al final, pero se deja para futuras mejoras
            Destroy(gameObject); // Destruimos la moneda
        }
    }
}
