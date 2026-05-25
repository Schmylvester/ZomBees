using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public struct IEnemyStats
{
    public string name;
    // what shows in the info box when I am hovered
    public string description;
    // how fast I move
    public float moveSpeed;
    // how much health I have
    public int health;
    // index of the sprite in the sprite manager
    public int spriteIndex;
    // damage I do to the palace/tower when I reach it
    public int damage;
    // money I drop on death
    public int yield;
    // percentage of non-pierce damage that I resist
    public int armour;
}

public class Enemy : MonoBehaviour
{
    public delegate void Defeat(IEnemyStats _stats);
    public Defeat onDefeat;

    [SerializeField] float m_healthBarOffset;
    [SerializeField] SpriteRenderer m_spriteRenderer;
    [SerializeField] Animator m_animationController;
    [SerializeField] GameObject m_healthBarPrefab;
    // I avoid getting within this distance of another ant
    [SerializeField] float m_politeness;
    [SerializeField] float m_maxSpriteOffset;
    [SerializeField] float m_deathTime = 0;
    float m_deathTimer;
    ConflictResolutionManager m_conflictResolutionManager;
    Transform m_healthBar;
    ResourceManager m_healthManager;
    List<Cell> m_path = new();
    List<Enemy> m_others = new();
    IEnemyStats m_stats;
    public IEnemyStats stats { get { return m_stats; } }

    public List<Cell> path { set { m_path = value; } }
    bool m_active = false;
    public bool active { get { return m_active; } set { m_active = value; } }

    public void takeDamage(int _power, int pierce)
    {
        if (m_healthManager)
        {
            var untaxableDamage = _power * (pierce / 100f);
            var taxableDamage = _power - untaxableDamage;
            var armourMod = (100f - m_stats.armour) / 100;
            var damage = Mathf.Max((int)(untaxableDamage + (taxableDamage * armourMod)), 1);
            m_healthManager.reduceResource(damage, true);
        }
    }

    private void Start()
    {
        var canvas = FindAnyObjectByType<Canvas>();
        m_healthBar = Instantiate(m_healthBarPrefab, canvas.transform).transform;
        syncHealthBar();
        m_healthManager = m_healthBar.GetComponent<ResourceManager>();
        m_healthManager.setInitResource(m_stats.health);
        m_healthManager.onResourceEmpty += markForDeath;

        var direction = new Vector3(Random.value - 0.5f, Random.value - 0.5f).normalized;
        var offset = direction * m_maxSpriteOffset;
        m_spriteRenderer.transform.position += offset;
    }

    public void initConflictResolution(ConflictResolutionManager _res)
    {
        m_conflictResolutionManager = _res;
    }

    public void initStats(IEnemyStats _stats)
    {
        m_stats = _stats;

        // instead of this, we should add component for the appt animation controller
        m_spriteRenderer.sprite = GameManager.instance.spriteManager.getEnemySprite(m_stats.spriteIndex);
        if (m_healthManager)
            m_healthManager.setInitResource(m_stats.health);
        m_deathTimer = -1;
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
        if (markedForDeath())
        {
            m_deathTimer -= Time.deltaTime;
            if (m_deathTimer < 0)
            {
                defeat();
            }
            return;
        }
        syncHealthBar();
        if (m_path.Count == 0)
        {
            return;
        }
        var distanceToTarget = getDistanceToCurrentTargetCell();
        bool wait = false;
        foreach (var other in m_others)
        {
            if (Vector3.Distance(transform.position, other.transform.position) < m_politeness)
            {
                wait = !m_conflictResolutionManager.resolveEnemyConflict(this, other);
            }
        }
        if (!wait)
        {
            var target = m_path[0].transform.position;
            transform.position += (target - transform.position).normalized * m_stats.moveSpeed * Time.deltaTime;
        }
        if (distanceToTarget < m_stats.moveSpeed * Time.deltaTime)
        {
            arriveAtTargetCell();
        }
    }

    void arriveAtTargetCell()
    {
        var prevTargetPos = m_path[0].transform.position;
        m_path.RemoveAt(0);
        if (m_path.Count == 0)
        {
            GameManager.instance.playerHealthManager.reduceResource(m_stats.damage, true);
            active = false;
        }
        else
        {
            var newTargetPos = m_path[0].transform.position;
            m_spriteRenderer.flipX = newTargetPos.x > prevTargetPos.x;
            var yMove = 0;
            if (newTargetPos.y > prevTargetPos.y)
            {
                yMove = 1;
            } else if (newTargetPos.y < prevTargetPos.y)
            {
                yMove = -1;
            }
            m_animationController.SetInteger("yMove", yMove);
        }
    }

    void syncHealthBar()
    {
        m_healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + (Vector3.up * m_healthBarOffset));
    }

    void markForDeath()
    {
        if (!markedForDeath())
        {
            m_deathTimer = m_deathTime;
        }
    }

    public bool markedForDeath()
    {
        return m_deathTimer >= 0;
    }

    void defeat()
    {
        onDefeat?.Invoke(m_stats);

        active = false;
    }


    /** this cell just got blocked, do I need to find another way? */
    public void checkFindAlternativePaths(Cell _cell, Pathfinder _pathfinder)
    {
        if (m_path.Contains(_cell))
        {
            m_path = _pathfinder.findPath(m_path[0], (c) => c.getBase(), _cell);
            if (m_path.Count == 0)
            {
                markForDeath();
            }
        }
    }

    private void OnDestroy()
    {
        m_healthManager.onResourceEmpty -= markForDeath;
        if (m_healthBar)
            Destroy(m_healthBar.gameObject);
    }

    public float getDistanceToCurrentTargetCell()
    {
        if (m_path.Count == 0)
        {
            return 0;
        }
        var target = m_path[0].transform.position;
        return Vector3.Distance(transform.position, target);
    }
}
