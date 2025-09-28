using System;
using UnityEngine;

public class Diamante : MonoBehaviour, IInteractuable
{
    [SerializeField] private Animator animator;
    [SerializeField] private int cantidad = 1;
    [SerializeField] private string tagObjetivo = "Player";
    [SerializeField] private VidaJugador jugador; // opcional por Inspector

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

        if (jugador != null)
            jugador.AgregarDiamantes(Mathf.Max(1, cantidad));
        else
            Debug.LogWarning("[Diamante] VidaJugador no encontrado. ¿Tag 'Player'?");

        DiamanteRecolectado?.Invoke();

        if (animator != null) animator.SetTrigger("Recoger");
        else DestruirObjeto();
    }

    // Llamar desde Animation Event al final de "Recoger"
    public void DestruirObjeto() => Destroy(gameObject);
}
