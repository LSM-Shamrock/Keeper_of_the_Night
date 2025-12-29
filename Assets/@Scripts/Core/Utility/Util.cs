using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Reflection;
using System.Collections;

public static class Util 
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

    public static Vector3 MousePosition => Manager.Object.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        



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




    public static void SetSpriteAndPolygon(SpriteRenderer spriteRenderer, PolygonCollider2D polygonCollider, Sprite sprite)
    {
        if (spriteRenderer != null) 
            spriteRenderer.sprite = sprite;

        if (polygonCollider != null)
        {
            if (sprite == null)
            {
                polygonCollider.pathCount = 0;
                return;
            }

            int shapeCount = sprite.GetPhysicsShapeCount();
            polygonCollider.pathCount = shapeCount;

            for (int i = 0; i < shapeCount; i++)
            {
                List<Vector2> points = new();
                sprite.GetPhysicsShape(i, points);
                polygonCollider.SetPath(i, points);
            }
        }
    }
    public static void SetSpriteAndPolygon(GameObject gameObject, Sprite sprite)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        PolygonCollider2D polygonCollider = gameObject.GetComponent<PolygonCollider2D>();
        SetSpriteAndPolygon(spriteRenderer, polygonCollider, sprite);
    }

}

