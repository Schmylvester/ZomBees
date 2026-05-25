using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_renderer;
    [SerializeField] float m_projectileTime;

    bool m_inUse = false;
    public bool isInUse() { return m_inUse; }

    public IEnumerator fire(Vector3 _from, Vector3 _to)
    {
        m_inUse = true;
        transform.position = _from;
        float t = 0;
        m_renderer.enabled = true;
        while (t < m_projectileTime)
        {
            transform.position = Vector3.Lerp(_from, _to, Mathf.Min(t / m_projectileTime, 1));
            t += Time.deltaTime;
            yield return null;
        }
        m_renderer.enabled = false;

        m_inUse = false;
        yield return null;
    }
}
