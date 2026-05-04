using UnityEngine;

public class EnemyInfo : Info
{
    [SerializeField] Enemy m_enemy;
    public override string getInfo()
    {
        return m_enemy.stats.description;
    }
}
