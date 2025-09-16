using System;
using UnityEngine;

public class Diamante : MonoBehaviour, IInteractuable
{
    [SerializeField] private Animator animator;
    [SerializeField] private int cantidad = 1;
    [SerializeField] private string tagObjetivo = "Player";
    [SerializeField] private VidaJugador jugador; // opcional: arrástralo por Inspector

    public static Action DiamanteRecolectado;
    private bool sePuedeUsar = true;

    private void Awake()
    {
        if (jugador == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag(tagObjetivo);
            if (go) jugador = go.GetComponent<VidaJugador>();
        }
    }

    public void Interactuar()
    {
        Recolectar();
    }

    private void Recolectar()
    {
        if (!sePuedeUsar) return;
        sePuedeUsar = false;

        // Actualizar contador del jugador
        if (jugador != null)
        {
            jugador.AgregarDiamantes(Mathf.Max(1, cantidad));

            // Bridge opcional hacia GameManager si lo usas en el HUD
            if (GameManager.instance != null)
            {
                GameManager.instance.totalDiamonds = jugador.GetDiamantes();
                GameManager.OnGameDataChanged?.Invoke();
            }
        }
        else
        {
            Debug.LogWarning("[Diamante] No se encontró VidaJugador. ¿Tag 'Player' asignado?");
        }

        // Evento legacy (por si alguien lo usa)
        DiamanteRecolectado?.Invoke();

        // Animación y destrucción (usa Animation Event para llamar DestruirObjeto)
        if (animator != null) animator.SetTrigger("Recoger");
        else DestruirObjeto();
    }

    public void DestruirObjeto()
    {
        Destroy(gameObject);
    }
}
