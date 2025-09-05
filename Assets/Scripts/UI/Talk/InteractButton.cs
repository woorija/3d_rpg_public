using UnityEngine;
using UnityEngine.UI;

public class InteractButton : MonoBehaviour
{
    Button button;
    public int interactKeyNumber { get; private set; }
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public void onClick()
    {
        button.onClick.Invoke();
    }
    public void SetInteract(int _index)
    {
        interactKeyNumber = _index;
    }
    public void DeleteInteract()
    {
        interactKeyNumber = -1;
    }
}
