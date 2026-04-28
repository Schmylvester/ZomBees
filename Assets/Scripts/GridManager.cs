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

    Grid m_grid;
    List<Cell> m_cells = new();

    void Start()
    {
        m_grid = GetComponent<Grid>();

        var xBuffer = (((m_gridSize * 2) + 1) - ((m_baseSize * 2) + 1)) / 2;
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
        assignNeighbours();
    }

    void assignNeighbours()
    {
        for (int i = 1; i < m_cells.Count; ++i) {
            var cellCoords = m_cells[i].gameObject.name.Split('.');
            var y = int.Parse(cellCoords[0]);
            var x = int.Parse(cellCoords[1]);
            var leftNeigbour = m_cells.Find(c => c.name == y + "." + (x - 1));
            if (leftNeigbour) { m_cells[i].setNeighbour(leftNeigbour, true); }
            var bottomLeftNeighbour = m_cells.Find(c => c.name == (y - 1) + "." + (y <= 0 ? x - 1 : x));
            if (bottomLeftNeighbour) { m_cells[i].setNeighbour(bottomLeftNeighbour, true); }
            var bottomRightNeighbour = m_cells.Find(c => c.name == (y - 1) + "." + (y <= 0 ? x : x + 1));
            if (bottomRightNeighbour) { m_cells[i].setNeighbour(bottomRightNeighbour, true); }
        }
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (hit.collider != null)
        {
            if (m_cells.Find(c => hit.collider.gameObject == c.gameObject))
            {
                var path = m_pathfinder.findPath(hit.collider.gameObject.GetComponent<Cell>(), TargetType.Base);
                foreach (var node in path)
                {
                    node.highlight();
                }
            }
        }
    }
}
