using UnityEngine;
using TMPro;

public class SistemaDiamantes : MonoBehaviour
{
    // Ya no necesitamos la cantidad de diamantes como una variable local.
    // El GameManager se encarga de eso.
    [SerializeField] private TextMeshProUGUI textoCantidadDiamantes;

    void OnEnable()
    {
        // El script se suscribe al evento global del GameManager
        // Este evento se invoca cuando los datos del juego cambian
        if (GameManager.instance != null)
        {
            GameManager.OnGameDataChanged.AddListener(UpdateDiamonds);
            UpdateDiamonds(); // Llama una vez para inicializar el texto
        }
        else
        {
            Debug.LogWarning("SistemaDiamantes: GameManager no encontrado. No se puede escuchar el evento.");
        }
    }

    void OnDisable()
    {
        // Se desuscribe del evento para evitar errores
        if (GameManager.instance != null)
        {
            GameManager.OnGameDataChanged.RemoveListener(UpdateDiamonds);
        }
    }

    // Este método ya no suma diamantes. Solo lee y actualiza la UI.
    private void UpdateDiamonds()
    {
        if (GameManager.instance == null) return;
        
        int cantidadDiamantes = GameManager.instance.totalDiamonds;
        
        textoCantidadDiamantes.text = cantidadDiamantes.ToString("D2");
    }
}