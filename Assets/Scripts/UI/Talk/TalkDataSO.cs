using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TalkData
{
    public int questId;
    public int questProgress;
    public int talkIndex;
    
    public int talkNpcId;
    public string talk;

    public TalkData(int _questId,  int _questProgress, int _talkIndex, int _talkNpcId, string _talk)
    {
        questId = _questId;
        questProgress = _questProgress;
        talkIndex = _talkIndex;
        talkNpcId = _talkNpcId;
        talk = _talk;
    }
}

[CreateAssetMenu(fileName = "TalkDataSO", menuName = "ScriptableObjects/TalkDataSO")]
public class TalkDataSO : ScriptableObject
{
    public List<TalkData> talkDatas;
    public void SetData(List<TalkData> _data)
    {
        talkDatas = _data;
    }
}
