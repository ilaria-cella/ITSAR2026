using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    float currentHealth;
    public float CurrentHealth => currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($" HP: s{currentHealth}/{maxHealth}");
        if (currentHealth <= 0f) Die();
    }

    public void Heal(float heal)
    {
        currentHealth += heal;
        Mathf.Clamp(currentHealth, 0f, maxHealth); //if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public void SetHealth (float _health, float _maxHealth)
    {
        currentHealth = _health;
        maxHealth = _maxHealth;
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
