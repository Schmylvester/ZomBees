using UnityEngine;
using UnityEngine.UI;

public class TowerUIInfo : UIInfo
{
    Image m_image;
    ITowerStats m_tower;
    Sprite m_sprite;
    [SerializeField] Sprite m_cancel;

    private void Awake()
    {
        m_image = GetComponent<Image>();
    }

    public ITowerStats tower {
        set
        {
            m_tower = value;
            m_sprite = GameManager.instance.spriteManager.getTowerSprite(m_tower.spriteIndex);
            m_image.sprite = m_sprite;
        }
    }

    public override string getInfo()
    {
        return m_tower.description;
    }

    public void setSelected()
    {
        m_image.sprite = m_cancel;
    }

    public void removeSelected()
    {
        m_image.sprite = m_sprite;
    }
}
