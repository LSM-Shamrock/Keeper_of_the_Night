using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SignalOfMonoBehaviour
{
    readonly Dictionary<MonoBehaviour, Action> _objectActionPairs = new();

    public void Add(MonoBehaviour keyObject, Action action)
    {
        if (_objectActionPairs.ContainsKey(keyObject))
            _objectActionPairs[keyObject] += action;
        else
            _objectActionPairs[keyObject] = action;
    }

    public void Call()
    {
        foreach (MonoBehaviour key in _objectActionPairs.Keys.ToList())
        {
            if (key == null)
                _objectActionPairs.Remove(key);
            else
                _objectActionPairs[key]?.Invoke();
        }
    }
}
