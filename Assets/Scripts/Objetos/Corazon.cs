using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Corazon : MonoBehaviour, IInteractuable
{
    [Header("Animación")]
    [SerializeField] private Animator animator;

    [Header("Curación")]
    [SerializeField] private int cantidadCuracion = 1;
<<<<<<< HEAD
    [SerializeField] private bool consumirAunqueEsteLleno = false;

    [Header("Detección")]
    [SerializeField] private string tagObjetivo = "Player";
    [SerializeField] private VidaJugador jugador; // referencia directa

    private bool sePuedeUsar = true;
    private Collider2D col;
=======
    [SerializeField] private bool consumirAunqueEsteLleno = false; // si true, se gasta igual

    [Header("Detección")]
    [SerializeField] private string tagObjetivo = "Player";
    [SerializeField] private VidaJugador jugador; // arrástralo o se resuelve por tag

    private bool sePuedeUsar = true;
    private Collider2D col;

    private void Reset()
    {
        col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        if (col != null && !col.isTrigger) col.isTrigger = true;

        if (jugador == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag(tagObjetivo);
            if (go) jugador = go.GetComponent<VidaJugador>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!EsJugador(other)) return;
        TryRecolectar();
    }
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f

    private void Reset()
    {
<<<<<<< HEAD
        col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        if (col != null && !col.isTrigger) col.isTrigger = true;
=======
        TryRecolectar();
    }

    private bool EsJugador(Collider2D c)
    {
        if (c.CompareTag(tagObjetivo)) return true;
        Transform root = c.transform.root;
        return root != null && root.CompareTag(tagObjetivo);
    }

    private void TryRecolectar()
    {
        if (!sePuedeUsar) return;
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f

        if (jugador == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag(tagObjetivo);
            if (go) jugador = go.GetComponent<VidaJugador>();
<<<<<<< HEAD
=======
            if (jugador == null) return;
        }

        if (!consumirAunqueEsteLleno && jugador.EstaAlMaximo())
            return;

        int curado = jugador.Curar(Mathf.Max(1, cantidadCuracion));

        if (curado > 0 || consumirAunqueEsteLleno)
        {
            sePuedeUsar = false;

            if (GameManager.instance != null)
            {
                GameManager.instance.currentHealth = jugador.GetVidaActual();
                GameManager.OnGameDataChanged?.Invoke();
            }

            if (animator != null) animator.SetTrigger("Recoger");
            else DestruirObjeto(); 
>>>>>>> 3495f95362d6d91f85984d67d2c988d6f360084f
        }
    }

    // Auto-pickup al entrar al trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!EsJugador(other)) return;
        TryRecolectar();
    }

    // Opcional: si lo usas también con botón
    public void Interactuar() => TryRecolectar();

    private bool EsJugador(Collider2D c)
    {
        if (c.CompareTag(tagObjetivo)) return true;
        Transform root = c.transform.root;
        return root != null && root.CompareTag(tagObjetivo);
    }

    private void TryRecolectar()
    {
        if (!sePuedeUsar) return;
        if (jugador == null) return;

        if (!consumirAunqueEsteLleno && jugador.EstaAlMaximo())
            return;

        int curado = jugador.Curar(Mathf.Max(1, cantidadCuracion));

        if (curado > 0 || consumirAunqueEsteLleno)
        {
            sePuedeUsar = false;
            if (animator != null) animator.SetTrigger("Recoger");
            else DestruirObjeto();
        }
    }

    // Animation Event al final
    public void DestruirObjeto() => Destroy(gameObject);
}
