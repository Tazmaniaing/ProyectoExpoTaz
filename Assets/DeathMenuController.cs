using UnityEngine;
using UnityEngine.UI;

public class DeathMenuController : MonoBehaviour
{
    public Button restartButton;
    public Button mainMenuButton;

    void Start()
    {
        // Aseg�rate de que el GameManager.instance no sea nulo.
        if (GameManager.instance != null)
        {
            // Asigna la funci�n de reinicio del GameManager al bot�n de reinicio.
            if (restartButton != null)
            {
                restartButton.onClick.AddListener(GameManager.instance.RestartLastScene);
            }

            // Asigna la funci�n de ir al men� principal del GameManager al bot�n correspondiente.
            if (mainMenuButton != null)
            {
                mainMenuButton.onClick.AddListener(GameManager.instance.GoToMenu);
            }
        }
        else
        {
            Debug.LogError("GameManager.instance es nulo. Aseg�rate de que tu GameManager se cargue antes que el men� de muerte.");
        }
    }
}