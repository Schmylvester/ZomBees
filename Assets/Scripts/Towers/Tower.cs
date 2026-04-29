using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] float m_range;
    [SerializeField] float m_fireRate;
    [SerializeField] int m_damage;
    float m_lastFire = 0f;
    Enemy m_targetEnemy = null;

    void Update()
    {
        if (m_targetEnemy == null)
        {
            var enemies = GameManager.instance.enemyManager.enemies;
            if (enemies.Count > 0)
            {
                enemies.Sort((Enemy a, Enemy b) => {
                    if (!a) return 1;
                    if (!b) return -1;
                    var aDist = Vector3.Distance(transform.position, a.transform.position);
                    var bDist = Vector3.Distance(transform.position, b.transform.position);
                    return aDist < bDist ? -1 : 1;
                 });
                if (Vector3.Distance(enemies[0].transform.position, transform.position) <= m_range)
                {
                    m_targetEnemy = enemies[0];
                }
            }
        }
        m_lastFire += Time.deltaTime;
        if (m_targetEnemy)
        {
            if (m_lastFire >= 1f / m_fireRate)
            {
                m_targetEnemy.takeDamage(m_damage);
                m_lastFire = 0;
            }
        }
    }
}
