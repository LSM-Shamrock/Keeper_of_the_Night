using UnityEngine;
using UnityEngine.UI;

public static class EffectExtention 
{
    public static void SetBrightness(this SpriteRenderer spriteRenderer, float value)
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();

        spriteRenderer?.GetPropertyBlock(block);

        block.SetFloat("_Brightness", Mathf.Clamp(value, -1f, 1f));

        spriteRenderer?.SetPropertyBlock(block);
    }
    public static float GetBrightness(this SpriteRenderer spriteRenderer)
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();

        spriteRenderer?.GetPropertyBlock(block);

        return block.GetFloat("_Brightness");
    }
    public static void AddBrightness(this SpriteRenderer spriteRenderer, float value)
    {
        if (spriteRenderer == null)
            return;

        spriteRenderer.SetBrightness(spriteRenderer.GetBrightness() + value);
    }

    public static void SetBrightness(this Image image, float value)
    {
        if (image == null)
            return;

        Color.RGBToHSV(image.color, out float h, out float s, out _);
        Color color = Color.HSVToRGB(h, s, (value + 1) / 2);
        color.a = image.color.a;
        image.color = color;
    }
    public static float GetBrightness(this Image image)
    {
        Color.RGBToHSV(image.color, out _, out _, out float v);

        return v * 2 - 1;
    }
    public static void AddBrightness(this Image image, float value)
    {
        if (image == null)
            return;

        image.SetBrightness(image.GetBrightness() + value);
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
}
