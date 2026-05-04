using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] ResourceManager m_manaManager;
    [SerializeField] TowerUIInfo[] m_info;
    [SerializeField] ITowerStats[] m_preparedTowers;
    [SerializeField] GameObject m_towerPrefab;

    private void Start()
    {
        for (int i = 0; i < m_preparedTowers.Length; i++) {
            m_info[i].tower = m_preparedTowers[i];
        }
    }

    public void addTower(Cell _cell, int _tower)
    {
        if (m_manaManager.canAfford(m_preparedTowers[_tower].cost))
        {
            m_manaManager.reduceResource(m_preparedTowers[_tower].cost);
            var instance = Instantiate(m_towerPrefab, transform);
            var tower = instance.GetComponent<Tower>();
            tower.initTowerStats(m_preparedTowers[_tower]);
            _cell.addTower(tower);
        } else
        {
            Debug.LogError("Not enough mana");
        }
    }

    public ITowerStats selectTower(int _index)
    {
        m_info[_index].setSelected();
        return m_preparedTowers[_index];
    }

    public void deselectTower(int _index)
    {
        m_info[_index].removeSelected();
    }
}
