using System;
using UnityEngine;

public class VidaJugador : MonoBehaviour
{
<<<<<<< HEAD
    // Eventos legacy (compatibilidad)
    public Action<int> JugadorTomoDaño;   // vidaActual tras daño
    public Action<int> JugadorSeCuro;     // vidaActual tras curación

    // Eventos para HUD
    public event Action<int, int> OnVidaCambiada;   // (vida, max)
    public event Action<int> OnDiamantesCambiados; // total
=======
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
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f

    private const string PREF_VIDA = "Vida";
    private const string PREF_DIAMANTES = "Diamantes";

    [Header("Vida")]
    [SerializeField] private int vidaMaxima = 5;
    [SerializeField] private int vidaActual = 5;

    [Header("Coleccionables")]
    [SerializeField] private int diamantes = 0;
<<<<<<< HEAD

    private void Awake()
    {
        vidaMaxima = Mathf.Max(1, vidaMaxima);
        vidaActual = Mathf.Clamp(PlayerPrefs.GetInt(PREF_VIDA, vidaMaxima), 0, vidaMaxima);
        diamantes = Mathf.Max(0, PlayerPrefs.GetInt(PREF_DIAMANTES, 0));
=======
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
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f
    }

    private void Start()
    {
<<<<<<< HEAD
        // Estado inicial para el HUD
=======
<<<<<<< HEAD
        // Emitir estado inicial para que el HUD se refresque al entrar a la escena
=======
>>>>>>> d279adf (Agregada escena y carpeta Sounds)
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f
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
=======
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
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f

    public void AgregarDiamantes(int cantidad)
    {
        if (cantidad <= 0) return;
<<<<<<< HEAD
        diamantes += cantidad;
        OnDiamantesCambiados?.Invoke(diamantes);
        PlayerPrefs.SetInt(PREF_DIAMANTES, diamantes);
=======

        diamantes += cantidad;

        OnDiamantesCambiados?.Invoke(diamantes);
        PlayerPrefs.SetInt(PREF_DIAMANTES, diamantes);
<<<<<<< HEAD
=======

        Debug.Log("💎 Diamantes obtenidos: " + cantidad);
>>>>>>> d279adf (Agregada escena y carpeta Sounds)
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f
    }

    private void DestruirJugador()
    {
<<<<<<< HEAD
        // HUD ya mostró 0 en OnVidaCambiada anterior; aquí NO emitimos nuevo evento de vida.
        // Resetear diamantes a 0 (HUD se actualiza a 0 inmediatamente).
        diamantes = 0;
        OnDiamantesCambiados?.Invoke(0);
        PlayerPrefs.SetInt(PREF_DIAMANTES, 0);

        // Guardar para el próximo respawn: Vida = 3 (sin notificar al HUD ahora).
        PlayerPrefs.SetInt(PREF_VIDA, 3);
        PlayerPrefs.Save();

=======
<<<<<<< HEAD
        // (Opcional) persistir aquí también, por si reinicias escena y quieres mantener estado
        PlayerPrefs.SetInt(PREF_VIDA, vidaActual);
        PlayerPrefs.SetInt(PREF_DIAMANTES, diamantes);
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f
        Destroy(gameObject);
    }

    // Getters
<<<<<<< HEAD
=======
=======
        PlayerPrefs.SetInt(PREF_VIDA, vidaActual);
        PlayerPrefs.SetInt(PREF_DIAMANTES, diamantes);

        Debug.Log("🔊 Reproduciendo sonido de muerte");
        soundController?.playMuerte();
        Destroy(gameObject);
    }

>>>>>>> d279adf (Agregada escena y carpeta Sounds)
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f
    public int GetVidaMaxima() => vidaMaxima;
    public int GetVidaActual() => vidaActual;
    public bool EstaAlMaximo() => vidaActual >= vidaMaxima;
    public int GetDiamantes() => diamantes;
}
