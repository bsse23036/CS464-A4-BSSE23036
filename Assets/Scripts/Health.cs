using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    [SerializeField] private int currentHealth;
    public bool shielded = false;
    [SerializeField] private float iFrameDuration = 1.0f;
    [SerializeField] private float defaultKnockbackForce = 10f;

    public event Action<int, int> Damaged;

    private float invincibleUntil;
    private Rigidbody rb;

    public int Current => currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
    }

    public void TakeDamage(int damage)
    {
        TakeDamage(damage, transform.position - transform.forward, defaultKnockbackForce);
    }

    public void TakeDamage(int damage, Vector3 damageSourcePosition, float force = 10f)
    {
        if (Time.time < invincibleUntil) return;

        if (shielded)
        {
            shielded = false;
            invincibleUntil = Time.time + iFrameDuration;
            Debug.Log("Shield absorbed the damage!");
            ApplyKnockback(damageSourcePosition, force);
            return;
        }

        currentHealth = Mathf.Max(0, currentHealth - damage);
        invincibleUntil = Time.time + iFrameDuration;

        ApplyKnockback(damageSourcePosition, force);

        // Notify all listeners (HealthBar, Hit SFX, Hit Sparks)
        Damaged?.Invoke(currentHealth, maxHealth);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateUI();
        }

        if (currentHealth <= 0 && GameManager.Instance != null)
        {
            GameManager.Instance.PlayerDied();
        }
    }

    private void ApplyKnockback(Vector3 sourcePosition, float force)
    {
        if (rb == null) return;

        Vector3 knockbackDir = transform.position - sourcePosition;
        knockbackDir.y = 0;
        knockbackDir = knockbackDir.normalized;

        knockbackDir.y = 0.4f;
        knockbackDir.Normalize();

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(knockbackDir * force, ForceMode.Impulse);
    }

    public void Restore(float invincibilityTime)
    {
        currentHealth = maxHealth;
        invincibleUntil = Time.time + invincibilityTime;
        Damaged?.Invoke(currentHealth, maxHealth);
    }
}