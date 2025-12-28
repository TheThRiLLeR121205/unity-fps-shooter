using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour
{
    [Header("Shooting Settings")]
    public Transform firePoint;          // Tip of enemy gun
    public GameObject bulletPrefab;      // Bullet prefab
    public float bulletSpeed = 30f;
    public float fireRate = 1.5f;        // Seconds between shots
    public int damage = 20;

    private Transform player;
    private float nextFireTime = 0f;

    void Start()
    {
        // Find player in the scene
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogError("Player not found! Make sure your player has tag 'Player'");
    }

    void Update()
{
    if (player == null || firePoint == null) return;

    // Rotate only on Y-axis toward player
    Vector3 lookPos = player.position - transform.position;
    lookPos.y = 0; // keep enemy upright
    transform.rotation = Quaternion.LookRotation(lookPos);

    // Shoot at intervals
    if (Time.time >= nextFireTime)
    {
        Shoot();
        nextFireTime = Time.time + fireRate;
    }
}


    void Shoot()
{
    if (bulletPrefab != null)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = firePoint.forward * bulletSpeed; // Always forward
    }
}
void OnDrawGizmos()
{
    if (firePoint != null)
    {
        Gizmos.color = Color.red; // color of the line
        Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.forward * 5f);
    }
}

}
