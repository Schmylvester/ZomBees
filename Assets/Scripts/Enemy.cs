using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float m_moveSpeed;
    List<Cell> m_path = new();
    public List<Cell> path { set { m_path = value; } }

    private void Update()
    {
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
                Debug.Log("I have arrived");
                Destroy(gameObject);
            }
        }
    }
}
