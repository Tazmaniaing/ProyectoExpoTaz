using TMPro;
using UnityEngine;

public class ContenedorDiamantes : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI texto;
    [SerializeField] private VidaJugador jugador;

    private bool usandoJugador; // guardamos de qué fuente venimos

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
        usandoJugador = jugador != null;

        if (usandoJugador)
        {
            jugador.OnDiamantesCambiados += OnDiamantesCambiados;
        }
        else if (GameManager.instance != null)
        {
            GameManager.OnGameDataChanged.AddListener(ActualizarPorGameManager);
        }

        ActualizarInicial();
    }

    private void OnDisable()
    {
        if (usandoJugador && jugador != null)
        {
            jugador.OnDiamantesCambiados -= OnDiamantesCambiados;
        }
        else if (GameManager.instance != null)
        {
            GameManager.OnGameDataChanged.RemoveListener(ActualizarPorGameManager);
        }
    }

    private void ActualizarInicial()
    {
        if (texto == null) return;

        if (usandoJugador)
        {
            texto.text = jugador.GetDiamantes().ToString();
        }
        else if (GameManager.instance != null)
        {
            texto.text = GameManager.instance.totalDiamonds.ToString();
        }
        else
        {
            texto.text = "0";
        }
    }

    private void OnDiamantesCambiados(int total)
    {
        if (texto != null) texto.text = total.ToString();
    }

    private void ActualizarPorGameManager()
    {
        // Si ya usamos VidaJugador, nunca dejes que GM pise el valor.
        if (usandoJugador || texto == null || GameManager.instance == null) return;
        texto.text = GameManager.instance.totalDiamonds.ToString();
    }
}
