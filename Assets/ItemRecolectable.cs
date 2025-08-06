using UnityEngine;

public class ItemRecolectable : MonoBehaviour
{
    public string nombreItem = "Diamante";

    private void Start()
    {
        if (MovimientoJugador.TieneItem(nombreItem))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MovimientoJugador.AgregarItem(nombreItem);
            Destroy(gameObject);
        }
    }
}
