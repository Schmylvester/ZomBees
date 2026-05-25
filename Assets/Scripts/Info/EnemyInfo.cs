using UnityEngine;

public class EnemyInfo : UIInfo
{
    IEnemyStats m_enemy;
    public IEnemyStats enemy { set { m_enemy = value; } }
    public override IInfo getInfo()
    {
        return new IInfo{
            name = m_enemy.name,
            description = m_enemy.description,
            stats = new IStatDisplay[]
            {
                new() { icon = "moveSpeed", value = m_enemy.moveSpeed.ToString() },
                new() { icon = "health", value = m_enemy.health.ToString() },
                new() { icon = "money", value = m_enemy.yield.ToString() },
            }
        };
    }
}
