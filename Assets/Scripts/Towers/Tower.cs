using UnityEngine;

[System.Serializable]
public struct ITowerStats
{
    public string description;
    public float range;
    public float fireRate;
    public int power;
    public int spriteIndex;
    public bool blocksCell;
    public int cost;
}

public class Tower : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_spriteRenderer;
    [SerializeField] SpriteRenderer m_rangeIndicator;
    [SerializeField] Projectile m_projectile;
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
                if (enemies[0])
                {
                    if (Vector3.Distance(enemies[0].transform.position, transform.position) <= m_stats.range)
                    {
                        m_targetEnemy = enemies[0];
                    }
                }
            }
        }
        m_lastFire += Time.deltaTime;
        if (m_targetEnemy)
        {
            if (m_lastFire >= 1f / m_stats.fireRate)
            {
                StartCoroutine(m_projectile.fire(m_targetEnemy.transform));
                m_targetEnemy.takeDamage(m_stats.power);
                m_lastFire = 0;
            }
        }
    }

    public void initTowerStats(ITowerStats _stats)
    {
        m_stats = _stats;
        m_spriteRenderer.sprite = GameManager.instance.spriteManager.getTowerSprite(m_stats.spriteIndex);
    }

    public string getTowerInfo()
    {
        return m_stats.description;
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
