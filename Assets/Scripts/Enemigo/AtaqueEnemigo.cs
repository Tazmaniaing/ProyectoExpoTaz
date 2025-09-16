using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueEnemigo : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform controladorAtaque;
    [SerializeField] private MovimientoEnemigo movimientoEnemigo;
    [SerializeField] private CircleCollider2D rangoTrigger; // Debe ser IsTrigger = true

    [Header("Ataque")]
    [SerializeField] private float radioCirculo = 0.35f;
    [SerializeField] private int dañoAtaque = 1;
    [SerializeField] private float duracionDelAtaque = 0.6f;
    [SerializeField] private float tiempoAEsperarDespuesDelAtaque = 0.75f;
    [SerializeField] private string tagObjetivo = "Player";
    [SerializeField] private float intervaloAutoAtaque = 1.5f;
    [SerializeField] private float retardoGolpe = 0.15f;
    [SerializeField] private float radioRangoFallback = 1f;
    [SerializeField] private float tiempoUltimoAtaque = -999f;

    private bool objetivoEnRango;
    private Coroutine rutina;

    // --- Helpers de detección ---
    private bool EsObjetivo(Collider2D other)
    {
        if (other == null) return false;
        if (other.CompareTag(tagObjetivo)) return true;

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
    }

    private void IntentarArrancarRutina()
    {
        if (objetivoEnRango && rutina == null)
            rutina = StartCoroutine(RutinaAutoAtaque());
    }

    // --- Triggers ---
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
        // Antes de apagar, verifica si sigue habiendo alguna parte del Player dentro
        objetivoEnRango = HayObjetivoDentro();
        if (!objetivoEnRango && rutina != null)
        {
            StopCoroutine(rutina);
            rutina = null;
        }
    }

    // --- Lógica de ataque automático ---
    private IEnumerator RutinaAutoAtaque()
    {
        while (objetivoEnRango)
        {
            // Cooldown antes del siguiente intento de ataque
            float proximo = tiempoUltimoAtaque + intervaloAutoAtaque;
            if (Time.time < proximo)
            {
                yield return new WaitForSeconds(proximo - Time.time);
                continue;
            }

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

            yield return new WaitForSeconds(Mathf.Max(0f, duracionDelAtaque - retardoGolpe));
            movimientoEnemigo.CambiarAEstadoEsperar(tiempoAEsperarDespuesDelAtaque);
        }

        // Limpieza por si sale del while
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
                v.TomarDaño(dañoAtaque);
                tiempoUltimoAtaque = Time.time; // cooldown empieza al golpear (mueve arriba si quieres cooldown aunque falle)
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (controladorAtaque) Gizmos.DrawWireSphere(controladorAtaque.position, radioCirculo);

        Gizmos.color = Color.yellow;
        if (rangoTrigger)
        {
            Vector3 centro = rangoTrigger.transform.TransformPoint(rangoTrigger.offset);
            float r = rangoTrigger.radius * Mathf.Max(
                rangoTrigger.transform.lossyScale.x,
                rangoTrigger.transform.lossyScale.y
            );
            Gizmos.DrawWireSphere(centro, r);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, radioRangoFallback);
        }
    }
}
