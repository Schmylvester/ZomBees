using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class LevelDesignManager : MonoBehaviour
{
    [SerializeField] GridManager m_gridManager;
    List<int> m_blockedCells = new();
    List<int> m_spawnCells = new();
    public void onSave()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, System.DateTime.Now.ToFileTimeUtc() + ".json");
        var file = File.CreateText(filePath);
        file.WriteLine("{" + getFile() + "}");
        file.Close();
    }

    public void toggleCell(Cell _cell)
    {
        var cellIndex = m_gridManager.getCellIndex(_cell);
        if (_cell.isEdge())
        {
            var edgeIndex = m_gridManager.getEdgeIndex(_cell);
            if (m_gridManager.isSpawnCell(_cell))
            {
                m_gridManager.removeSpawnCell(edgeIndex);
                m_spawnCells.Remove(edgeIndex);
            } else
            {
                m_gridManager.addSpawnCell(edgeIndex, false);
                m_spawnCells.Add(edgeIndex);
            }
        } else
        {
            if (_cell.accessible)
            {
                _cell.setAccessible(false);
                m_blockedCells.Add(cellIndex);
            } else
            {
                _cell.setAccessible(true);
                m_blockedCells.Remove(cellIndex);
            }
        }
    }

    string getFile()
    {
        string blockedCells = "\"blockedCells\": [";
        for (int i = 0; i < m_blockedCells.Count; ++i)
        {
            blockedCells += m_blockedCells[i].ToString();
            if (i < m_blockedCells.Count - 1)
            {
                blockedCells += ", ";
            }
        }
        blockedCells += ']';

        string spawnCells = "\"spawnCells\": [";
        for (int i = 0; i < m_spawnCells.Count; i++)
        {
            spawnCells += m_spawnCells[i].ToString();
            if (i < m_spawnCells.Count - 1)
            {
                spawnCells += ", ";
            }
        }
        spawnCells += "]";
        return "{" + blockedCells + ", " + spawnCells + "}";
    }
}
