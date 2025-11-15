using System.Collections.Generic;
using UnityEngine;

public class CraftList : MonoBehaviour
{
    [SerializeField] Transform equipTypeParent;
    [SerializeField] Transform UseableTypeParent;
    [SerializeField] Transform MiscTypeParent;

    public void SetList()
    {
        List<int> craftIds = new List<int>(CraftDataBase.CraftDB.Keys);
        CraftSettingButton temp;

        for(int i = 0; i < craftIds.Count; i++)
        {
            temp = PoolManager.Instance.craftButtonPool.Get();
            temp.SetIndex(craftIds[i]);

            int id = temp.GetItemId();
            if(id >= 300000000)
            {
                temp.gameObject.transform.SetParent(MiscTypeParent);
            }
            else if(id >= 200000000)
            {
                temp.gameObject.transform.SetParent(UseableTypeParent);
            }
            else
            {
                temp.gameObject.transform.SetParent(equipTypeParent);
            }
            temp.SetData();
        }
    }
}
