using System;
using UnityEngine;

public class Diamante : MonoBehaviour, IInteractuable
{
    [SerializeField] private Animator animator;
    [SerializeField] private int cantidad = 1;
    [SerializeField] private string tagObjetivo = "Player";
<<<<<<< HEAD
    [SerializeField] private VidaJugador jugador; // opcional por Inspector
=======
    [SerializeField] private VidaJugador jugador; // opcional: arrástralo por Inspector
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f

    public static Action DiamanteRecolectado;
    private bool sePuedeUsar = true;

    private void Awake()
<<<<<<< HEAD
=======
    {
        if (jugador == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag(tagObjetivo);
            if (go) jugador = go.GetComponent<VidaJugador>();
        }
    }

    public void Interactuar()
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f
    {
        if (jugador == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag(tagObjetivo);
            if (go) jugador = go.GetComponent<VidaJugador>();
        }
    }

    public void Interactuar() => Recolectar();

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si también quieres auto-pickup, deja este método.
        if (!other.CompareTag(tagObjetivo) && (other.transform.root == null || !other.transform.root.CompareTag(tagObjetivo)))
            return;
        Recolectar();
    }

    private void Recolectar()
    {
        if (!sePuedeUsar) return;
        sePuedeUsar = false;

<<<<<<< HEAD
        if (jugador != null)
            jugador.AgregarDiamantes(Mathf.Max(1, cantidad));
        else
            Debug.LogWarning("[Diamante] VidaJugador no encontrado. ¿Tag 'Player'?");

        DiamanteRecolectado?.Invoke();

=======
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
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f
        if (animator != null) animator.SetTrigger("Recoger");
        else DestruirObjeto();
    }

<<<<<<< HEAD
    // Llamar desde Animation Event al final de "Recoger"
    public void DestruirObjeto() => Destroy(gameObject);
=======
    public void DestruirObjeto()
    {
        Destroy(gameObject);
    }
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f
}
