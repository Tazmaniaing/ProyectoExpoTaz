using System.Collections;
<<<<<<< HEAD
=======
using System.Collections.Generic;
>>>>>>> 21bb9f39121e19c86a89769d654b3695d9b3f479
using UnityEngine;

public class AtaqueEnemigo : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform controladorAtaque;
    [SerializeField] private MovimientoEnemigo movimientoEnemigo;
<<<<<<< HEAD
    [SerializeField] private CircleCollider2D rangoTrigger;

    [Header("Ataque")]
    [SerializeField] private bool usarProyectil = false;
    [SerializeField] private float radioCirculo = 0.35f;
    [SerializeField] private int danoAtaque = 1;
=======
    [SerializeField] private CircleCollider2D rangoTrigger; // Debe ser IsTrigger = true

    [Header("Ataque")]
    [SerializeField] private float radioCirculo = 0.35f;
    [SerializeField] private int dañoAtaque = 1;
>>>>>>> 21bb9f39121e19c86a89769d654b3695d9b3f479
    [SerializeField] private float duracionDelAtaque = 0.6f;
    [SerializeField] private float tiempoAEsperarDespuesDelAtaque = 0.75f;
    [SerializeField] private string tagObjetivo = "Player";
    [SerializeField] private float intervaloAutoAtaque = 1.5f;
    [SerializeField] private float retardoGolpe = 0.15f;
    [SerializeField] private float radioRangoFallback = 1f;
    [SerializeField] private float tiempoUltimoAtaque = -999f;

<<<<<<< HEAD
    [Header("Proyectil")]
    [SerializeField] private ProyectilEnemigo prefabProyectil;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private float velocidadProyectil = 8f;
    [SerializeField] private float vidaProyectil = 4f;

    private bool objetivoEnRango;
    private Coroutine rutina;

=======
    private bool objetivoEnRango;
    private Coroutine rutina;

    // --- Helpers de detección ---
>>>>>>> 21bb9f39121e19c86a89769d654b3695d9b3f479
    private bool EsObjetivo(Collider2D other)
    {
        if (other == null) return false;
        if (other.CompareTag(tagObjetivo)) return true;
<<<<<<< HEAD
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
=======

        // Revisa el root por si el collider es un hijo (arma, hitbox, etc.)
        Transform root = other.transform.root;
        return root != null && root.CompareTag(tagObjetivo);
    }

    private bool HayObjetivoDentro()
    {
        // Calcula el círculo real del trigger en mundo
        if (rangoTrigger != null)
        {
            Vector3 centro = rangoTrigger.transform.TransformPoint(rangoTrigger.offset);
            float r = rangoTrigger.radius * Mathf.Max(
                rangoTrigger.transform.lossyScale.x,
                rangoTrigger.transform.lossyScale.y
            );

            Collider2D[] cols = Physics2D.OverlapCircleAll(centro, r);
            foreach (var c in cols)
            {
                if (EsObjetivo(c)) return true;
            }
            return false;
        }
        else
        {
            // Fallback: usa esfera alrededor del enemigo
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radioRangoFallback);
            foreach (var c in cols)
            {
                if (EsObjetivo(c)) return true;
            }
            return false;
        }
>>>>>>> 21bb9f39121e19c86a89769d654b3695d9b3f479
    }

    private void IntentarArrancarRutina()
    {
<<<<<<< HEAD
        if (objetivoEnRango && rutina == null) rutina = StartCoroutine(RutinaAutoAtaque());
    }

=======
        if (objetivoEnRango && rutina == null)
            rutina = StartCoroutine(RutinaAutoAtaque());
    }

    // --- Triggers ---
>>>>>>> 21bb9f39121e19c86a89769d654b3695d9b3f479
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
<<<<<<< HEAD
        objetivoEnRango = BuscarObjetivoEnRango() != null;
=======
        // Antes de apagar, verifica si sigue habiendo alguna parte del Player dentro
        objetivoEnRango = HayObjetivoDentro();
>>>>>>> 21bb9f39121e19c86a89769d654b3695d9b3f479
        if (!objetivoEnRango && rutina != null)
        {
            StopCoroutine(rutina);
            rutina = null;
        }
    }

