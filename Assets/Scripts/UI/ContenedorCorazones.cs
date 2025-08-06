using System;
using UnityEngine;

public class ContenedorCorazones : MonoBehaviour
{
    [SerializeField] private CorazonUI[] corazones;

    void OnEnable()
    {
        // Verifica si el GameManager ya existe antes de suscribirse.
        if (GameManager.instance != null)
        {
            GameManager.OnGameDataChanged.AddListener(UpdateHearts);
        }
        else
        {
            Debug.LogWarning("ContenedorCorazones: GameManager no encontrado. No se puede escuchar el evento.");
        }
        
        // También actualiza los corazones al inicio, con un chequeo seguro.
        UpdateHearts();
    }

    void OnDisable()
    {
        // Verifica si el GameManager existe antes de desuscribirse.
        if (GameManager.instance != null)
        {
            GameManager.OnGameDataChanged.RemoveListener(UpdateHearts);
        }
    }

    private void UpdateHearts()
    {
        // --- ¡Esta es la línea clave que se ha mejorado! ---
        // Verifica si el GameManager existe antes de obtener la vida.
        if (GameManager.instance == null)
        {
            Debug.LogWarning("ContenedorCorazones: No se puede actualizar los corazones. GameManager.instance es nulo.");
            return;
        }

        int vidaActual = GameManager.instance.currentHealth;
        // -----------------------------------------------------

        for (int i = 0; i < corazones.Length; i++)
        {
            if (i < vidaActual)
            {
                if (corazones[i].EstaActivo()) { continue; }
                corazones[i].ActivarCorazon();
            }
            else
            {
                if (!corazones[i].EstaActivo()) { continue; }
                corazones[i].DesactivarCorazon();
            }
        }
    }
}