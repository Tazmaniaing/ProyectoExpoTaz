using UnityEngine;

public class ProyectilEnemigo : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private int dano;
    private string tagObjetivo;
    private float vidaRestante;

    public void Init(Vector2 direccion, float velocidad, int dano, string tagObjetivo, float vidaSegundos)
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        this.dano = dano;
        this.tagObjetivo = tagObjetivo;
        this.vidaRestante = vidaSegundos;
        if (rb) rb.linearVelocity = direccion.normalized * velocidad;
    }

    private void Update()
    {
        vidaRestante -= Time.deltaTime;
        if (vidaRestante <= 0f) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(tagObjetivo) && !other.transform.root.CompareTag(tagObjetivo)) return;

        VidaJugador v = other.GetComponent<VidaJugador>();
        if (v == null) v = other.GetComponentInParent<VidaJugador>();
        if (v != null) v.TomarDaño(dano);
        Destroy(gameObject);
    }
}
