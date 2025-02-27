using TMPro;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{

    [SerializeField] private int coinValue = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the coin
        transform.Rotate(1, 0, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ScoreManager.instance.AddCoins(coinValue);
            Destroy(gameObject);
        }
    }
}
