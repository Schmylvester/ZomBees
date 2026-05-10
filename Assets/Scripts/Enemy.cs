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
    public int damage;
    public int yield;
}

public class Enemy : MonoBehaviour
{
    public delegate void Defeat(IEnemyStats _stats);
    public Defeat onDefeat;

    [SerializeField] float m_healthBarOffset;
    [SerializeField] SpriteRenderer m_spriteRenderer;
    [SerializeField] GameObject m_healthBarPrefab;
    // I avoid getting within this distance of another ant
    [SerializeField] float m_politeness;
    Transform m_healthBar;
    ResourceManager m_healthManager;
    List<Cell> m_path = new();
    List<Enemy> m_others = new();
    IEnemyStats m_stats;
    public IEnemyStats stats { get { return m_stats; } }

    public List<Cell> path { set { m_path = value; } }
    bool m_active = false;
    public bool active { get { return m_active; } set { m_active = value; } }

    public void takeDamage(int _damage)
    {
        if (m_healthManager)
            m_healthManager.reduceResource(_damage, true);
    }

    private void Start()
    {
        var canvas = FindAnyObjectByType<Canvas>();
        m_healthBar = Instantiate(m_healthBarPrefab, canvas.transform).transform;
        syncHealthBar();
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

    public void addOther(Enemy other, bool reciprocate = false)
    {
        if (other != this && m_others.Find(o => o == other) == null)
        {
            m_others.Add(other);
            if (reciprocate)
            {
                other.addOther(this);
            }
        }
    }

    private void Update()
    {
        syncHealthBar();
        if (m_path.Count == 0)
        {
            return;
        }
        var target = m_path[0].transform.position;
        var distanceToTarget = Vector3.Distance(transform.position, target);
        bool wait = false;
        foreach (var other in m_others)
        {
            if (Vector3.Distance(transform.position, other.transform.position) < m_politeness)
            {
                var otherDistanceToTarget = Vector3.Distance(other.transform.position, target);
                if (otherDistanceToTarget < distanceToTarget) {
                    wait = true;
                    break;
                }
                if (otherDistanceToTarget == distanceToTarget)
                {
                    wait = Random.value < 0.5f;
                }
            }
        }
        if (!wait)
        {
            transform.position += (target - transform.position).normalized * m_stats.moveSpeed * Time.deltaTime;
        }
        if (distanceToTarget < m_stats.moveSpeed * Time.deltaTime)
        {
            m_path.RemoveAt(0);
            if (m_path.Count == 0)
            {
                GameManager.instance.playerHealthManager.reduceResource(m_stats.damage, true);
                active = false;
            }
        }
    }

    void syncHealthBar()
    {
        m_healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + (Vector3.up * m_healthBarOffset));
    }

    void defeat()
    {
        onDefeat?.Invoke(m_stats);

        active = false;
    }

    private void OnDestroy()
    {
        m_healthManager.onResourceEmpty -= defeat;
        if (m_healthBar)
            Destroy(m_healthBar.gameObject);
    }
}
