using UnityEngine;
using UnityEngine.EventSystems;


public class UIDragButton : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI최상위 오브젝트")]
    [SerializeField] RectTransform rt;
    [Header("해당UI 캔버스")]
    [SerializeField] Canvas canvas;
    [Header("드래그 제한 좌표")]
    [SerializeField] float left;
    [SerializeField] float right, up, down;
    float tempDelta; // UI와 마우스의 좌표이동량의 어긋남을 조정해주는 변수

    private void Start()
    {
        UIManager.Instance.SortReset += SortReset;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        tempDelta = 1920f / Screen.width;
        UIManager.Instance.ChangeSortOrder(canvas);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rt.anchoredPosition += eventData.delta * tempDelta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 anchoredPosition = rt.anchoredPosition;

        anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, left, right);
        anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, down, up);

        rt.anchoredPosition = anchoredPosition;
    }
    public void SortReset()
    {
        canvas.sortingOrder -= UIManager.Instance.maxSortOrder;
    }
}
