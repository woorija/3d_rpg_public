using System.Collections.Generic;
using UnityEngine;

public struct CooltimeData
{
    public float currentCooltime;
    public float cooltime;
    public CooltimeData(float _cooltime)
    {
        cooltime = _cooltime;
        currentCooltime = _cooltime;
    }
    public void Update(float _time)
    {
        currentCooltime -= _time;
    }
    public bool IsNotCooltime()
    {
        return currentCooltime <= 0;
    }
}
public class CooltimeManager : SingletonBehaviour<CooltimeManager>
{
    private Dictionary<int, CooltimeData> cooltimeDatas = new Dictionary<int, CooltimeData>(32);
    private Queue<int> removeKeyQueue = new Queue<int>();
    List<int> keys = new List<int>(32);
    [SerializeField] PlayerStatus playerStatus;

    protected override void Awake()
    {
        base.Awake();
    }
    private void Update()
    {
        float deltaTime = Time.deltaTime;

        //업데이트 전 리스트 재설정
        int index = 0;
        foreach(var kvp in cooltimeDatas)
        {
            if(keys.Count > index)
            {
                keys[index] = kvp.Key;
            }
            else
            {
                keys.Add(kvp.Key);
            }
            index++;
        }
        if(keys.Count > index)
        {
            keys.RemoveRange(index, keys.Count - index);
        }

        //업데이트
        foreach (var key in keys)
        {
            var data = cooltimeDatas[key];

            if(key > 200000000) // 소비템이면
            {
                data.Update(deltaTime);
            }
            else // 스킬이면
            {
                data.Update(deltaTime * playerStatus.CooltimeReduceMultipler);
            }

            
            if (data.IsNotCooltime())
            {
                removeKeyQueue.Enqueue(key);
            }
            else
            {
                cooltimeDatas[key] = data;
            }
        }
        while (removeKeyQueue.Count > 0)
        {
            var key = removeKeyQueue.Dequeue();
            cooltimeDatas.Remove(key);
        }
    }
    public void AddCooltime(int _id, float _cooltime)
    {
        cooltimeDatas[_id] = new CooltimeData(_cooltime);
    }
    public bool IsCooltime(int _id)
    {
        return cooltimeDatas.ContainsKey(_id);
    }
    public float GetCooltimeProgress(int _id)
    {
        if (cooltimeDatas.TryGetValue(_id, out CooltimeData data))
        {
            return Mathf.Clamp01(data.currentCooltime / data.cooltime);
        }
        return 0f;
    }
}
