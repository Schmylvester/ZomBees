using UnityEngine;
using UnityEngine.InputSystem;

public enum EInputState
{
    Idle,
    PlaceTower,

    LevelDesign,
}

public class InputHandler : MonoBehaviour
{
    [SerializeField] InputActionReference m_click;
    [SerializeField] GridManager m_gridManager;
    [SerializeField] TowerManager m_towerManager;
    [SerializeField] LevelDesignManager m_levelDesignManager;
    /** sample tower gives player a preview of tower placement and range */
    [SerializeField] Tower m_sampleTower;
    [SerializeField] EInputState m_activeState = EInputState.Idle;
    Cell m_hoveredCell = null;
    int m_selectedTower = -1;

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
            switch (m_activeState)
            {
                case EInputState.PlaceTower:
                    m_towerManager.addTower(m_hoveredCell, m_selectedTower);
                    deselectTower();
                    break;
                case EInputState.LevelDesign:
                    m_levelDesignManager.toggleCell(m_hoveredCell);
                    break;
                default:
                    break;
            }
        }
    }

    private void Update()
    {
        if (m_hoveredCell)
        {
            m_hoveredCell.removeHighlight();
            m_hoveredCell = null;
        }
        var mousePos = Mouse.current.position.ReadValue();
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(mousePos));
        if (hit.collider != null)
        {
            if (m_gridManager.wasHit(hit.collider))
            {
                var cell = hit.collider.GetComponent<Cell>();
                m_hoveredCell = cell;
                m_hoveredCell.addHighlight();
                if (m_activeState == EInputState.PlaceTower)
                {
                    m_sampleTower.transform.position = m_hoveredCell.transform.position;
                }
            }
        }
    }

    public void onTowerSelected(int _index)
    {
        if (m_selectedTower == _index)
        {
            deselectTower();
        }
        else
        {
            m_activeState = EInputState.PlaceTower;
            m_selectedTower = _index;
            var towerStats = m_towerManager.selectTower(_index);
            m_sampleTower.gameObject.SetActive(true);
            m_sampleTower.initTowerStats(towerStats);
            m_sampleTower.addHighlight();
            // send it off screen until they highlight a cell
            m_sampleTower.transform.position = Vector3.one * 200;
        }
    }

    void deselectTower()
    {
        m_sampleTower.gameObject.SetActive(false);
        m_activeState = EInputState.Idle;
        m_towerManager.deselectTower(m_selectedTower);
        m_selectedTower = -1;
    }
}
