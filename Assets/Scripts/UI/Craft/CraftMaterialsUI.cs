using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftMaterialsUI : MonoBehaviour
{
    [SerializeField] CraftItemInformation[] craftMaterialsInfor;
    List<CraftItemData> craftMaterials;
    public void SetInformation(int _index)
    {
        craftMaterials = CraftDataBase.CraftDB[_index].materialItems;
        for(int i = 0; i < craftMaterials.Count; i++)
        {
            craftMaterialsInfor[i].SetData(craftMaterials[i]);
            craftMaterialsInfor[i].SetText(CraftType.Materials);
        }
        for(int i = craftMaterials.Count + 1; i < 8; i++)
        {
            craftMaterialsInfor[i].SetNull();
        }
    }
    public void Init()
    {
        for(int i = 0; i< craftMaterialsInfor.Length; i++)
        {
            craftMaterialsInfor[i].SetNull();
        }
    }
}
