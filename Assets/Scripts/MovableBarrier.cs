using UnityEngine;

public class MovableBarrier : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Velocidad del movimiento
    [SerializeField] private float moveDistance = 15f; // Distancia total del movimiento

    private float startX; // Posición inicial en X

    void Start()
    {
        startX = transform.position.x; // Guardar la posición inicial
    }

    void FixedUpdate()
    {
        float newX = startX + Mathf.PingPong(Time.time * speed, moveDistance); // Calcular la nueva posición en X con un movimiento de ida y vuelta (PingPong)
        transform.position = new Vector3(newX, transform.position.y, transform.position.z); // Asignar la nueva posición
    }
}
