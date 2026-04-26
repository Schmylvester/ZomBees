using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [SerializeField] GameObject m_cellObject;
    /** grid size is determined by how many cells the center is from the edge */
    [SerializeField] int m_gridSize;

    Grid m_grid;
    List<Cell> m_cells = new();

    void Start()
    {
        m_grid = GetComponent<Grid>();
        for (int y = -m_gridSize; y < m_gridSize + 1; ++y)
        {
            // calculate width of the current row
            var width = ((2 * m_gridSize) + 1) - Mathf.Abs(y);
            // calculate how far the current row is offset from x = 0
            var offset = -(m_gridSize - (Mathf.Abs(y) / 2));
            for (int x = 0; x < width; ++x) {
                var instance = Instantiate(m_cellObject);
                instance.transform.position = m_grid.CellToLocal(new Vector3Int(x + offset, y));
                var cell = instance.GetComponent<Cell>();
                if (cell != null) {
                    m_cells.Add(cell);
                    foreach (var neighbour in getNeighbourIndices(x, y))
                    {
                        cell.setNeighbour(m_cells[neighbour], true);
                    }
                }
            }
        }
    }

    private int[] getNeighbourIndices(int x, int y)
    {
        return new int[0];
    }
}
