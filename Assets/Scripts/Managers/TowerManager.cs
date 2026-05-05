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
        if (m_manaManager.canAfford(m_preparedTowers[_tower].stats.cost))
        {
            m_manaManager.reduceResource(m_preparedTowers[_tower].stats.cost);
            var instance = Instantiate(m_towerPrefab, transform);
            var tower = instance.GetComponent<Tower>();
            tower.initTowerStats(m_preparedTowers[_tower].stats);
            _cell.addTower(tower);
        } else
        {
            Debug.LogError("Not enough mana");
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
}
