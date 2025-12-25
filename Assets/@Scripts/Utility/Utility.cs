using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Reflection;

public static class Utility 
{
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
            return Manager.Object.MainCamera.ScreenToWorldPoint(Input.mousePosition);
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



    public static Transform FindChild(Transform root, string name)
    {
        if (root == null)
            return null;

        foreach (Transform child in root)
        {
            if (child.name == name)
                return child;

            Transform rec = FindChild(child, name);
            if (rec != null)
                return rec;
        }
        return null;
    }

}

