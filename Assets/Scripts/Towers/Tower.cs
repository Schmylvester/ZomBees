using UnityEngine;

[System.Serializable]
public struct ITowerStats
{
    public string name;
    // what shows in the info box when I am hovered
    public string description;
    // distance that an enemy must be within for me to fire at them
    public float range;
    // shots per second
    public float fireRate;
    // amount of damage done to an enemy with 0 armour
    public int power;
    // index of the sprite in the sprite manager
    public int spriteIndex;
    // if this tower is in a cell, enemies can not cross it
    public bool blocksCell;
    public int cost;
    // percetage of damage that ignores armour
    public int pierce;

    public IInfo getInfo()
    {
        return new IInfo
        {
            name = name,
            description = description,
            stats = new IStatDisplay[]
            {
                new() {
                    icon = "range",
                    value = range.ToString(),
                },
                new() {
                    icon = "power",
                    value = power.ToString(),
                },
                new() {
                    icon = "fireRate",
                    value = fireRate.ToString(),
                },
                new() {
                    icon = "money",
                    value = cost.ToString(),
                },
                new() {
                    icon = "blocksCell",
                    value = blocksCell.ToString(),
                },
                new() {
                    icon = "pierce",
                    value = pierce.ToString(),
                }
            }
        };
    }
}

public class Tower : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_spriteRenderer;
    [SerializeField] SpriteRenderer m_rangeIndicator;
    [SerializeField] bool m_active = false;
    ITowerStats m_stats;
    float m_lastFire = 0f;
    Enemy m_targetEnemy = null;
    // the range indicator sprite needs to be scaled by this much to match up with the tower range
    float m_rangeScale = 28f;

    void Update()
    {
        if (!m_active)
        {
            return;
        }
        m_lastFire += Time.deltaTime;
        if (m_targetEnemy == null)
        {
            var enemies = GameManager.instance.enemyManager.enemies;
            if (enemies.Count > 0)
            {
                enemies.Sort((Enemy a, Enemy b) =>
                {
                    if (!a) return 1;
                    if (!b) return -1;
                    var aDist = Vector3.Distance(transform.position, a.transform.position);
                    var bDist = Vector3.Distance(transform.position, b.transform.position);
                    return aDist < bDist ? -1 : 1;
                });
                foreach (var enemy in enemies) {
                    if (!enemy.markedForDeath())
                    {
                        if (Vector3.Distance(enemy.transform.position, transform.position) <= m_stats.range)
                        {
                            m_targetEnemy = enemy;
                        }
                        break;
                    }
                }
            }
        }
        if (m_targetEnemy)
        {
            if (Vector3.Distance(m_targetEnemy.transform.position, transform.position) > m_stats.range || m_targetEnemy.markedForDeath())
            {
                m_targetEnemy = null;
            } else
            {
                if (m_lastFire >= 1f / m_stats.fireRate)
                {
                    StartCoroutine(GameManager.instance.projectilePool.getFreeProjectile().fire(transform.position, m_targetEnemy.transform.position));
                    m_targetEnemy.takeDamage(m_stats.power, m_stats.pierce);
                    m_lastFire = 0;
                }
            }
        }
    }

    public void initTowerStats(ITowerStats _stats)
    {
        m_stats = _stats;
        m_spriteRenderer.sprite = GameManager.instance.spriteManager.getTowerSprite(m_stats.spriteIndex);
    }

    public IInfo getTowerInfo()
    {
        return m_stats.getInfo();
    }

    public void addHighlight()
    {
        if (m_rangeIndicator)
        {
            m_rangeIndicator.enabled = true;
            m_rangeIndicator.transform.localScale = m_stats.range * m_rangeScale * Vector3.one;
        }
    }

    public void removeHighlight()
    {
        if (m_rangeIndicator)
        {
            m_rangeIndicator.enabled = false;
        }
    }
}
