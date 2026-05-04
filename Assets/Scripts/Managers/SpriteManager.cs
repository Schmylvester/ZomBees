using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [SerializeField] Sprite[] m_towers;
    [SerializeField] Sprite[] m_enemies;

    public Sprite getTowerSprite(int index) {  return m_towers[index]; }
    public Sprite getEnemySprite(int index) {return m_enemies[index]; }
}
