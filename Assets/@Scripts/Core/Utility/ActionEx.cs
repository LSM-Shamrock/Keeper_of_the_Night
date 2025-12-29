using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionEx
{
    readonly Dictionary<MonoBehaviour, Action> _objectActionPairs = new();

    public void Add(MonoBehaviour checker, Action action)
    {
        if (_objectActionPairs.ContainsKey(checker))
            _objectActionPairs[checker] += action;
        else
            _objectActionPairs[checker] = action;
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
