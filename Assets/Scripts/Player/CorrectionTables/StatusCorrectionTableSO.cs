using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseStatusCorrection
{
    public StatusType statusType;
    public List<SubStatusCorrection> correctionList;
}
[Serializable]
public class SubStatusCorrection
{
    public StatusType statusType;
    public float valuePerPoint;
}
public struct StatusCorrection
{
    public StatusType statusType;
    public float valuePerPoint;
    public StatusCorrection(StatusType _statusType, float _valuePerPoint)
    {
        statusType = _statusType;
        valuePerPoint = _valuePerPoint;
    }
}
[CreateAssetMenu(fileName = "StatusCorrectionTableSO", menuName = "ScriptableObjects/StatusCorrectionTableSO")]
public class StatusCorrectionTableSO : ScriptableObject
{
    [SerializeField] string className;

    public int[] levelUpStatus = new int[9];
    [SerializeField] List<BaseStatusCorrection> statusList;
    public void InitBaseStatusTable(Dictionary<StatusType, BaseStatusCorrection> _baseStatusTable)
    {
        _baseStatusTable.Clear();
        for (int i = 0; i < statusList.Count; i++)
        {
            var baseStatusCorrection = statusList[i];
            _baseStatusTable[baseStatusCorrection.statusType] = baseStatusCorrection;
        }
    }
    public void InitSubToBaseTable(Dictionary<StatusType, List<StatusCorrection>> _subToBaseTable)
    {
        _subToBaseTable.Clear();
        for (int i = 0; i < statusList.Count; i++)
        {
            var baseCorrection = statusList[i];
            var baseStatus = baseCorrection.statusType;
            var corrections = baseCorrection.correctionList;

            for (int j = 0; j < corrections.Count; j++)
            {
                var subCorrection = corrections[j];
                if (!_subToBaseTable.TryGetValue(subCorrection.statusType, out var list))
                {
                    list = new List<StatusCorrection>();
                    _subToBaseTable[subCorrection.statusType] = list;
                }
                list.Add(new StatusCorrection(baseStatus, subCorrection.valuePerPoint));
            }
        }
    }
#if UNITY_EDITOR
    public void EditorInit()
    {
        var requiredTypes = new[]
        {
            StatusType.Strength,
            StatusType.Dexterity,
            StatusType.Vitality,
            StatusType.Intelligence,
            StatusType.Wisdom,
            StatusType.Agility
        };
        if(statusList == null)
        {
            statusList = new List<BaseStatusCorrection>();
            foreach (var type in requiredTypes)
            {
                statusList.Add(new BaseStatusCorrection
                {
                    statusType = type,
                    correctionList = new List<SubStatusCorrection>()
                });
            }
        }
    }
#endif
}
