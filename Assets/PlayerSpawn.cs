using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Configuración del Spawner")]
    [Tooltip("Arrastra aquí el prefab de tu personaje 2D. Es la mejor práctica.")]
    [SerializeField] private GameObject playerPrefab;
    
    [Tooltip("Define si el personaje debe aparecer automáticamente al inicio de la escena.")]
    [SerializeField] private bool spawnOnStart = true;

    [Tooltip("Referencia al personaje instanciado en caso de que necesites interactuar con él.")]
    private GameObject instantiatedPlayer;

    private void Start()
    {
        if (spawnOnStart && playerPrefab != null)
        {
            SpawnPlayer();
        }
    }

    /// <summary>
    /// Instancia una nueva copia del prefab del jugador en la posición del spawner.
    /// </summary>
    public GameObject SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("Error: No se ha asignado un prefab de jugador al Spawner.", this);
            return null;
        }

        // Si ya hay un jugador instanciado por este spawner, lo destruimos antes de crear uno nuevo.
        if (instantiatedPlayer != null)
        {
            Destroy(instantiatedPlayer);
        }

        // Instancia el prefab en la posición del spawner.
        instantiatedPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);
        
        Debug.Log("Personaje 2D instanciado correctamente en: " + instantiatedPlayer.name + " en la posición: " + transform.position);

        return instantiatedPlayer;
    }
}