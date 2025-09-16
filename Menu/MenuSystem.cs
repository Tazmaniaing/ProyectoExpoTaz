using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] private string nombreEscena; // Cambia el nombre de la escena desde el Inspector

    // M�todo gen�rico para cargar la escena
    public void CargarEscena()
    {
        SceneManager.LoadScene(nombreEscena);
    }

    // M�todo para salir del juego
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}



