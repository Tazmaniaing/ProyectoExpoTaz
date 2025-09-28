using System;
using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    // Eventos legacy (compatibilidad)
    public Action<int> JugadorTomoDaño;   // vidaActual tras daño
    public Action<int> JugadorSeCuro;     // vidaActual tras curación

    // Eventos para HUD
    public event Action<int, int> OnVidaCambiada;   // (vida, max)
    public event Action<int> OnDiamantesCambiados; // total

    private const string PREF_VIDA = "Vida";
    private const string PREF_DIAMANTES = "Diamantes";

    [Header("Vida")]
    [SerializeField] private int vidaMaxima = 5;
    [SerializeField] private int vidaActual = 5;

    [Header("Coleccionables")]
    [SerializeField] private int diamantes = 0;

    private void Awake()
    {
        vidaMaxima = Mathf.Max(1, vidaMaxima);
        vidaActual = Mathf.Clamp(PlayerPrefs.GetInt(PREF_VIDA, vidaMaxima), 0, vidaMaxima);
        diamantes = Mathf.Max(0, PlayerPrefs.GetInt(PREF_DIAMANTES, 0));
    }

    private void Start()
    {
        // Estado inicial para el HUD
        JugadorSeCuro?.Invoke(vidaActual);
        OnVidaCambiada?.Invoke(vidaActual, vidaMaxima);
        OnDiamantesCambiados?.Invoke(diamantes);
    }

    public void TomarDaño(int daño)
    {
        if (daño <= 0) return;

        int antes = vidaActual;
        vidaActual = Mathf.Clamp(vidaActual - daño, 0, vidaMaxima);

        if (vidaActual != antes)
        {
            // HUD ve el cambio (incluido cuando llega a 0)
            JugadorTomoDaño?.Invoke(vidaActual);
            OnVidaCambiada?.Invoke(vidaActual, vidaMaxima);
            PlayerPrefs.SetInt(PREF_VIDA, vidaActual); // se sobrescribe luego en muerte
        }

        if (vidaActual <= 0) DestruirJugador();
    }

    public int Curar(int cantidad)
    {
        if (cantidad <= 0) return 0;

        int antes = vidaActual;
        vidaActual = Mathf.Clamp(vidaActual + cantidad, 0, vidaMaxima);
        int curado = vidaActual - antes;

        if (curado != 0)
        {
            JugadorSeCuro?.Invoke(vidaActual);
            OnVidaCambiada?.Invoke(vidaActual, vidaMaxima);
            PlayerPrefs.SetInt(PREF_VIDA, vidaActual);
        }
        return curado;
    }

    // Compatibilidad con tu API anterior
    public void CurarVida(int curacion) => Curar(curacion);

    public void AgregarDiamantes(int cantidad)
    {
        if (cantidad <= 0) return;
        diamantes += cantidad;
        OnDiamantesCambiados?.Invoke(diamantes);
        PlayerPrefs.SetInt(PREF_DIAMANTES, diamantes);
    }

    private void DestruirJugador()
    {
        // HUD ya mostró 0 en OnVidaCambiada anterior; aquí NO emitimos nuevo evento de vida.
        // Resetear diamantes a 0 (HUD se actualiza a 0 inmediatamente).
        diamantes = 0;
        OnDiamantesCambiados?.Invoke(0);
        PlayerPrefs.SetInt(PREF_DIAMANTES, 0);

        // Guardar para el próximo respawn: Vida = 3 (sin notificar al HUD ahora).
        PlayerPrefs.SetInt(PREF_VIDA, 3);
        PlayerPrefs.Save();

        Destroy(gameObject);
    }

    // Getters
    public int GetVidaMaxima() => vidaMaxima;
    public int GetVidaActual() => vidaActual;
    public bool EstaAlMaximo() => vidaActual >= vidaMaxima;
    public int GetDiamantes() => diamantes;
}
