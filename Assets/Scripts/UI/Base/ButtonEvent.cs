using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonEvent : MonoBehaviour, IPointerClickHandler
{
    [Header("좌클릭 이벤트 설정하기")]
    [SerializeField] protected UnityEvent leftClickEvent;
    [Header("우클릭 이벤트 설정하기")]
    [SerializeField] protected UnityEvent rightClickEvent;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            leftClickEvent.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            rightClickEvent.Invoke();
        }
    }
}
