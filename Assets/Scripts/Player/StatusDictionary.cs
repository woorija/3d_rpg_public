using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class StatusDictionary
{
    private Dictionary<StatusType, int> dictionary = new Dictionary<StatusType, int>();

    private Action<StatusType> OnGlobalValueChanged;

    private Dictionary<StatusType, List<UnityEvent>> OnSpecificValueChanged = new Dictionary<StatusType, List<UnityEvent>>();
    private Dictionary<StatusType, Action> OnSpecificValueChangedAction = new Dictionary<StatusType, Action>();

    public int this[StatusType key]
    {
        get => dictionary[key];
        set
        {
            if (!dictionary.TryGetValue(key, out int existingValue) || existingValue != value)
            {
                dictionary[key] = value;
                if(OnSpecificValueChanged.TryGetValue(key, out var events))
                {
                    foreach(UnityEvent unityEvent in events)
                    {
                        unityEvent?.Invoke();
                    }
                }
                if(OnSpecificValueChangedAction.TryGetValue(key, out var action))
                {
                    action?.Invoke();
                }
                OnGlobalValueChanged?.Invoke(key);
            }
        }
    }

    public float PercentageReturn(StatusType _key)
    {
        return dictionary[_key] * 0.01f;
    }

    public void Add(StatusType _key, int _value)
    {
        dictionary.Add(_key, _value);
        OnGlobalValueChanged?.Invoke(_key);
    }

    public bool Remove(StatusType _key)
    {
        return dictionary.Remove(_key);
    }

    public bool ContainsKey(StatusType _key) => dictionary.ContainsKey(_key);

    public void Clear()
    {
        dictionary.Clear();
    }

    public IEnumerable<StatusType> Keys => dictionary.Keys;
    public IEnumerable<int> Values => dictionary.Values;

    public void InitValue(StatusType _key, int _value)
    {
        dictionary[_key] = _value;
    }

    public void RegisterGlobalEvent(Action<StatusType> _action)
    {
        OnGlobalValueChanged += _action;
    }

    public void UnRegisterGlobalEvent(Action<StatusType> _action)
    {
        OnGlobalValueChanged -= _action;
    }

    public void RegisterStatusEvent(StatusType _type, UnityEvent _event)
    {
        if (!OnSpecificValueChanged.ContainsKey(_type))
        {
            OnSpecificValueChanged[_type] = new List<UnityEvent>();
        }
        OnSpecificValueChanged[_type].Add(_event);
    }
    public void RegisterStatusEvent(StatusType _type, Action _action)
    {
        if (!OnSpecificValueChangedAction.ContainsKey(_type))
        {
            OnSpecificValueChangedAction[_type] = null;
        }
        OnSpecificValueChangedAction[_type] += _action;
    }

    public void UnRegisterStatusEvent(StatusType _type, UnityEvent _event)
    {
        if(OnSpecificValueChanged.TryGetValue(_type, out List<UnityEvent> events))
        {
            events.Remove(_event);

            if (events.Count == 0)
            {
                OnSpecificValueChanged.Remove(_type);
            }
        }
    }
    public void UnRegisterStatusEvent(StatusType _type, Action _action)
    {
        OnSpecificValueChangedAction[_type] -= _action;
    }
}