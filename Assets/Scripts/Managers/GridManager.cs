using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridManager : MonoBehaviour
{
    [SerializeField] EnemyManager m_enemyManager;
    [SerializeField] Pathfinder m_pathfinder;
    [SerializeField] GameObject m_cellObject;
    /** grid size is determined by how many cells the center is from the edge */
    [SerializeField] int m_gridSize;
    [SerializeField] int m_baseSize;

    [SerializeField] Grid m_grid;
    List<Cell> m_cells = new();
    List<Cell> m_edgeCells = new();
    List<Cell> m_spawnCells = new();
    List<Cell> m_essentialPathCells = new();
    bool m_mapLoaded = false;

    void Awake()
    {
        var xBuffer = (((m_gridSize * 2) + 1) - ((m_baseSize * 2) + 1)) / 2;
        for (int y = -m_gridSize; y < m_gridSize + 1; ++y)
        {
            // calculate width of the current row
            var width = ((2 * m_gridSize) + 1) - Mathf.Abs(y);
            // calculate how far the current row is offset from x = 0
            var offset = -(m_gridSize - (Mathf.Abs(y) / 2));
            for (int x = 0; x < width; ++x) {
                var instance = Instantiate(m_cellObject, transform);
                instance.transform.position = m_grid.CellToLocal(new Vector3Int(x + offset, y));
                var cell = instance.GetComponent<Cell>();
                cell.cellIndex = new Vector2Int(x, y);
                if (cell != null) {
                    cell.cellAccessibleUpdate += (bool _accessible) => cellAccessibilityUpdate(cell, _accessible);
                    m_cells.Add(cell);
                    if (Mathf.Abs(y) <= m_baseSize && x >= xBuffer && x < width - xBuffer)
                    {
                        cell.setBase();
                    }
                    var leftNeigbour = m_cells.Find(c => c.cellIndex.y == y && c.cellIndex.x == x - 1);
                    if (leftNeigbour) { cell.setNeighbour(leftNeigbour, true); }
                    var bottomLeftNeighbour = m_cells.Find(c => c.name == (y - 1) + "." + (y <= 0 ? x - 1 : x));
                    if (bottomLeftNeighbour) { cell.setNeighbour(bottomLeftNeighbour, true); }
                    var bottomRightNeighbour = m_cells.Find(c => c.name == (y - 1) + "." + (y <= 0 ? x : x + 1));
                    if (bottomRightNeighbour) { cell.setNeighbour(bottomRightNeighbour, true); }
                }
            }
        }

        foreach (var cell in m_cells) {
            if (cell.isEdge())
            {
                m_edgeCells.Add(cell);
            }
        }
    }

    public void setCustomMap(int[] _blockedCells)
    {
        foreach (var cell in _blockedCells)
        {
            if (!m_cells[cell].getBase() && !m_cells[cell].isEdge())
            {
                m_cells[cell].setAccessible(false);
            }
        }
    }

    public void setRandomMap(float _blockedCellRate)
    {
        foreach (var cell in m_cells) {
            if (!cell.getBase() && !cell.isEdge())
            {
                cell.setAccessible(Random.value >= _blockedCellRate);
            }
        }
    }

    public void addSpawnCell()
    {
        m_edgeCells = m_edgeCells.OrderBy(x => Random.value).ToList();
        var spawnCell = m_edgeCells.FindIndex(cell => {
            return (m_spawnCells.FindIndex(sc => sc == cell) == -1);
        });
        if (spawnCell == -1)
        {
            Debug.Log("All available edge cells have been set as spawn cells");
            return;
        }
        addSpawnCell(spawnCell);
    }

    public void removeSpawnCell(int i)
    {
        m_spawnCells.Remove(m_edgeCells[i]);
        m_edgeCells[i].unsetSpawnCell();
    }

    public void addSpawnCell(int i)
    {
        m_mapLoaded = true;
        m_spawnCells.Add(m_edgeCells[i]);
        m_edgeCells[i].setSpawnCell();
    }

    public Cell wasHit(Collider2D _hit)
    {
        return m_cells.Find(c => _hit.gameObject == c.gameObject);
    }

    public int getCellIndex(Cell _cell)
    {
        return m_cells.FindIndex(c => c == _cell);
    }

    public int getEdgeIndex(Cell _cell)
    {
        return m_edgeCells.FindIndex(c => c == _cell);
    }

    public bool isSpawnCell(Cell _cell)
    {
        return m_spawnCells.FindIndex(c => c == _cell) != -1;
    }

    public Cell getRandomSpawnCell()
    {
        return m_spawnCells[Random.Range(0, m_spawnCells.Count)];
    }

    public bool isEssential(Cell _cell)
    {
        return m_essentialPathCells.Contains(_cell);
    }

    public void identifyEssentialCells()
    {
        if (!m_mapLoaded)
        {
            return;
        }
        m_essentialPathCells.Clear();
        foreach (Cell cell in m_cells)
        {
            if (!cell.isEdge() && cell.accessible)
            {
                // all edge cells are unblockable, so if we can get a path from any spawn cell, we can get a path from them all
                var essential = m_pathfinder.findPath(m_spawnCells[0], (c) => c.getBase(), cell).Count == 0;
                if (essential)
                {
                    m_essentialPathCells.Add(cell);
                }
            }
        }
    }

    void cellAccessibilityUpdate(Cell _cell, bool _accessible)
    {
        identifyEssentialCells();
        if (!_accessible)
        {
            m_enemyManager.checkFindAlternativePaths(_cell);
        }
    }

    private void OnDestroy()
    {
        foreach(var cell in m_cells)
        {
            cell.cellAccessibleUpdate -= (bool _accessible) => cellAccessibilityUpdate(cell, _accessible);
        }
    }
}
