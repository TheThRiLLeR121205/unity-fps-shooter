using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    public Transform firePoint;        // Assign tip of gun
    public GameObject bulletPrefab;    // Optional: for bullet prefab shooting
    public float bulletSpeed = 50f;
    public int damage = 25;
    public float range = 100f;

    [Header("Visuals")]
    public LineRenderer lineRenderer;

    private PlayerInput playerInput;
    private InputAction fireAction;

    void Awake()
    {
        // Try to get PlayerInput from this object or parent
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
            playerInput = GetComponentInParent<PlayerInput>();

        if (playerInput != null)
        {
            fireAction = playerInput.actions["Fire"]; // Make sure your Input Action is named "Fire"
        }
        else
        {
            Debug.LogError("PlayerInput component not found on this object or parent!");
        }

        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    void Update()
    {
        if (fireAction != null && fireAction.WasPerformedThisFrame())
        {
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        Vector3 shootDirection = firePoint.forward;

        if (Physics.Raycast(firePoint.position, shootDirection, out hit, range))
        {
            // Deal damage if enemy is hit
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
                enemy.TakeDamage(damage);

            // Draw bullet trace to hit point
            StartCoroutine(ShowLine(hit.point));
        }
        else
        {
            // Draw bullet trace to max range
            StartCoroutine(ShowLine(firePoint.position + shootDirection * range));
        }
    }

    IEnumerator ShowLine(Vector3 endPosition)
    {
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, endPosition);
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.05f);
        lineRenderer.enabled = false;
    }
}
