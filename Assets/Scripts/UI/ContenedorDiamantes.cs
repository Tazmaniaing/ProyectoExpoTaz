using TMPro;
using UnityEngine;

public class ContenedorDiamantes : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI texto;
    [SerializeField] private VidaJugador jugador;

<<<<<<< HEAD
    private bool usandoJugador; // guardamos de qué fuente venimos

=======
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f
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
<<<<<<< HEAD
        usandoJugador = jugador != null;

        if (usandoJugador)
        {
            jugador.OnDiamantesCambiados += OnDiamantesCambiados;
        }
        else if (GameManager.instance != null)
        {
            GameManager.OnGameDataChanged.AddListener(ActualizarPorGameManager);
        }
=======
        if (jugador != null)
            jugador.OnDiamantesCambiados += OnDiamantesCambiados;

        if (GameManager.instance != null)
            GameManager.OnGameDataChanged.AddListener(ActualizarPorGameManager);
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f

        ActualizarInicial();
    }

    private void OnDisable()
    {
<<<<<<< HEAD
        if (usandoJugador && jugador != null)
        {
            jugador.OnDiamantesCambiados -= OnDiamantesCambiados;
        }
        else if (GameManager.instance != null)
        {
            GameManager.OnGameDataChanged.RemoveListener(ActualizarPorGameManager);
        }
=======
        if (jugador != null)
            jugador.OnDiamantesCambiados -= OnDiamantesCambiados;

        if (GameManager.instance != null)
            GameManager.OnGameDataChanged.RemoveListener(ActualizarPorGameManager);
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f
    }

    private void ActualizarInicial()
    {
        if (texto == null) return;

<<<<<<< HEAD
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
=======
        if (jugador != null)
            texto.text = jugador.GetDiamantes().ToString();
        else if (GameManager.instance != null)
            texto.text = GameManager.instance.totalDiamonds.ToString();
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f
    }

    private void OnDiamantesCambiados(int total)
    {
        if (texto != null) texto.text = total.ToString();
    }

    private void ActualizarPorGameManager()
    {
<<<<<<< HEAD
        // Si ya usamos VidaJugador, nunca dejes que GM pise el valor.
        if (usandoJugador || texto == null || GameManager.instance == null) return;
=======
        if (texto == null || GameManager.instance == null) return;
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f
        texto.text = GameManager.instance.totalDiamonds.ToString();
    }
}
