using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] IEnemyStats[] m_stats;
    [SerializeField] GameObject m_enemyPrefab = null;
    [SerializeField] GridManager m_gridManager = null;
    [SerializeField] Pathfinder m_pathfinder = null;
    [SerializeField] int m_enemyCount = 1;
    List<Enemy> m_enemies = new();
    public List<Enemy> enemies { get { return m_enemies; } }

    void Update()
    {
        for (int i = 0; i < m_enemyCount; ++i) {
            if (m_enemies.Count == i)
            {
                m_enemies.Add(null);
            }
            if (!m_enemies[i])
            {
                m_enemies[i] = Instantiate(m_enemyPrefab, transform).GetComponent<Enemy>();
                m_enemies[i].initStats(m_stats[Random.Range(0, m_stats.Length)]);

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
}
