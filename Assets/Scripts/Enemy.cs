using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IEnemyStats
{
    public string description;
    public float moveSpeed;
    public int health;
    public int spriteIndex;
}

public class Enemy : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_spriteRenderer;
    [SerializeField] GameObject m_healthBarPrefab;
    Transform m_healthBar;
    HealthManager m_healthManager;
    List<Cell> m_path = new();
    IEnemyStats m_stats;
    public IEnemyStats stats { get { return m_stats; } }

    public List<Cell> path { set { m_path = value; } }

    public void takeDamage(int _damage)
    {
        if (m_healthManager)
            m_healthManager.takeDamage(_damage);
    }

    private void Start()
    {
        var canvas = FindAnyObjectByType<Canvas>();
        m_healthBar = Instantiate(m_healthBarPrefab, canvas.transform).transform;
        m_healthBar.localScale = new Vector3(0.02f, 0.2f);
        m_healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position) + (Vector3.up * 20);
        m_healthManager = m_healthBar.GetComponent<HealthManager>();
        m_healthManager.setInitHealth(m_stats.health);
        m_healthManager.onHealthEmpty += () => Destroy(gameObject);
    }

    public void initStats(IEnemyStats _stats)
    {
        m_stats = _stats;
        m_spriteRenderer.sprite = GameManager.instance.spriteManager.getEnemySprite(m_stats.spriteIndex);
        if (m_healthManager)
            m_healthManager.setInitHealth(m_stats.health);
    }

    private void Update()
    {
        m_healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position) + (Vector3.up * 20);
        if (m_path.Count == 0)
        {
            return;
        }
        var target = m_path[0].transform.position;
        transform.position += (target - transform.position).normalized * m_stats.moveSpeed * Time.deltaTime;
        if (Vector3.Distance(transform.position, target) < m_stats.moveSpeed * Time.deltaTime)
        {
            m_path.RemoveAt(0);
            if (m_path.Count == 0)
            {
                GameManager.instance.playerHealthManager.takeDamage(3);
                Destroy(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        m_healthManager.onHealthEmpty -= () => Destroy(gameObject);
        if (m_healthBar)
            Destroy(m_healthBar.gameObject);
    }
}
