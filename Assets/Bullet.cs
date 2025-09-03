using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Puedes personalizar esto según si quieres dañar al jugador
        if (other.CompareTag("Player"))
        {
            // Dañar jugador aquí
            Destroy(gameObject);
        }
    }
}