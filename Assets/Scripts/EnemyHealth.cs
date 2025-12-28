using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public int health = 50;
    public int scoreValue = 1;

    // This is the OnDeath event
    public Action OnDeath;

    private UIManager uiManager;

    void Start()
    {
        // For newer Unity versions
        uiManager = FindFirstObjectByType<UIManager>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Add score
        if (uiManager != null)
            uiManager.AddScore(scoreValue);

        // Trigger respawn event
        OnDeath?.Invoke();

        Destroy(gameObject);
    }
}
