using System.Collections;
using UnityEngine;

public class CharacterSkill_Water : PlaySceneObjectBase
{
    protected override void Start()
    {
        Init();
    }

    GameObject _child;

    void Init()
    {
        _child = transform.GetChild(0).gameObject;
        StartCoroutine(Loop_Throw());
    }

    IEnumerator Loop_Throw()
    {
        while (true)
        {
            yield return WaitUntil(() => currentCharacter == Sprites.Characters.Rather);
            if (IsMouseClicked)
            {
                StartCoroutine(Throw());
                yield return WaitForSeconds(0.3f);
                yield return waitForFixedUpdate;
            }
        }
    }

    IEnumerator Throw()
    {
        var go = _child.CreateClone();
        var transform = go.transform;
        var col = go.Component<Collider2D>();

        float size = 20f;
        transform.localScale = Vector3.one * size;

        transform.position = Character.position;

        Vector3 direction = (Utile.MousePosition - transform.position).normalized;
        transform.rotation = Utile.Direction2Rotation(direction);

        go.SetActive(true);

        float fallAndSlowdown = 0.5f;

        while (!col.IsContact(Objects.Ground))
        {
            transform.position += direction * (4f / (fallAndSlowdown / 2f));
            transform.AddY(-fallAndSlowdown);
            fallAndSlowdown += 0.05f;

            if (col.IsContact(Prefabs.Enemy))
            {
                if (size > 10f)
                {
                    size -= 0.5f;
                    transform.localScale = Vector3.one * size;
                }
                else
                {
                    Destroy(go);
                    yield break;
                }
            }

            yield return waitForFixedUpdate;
        }
        Destroy(go);
    }
}
