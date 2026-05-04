using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public abstract class UIInfo : Info, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void PointerEnterHandler(UIInfo _);
    public delegate void PointerExitHander();
    public PointerEnterHandler onPointerEnter;
    public PointerExitHander onPointerExit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke();
    }
}
