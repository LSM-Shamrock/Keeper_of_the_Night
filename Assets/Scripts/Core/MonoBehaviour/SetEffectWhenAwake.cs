using UnityEngine;

public class SetEffectWhenAwake : MonoBehaviour
{
    [SerializeField]
    float _brightness;
    [SerializeField]
    float _transparency;

    private void Awake()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.SetBrightness(_brightness);
        sr.SetTransparency(_transparency);
    }
}
