using UnityEngine;

public class AttackBullet : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            VidaJugador player = other.GetComponent<VidaJugador>();
            if (player != null)
            {
                player.TomarDaño(damage);
            }

            Destroy(gameObject); // Destruye la bala después de impactar
        }
        else if (!other.isTrigger) // Si choca con otra cosa (pared, etc.)
        {
            Destroy(gameObject);
        }
    }
}