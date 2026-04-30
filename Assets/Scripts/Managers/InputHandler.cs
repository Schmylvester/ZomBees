using UnityEngine;
using UnityEngine.InputSystem;

public enum EInputState
{
    Idle,
    PlaceTower,
}

public class InputHandler : MonoBehaviour
{
    [SerializeField] GridManager m_gridManager;
    EInputState m_activeState = EInputState.Idle;
    Cell m_hoveredCell = null;

    private void Update()
    {
        if (m_activeState == EInputState.Idle)
        {
            // if i click a tower, my state should change, I should have that tower ID selected
        }
        if (m_hoveredCell)
        {
            m_hoveredCell.removeHighlight();
            m_hoveredCell = null;
        }
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (hit.collider != null)
        {
            if (m_gridManager.cells.Find(c => hit.collider.gameObject == c.gameObject))
            {
                var cell = hit.collider.GetComponent<Cell>();
                m_hoveredCell = cell;
                m_hoveredCell.addHighlight(Color.green, 0.5f);
            }
        }

    }
}
