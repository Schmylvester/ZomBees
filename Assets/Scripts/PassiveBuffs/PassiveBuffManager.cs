using UnityEngine;
using System.Collections.Generic;

public class PassiveBuffManager : MonoBehaviour
{
    List<EPassiveBuff> m_playerBuffs = new();

    public bool playerHasBuff(EPassiveBuff _buff)
    {
        return m_playerBuffs.Contains(_buff);
    }

    public void addBuff(EPassiveBuff _buff)
    {
        if (!playerHasBuff(_buff))
        {
            m_playerBuffs.Add(_buff);
        }
    }

    public void removeBuff(EPassiveBuff _buff)
    {
        if (playerHasBuff(_buff))
        {
            m_playerBuffs.Remove(_buff);
        }
    }
}
