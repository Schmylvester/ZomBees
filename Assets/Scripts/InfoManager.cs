using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InfoManager : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    void Update()
    {
        var infoText = "";
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));
        if (hit.collider != null)
        {
            var info = hit.collider.GetComponent<Info>();
            if (info)
            {
                infoText = info.getInfo();
            }
        }

        if (infoText != text.text)
        {
            text.text = infoText;
        }
    }
}
