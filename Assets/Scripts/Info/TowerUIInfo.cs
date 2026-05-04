using UnityEngine;
using UnityEngine.UI;

public class TowerUIInfo : UIInfo
{
    ITowerStats m_tower;
    public ITowerStats tower {
        set
        {
            m_tower = value;
            GetComponent<Image>().sprite = GameManager.instance.spriteManager.getTowerSprite(m_tower.spriteIndex);
        }
    }

    public override string getInfo()
    {
        return m_tower.description;
    }
}
