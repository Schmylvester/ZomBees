using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] GameObject m_towerPrefab;
    public void addTower(Cell _cell)
    {
        var instance = Instantiate(m_towerPrefab, transform);
        var tower = instance.GetComponent<Tower>();
        _cell.addTower(tower);
    }
}
