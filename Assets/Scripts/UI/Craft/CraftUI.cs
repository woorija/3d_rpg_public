using UnityEngine;

public class CraftUI : MonoBehaviour, ICloseable
{
    [SerializeField] CraftList craftList;
    [SerializeField] CraftMaterialsUI craftMaterialsUI;
    [SerializeField] CraftItemInformation resultInformation;
    [SerializeField] CraftButton craftButton;
    private void Start()
    {
        craftList.SetList();
        craftMaterialsUI.Init();
        resultInformation.SetNull();
        craftButton.gameObject.SetActive(false);
    }
    public void OpenUI()
    {
        GameManager.Instance.GameModeChange(GameMode.ForcedUIMode);
        UIManager.Instance.OpenUI(this);
        gameObject.SetActive(true);
    }
    public void SetCraftUI(int _index)
    {
        craftMaterialsUI.SetInformation(_index);
        resultInformation.SetData(CraftDataBase.CraftDB[_index].resultItem);
        resultInformation.SetText(CraftType.Result);
        craftButton.gameObject.SetActive(true);
        craftButton.SetOnClickButton(_index);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }
}
