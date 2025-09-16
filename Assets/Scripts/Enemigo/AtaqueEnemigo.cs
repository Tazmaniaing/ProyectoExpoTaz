using System.Collections;
using UnityEngine;

public class AtaqueEnemigo : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform controladorAtaque;
    [SerializeField] private MovimientoEnemigo movimientoEnemigo;
    [SerializeField] private CircleCollider2D rangoTrigger;

    [Header("Ataque")]
    [SerializeField] private bool usarProyectil = false;
    [SerializeField] private float radioCirculo = 0.35f;
    [SerializeField] private int danoAtaque = 1;
    [SerializeField] private float duracionDelAtaque = 0.6f;
    [SerializeField] private float tiempoAEsperarDespuesDelAtaque = 0.75f;
    [SerializeField] private string tagObjetivo = "Player";
    [SerializeField] private float intervaloAutoAtaque = 1.5f;
    [SerializeField] private float retardoGolpe = 0.15f;
    [SerializeField] private float radioRangoFallback = 1f;
    [SerializeField] private float tiempoUltimoAtaque = -999f;

    [Header("Proyectil")]
    [SerializeField] private ProyectilEnemigo prefabProyectil;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float velocidadProyectil = 8f;
    [SerializeField] private float vidaProyectil = 4f;

    private bool objetivoEnRango;
    private Coroutine rutina;

    private bool EsObjetivo(Collider2D other)
    {
        if (other == null) return false;
        if (other.CompareTag(tagObjetivo)) return true;
        Transform root = other.transform.root;
        return root != null && root.CompareTag(tagObjetivo);
    }

    private Transform BuscarObjetivoEnRango()
    {
        Vector3 centro;
        float r;
        if (rangoTrigger != null)
        {
            centro = rangoTrigger.transform.TransformPoint(rangoTrigger.offset);
            r = rangoTrigger.radius * Mathf.Max(rangoTrigger.transform.lossyScale.x, rangoTrigger.transform.lossyScale.y);
        }
        else
        {
            centro = transform.position;
            r = radioRangoFallback;
        }

        Collider2D[] cols = Physics2D.OverlapCircleAll(centro, r);
        for (int i = 0; i < cols.Length; i++)
        {
            if (EsObjetivo(cols[i])) return cols[i].transform.root;
        }
        return null;
    }

    private void IntentarArrancarRutina()
    {
        if (objetivoEnRango && rutina == null) rutina = StartCoroutine(RutinaAutoAtaque());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!EsObjetivo(other)) return;
        objetivoEnRango = true;
        IntentarArrancarRutina();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!EsObjetivo(other)) return;
        objetivoEnRango = true;
        IntentarArrancarRutina();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!EsObjetivo(other)) return;
        objetivoEnRango = BuscarObjetivoEnRango() != null;
        if (!objetivoEnRango && rutina != null)
        {
            StopCoroutine(rutina);
            rutina = null;
        }
    }

    private IEnumerator RutinaAutoAtaque()
    {
        while (objetivoEnRango)
        {
            float proximo = tiempoUltimoAtaque + intervaloAutoAtaque;
            if (Time.time < proximo)
            {
                yield return new WaitForSeconds(proximo - Time.time);
                continue;
            }

            if (movimientoEnemigo.GetEstadoActual() == EstadosEnemigo.Ocupado || !movimientoEnemigo.EstaEnElSuelo())
            {
                yield return null;
                continue;
            }

            movimientoEnemigo.CambiarAEstadoOcupado(duracionDelAtaque, transform);
            rb2D.linearVelocity = Vector2.zero;
            animator.SetTrigger("Atacar");
            yield return new WaitForSeconds(retardoGolpe);

            if (usarProyectil) Disparar();
            else Atacar();

            yield return new WaitForSeconds(Mathf.Max(0f, duracionDelAtaque - retardoGolpe));
            movimientoEnemigo.CambiarAEstadoEsperar(tiempoAEsperarDespuesDelAtaque);
        }
        rutina = null;
    }

    public void Atacar()
    {
        Collider2D[] objetosTocados = Physics2D.OverlapCircleAll(controladorAtaque.position, radioCirculo);
        for (int i = 0; i < objetosTocados.Length; i++)
        {
            VidaJugador v = objetosTocados[i].GetComponent<VidaJugador>();
            if (v == null) v = objetosTocados[i].GetComponentInParent<VidaJugador>();
            if (v != null && EsObjetivo(objetosTocados[i]))
            {
                v.TomarDaño(danoAtaque);
                tiempoUltimoAtaque = Time.time;
                break;
            }
        }
    }

    private void Disparar()
    {
        if (prefabProyectil == null)
        {
            tiempoUltimoAtaque = Time.time;
            return;
        }
        Transform origen = puntoDisparo != null ? puntoDisparo : controladorAtaque;
        Vector2 dir;
        Transform objetivo = BuscarObjetivoEnRango();
        if (objetivo != null) dir = ((Vector2)(objetivo.position - origen.position)).normalized;
        else dir = transform.localScale.x >= 0 ? Vector2.right : Vector2.left;

        ProyectilEnemigo p = Instantiate(prefabProyectil, origen.position, Quaternion.identity);
        p.Init(dir, velocidadProyectil, danoAtaque, tagObjetivo, vidaProyectil);
        tiempoUltimoAtaque = Time.time;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (!usarProyectil && controladorAtaque) Gizmos.DrawWireSphere(controladorAtaque.position, radioCirculo);

        Gizmos.color = Color.yellow;
        if (rangoTrigger)
        {
            Vector3 centro = rangoTrigger.transform.TransformPoint(rangoTrigger.offset);
            float r = rangoTrigger.radius * Mathf.Max(rangoTrigger.transform.lossyScale.x, rangoTrigger.transform.lossyScale.y);
            Gizmos.DrawWireSphere(centro, r);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, radioRangoFallback);
        }

        if (usarProyectil)
        {
            Transform origen = puntoDisparo != null ? puntoDisparo : controladorAtaque;
            if (origen)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(origen.position, origen.position + (transform.localScale.x >= 0 ? Vector3.right : Vector3.left));
                Gizmos.DrawWireSphere(origen.position, 0.06f);
            }
        }
    }
}
