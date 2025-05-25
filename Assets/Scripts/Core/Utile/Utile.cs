using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Reflection;

public static class Utile 
{
    static Dictionary<(Type, string), object> s_resources = new();
    public static T LoadResource<T>(Enum pathEnumValue) where T : UnityEngine.Object
    {
        Type type = typeof(T);
        string name = pathEnumValue.ToString();
        string root = pathEnumValue.GetType().FullName;
        root = root.Replace('.', '/');
        root = root.Replace('+', '/');
        string path = root + '/' + name;

        if (s_resources.TryGetValue((type, path), out object saved))
            return saved as T; 

        T loaded = Resources.Load<T>(path);
        s_resources.Add((type, path), loaded);
        return loaded;
    }

    public static GameObject FindGameObject(Enum findName)
    {
        return GameObject.Find(findName.ToString());
    }

    public static void StartScene(Scenes scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public static Color StringToColor(string colorCode)
    {
        Color color;
        ColorUtility.TryParseHtmlString(colorCode, out color);
        return color;
    }

    public static Quaternion Direction2Rotation(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, angle);
    }

    public static Vector3 MousePosition
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    public static float MouseX => MousePosition.x;
    public static float MouseY => MousePosition.y;

    public static int RandomNumber(int min, int max)
    {
        return UnityEngine.Random.Range(min, max + 1);
    }
    public static float RandomNumber(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }
    public static T RandomElement<T>(IEnumerable<T> collection)
    {
        int count = collection.Count();

        int randomIndex = UnityEngine.Random.Range(0, count);

        T result = collection.ElementAt(randomIndex);

        return result;
    }

}

