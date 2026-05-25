using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] ConflictResolutionManager m_conflictResolutionManager;
    [SerializeField] EnemyInfoPanel m_enemyInfoPanel;
    [SerializeField] IEnemyStats[] m_stats;
    [SerializeField] GameObject m_enemyPrefab = null;
    [SerializeField] GridManager m_gridManager = null;
    [SerializeField] Pathfinder m_pathfinder = null;
    [SerializeField] PlayerCashManager m_cashManager;
    List<Enemy> m_enemies = new();
    public List<Enemy> enemies { get { return m_enemies; } }
    int m_enemyCount;
    int[] m_currentEnemyRoster;

    void Update()
    {
        for (int i = 0; i < m_enemyCount; ++i) {
            if (m_enemies.Count == i)
            {
                addEnemy();
            }
            if (!m_enemies[i].active)
            {
                var stats = Random.Range(0, m_currentEnemyRoster.Length);
                m_enemies[i].initStats(m_stats[m_currentEnemyRoster[stats]]);

                var spawn = m_gridManager.getRandomSpawnCell();
                List<Cell> path = m_pathfinder.findPath(spawn, (c) => c.getBase());
                m_enemies[i].transform.position = spawn.transform.position;
                m_enemies[i].path = new(path);
                m_enemies[i].active = true;
            }
        }
    }

    void addEnemy()
    {
        var instance = Instantiate(m_enemyPrefab, transform).GetComponent<Enemy>();
        foreach (var enemy in m_enemies)
        {
            enemy.addOther(instance, true);
        }
        instance.onDefeat += enemyDefeated;
        instance.gameObject.name = "Enemy " + m_enemies.Count;
        instance.initConflictResolution(m_conflictResolutionManager);
        m_enemies.Add(instance);
    }

    void enemyDefeated(IEnemyStats _enemy)
    {
        m_cashManager.updateCash(_enemy.yield);
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

    /** this cell just got blocked, do I need to find another way? */
    public void checkFindAlternativePaths(Cell _cell)
    {
        foreach (var enemy in m_enemies)
        {
            enemy.checkFindAlternativePaths(_cell, m_pathfinder);
        }
    }
}
