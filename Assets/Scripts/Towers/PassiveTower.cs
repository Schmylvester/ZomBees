using UnityEngine;

public class PassiveTower : MonoBehaviour
{
    EPassiveBuff m_buff;
    float m_healthThreshold;
    [SerializeField] ResourceManager m_playerHealth;
    [SerializeField] PassiveBuffManager m_passiveManager;

    private void Start()
    {
        m_playerHealth.onResourceChange += checkActiveChange;
    }

    private void OnDestroy()
    {
        m_playerHealth.onResourceChange -= checkActiveChange;
    }

    void checkActiveChange(IStatChange _currentHealth)
    {
        if (_currentHealth.percentage < m_healthThreshold)
        {
            m_passiveManager.removeBuff(m_buff);
        } else
        {
            m_passiveManager.addBuff(m_buff);
        }
    }
}
