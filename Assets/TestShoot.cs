using UnityEngine;

public class TestShoot : MonoBehaviour
{
    public GameObject bulletPrefab;

    void Start()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.right * 10f;
    }
}