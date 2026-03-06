using TMPro;
using UnityEngine;

public class NpcNameTag : MonoBehaviour
{
    [SerializeField] TMP_Text nameTag;

    private void Awake()
    {
        nameTag.alpha = 0f;
    }
    private void Update()
    {
        nameTag.transform.rotation = Camera.main.transform.rotation;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Layers.Player)
        {
            nameTag.alpha = 1f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Layers.Player)
        {
            nameTag.alpha = 0f;
        }
    }

#if UNITY_EDITOR
    public void SetNameTag(string _str)
    {
        nameTag.text = _str;
    }
#endif
}
