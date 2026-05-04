using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] EnemyManager m_enemyManager;
    public EnemyManager enemyManager {  get { return m_enemyManager; } }
    [SerializeField] HealthManager m_playerHealthManager;
    public HealthManager playerHealthManager { get { return m_playerHealthManager; } }
    [SerializeField] SpriteManager m_spriteManager;
    public SpriteManager spriteManager {  get { return m_spriteManager; } }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
}
