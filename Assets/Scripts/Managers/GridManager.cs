using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    [SerializeField] GameObject m_cellObject;
    [SerializeField] Pathfinder m_pathfinder;
    /** grid size is determined by how many cells the center is from the edge */
    [SerializeField] int m_gridSize;
    [SerializeField] int m_baseSize;

    [SerializeField] Grid m_grid;
    List<Cell> m_cells = new();
    public List<Cell> cells {  get { return m_cells; } }
    List<Cell> m_edgeCells = new();

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

    public Cell getRandomEdgeCell()
    {
        return m_edgeCells[Random.Range(0, m_edgeCells.Count)];
    }
}
