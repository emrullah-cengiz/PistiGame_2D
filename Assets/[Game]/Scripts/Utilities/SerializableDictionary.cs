using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] public List<TKey> _keys = new();
    [SerializeField] public List<TValue> _values = new();

    public void OnBeforeSerialize()
    {
        // _keys.Clear();
        // _values.Clear();
        // foreach (var kvp in this)
        // {
        //     _keys.Add(kvp.Key);
        //     _values.Add(kvp.Value);
        // }
    }

    public void OnAfterDeserialize()
    {
        Clear();

        if (_keys.Count < _values.Count)
            _values.RemoveRange(_keys.Count, _values.Count - _keys.Count);
        
        if (_keys.Count > _values.Count)
            for (int i = 0; i < _keys.Count - _values.Count; i++)
                _values.Add(default);

        for (int i = 0; i < _keys.Count; i++)
            if (!ContainsKey(_keys[i]))
                Add(_keys[i], _values[i]);
    }
}