using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    void Start()
    {
        // Busca al jugador por su etiqueta
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Si se encontr� un objeto con la etiqueta "Player"
        if (player != null)
        {
            // Mueve la posici�n del jugador a la posici�n del spawner
            player.transform.position = transform.position;
        }
    }
}