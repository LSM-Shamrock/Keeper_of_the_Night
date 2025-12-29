using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class Extention
{
    public static float GetX(this Transform transform) => transform.position.x;
    public static float GetY(this Transform transform) => transform.position.y;
    public static void AddX(this Transform transform, float value) => transform.position += Vector3.right * value;
    public static void AddY(this Transform transform, float value) => transform.position += Vector3.up * value;
    public static void SetX(this Transform transform, float value)
    {
        Vector3 position = transform.position;
        position.x = value;
        transform.position = position;
    }
    public static void SetY(this Transform transform, float value)
    {
        Vector3 position = transform.position;
        position.y = value;
        transform.position = position;
    }

    public static IEnumerator MoveOverTime(this Transform transform, float seconds, Vector3 vector)
    {
        float t = 0f;
        while (t < seconds)
        {
            yield return null;
            t += Time.deltaTime;
            transform.position += vector * Time.deltaTime / seconds;
        }
    }
    public static IEnumerator MoveToPositionOverTime(this Transform transform, float seconds, Vector3 destination)
    {
        Vector3 start = transform.position;
        float t = 0;
        while (t < seconds)
        {
            var progress = t / seconds;
            transform.position = Vector3.Lerp(start, destination, progress);
            yield return null;
            t += Time.deltaTime;
        }
    }
    public static IEnumerator MoveToTransformOverTime(this Transform transform, float seconds, Transform target)
    {
        float t = 0;
        while (t < seconds)
        {
            var progress = t / seconds;

            Vector3 start = transform.position;
            Vector3 destination = target.position;

            transform.position = Vector3.Lerp(start, destination, progress);
            yield return null;
            t += Time.deltaTime;
        }
    }


    public static bool IsContact<T>(this Collider2D collider, T checkRigidbodyName) where T : Enum
    {
        if (collider.isActiveAndEnabled == false)
            return false;

        string name = checkRigidbodyName.ToString();

        ContactFilter2D filter = new ContactFilter2D();
        List<Collider2D> cols = new List<Collider2D>();
        Physics2D.OverlapCollider(collider, filter.NoFilter(), cols);
        return cols.Any(c =>
        {
            return 
            c.gameObject.name == name ||
            c.attachedRigidbody != null && 
            c.attachedRigidbody.gameObject.name == name;
        });
    }
    public static bool IsContact<T>(this Rigidbody2D rigidbody, T checkRigidbodyName) where T : Enum
    {
        List<Collider2D> colliders = new List<Collider2D>();
        rigidbody.GetAttachedColliders(colliders);

        foreach (Collider2D collider in colliders)
        {
            if (IsContact(collider, checkRigidbodyName))
                return true;
        }
        return false;
    }






    static readonly Dictionary<(GameObject, Type), Component> s_components = new();
    public static T Component<T>(this GameObject gameObject) where T : Component
    {
        Type type = typeof(T);
        if (s_components.TryGetValue((gameObject, type), out Component saved))
        {
            return saved as T;
        }

        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            return null;
        }
        s_components.Add((gameObject, type), component);
        return component;
    }

    public static GameObject CreateClone(this GameObject original)
    {
        GameObject go = GameObject.Instantiate<GameObject>(original);
        go.name = original.name;
        return go;
    }


    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        T get = obj.GetComponent<T>();
        if (get == null)
            return obj.AddComponent<T>();
        else 
            return get;
    }
    public static T GetOrAddComponent<T>(this Component com) where T : Component
    {
        return GetOrAddComponent<T>(com.gameObject);
    }
}
