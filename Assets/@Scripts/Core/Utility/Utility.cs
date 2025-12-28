using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Reflection;
using System.Collections;

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
    public static int RandomSign()
    {
        return UnityEngine.Random.Range(0, 2) * 2 - 1;
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



    public static IEnumerable<int> Count(int count)
    {
        for (int i = 0; i < count; i++)
            yield return i;
    }






    public static IEnumerator RunForSec(float sec, Action<float> action)
    {
        for (float s = 0.0f;  s < sec; s += Time.deltaTime)
        {
            action?.Invoke(s);
            yield return null;
        }
    }
    public static void StartRunForSec(MonoBehaviour obj, float sec, Action<float> action)
    { 
        obj.StartCoroutine(RunForSec(sec, action)); 
    }

    public static IEnumerator WaitAndRun(float sec, Action action)
    {
        yield return new WaitForSeconds(sec);
        action?.Invoke();
    }
    public static void StartWaitAndRun(MonoBehaviour obj, float sec, Action action)
    {
        obj.StartCoroutine(WaitAndRun(sec, action)); 
    }
}

