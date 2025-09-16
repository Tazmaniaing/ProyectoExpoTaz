using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Animator animator;
    [SerializeField] private EstadosEnemigo estadoActual = EstadosEnemigo.Correr;
    [SerializeField] private LayerMask capasSuelo;

    [Header("Visual")]
    [SerializeField] private Transform visual;
    [SerializeField] private bool iniciarHaciaDerecha = false;

    [Header("Movimiento Horizontal")]
    [SerializeField] private float velocidadDeMovimientoBase = 2f;
    [SerializeField] private float velocidadDeMovimientoActual = 2f;
    [SerializeField] private Transform controladorFrente;
    [SerializeField] private float distanciaRayoFrente = 0.75f;
    [SerializeField] private Transform controladorFrenteArriba;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private float distanciaRayoSuelo = 0.25f;

    [Header("Esperar")]
    [SerializeField] private float tiempoAEsperarPorDefecto = 3f;

    [Header("Saltar")]
    [SerializeField] private float fuerzaSalto = 5f;
    [SerializeField] private Vector2 dimensionesCaja = new Vector2(0.36f, 0.05f);
    [SerializeField] private Transform controladorEstaEnSuelo;

    [Header("Ocupar")]
    [SerializeField] private float tiempoParaDesocupar = 0f;

    private bool tocandoSuelo;
    private float esperarRestante;
    private float ocupadoRestante;
    private int direccion; // -1 = izq, +1 = der

    private void Awake()
    {
        if (!rb2D) rb2D = GetComponent<Rigidbody2D>();
        if (!animator) animator = GetComponent<Animator>();
    }

    private void Start()
    {
        velocidadDeMovimientoActual = velocidadDeMovimientoBase;
        direccion = iniciarHaciaDerecha ? 1 : -1;
        Vector3 sRoot = transform.localScale;
        transform.localScale = new Vector3(Mathf.Abs(sRoot.x), sRoot.y, sRoot.z);
        AplicarEscalaVisual();
    }

    private void Update()
    {
        ActualizarSuelo();
        ActualizarEstadosTemporales();
        ActualizarAnimacion();
    }

    private void FixedUpdate()
    {
        if (estadoActual == EstadosEnemigo.Correr) Mover();
    }

    private void Mover()
    {
        rb2D.linearVelocity = new Vector2(direccion * velocidadDeMovimientoActual, rb2D.linearVelocity.y);

        bool hayPared = HayParedDelante(direccion);
        bool hayHueco = HayHuecoDelante(direccion);

        if (hayPared || hayHueco) Girar();
    }

    private bool HayParedDelante(int dir)
    {
        if (!controladorFrente) return false;
        Vector2 origenA = controladorFrente.position;
        Vector2 origenB = controladorFrenteArriba ? (Vector2)controladorFrenteArriba.position : origenA + Vector2.up * 0.25f;
        Vector2 d = new Vector2(dir, 0f);
        RaycastHit2D hitA = Physics2D.Raycast(origenA, d, distanciaRayoFrente, capasSuelo);
        RaycastHit2D hitB = Physics2D.Raycast(origenB, d, distanciaRayoFrente, capasSuelo);
        return hitA.collider != null || hitB.collider != null;
    }

    private bool HayHuecoDelante(int dir)
    {
        if (!controladorSuelo) return false;
        Vector2 origen = (Vector2)controladorSuelo.position + new Vector2(0.25f * dir, 0f);
        RaycastHit2D hit = Physics2D.Raycast(origen, Vector2.down, distanciaRayoSuelo, capasSuelo);
        return hit.collider == null;
    }

    private void Girar()
    {
        direccion *= -1;
        AplicarEscalaVisual();
    }

    private void AplicarEscalaVisual()
    {
        Transform t = visual ? visual : transform;
        Vector3 s = t.localScale;
        s.x = direccion == -1 ? Mathf.Abs(s.x) : -Mathf.Abs(s.x);
        t.localScale = s;
    }

    private void ActualizarSuelo()
    {
        if (controladorEstaEnSuelo)
        {
            Collider2D c = Physics2D.OverlapBox(controladorEstaEnSuelo.position, dimensionesCaja, 0f, capasSuelo);
            tocandoSuelo = c != null;
        }
        else
        {
            Vector2 origen = controladorSuelo ? (Vector2)controladorSuelo.position : (Vector2)transform.position;
            tocandoSuelo = Physics2D.Raycast(origen, Vector2.down, distanciaRayoSuelo, capasSuelo).collider != null;
        }
    }

    private void ActualizarEstadosTemporales()
    {
        if (estadoActual == EstadosEnemigo.Esperar)
        {
            if (esperarRestante > 0f) esperarRestante -= Time.deltaTime;
            if (esperarRestante <= 0f) estadoActual = EstadosEnemigo.Correr;
        }
        else if (estadoActual == EstadosEnemigo.Ocupado)
        {
            float limite = tiempoParaDesocupar > 0f ? tiempoParaDesocupar : ocupadoRestante;
            if (limite > 0f)
            {
                ocupadoRestante -= Time.deltaTime;
                if (ocupadoRestante <= 0f) estadoActual = EstadosEnemigo.Correr;
            }
        }
    }

    private void ActualizarAnimacion()
    {
        if (!animator) return;
        animator.SetFloat("VelocidadHorizontal", Mathf.Abs(rb2D.linearVelocity.x));
        animator.SetBool("EnSuelo", tocandoSuelo);
        animator.SetBool("Ocupado", estadoActual == EstadosEnemigo.Ocupado);
    }

    public EstadosEnemigo GetEstadoActual() => estadoActual;
    public bool EstaEnElSuelo() => tocandoSuelo;

    public void CambiarAEstadoOcupado(float duracion, Transform origen)
    {
        estadoActual = EstadosEnemigo.Ocupado;
        ocupadoRestante = duracion;
        if (animator) animator.SetBool("Ocupado", true);
        rb2D.linearVelocity = new Vector2(0f, rb2D.linearVelocity.y);
    }

    public void CambiarAEstadoEsperar(float tiempo)
    {
        estadoActual = EstadosEnemigo.Esperar;
        esperarRestante = tiempo > 0f ? tiempo : tiempoAEsperarPorDefecto;
        if (animator) animator.SetBool("Ocupado", false);
    }

    private void OnDrawGizmosSelected()
    {
        int dir = direccion != 0 ? direccion :
                  (visual ? (visual.localScale.x < 0f ? 1 : -1) :
                            (transform.localScale.x < 0f ? 1 : -1));

        Gizmos.color = Color.green;
        if (controladorSuelo) Gizmos.DrawLine(controladorSuelo.position, controladorSuelo.position + Vector3.down * distanciaRayoSuelo);

        if (controladorFrente)
        {
            Vector3 a = controladorFrente.position;
            Vector3 b = a + Vector3.right * dir * distanciaRayoFrente;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(a, b);

            Vector3 a2 = controladorFrenteArriba ? controladorFrenteArriba.position : a + Vector3.up * 0.25f;
            Vector3 b2 = a2 + Vector3.right * dir * distanciaRayoFrente;
            Gizmos.DrawLine(a2, b2);

            Vector3 origen = controladorSuelo ? controladorSuelo.position : transform.position;
            Vector3 origenAdelante = origen + Vector3.right * dir * 0.25f;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(origenAdelante, origenAdelante + Vector3.down * distanciaRayoSuelo);
        }

        if (controladorEstaEnSuelo)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(controladorEstaEnSuelo.position, dimensionesCaja);
        }
    }
}
