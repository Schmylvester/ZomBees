using UnityEngine;
using UnityEngine.InputSystem;

public enum EInputState
{
    Idle,
    PlaceTower,
}

public class InputHandler : MonoBehaviour
{
    [SerializeField] InputActionReference m_click;
    [SerializeField] GridManager m_gridManager;
    [SerializeField] TowerManager m_towerManager;
    EInputState m_activeState = EInputState.Idle;
    Cell m_hoveredCell = null;

    private void Awake()
    {
        m_click.action.performed += onClick;
    }

    private void OnDestroy()
    {
        m_click.action.performed -= onClick;
    }

    void onClick(InputAction.CallbackContext _context)
    {
        if (m_hoveredCell)
        {
            m_towerManager.addTower(m_hoveredCell);
        }
    }

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
                m_hoveredCell.addHighlight();
            }
        }
    }
}
