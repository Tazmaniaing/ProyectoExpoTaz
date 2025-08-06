using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.15f;      // Frecuencia de disparo dentro de la ráfaga
    public float bulletSpeed = 15f;

    [Header("Objetivo")]
    [Tooltip("La etiqueta (Tag) del objeto del jugador que se buscará en la escena.")]
    public string playerTag = "Player";
    public float attackRange = 10f;
    
    [Header("Patrón de Ráfaga")]
    public int bulletsPerBurst = 3;     // Cuántas balas se disparan en una ráfaga
    public float burstPauseTime = 2f;   // El tiempo de espera entre cada ráfaga
    
    private Transform player;
    private float timer;
    private int bulletsFiredInBurst;

    void Start()
    {
        // Se inicializa el temporizador con el tiempo de pausa para que el cañón espere antes del primer ataque.
        timer = burstPauseTime;
    }

    void Update()
    {
        // Si el objetivo (jugador) aún no ha sido encontrado, lo buscamos en la escena.
        // Esto se ejecutará en cada frame hasta que el jugador aparezca y sea asignado.
        if (player == null)
        {
            FindPlayerByTag();
        }

        // Si el jugador, el prefab de la bala o el punto de disparo no están asignados,
        // no hacemos nada para evitar errores.
        if (player == null || bulletPrefab == null || firePoint == null)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            // Hacemos que el cañón apunte al jugador.
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

    /// <summary>
    /// Busca un objeto con la etiqueta especificada y lo asigna como el objetivo del cañón.
    /// </summary>
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
    /// Gira el cañón para que apunte directamente al jugador.
    /// </summary>
    void AimAtPlayer()
    {
        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
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
            rb.linearVelocity = firePoint.right * bulletSpeed;
        }
        
        Destroy(bullet, 5f);
    }
}