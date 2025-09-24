using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    void Start()
    {
        // Buscar al jugador persistente
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            // Mover al jugador a la posici�n del spawn
            player.transform.position = transform.position;
        }
    }
}
