using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] EnemyManager m_enemyManager;
    public EnemyManager enemyManager { get { return m_enemyManager; } }
    [SerializeField] ResourceManager m_playerHealthManager;
    public ResourceManager playerHealthManager { get { return m_playerHealthManager; } }
    [SerializeField] SpriteManager m_spriteManager;
    public SpriteManager spriteManager { get { return m_spriteManager; } }
    [SerializeField] InfoManager m_infoManager;
    public InfoManager infoManager { get { return m_infoManager; } }

    [SerializeField] ProjectilePool m_projectilePool;
    public ProjectilePool projectilePool { get { return m_projectilePool; } }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
