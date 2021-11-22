using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Click : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public UnityEvent PointerDown;
    public UnityEvent PointerUp;
    public UnityEvent PointerClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (PointerDown != null)
            PointerDown.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (PointerUp != null)
            PointerUp.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PointerClick != null)
            PointerClick.Invoke();
    }
}