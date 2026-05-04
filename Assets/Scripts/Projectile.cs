using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_renderer;
    [SerializeField] float m_projectileTime;

    public IEnumerator fire(Transform _target)
    {
        float t = 0;
        m_renderer.enabled = true;
        var targetPos = _target.position;
        while (t < m_projectileTime)
        {
            transform.position = Vector3.Lerp(transform.parent.position, targetPos, Mathf.Min(t / m_projectileTime, 1));
            t += Time.deltaTime;
            if (_target)
            {
                targetPos = _target.position;
            }
            yield return null;
        }
        m_renderer.enabled = false;

        yield return null;
    }
}
