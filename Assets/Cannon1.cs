using UnityEngine;

public class Cannon1 : MonoBehaviour
{
    [Header("Configuraci�n de Disparo")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.15f;
    public float bulletSpeed = 15f;

    [Header("Objetivo")]
    public string playerTag = "Player";
    public float attackRange = 10f;

    [Header("Patr�n de R�faga")]
    public int bulletsPerBurst = 3;
    public float burstPauseTime = 2f;

    // Este campo te permite definir la direcci�n inicial del ca��n.
    [Header("Orientaci�n Inicial")]
    public Vector2 defaultDirection = Vector2.right; // Por defecto apunta a la derecha

    private Transform player;
    private float timer;
    private int bulletsFiredInBurst;

    void Start()
    {
        timer = burstPauseTime;
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayerByTag();
        }

        if (player == null || bulletPrefab == null || firePoint == null)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            AimAtPlayer();

            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                if (bulletsFiredInBurst < bulletsPerBurst)
                {
                    ShootAtPlayer();
                    bulletsFiredInBurst++;
                    timer = fireRate;
                }
                else
                {
                    bulletsFiredInBurst = 0;
                    timer = burstPauseTime;
                }
            }
        }
    }

    private void FindPlayerByTag()
    {
        GameObject playerObject = GameObject.FindWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
            Debug.Log("Ca��n Enemigo: Objetivo (jugador) encontrado por etiqueta.");
        }
    }

    /// <summary>
    /// Gira el ca��n para que apunte al jugador, bas�ndose en su orientaci�n inicial.
    /// </summary>
    void AimAtPlayer()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        // Calculamos el �ngulo entre la direcci�n inicial del ca��n y la direcci�n hacia el jugador.
        float angleOffset = Vector2.SignedAngle(defaultDirection, directionToPlayer);

        // Rotamos el ca��n para que apunte al jugador desde su orientaci�n inicial.
        transform.rotation = Quaternion.Euler(0, 0, angleOffset);
    }

    /// <summary>
    /// Crea y dispara una bala en la direcci�n en la que el ca��n est� apuntando.
    /// </summary>
    void ShootAtPlayer()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = (player.position - firePoint.position).normalized * bulletSpeed;
        }

        Destroy(bullet, 5f);
    }
}