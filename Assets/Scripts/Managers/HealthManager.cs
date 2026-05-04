using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public delegate void healthEmpty();
    public healthEmpty onHealthEmpty;

    [SerializeField] Transform m_healthBar;

    [SerializeField] int m_totalHealth = 100;
    int m_currentHealth;
    int currentHealth {  get { return m_currentHealth; } set {
            m_currentHealth = value;
     } }

    private void Start()
    {
        currentHealth = m_totalHealth;
    }

    private void Update()
    {
        if (m_healthBar)
        {
            m_healthBar.localScale = new Vector3((float)currentHealth / m_totalHealth, 1, 1);
        }
    }

    public void takeDamage(int damage)
    {
        currentHealth -= Mathf.Max(damage, 1);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            onHealthEmpty?.Invoke();
        }
    }

    public void setInitHealth(int _health)
    {
        m_totalHealth = _health;
        m_currentHealth = _health;
    }
}
