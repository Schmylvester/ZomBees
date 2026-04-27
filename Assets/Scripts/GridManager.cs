using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    [SerializeField] GameObject m_cellObject;
    /** grid size is determined by how many cells the center is from the edge */
    [SerializeField] int m_gridSize;
    [SerializeField] int m_baseSize;

    Grid m_grid;
    List<Cell> m_cells = new();

    void Start()
    {
        m_grid = GetComponent<Grid>();

        var xBuffer = (((m_gridSize * 2) + 1) - ((m_baseSize * 2) + 1)) / 2;
        Debug.Log(xBuffer);
        for (int y = -m_gridSize; y < m_gridSize + 1; ++y)
        {
            // calculate width of the current row
            var width = ((2 * m_gridSize) + 1) - Mathf.Abs(y);
            // calculate how far the current row is offset from x = 0
            var offset = -(m_gridSize - (Mathf.Abs(y) / 2));
            for (int x = 0; x < width; ++x) {
                var instance = Instantiate(m_cellObject, transform);
                instance.name = y.ToString() + "." + x.ToString();
                instance.transform.position = m_grid.CellToLocal(new Vector3Int(x + offset, y));
                var cell = instance.GetComponent<Cell>();
                if (cell != null) {
                    m_cells.Add(cell);
                    if (Mathf.Abs(y) <= m_baseSize && x >= xBuffer && x < width - xBuffer)
                    {
                        cell.setBase();
                    }
                }
            }
        }
    }
}
