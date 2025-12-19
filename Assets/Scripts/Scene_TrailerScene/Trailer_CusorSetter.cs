using UnityEngine;

public class Trailer_CusorSetter : BaseController
{
    protected override void Start()
    {
        Texture2D texture2D = Utility.LoadResource<Texture2D>(Sprites.Cursor.Moonlightsword);
        Cursor.SetCursor(texture2D, Vector2.zero, CursorMode.Auto);
    }
}
