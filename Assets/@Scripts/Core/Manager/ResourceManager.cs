using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    private Dictionary<(Type, string), object> s_resources = new();

    public T LoadResource<T>(Enum pathEnumValue) where T : UnityEngine.Object
    {
        Type t = typeof(T);

        string root = pathEnumValue.GetType().Name;
        
        if (t == typeof(Sprite) || t == typeof(Texture2D)) 
            root = $"Sprites/{root}";

        if (t == typeof(GameObject)) 
            root = $"Prefabs/{root}";

        string file = pathEnumValue.ToString();
        string path = $"{root}/{file}";


        if (s_resources.TryGetValue((t, path), out object saved))
            return saved as T;
        
        T loaded = Resources.Load<T>(path);
        s_resources.Add((t, path), loaded);

        return loaded;
    }
}
