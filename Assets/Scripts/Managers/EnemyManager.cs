using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] IEnemyStats[] m_stats;
    [SerializeField] GameObject m_enemyPrefab = null;
    [SerializeField] GridManager m_gridManager = null;
    [SerializeField] Pathfinder m_pathfinder = null;
    [SerializeField] int m_enemyCount = 1;
    [SerializeField] float m_spawnAcceleration;
    [SerializeField] ResourceManager m_manaManager;
    List<Enemy> m_enemies = new();
    public List<Enemy> enemies { get { return m_enemies; } }
    float m_lastSpawn;

    void Update()
    {
        m_lastSpawn += Time.deltaTime;
        if (m_lastSpawn > m_spawnAcceleration)
        {
            m_enemyCount++;
            m_lastSpawn = 0;
        }
        for (int i = 0; i < m_enemyCount; ++i) {
            if (m_enemies.Count == i)
            {
                m_enemies.Add(null);
            }
            if (!m_enemies[i])
            {
                m_enemies[i] = Instantiate(m_enemyPrefab, transform).GetComponent<Enemy>();
                m_enemies[i].initStats(m_stats[Random.Range(0, m_stats.Length)]);
                m_enemies[i].onDefeat += enemyDefeated;

                Cell spawnCell = null;
                var loopBreaker = 0;
                List<Cell> path = new();
                do
                {
                    if (++loopBreaker > 5000)
                    {
                        throw new System.Exception("Are all the edge cells inaccessible");
                    }
                    spawnCell = m_gridManager.getRandomEdgeCell();
                    path = m_pathfinder.findPath(spawnCell, TargetType.Base);
                } while (!spawnCell.accessible || path.Count == 0);

                m_enemies[i].transform.position = spawnCell.transform.position;
                m_enemies[i].path = path;
            }
        }
    }

    void enemyDefeated(IEnemyStats _enemy)
    {
        m_manaManager.addResource(_enemy.yield);
    }
}
