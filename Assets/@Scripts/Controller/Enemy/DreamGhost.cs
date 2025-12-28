using System.Collections;
using UnityEngine;
using static Utility;

public class DreamGhost : EnemyBase
{
    protected override void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        Manager.Game.onDreamghostAppearance.Add(this, () => StartCoroutine(OnAppearance()));
        Manager.Game.onNightmareEvent.Add(this, () => StartCoroutine(OnNightmareEvent()));
    }

    void CreateButterflyParticle()
    {
        var prefab = Manager.Resource.LoadResource<GameObject>(Prefabs.Play.Enemy_Dreamghost_ButterflyParticle);
        var go = Instantiate(prefab, transform.position, Quaternion.identity);
        var butterfly = go.GetComponent<DreamGhost_ButterflyParticle>();
        butterfly.OnCreated();
    }
    void CreatePhantom()
    {
        var prefab = Manager.Resource.LoadResource<GameObject>(Prefabs.Play.Enemy_Dreamghost_Phantom);
        var go = Instantiate(prefab);
        var phantom = go.GetComponent<DreamGhost_Phantom>();
        phantom.Init();
    }
    IEnumerator WhiteoutEffect()
    {
        var prefab = Manager.Resource.LoadResource<GameObject>(Prefabs.Play.Enemy_Dreamghost_Whiteout);
        var go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        var sr = go.GetComponent<SpriteRenderer>();

        sr.SetTransparency(1f);
        foreach (int i in Count(20))
        {
            sr.AddTransparency(-0.05f);
            yield return new WaitForFixedUpdate();
        }
        Manager.Game.IsNightmare = Manager.Game.wave == 7;
        Manager.Game.onNightmareEvent.Call();
        foreach (int i in Count(100))
        {
            sr.AddTransparency(0.01f);
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitUntil(() => Manager.Game.wave == 8);
        sr.SetTransparency(1f);
        foreach (int i in Count(20))
        {
            sr.AddTransparency(-0.05f);
            yield return new WaitForFixedUpdate();
        }
        Manager.Game.IsNightmare = Manager.Game.wave == 7;
        Manager.Game.onNightmareEvent.Call();
        foreach (int i in Count(100))
        {
            sr.AddTransparency(0.01f);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator OnNightmareEvent()
    {
        if (Manager.Game.wave == 7)
            Hide();

        while (Manager.Game.wave != 8)
        {
            CreatePhantom();
            yield return new WaitForSeconds(RandomNumber(2, 4));
        }
    }
    IEnumerator OnAppearance()
    {
        transform.SetY(100f);
        transform.SetX(Manager.Object.Character.position.x > 0 ? -250f : 250f);
        int moveX = Manager.Object.Character.position.x > 0 ? 1 : -1;
        foreach (int i in Count(60))
        {
            transform.AddX(2f * moveX);
            transform.AddY(-3);
            CreateButterflyParticle();
            yield return new WaitForFixedUpdate();
        }

        Show();
        yield return new WaitForSeconds(1f);
        Hide();

        StartCoroutine(WhiteoutEffect());
        yield return new WaitUntil(() => Manager.Game.wave == 8);
        transform.SetX(Manager.Object.Character.position.x > 0 ? -200f : 200f);
        Show();

        while (true)
        {
            yield return new WaitForFixedUpdate();
            
            Vector3 direction = (Manager.Object.Character.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.Euler(Vector3.up * (direction.x > 0 ? 0 : 180));
            transform.position += direction * 1f;

            if (IsContactGround == false)
            {
                transform.AddY(-1f);
                continue;
            }

            if (Mathf.Abs(transform.GetX() - Manager.Object.Character.GetX()) < 30)
            {
                foreach (int i in Count(5))
                {
                    transform.AddY(5f);
                    yield return new WaitForFixedUpdate();
                }
                if (IsContactCharacter)
                    Manager.Game.TakeDamageToPlayer(9);

                while (!IsContactGround)
                {
                    transform.AddY(-2f);
                    yield return new WaitForFixedUpdate();
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    protected override IEnumerator WhenTakingDamage(int damage)
    {
        if (Manager.Game.wave == 8)
            Hide();
        yield break;
    }
}
