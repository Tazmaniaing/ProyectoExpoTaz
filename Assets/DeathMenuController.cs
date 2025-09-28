using UnityEngine;
using UnityEngine.UI;

public class DeathMenuController : MonoBehaviour
{
    public Button restartButton;
    public Button mainMenuButton;

    void Start()
    {
        // Asegúrate de que el GameManager.instance no sea nulo.
        if (GameManager.instance != null)
        {
            // Asigna la función de reinicio del GameManager al botón de reinicio.
            if (restartButton != null)
            {
                restartButton.onClick.AddListener(GameManager.instance.RestartLastScene);
            }

            // Asigna la función de ir al menú principal del GameManager al botón correspondiente.
            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.AddListener(GameManager.instance.GoToMenu);
            }
        }
        else
        {
            Debug.LogError("GameManager.instance es nulo. Asegúrate de que tu GameManager se cargue antes que el menú de muerte.");
        }
    }
}