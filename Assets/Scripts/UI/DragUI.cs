using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DragUI : MonoBehaviour
{
    Image iconImage;
    Color baseColor = new Color(1, 1, 1, 0);
    Color transparentColor = new Color(1, 1, 1, 0.5f);
    private void Awake()
    {
        iconImage = GetComponent<Image>();
    }
    private void Start()
    {
        iconImage.color = baseColor;
    }
    private void Update()
    {
        if (DragManager.Instance.isClick)
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }
    public void SetImage(Sprite _sprite)
    {
        iconImage.sprite = _sprite;
        iconImage.color = transparentColor;
    }
    public void ResetImage()
    {
        iconImage.sprite = null;
        iconImage.color = baseColor;
    }
}
