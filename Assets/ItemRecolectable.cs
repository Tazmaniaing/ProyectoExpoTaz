using UnityEngine;

public enum TipoItem { Generico, Diamante, Vida }

public class ItemRecolectable : MonoBehaviour
{
    [Header("Identidad")]
    public string nombreItem = "Diamante";
    [SerializeField] private TipoItem tipo = TipoItem.Diamante;

    [Header("Efecto")]
    [SerializeField] private int cantidad = 1;
    [SerializeField] private bool consumirAunqueNoSirva = false; // p.ej. vida llena

    [Header("Detección")]
    [SerializeField] private string tagObjetivo = "Player";

    private void Start()
    {
        // Compatibilidad con tu inventario estático (opcional)
        if (MovimientoJugador.TieneItem(nombreItem))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Asegura que el root o el propio collider pertenezca al Player
        if (!EsJugador(collision)) return;

        // 1) Inventario legacy (si lo necesitas)
        MovimientoJugador.AgregarItem(nombreItem);

        // 2) Aplicar efecto real al jugador (VidaJugador)
        VidaJugador v = collision.GetComponentInParent<VidaJugador>();
        if (v == null) v = collision.GetComponent<VidaJugador>();

        switch (tipo)
        {
            case TipoItem.Diamante:
                if (v != null)
                {
                    v.AgregarDiamantes(Mathf.Max(1, cantidad));

                    // Bridge opcional hacia GameManager (si tu HUD aún lo usa)
                    if (GameManager.instance != null)
                    {
                        GameManager.instance.totalDiamonds = v.GetDiamantes();
                        GameManager.OnGameDataChanged?.Invoke();
                    }
                }
                Destroy(gameObject);
                break;

            case TipoItem.Vida:
                if (v != null)
                {
                    int curado = v.Curar(Mathf.Max(1, cantidad));
                    if (curado > 0 || consumirAunqueNoSirva)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        // No se destruye si no curó y no queremos gastarlo en vano
                        Debug.Log("[ItemRecolectable] Vida llena, pickup no consumido.");
                    }
                }
                else
                {
                    // Si no hay VidaJugador, decide si consumir igualmente
                    if (consumirAunqueNoSirva) Destroy(gameObject);
                }
                break;

            default: // Generico u otros
                Destroy(gameObject);
                break;
        }
    }

    private bool EsJugador(Collider2D c)
    {
        if (c.CompareTag(tagObjetivo)) return true;
        Transform root = c.transform.root;
        return root != null && root.CompareTag(tagObjetivo);
    }
}
