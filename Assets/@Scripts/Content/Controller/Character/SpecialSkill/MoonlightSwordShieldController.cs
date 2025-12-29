using UnityEngine;

public class MoonlightSwordShieldController : CharacterSkillController
{
    private GameObject _body;
    private SpriteRenderer _bodySR;
    private PolygonCollider2D _bodyCol;
    private SpriteRenderer[] _effects;


    protected override void Start()
    {
        base.Start();
    }

    private void Init()
    {
        _body = transform.GetChild(0).gameObject;
        _bodySR = _body.GetComponent<SpriteRenderer>();
        _bodyCol = _body.GetComponent<PolygonCollider2D>();

        _effects = new SpriteRenderer[3];
        _effects[0] = transform.GetChild(1).GetComponent<SpriteRenderer>();
        _effects[1] = transform.GetChild(2).GetComponent<SpriteRenderer>();
        _effects[2] = transform.GetChild(3).GetComponent<SpriteRenderer>();
    }

    public void OnSkill()
    {

    }
}
