using UnityEngine;

[System.Serializable]
struct IPreparedTowers
{
    // just for the inspector
    public string name;
    public TowerUIInfo info;
    public ITowerStats stats;
}

public class TowerManager : MonoBehaviour
{
    [SerializeField] GridManager m_gridManager;
    [SerializeField] ResourceManager m_manaManager;
    [SerializeField] IPreparedTowers[] m_preparedTowers;
    [SerializeField] GameObject m_towerPrefab;

    private void Start()
    {
        foreach (var tower in m_preparedTowers) {
            tower.info.tower = tower.stats;
        }
    }

    public void addTower(Cell _cell, int _tower)
    {
        if (canPlaceTower(_cell, _tower))
        {
            m_manaManager.reduceResource(m_preparedTowers[_tower].stats.cost);
            var instance = Instantiate(m_towerPrefab, transform);
            var tower = instance.GetComponent<Tower>();
            tower.initTowerStats(m_preparedTowers[_tower].stats);
            _cell.addTower(tower, m_preparedTowers[_tower].stats.blocksCell);
        }
    }

    public ITowerStats selectTower(int _index)
    {
        m_preparedTowers[_index].info.setSelected();
        return m_preparedTowers[_index].stats;
    }

    public void deselectTower(int _index)
    {
        m_preparedTowers[_index].info.removeSelected();
    }

    public bool canPlaceTower(Cell _cell, int _tower)
    {
        if (_cell.isEdge() || _cell.getBase())
        {
            GameManager.instance.infoManager.overrideInfo("Can't place tower here", 2f, Color.red);
            return false;
        }
       if (!m_manaManager.canAfford(m_preparedTowers[_tower].stats.cost))
        {
            GameManager.instance.infoManager.overrideInfo("Insufficient mana", 2f, Color.red);
            return false;
        }

       if (m_preparedTowers[_tower].stats.blocksCell && m_gridManager.isEssential(_cell))
        {
            GameManager.instance.infoManager.overrideInfo("Can't block this cell", 2f, Color.red);
            return false;
        }
        return true;
    }
}
