using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BinaryUtility
{
    public static void SaveData(BinaryWriter _writer, int _value)
    {
        _writer.Write(_value);
    }
    public static void SaveData(BinaryWriter _writer, long _value)
    {
        _writer.Write(_value);
    }
    public static void SaveData(BinaryWriter _writer, bool _value)
    {
        _writer.Write(_value);
    }
    public static void SaveData(BinaryWriter _writer, float _value)
    {
        _writer.Write(_value);
    }
    public static void SaveData(BinaryWriter _writer, string _value)
    {
        _writer.Write(_value);
    }
    public static void SaveData(BinaryWriter _writer, Vector3 _value)
    {
        _writer.Write(_value.x);
        _writer.Write(_value.y);
        _writer.Write(_value.z);
    }
    public static void SaveData(BinaryWriter _writer, int[] _arr)
    {
        _writer.Write(_arr.Length);
        for(int i = 0; i < _arr.Length; i++)
        {
            _writer.Write(_arr[i]);
        }
    }
    public static void SaveData(BinaryWriter _writer, List<int> _list)
    {
        _writer.Write(_list.Count);
        for(int i = 0; i < _list.Count; i++)
        {
            _writer.Write(_list[i]);
        }
    }
    public static void SaveData(BinaryWriter _writer, HashSet<int> _set)
    {
        _writer.Write(_set.Count);
        foreach(int id in _set)
        {
            _writer.Write(id);
        }
    }
    public static void SaveData(BinaryWriter _writer, Dictionary<int, Dictionary<int, int>> _nestedDict)
    {
        _writer.Write(_nestedDict.Count);

        foreach (var outerKvp in _nestedDict)
        {
            int outerKey = outerKvp.Key;
            Dictionary<int,int> innerDict = outerKvp.Value;
            _writer.Write(outerKey);
            _writer.Write(innerDict.Count);

            foreach (var innerKvp in innerDict)
            {
                _writer.Write(innerKvp.Key);
                _writer.Write(innerKvp.Value);
            }
        }
    }
    public static void SaveData(BinaryWriter _writer, Dictionary<int, Dictionary<int, bool>> _nestedDict)
    {
        _writer.Write(_nestedDict.Count);

        foreach (var outerKvp in _nestedDict)
        {
            int outerKey = outerKvp.Key;
            Dictionary<int, bool> innerDict = outerKvp.Value;
            _writer.Write(outerKey);
            _writer.Write(innerDict.Count);

            foreach (var innerKvp in innerDict)
            {
                _writer.Write(innerKvp.Key);
                _writer.Write(innerKvp.Value);
            }
        }
    }
    public static int LoadIntData(BinaryReader _reader) 
    {
        return _reader.ReadInt32();
    }
    public static long LoadLongData(BinaryReader _reader)
    {
        return _reader.ReadInt64();
    }
    public static bool LoadBoolData(BinaryReader _reader)
    {
        return _reader.ReadBoolean();
    }
    public static float LoadFloatData(BinaryReader _reader)
    {
        return _reader.ReadSingle();
    }
    public static string LoadStringData(BinaryReader _reader)
    {
        return _reader.ReadString();
    }
    public static Vector3 LoadVector3Data(BinaryReader _reader)
    {
        Vector3 vector3 = new Vector3(_reader.ReadSingle(), _reader.ReadSingle(), _reader.ReadSingle());
        return vector3;
    }
    public static List<int> LoadIntListData(BinaryReader _reader)
    {
        int count = _reader.ReadInt32();
        List<int> list = new List<int>(count);

        for(int i = 0; i < count; i++)
        {
            list.Add(_reader.ReadInt32());
        }

        return list;
    }
    public static HashSet<int> LoadIntHashSetData(BinaryReader _reader)
    {
        int count = _reader.ReadInt32();
        HashSet<int> set = new HashSet<int>(count);

        for (int i = 0; i < count; i++)
        {
            set.Add(_reader.ReadInt32());
        }

        return set;
    }
    public static int[] LoadIntArrayData(BinaryReader _reader)
    {
        int length = _reader.ReadInt32();
        int[] arr = new int[length];

        for(int i = 0; i < length; i++)
        {
            arr[i] = _reader.ReadInt32();
        }

        return arr;
    }
    public static Dictionary<int,Dictionary<int,int>> LoadNestedDictIntData(BinaryReader _reader)
    {
        int outerCount = _reader.ReadInt32();
        Dictionary<int,Dictionary<int,int>> nestedDict = new Dictionary<int, Dictionary<int, int>>(outerCount);

        for(int i = 0;i < outerCount; i++)
        {
            int outerKey = _reader.ReadInt32();

            int innerCount = _reader.ReadInt32();
            Dictionary<int, int> innerDict = new Dictionary<int, int>(innerCount);

            for(int j = 0; j < innerCount; j++)
            {
                innerDict.Add(_reader.ReadInt32(), _reader.ReadInt32());
            }
            nestedDict.Add(outerKey, innerDict);
        }
        return nestedDict;
    }
    public static Dictionary<int, Dictionary<int, bool>> LoadNestedDictBoolData(BinaryReader _reader)
    {
        int outerCount = _reader.ReadInt32();
        Dictionary<int, Dictionary<int, bool>> nestedDict = new Dictionary<int, Dictionary<int, bool>>(outerCount);

        for (int i = 0; i < outerCount; i++)
        {
            int outerKey = _reader.ReadInt32();

            int innerCount = _reader.ReadInt32();
            Dictionary<int, bool> innerDict = new Dictionary<int, bool>(innerCount);

            for (int j = 0; j < innerCount; j++)
            {
                innerDict.Add(_reader.ReadInt32(), _reader.ReadBoolean());
            }
            nestedDict.Add(outerKey, innerDict);
        }
        return nestedDict;
    }
}