<<<<<<< HEAD
=======
    // --- Lógica de ataque automático ---
>>>>>>> 21bb9f39121e19c86a89769d654b3695d9b3f479
    private IEnumerator RutinaAutoAtaque()
    {
        while (objetivoEnRango)
        {
<<<<<<< HEAD
=======
            // Cooldown antes del siguiente intento de ataque
>>>>>>> 21bb9f39121e19c86a89769d654b3695d9b3f479
            float proximo = tiempoUltimoAtaque + intervaloAutoAtaque;
            if (Time.time < proximo)
            {
                yield return new WaitForSeconds(proximo - Time.time);
                continue;
            }

<<<<<<< HEAD
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
=======
            // Comp puerta: no atacar si está ocupado o no está en el suelo
            bool ocupado = movimientoEnemigo.GetEstadoActual() == EstadosEnemigo.Ocupado;
            bool enSuelo = movimientoEnemigo.EstaEnElSuelo();

            if (ocupado || !enSuelo)
            {
                Debug.Log($"[AutoAtaque] bloqueado: ocupado={ocupado}, enSuelo={enSuelo}, t={Time.time:F2}");
                yield return new WaitForFixedUpdate();
                continue;
            }

            // (Opcional) quita comentario si quieres cooldown aunque falle el golpe:
            // tiempoUltimoAtaque = Time.time;

            // Lanzar ataque
            movimientoEnemigo.CambiarAEstadoOcupado(duracionDelAtaque, transform);
            rb2D.linearVelocity = Vector2.zero;
            //animator.SetTrigger("Atacar");

            yield return new WaitForSeconds(retardoGolpe);
            Atacar();
>>>>>>> 21bb9f39121e19c86a89769d654b3695d9b3f479

            yield return new WaitForSeconds(Mathf.Max(0f, duracionDelAtaque - retardoGolpe));
            movimientoEnemigo.CambiarAEstadoEsperar(tiempoAEsperarDespuesDelAtaque);
        }
<<<<<<< HEAD
=======

        // Limpieza por si sale del while
>>>>>>> 21bb9f39121e19c86a89769d654b3695d9b3f479
        rutina = null;
    }

    public void Atacar()
    {
        Collider2D[] objetosTocados = Physics2D.OverlapCircleAll(controladorAtaque.position, radioCirculo);
        for (int i = 0; i < objetosTocados.Length; i++)
        {
            VidaJugador v = objetosTocados[i].GetComponent<VidaJugador>();
            if (v == null) v = objetosTocados[i].GetComponentInParent<VidaJugador>();
<<<<<<< HEAD
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
=======

            if (v != null && EsObjetivo(objetosTocados[i]))
            {
                v.TomarDaño(dañoAtaque);
                tiempoUltimoAtaque = Time.time; // cooldown empieza al golpear (mueve arriba si quieres cooldown aunque falle)
                break;
            }
        }
>>>>>>> 21bb9f39121e19c86a89769d654b3695d9b3f479
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
<<<<<<< HEAD
        if (!usarProyectil && controladorAtaque) Gizmos.DrawWireSphere(controladorAtaque.position, radioCirculo);
=======
        if (controladorAtaque) Gizmos.DrawWireSphere(controladorAtaque.position, radioCirculo);
>>>>>>> 21bb9f39121e19c86a89769d654b3695d9b3f479

        Gizmos.color = Color.yellow;
        if (rangoTrigger)
        {
            Vector3 centro = rangoTrigger.transform.TransformPoint(rangoTrigger.offset);
<<<<<<< HEAD
            float r = rangoTrigger.radius * Mathf.Max(rangoTrigger.transform.lossyScale.x, rangoTrigger.transform.lossyScale.y);
=======
            float r = rangoTrigger.radius * Mathf.Max(
                rangoTrigger.transform.lossyScale.x,
                rangoTrigger.transform.lossyScale.y
            );
>>>>>>> 21bb9f39121e19c86a89769d654b3695d9b3f479
            Gizmos.DrawWireSphere(centro, r);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, radioRangoFallback);
        }
<<<<<<< HEAD

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
=======
>>>>>>> 21bb9f39121e19c86a89769d654b3695d9b3f479
    }
}
