using UnityEngine;

public class ContenedorCorazones : MonoBehaviour
{
    [SerializeField] private CorazonUI[] corazones;
    [SerializeField] private VidaJugador jugador;

    private void Awake()
    {
        if (jugador == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go) jugador = go.GetComponent<VidaJugador>();
        }
    }

    private void OnEnable()
    {
        if (jugador != null)
            jugador.OnVidaCambiada += OnVidaCambiada;

        if (GameManager.instance != null)
            GameManager.OnGameDataChanged.AddListener(ActualizarPorGameManager);

        ActualizarInicial();
    }

    private void OnDisable()
    {
        if (jugador != null)
            jugador.OnVidaCambiada -= OnVidaCambiada;

        if (GameManager.instance != null)
            GameManager.OnGameDataChanged.RemoveListener(ActualizarPorGameManager);
    }

    private void ActualizarInicial()
    {
        if (jugador != null)
            OnVidaCambiada(jugador.GetVidaActual(), jugador.GetVidaMaxima());
        else if (GameManager.instance != null)
            UpdateHearts(GameManager.instance.currentHealth);
    }

    private void OnVidaCambiada(int vida, int max)
    {
        UpdateHearts(vida);
    }

    private void ActualizarPorGameManager()
    {
        if (GameManager.instance == null) return;
        UpdateHearts(GameManager.instance.currentHealth);
    }

    private void UpdateHearts(int vidaActual)
    {
        if (corazones == null) return;

        for (int i = 0; i < corazones.Length; i++)
        {
            if (i < vidaActual)
            {
                if (!corazones[i].EstaActivo())
                    corazones[i].ActivarCorazon();
            }
            else
            {
                if (corazones[i].EstaActivo())
                    corazones[i].DesactivarCorazon();
            }
        }
    }
}
