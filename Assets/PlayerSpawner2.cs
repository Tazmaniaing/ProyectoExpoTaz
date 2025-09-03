using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        // Busca al jugador por su etiqueta
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Si se encontró un objeto con la etiqueta "Player"
        if (player != null)
        {
            // Mueve la posición del jugador a la posición del spawner
            player.transform.position = transform.position;
        }
    }
}