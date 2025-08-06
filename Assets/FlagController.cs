using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar escenas

public class FlagController : MonoBehaviour
{
    // Esta variable pública te permite especificar el nombre de la siguiente escena
    // directamente desde el Inspector de Unity.
    public string nextSceneName;

    // Esta función se llama automáticamente cuando otro objeto entra en el trigger de la bandera.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificamos si el objeto que entró tiene el tag "Player".
        // Es crucial que tu personaje tenga este tag asignado.
        if (other.CompareTag("Player"))
        {
            Debug.Log("¡El personaje llegó a la bandera! Cargando siguiente escena...");
            
            // Llamamos a la función para cargar la nueva escena.
            LoadNextScene();
        }
    }

    // Método que contiene la lógica para cargar la escena.
    private void LoadNextScene()
    {
        // Verificamos si el nombre de la escena ha sido especificado en el Inspector.
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("Error: El nombre de la siguiente escena no ha sido asignado en el Inspector del script FlagController.");
            return; // Detenemos la ejecución si no hay nombre.
        }

        // Cargamos la escena. Asegúrate de que esta escena esté en la lista de Build Settings (File > Build Settings).
        SceneManager.LoadScene(nextSceneName);
    }
}
