using UnityEngine;

public class Cannon1 : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.15f;
    public float bulletSpeed = 15f;

    [Header("Objetivo")]
    public string playerTag = "Player";
    public float attackRange = 10f;

    [Header("Patrón de Ráfaga")]
    public int bulletsPerBurst = 3;
    public float burstPauseTime = 2f;

    // Este campo te permite definir la dirección inicial del cañón.
    [Header("Orientación Inicial")]
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
            Debug.Log("Cañón Enemigo: Objetivo (jugador) encontrado por etiqueta.");
        }
    }

    /// <summary>
    /// Gira el cañón para que apunte al jugador, basándose en su orientación inicial.
    /// </summary>
    void AimAtPlayer()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;

        // Calculamos el ángulo entre la dirección inicial del cañón y la dirección hacia el jugador.
        float angleOffset = Vector2.SignedAngle(defaultDirection, directionToPlayer);

        // Rotamos el cañón para que apunte al jugador desde su orientación inicial.
        transform.rotation = Quaternion.Euler(0, 0, angleOffset);
    }

    /// <summary>
    /// Crea y dispara una bala en la dirección en la que el cañón está apuntando.
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