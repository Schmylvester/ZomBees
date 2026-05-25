using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] GameObject m_projectilePrefab;
    Projectile[] m_projectilePool;

    private void Start()
    {
        m_projectilePool = GetComponentsInChildren<Projectile>();
    }

    public Projectile getFreeProjectile()
    {
        foreach (var p in m_projectilePool)
        {
            if (!p.isInUse())
            {
                return p;
            }
        }

        var tempPool = new Projectile[m_projectilePool.Length + 1];
        for (int i = 0; i < m_projectilePool.Length; ++i)
        {
            tempPool[i] = m_projectilePool[i]; 
        }

        var newInstance = Instantiate(m_projectilePrefab, transform).GetComponent<Projectile>();
        tempPool[tempPool.Length - 1] = newInstance;

        m_projectilePool = tempPool;
        return newInstance;
    }
}
