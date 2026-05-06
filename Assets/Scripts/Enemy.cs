using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IEnemyStats
{
    public string name;
    public string description;
    public float moveSpeed;
    public int health;
    public int spriteIndex;
    public int yield;
}

public class Enemy : MonoBehaviour
{
    public delegate void Defeat(IEnemyStats _stats);
    public Defeat onDefeat;

    [SerializeField] SpriteRenderer m_spriteRenderer;
    [SerializeField] GameObject m_healthBarPrefab;
    Transform m_healthBar;
    ResourceManager m_healthManager;
    List<Cell> m_path = new();
    IEnemyStats m_stats;
    public IEnemyStats stats { get { return m_stats; } }

    public List<Cell> path { set { m_path = value; } }

    public void takeDamage(int _damage)
    {
        if (m_healthManager)
            m_healthManager.reduceResource(_damage, true);
    }

    private void Start()
    {
        var canvas = FindAnyObjectByType<Canvas>();
        m_healthBar = Instantiate(m_healthBarPrefab, canvas.transform).transform;
        m_healthBar.localScale = new Vector3(0.02f, 0.2f);
        m_healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position) + (Vector3.up * 20);
        m_healthManager = m_healthBar.GetComponent<ResourceManager>();
        m_healthManager.setInitResource(m_stats.health);
        m_healthManager.onResourceEmpty += defeat;
    }

    public void initStats(IEnemyStats _stats)
    {
        m_stats = _stats;
        m_spriteRenderer.sprite = GameManager.instance.spriteManager.getEnemySprite(m_stats.spriteIndex);
        if (m_healthManager)
            m_healthManager.setInitResource(m_stats.health);
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
                GameManager.instance.playerHealthManager.reduceResource(3, true);
                Destroy(gameObject);
            }
        }
    }

    void defeat()
    {
        onDefeat?.Invoke(m_stats);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        m_healthManager.onResourceEmpty -= defeat;
        if (m_healthBar)
            Destroy(m_healthBar.gameObject);
    }
}
