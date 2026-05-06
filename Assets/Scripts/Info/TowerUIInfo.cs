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

    public override IInfo getInfo()
    {
        return new IInfo
        {
            name = m_tower.name,
            description = m_tower.description,
            stats = new IStatDisplay[]
            {
                new() {
                    icon = "range",
                    value = m_tower.range.ToString(),
                },
                new() {
                    icon = "power",
                    value = m_tower.power.ToString(),
                },
                new() {
                    icon = "fireRate",
                    value = m_tower.fireRate.ToString(),
                },
                new() {
                    icon = "range",
                    value = m_tower.range.ToString(),
                },
                new() {
                    icon = "blocksCell",
                    value = m_tower.blocksCell.ToString(),
                }
            }
        };
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
