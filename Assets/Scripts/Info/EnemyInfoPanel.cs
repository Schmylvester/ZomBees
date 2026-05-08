using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

struct IInfoPanelData
{
    public GameObject gameObject;
    public Image spriteRenderer;
    public EnemyInfo enemyInfo;
}
public class EnemyInfoPanel : MonoBehaviour
{
    [SerializeField] float m_padding;
    [SerializeField] float m_previewSize;
    [SerializeField] RectTransform m_transform;
    IInfoPanelData[] m_children = null;

    private void init()
    {
        var renderers = new List<Image>();
        var info = new List<EnemyInfo>();
        for (var i = 0; i < transform.childCount; ++i)
        {
            var child = transform.GetChild(i);
            renderers.Add(child.GetComponent<Image>());
            info.Add(child.GetComponent<EnemyInfo>());
        }

        m_children = new IInfoPanelData[renderers.Count];
        for (var i = 0;i < m_children.Length; ++i) {
            m_children[i] = new()
            {
                gameObject = info[i].gameObject,
                enemyInfo = info[i],
                spriteRenderer = renderers[i]
            };
        }
    }

    public void setPreviews(int[] _enemyPool, IEnemyStats[] _stats)
    {
        if (m_children == null)
        {
            init();
        }
        for (var i = 0; i < m_children.Length; ++i)
        {
            if (i < _enemyPool.Length)
            {
                m_children[i].gameObject.SetActive(true);
                var stats = _stats[_enemyPool[i]];
                m_children[i].enemyInfo.enemy = stats;
                m_children[i].spriteRenderer.sprite = GameManager.instance.spriteManager.getEnemySprite(stats.spriteIndex);

            }
            else
            {
                m_children[i].gameObject.SetActive(false);
            }
        }

        // set rect size
        var w = _enemyPool.Length > 1 ? (m_previewSize * 2) + (m_padding * 3) : m_previewSize + m_padding * 2;
        var h = m_padding + (((1 + _enemyPool.Length) / 2) * (m_previewSize + m_padding));
        m_transform.sizeDelta = new Vector2(w, h);
    }

}
