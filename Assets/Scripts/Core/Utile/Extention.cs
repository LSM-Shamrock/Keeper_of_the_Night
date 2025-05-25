using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public static class Extention
{

    #region Transform Extentions

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

    #endregion


    #region SpriteRenderer And Image Extentions

    public static void SetBrightness(this SpriteRenderer spriteRenderer, float value)
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        spriteRenderer?.GetPropertyBlock(block);
        block.SetFloat("_Brightness", Mathf.Clamp(value, -1f, 1f));
        spriteRenderer?.SetPropertyBlock(block);
    }
    public static void AddBrightness(this SpriteRenderer spriteRenderer, float value)
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        spriteRenderer?.GetPropertyBlock(block);
        float current = block.GetFloat("_Brightness");
        float next = Mathf.Clamp(current + value, -1f, 1f);
        block.SetFloat("_Brightness", next);
        spriteRenderer?.SetPropertyBlock(block);
    }

    public static void SetAlpha(this SpriteRenderer spriteRenderer, float value)
    {
        Color color = spriteRenderer.color;
        color.a = value;
        color.a = Mathf.Clamp01(color.a);
        spriteRenderer.color = color;
    }
    public static void AddAlpha(this SpriteRenderer spriteRenderer, float value)
    {
        Color color = spriteRenderer.color;
        color.a += value;
        color.a = Mathf.Clamp01(color.a);
        spriteRenderer.color = color;
    }
    public static void SetTransparency(this SpriteRenderer spriteRenderer, float value)
    {
        SetAlpha(spriteRenderer, 1f - value);
    }
    public static void AddTransparency(this SpriteRenderer spriteRenderer, float value)
    {
        AddAlpha(spriteRenderer, -value);
    }

    public static void SetAlpha(this Image image, float value)
    {
        Color color = image.color;
        color.a = value;
        color.a = Mathf.Clamp01(color.a);
        image.color = color;
    }
    public static void AddAlpha(this Image image, float value)
    {
        Color color = image.color;
        color.a += value;
        color.a = Mathf.Clamp01(color.a);
        image.color = color;
    }
    public static void SetTransparency(this Image image, float value)
    {
        SetAlpha(image, 1f - value);
    }
    public static void AddTransparency(this Image image, float value)
    {
        AddAlpha(image, -value);
    }

    #endregion


    public static bool IsContact<T>(this Collider2D collider, T checkRigidbodyName) where T : Enum
    {
        ContactFilter2D filter = new ContactFilter2D();
        List<Collider2D> cols = new List<Collider2D>();
        Physics2D.OverlapCollider(collider, filter.NoFilter(), cols);
        return cols.Any(c => c.attachedRigidbody != null && c.attachedRigidbody.gameObject.name == checkRigidbodyName.ToString());
    }

    public static void SetSpriteAndPolygon(this GameObject gameObject, Sprite sprite)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        PolygonCollider2D polygonCollider = gameObject.GetComponent<PolygonCollider2D>();

        spriteRenderer.sprite = sprite;

        if (polygonCollider == null)
            return;

        int shapeCount = sprite.GetPhysicsShapeCount();
        polygonCollider.pathCount = sprite.GetPhysicsShapeCount();
        for (int i = 0; i < shapeCount; i++)
        {
            List<Vector2> points = new();
            sprite.GetPhysicsShape(i, points);
            polygonCollider.SetPath(i, points);
        }
    }


    static readonly Dictionary<(GameObject, Type), Component> s_components = new();
    public static TComponent Component<TComponent>(this GameObject gameObject) where TComponent : Component
    {
        Type type = typeof(TComponent);
        if (s_components.TryGetValue((gameObject, type), out Component saved))
        {
            return saved as TComponent;
        }

        TComponent component = gameObject.GetComponent<TComponent>();
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

}
