using UnityEngine;

public class Trailer_CusorSetter : ObjectBase
{
    protected override void Start()
    {
        Texture2D texture2D = Utile.LoadResource<Texture2D>(Sprites.Cursor.Moonlightsword);
        Cursor.SetCursor(texture2D, Vector2.zero, CursorMode.Auto);
    }
}
