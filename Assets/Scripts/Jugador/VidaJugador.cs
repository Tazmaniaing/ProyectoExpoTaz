using System;
using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    // Eventos legacy (mantengo para compatibilidad con tu código)
    public Action<int> JugadorTomoDaño;   // pasa vidaActual tras daño
    public Action<int> JugadorSeCuro;     // pasa vidaActual tras curación

<<<<<<< HEAD
    // Eventos nuevos, cómodos para el HUD
    public event Action<int, int> OnVidaCambiada;   // (vidaActual, vidaMaxima)
    public event Action<int> OnDiamantesCambiados; // total de diamantes
=======
    public event Action<int, int> OnVidaCambiada;
    public event Action<int> OnDiamantesCambiados;
>>>>>>> d279adf (Agregada escena y carpeta Sounds)

    private const string PREF_VIDA = "Vida";
    private const string PREF_DIAMANTES = "Diamantes";

    [Header("Vida")]
    [SerializeField] private int vidaMaxima = 5;
    [SerializeField] private int vidaActual = 5;

    [Header("Coleccionables")]
    [SerializeField] private int diamantes = 0;
<<<<<<< HEAD
=======

    [Header("Referencias")]
    [SerializeField] private PlayerSoundController soundController; // 🔊 Nuevo
>>>>>>> d279adf (Agregada escena y carpeta Sounds)

    private void Awake()
    {
        vidaActual = Mathf.Clamp(PlayerPrefs.GetInt(PREF_VIDA, vidaMaxima), 0, vidaMaxima);
        diamantes = Mathf.Max(0, PlayerPrefs.GetInt(PREF_DIAMANTES, 0));

<<<<<<< HEAD
        if(vidaActual == 0)
        {
            vidaActual = vidaMaxima;
        }
=======
        if (vidaActual == 0)
        {
            vidaActual = vidaMaxima;
        }

        if (!soundController) soundController = GetComponent<PlayerSoundController>(); // 🔊 auto-asignación
>>>>>>> d279adf (Agregada escena y carpeta Sounds)
    }

    private void Start()
    {
<<<<<<< HEAD
        // Emitir estado inicial para que el HUD se refresque al entrar a la escena
=======
>>>>>>> d279adf (Agregada escena y carpeta Sounds)
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
<<<<<<< HEAD
            // Notificar
            JugadorTomoDaño?.Invoke(vidaActual);
            OnVidaCambiada?.Invoke(vidaActual, vidaMaxima);

            // Guardar
=======
            JugadorTomoDaño?.Invoke(vidaActual);
            OnVidaCambiada?.Invoke(vidaActual, vidaMaxima);

            Debug.Log("🔊 Reproduciendo sonido de recibir daño");
            soundController?.playRecibirDaño();

>>>>>>> d279adf (Agregada escena y carpeta Sounds)
            PlayerPrefs.SetInt(PREF_VIDA, vidaActual);
        }

        if (vidaActual <= 0)
            DestruirJugador();
    }

<<<<<<< HEAD
    // Método nuevo que devuelve cuánto se curó realmente (útil para pickups)
=======
>>>>>>> d279adf (Agregada escena y carpeta Sounds)
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
<<<<<<< HEAD
=======

            Debug.Log("✅ Jugador curado");
>>>>>>> d279adf (Agregada escena y carpeta Sounds)
        }
        return curado;
    }

<<<<<<< HEAD
    // Conserva tu firma original para compatibilidad; internamente usa Curar(...)
    public void CurarVida(int curacion)
    {
        Curar(curacion);
    }
=======
    public void CurarVida(int curacion) => Curar(curacion);
>>>>>>> d279adf (Agregada escena y carpeta Sounds)

    public void AgregarDiamantes(int cantidad)
    {
        if (cantidad <= 0) return;

        diamantes += cantidad;

        OnDiamantesCambiados?.Invoke(diamantes);
        PlayerPrefs.SetInt(PREF_DIAMANTES, diamantes);
<<<<<<< HEAD
=======

        Debug.Log("💎 Diamantes obtenidos: " + cantidad);
>>>>>>> d279adf (Agregada escena y carpeta Sounds)
    }

    private void DestruirJugador()
    {
<<<<<<< HEAD
        // (Opcional) persistir aquí también, por si reinicias escena y quieres mantener estado
        PlayerPrefs.SetInt(PREF_VIDA, vidaActual);
        PlayerPrefs.SetInt(PREF_DIAMANTES, diamantes);
        Destroy(gameObject);
    }

    // Getters
=======
        PlayerPrefs.SetInt(PREF_VIDA, vidaActual);
        PlayerPrefs.SetInt(PREF_DIAMANTES, diamantes);

        Debug.Log("🔊 Reproduciendo sonido de muerte");
        soundController?.playMuerte();
        Destroy(gameObject);
    }

>>>>>>> d279adf (Agregada escena y carpeta Sounds)
    public int GetVidaMaxima() => vidaMaxima;
    public int GetVidaActual() => vidaActual;
    public bool EstaAlMaximo() => vidaActual >= vidaMaxima;
    public int GetDiamantes() => diamantes;
}
