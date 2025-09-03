using UnityEngine;
using UnityEngine.SceneManagement; // Agregamos esta l�nea para poder usar SceneManager

public class ExitDoor : MonoBehaviour
{
    // Asigna el nombre de la siguiente escena desde el Inspector
    [SerializeField] private string nextSceneName;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Revisa si el objeto que colision� es el jugador
        // Aseg�rate de que tu jugador tenga la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            Debug.Log("Cambiando de escena a: " + nextSceneName);

            // Verifica que el nombre de la escena no est� vac�o
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                // Carga la siguiente escena directamente
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}