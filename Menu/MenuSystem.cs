using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] private string nombreEscena; // Cambia el nombre de la escena desde el Inspector

    // Método genérico para cargar la escena
    public void CargarEscena()
    {
        SceneManager.LoadScene(nombreEscena);
    }

    // Método para salir del juego
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}



