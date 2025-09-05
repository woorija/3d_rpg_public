using System;
using System.Collections.Generic;
using UnityEngine;
public struct PlayerEffectKey : IEquatable<PlayerEffectKey>
{
    public int id;
    public int index;
    public PlayerEffectKey(int _id, int _index)
    {
        id = _id;
        index = _index;
    }

    public bool Equals(PlayerEffectKey other)
    {
        return id == other.id && index == other.index;
    }
    public override bool Equals(object obj)
    {
        return obj is PlayerEffectKey other && Equals(other);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(id, index);
    }
}
public class PlayerEffectManager : MonoBehaviour
{
    Dictionary<PlayerEffectKey, PlayerEffect> effectList = new Dictionary<PlayerEffectKey, PlayerEffect>();

    /// <summary>
    /// 초기 이펙트 생성 함수 기본값은 _pos = Vector3.zero, _rotate = Quaternion.identity
    /// </summary>
    /// <param name="_id">스킬 id</param>
    /// <param name="_index">스킬 이펙트 인덱스</param>
    /// <param name="_pos">이펙트 좌표 초기값 Vector3.zero</param>
    /// <param name="_rotate">이펙트 회전값 초기값 Quaternion.identity</param>
    public void SetEffect(int _id, int _index, Vector3 _pos, Quaternion _rotate)
    {
        SetEffect(new PlayerEffectKey(_id, _index), _pos, _rotate);
    }
    public async void SetEffect(PlayerEffectKey _key, Vector3 _pos, Quaternion _rotate)
    {
        var effectObj = await AddressableManager.Instance.InstantiateAsync($"S{_key.id}-{_key.index}");
        
        if (effectObj == null) return;
        PlayerEffect effect = effectObj.GetComponent<PlayerEffect>();
        effect.transform.parent = transform;
        effect.transform.localPosition = _pos;
        effect.transform.localRotation = _rotate;
        effect.Init();
        effectList.Add(_key, effect);
    }
    public void SetNormalAttackEffect(int _id)
    {
        Vector3 pos;
        Quaternion rot;
        switch (_id)
        {
            case 0:
            case 10:
            case 21:
            case 22:
            case 23:
            case 24:
                pos = new Vector3(0f, 0.8f, 0f);
                rot = Quaternion.Euler(new Vector3(-95f, -110f, 0f));
                break;
            default:
                pos = Vector3.zero;
                rot = Quaternion.identity;
                break;
        }
        SetEffect(_id, 1, pos, rot);
    }
    public void SetClassEffect(int _id)
    {
        switch(_id)
        {
            case 0:
                break;
            case 10:
                SetEffect(100001, 1, new Vector3(0f, 0.8f, 0f), Quaternion.Euler(new Vector3(-95f, -110f, 0f)));
                SetEffect(100002, 1, new Vector3(0f, 0.8f, 0f), Quaternion.Euler(new Vector3(-95f, -110f, 0f)));
                break;
            case 21:
                break;
            case 22:
                break;
            case 23:
                break;
            case 24:
                break;
            case 31:
                break;
        }
    }
    public void PlayEffect(PlayerEffectKey _key)
    {
        if(effectList.TryGetValue(_key, out var effect))
        {
            effect.PlayEffect();
        }
        else
        {
            DevelopUtility.Log($"Effect not found: {_key}");
        }
    }
    public void PlayEffect(int _id, int _index)
    {
        
        if (effectList.TryGetValue(new PlayerEffectKey(_id, _index), out var effect))
        {
            DevelopUtility.Log($"S{_id}-{_index} Effect Play");
            effect.PlayEffect();
        }
        else
        {
            DevelopUtility.Log($"Effect not found: {_id}, {_index}");
        }
    }
}
