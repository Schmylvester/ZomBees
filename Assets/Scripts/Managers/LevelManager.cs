using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public struct IDifficultyData
{
    public int spawnCells;
    public int[] setSpawnCells;

    public int rosterCount;
    public int[] fullRoster;

    public int enemyCount;

    public int[] setLockedCells;
    public float lockedCellRate;

    public float difficultyIncrementTimer;
    public string[] difficultyIncrementOrder;

    /** if you survive all difficulty increments, you must then survive this much time to beat the level */
    public float survivalTime;
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] GridManager m_gridManager;
    [SerializeField] EnemyManager m_enemyManager;
    [SerializeField] string m_level;

    [SerializeField] IDifficultyData m_difficulty;
    int m_currentDifficultyScale = 0;
    float m_difficultyScaleTimer = 0;

    private void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Levels/" + m_level + ".json");
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            m_difficulty = JsonUtility.FromJson<IDifficultyData>(dataAsJson);
        }
        if (m_difficulty.setLockedCells.Length > 0)
        {
            m_gridManager.setCustomMap(m_difficulty.setLockedCells);
        }
        else
        {
            m_gridManager.setRandomMap(m_difficulty.lockedCellRate);
        }
        for (int i = 0; i < m_difficulty.spawnCells; ++i)
        {
            if (m_difficulty.setSpawnCells.Length > i)
            {
            m_gridManager.addSpawnCell(m_difficulty.setSpawnCells[i]);

            } else
            {
            m_gridManager.addSpawnCell();

            }
        }

        var initRoster = new int[m_difficulty.rosterCount];
        for (int i = 0; i < initRoster.Length; ++i)
        {
            initRoster[i] = m_difficulty.fullRoster[i];
        }
        m_enemyManager.updateEnemyRoster(initRoster);
        m_enemyManager.setEnemyCount(m_difficulty.enemyCount);
    }

    private void Update()
    {
        m_difficultyScaleTimer += Time.deltaTime;
        if (m_currentDifficultyScale < m_difficulty.difficultyIncrementOrder.Length)
        {
            if (m_difficultyScaleTimer >= m_difficulty.difficultyIncrementTimer)
            {
                m_difficultyScaleTimer = 0;
                switch (m_difficulty.difficultyIncrementOrder[m_currentDifficultyScale])
                {
                    case "spawnCells":
                        if (m_difficulty.spawnCells < m_difficulty.setSpawnCells.Length)
                        {
                            m_gridManager.addSpawnCell(m_difficulty.setSpawnCells[m_difficulty.spawnCells]);
                        } else
                        {
                            m_gridManager.addSpawnCell();
                        }
                        m_difficulty.spawnCells++;
                        break;
                    case "roster":
                        m_difficulty.rosterCount++;
                        var roster = new int[m_difficulty.rosterCount];
                        for (int i = 0; i < roster.Length; ++i)
                        {
                            roster[i] = m_difficulty.fullRoster[i];
                        }
                        m_enemyManager.updateEnemyRoster(roster);
                        break;
                    case "enemyCount":
                        m_difficulty.enemyCount++;
                        m_enemyManager.setEnemyCount(m_difficulty.enemyCount);
                        break;
                    default:
                        Debug.LogWarning("Incorrect difficulty increment order element " + m_difficulty.difficultyIncrementOrder[m_currentDifficultyScale]);
                        break;
                }
                m_currentDifficultyScale++;
            }
        } else if (m_difficultyScaleTimer > m_difficulty.survivalTime)
        {
            Debug.Log("Level clear");
        }
    }
}
