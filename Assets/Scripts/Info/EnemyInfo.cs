using UnityEngine;

public class EnemyInfo : UIInfo
{
    IEnemyStats m_enemy;
    public IEnemyStats enemy { set { m_enemy = value; } }
    public override string getInfo()
    {
        return m_enemy.description;
    }
}
