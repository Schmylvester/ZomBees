using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject m_healthBarPrefab;
    Transform m_healthBar;
    HealthManager m_healthManager;
    [SerializeField] float m_moveSpeed;
    List<Cell> m_path = new();
    public List<Cell> path { set { m_path = value; } }
    bool m_markedForDeath = false;

    public void takeDamage(int _damage)
    {
        m_healthManager.takeDamage(_damage);
    }

    private void Start()
    {
        var canvas = FindAnyObjectByType<Canvas>();
        m_healthBar = Instantiate(m_healthBarPrefab, canvas.transform).transform;
        m_healthBar.localScale = new Vector3(0.05f, 0.4f);
        m_healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position) + (Vector3.up * 20);
        m_healthManager = m_healthBar.GetComponent<HealthManager>();
        m_healthManager.onHealthEmpty += () => m_markedForDeath = true;
    }

    private void Update()
    {
        if (m_markedForDeath)
        {
            Destroy(gameObject);
            return;
        }
        m_healthBar.transform.position = Camera.main.WorldToScreenPoint(transform.position) + (Vector3.up * 20);
        if (m_path.Count == 0)
        {
            return;
        }
        var target = m_path[0].transform.position;
        transform.position += (target - transform.position).normalized * m_moveSpeed * Time.deltaTime;
        if (Vector3.Distance(transform.position, target) < m_moveSpeed * Time.deltaTime)
        {
            m_path.RemoveAt(0);
            if (m_path.Count == 0)
            {
                GameManager.instance.playerHealthManager.takeDamage(3);
                m_markedForDeath = true;
            }
        }
    }

    private void OnDestroy()
    {
        if (m_healthBar)
            Destroy(m_healthBar.gameObject);
    }
}
