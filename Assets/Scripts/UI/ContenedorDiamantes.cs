using TMPro;
using UnityEngine;

public class ContenedorDiamantes : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI texto;
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
            jugador.OnDiamantesCambiados += OnDiamantesCambiados;

        if (GameManager.instance != null)
            GameManager.OnGameDataChanged.AddListener(ActualizarPorGameManager);

        ActualizarInicial();
    }

    private void OnDisable()
    {
        if (jugador != null)
            jugador.OnDiamantesCambiados -= OnDiamantesCambiados;

        if (GameManager.instance != null)
            GameManager.OnGameDataChanged.RemoveListener(ActualizarPorGameManager);
    }

    private void ActualizarInicial()
    {
        if (texto == null) return;

        if (jugador != null)
            texto.text = jugador.GetDiamantes().ToString();
        else if (GameManager.instance != null)
            texto.text = GameManager.instance.totalDiamonds.ToString();
    }

    private void OnDiamantesCambiados(int total)
    {
        if (texto != null) texto.text = total.ToString();
    }

    private void ActualizarPorGameManager()
    {
        if (texto == null || GameManager.instance == null) return;
        texto.text = GameManager.instance.totalDiamonds.ToString();
    }
}
