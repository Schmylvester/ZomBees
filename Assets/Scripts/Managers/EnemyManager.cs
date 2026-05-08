using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyInfoPanel m_enemyInfoPanel;
    [SerializeField] IEnemyStats[] m_stats;
    [SerializeField] GameObject m_enemyPrefab = null;
    [SerializeField] GridManager m_gridManager = null;
    [SerializeField] ResourceManager m_manaManager;
    List<Enemy> m_enemies = new();
    public List<Enemy> enemies { get { return m_enemies; } }
    int m_enemyCount;
    int[] m_currentEnemyRoster;

    void Update()
    {
        for (int i = 0; i < m_enemyCount; ++i) {
            if (m_enemies.Count == i)
            {
                m_enemies.Add(null);
                m_enemies[i] = Instantiate(m_enemyPrefab, transform).GetComponent<Enemy>();
                m_enemies[i].onDefeat += enemyDefeated;
            }
            if (!m_enemies[i].active)
            {
                var stats = Random.Range(0, m_currentEnemyRoster.Length);
                m_enemies[i].initStats(m_stats[m_currentEnemyRoster[stats]]);

                List<Cell> path = m_gridManager.getRandomPath();
                m_enemies[i].transform.position = path[0].transform.position;
                m_enemies[i].path = new(path);
                m_enemies[i].active = true;
            }
        }
    }

    void enemyDefeated(IEnemyStats _enemy)
    {
        m_manaManager.addResource(_enemy.yield);
    }

    public void setEnemyCount(int _count)
    {
        m_enemyCount = _count;
    }

    public void updateEnemyRoster(int[] _pool)
    {
        m_currentEnemyRoster = _pool;
        m_enemyInfoPanel.setPreviews(m_currentEnemyRoster, m_stats);
    }
}
