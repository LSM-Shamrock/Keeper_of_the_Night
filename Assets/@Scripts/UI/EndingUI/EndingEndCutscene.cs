using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;


public class EndingEndCutscene : EndingBase
{
    Image _back;
    Image _front;

    Sprite ThankYouForPlaying => Manager.Resource.LoadResource<Sprite>(Sprites.Ending.ThankYouForPlaying);
    Sprite Endcard_1 => Manager.Resource.LoadResource<Sprite>(Sprites.Ending.Endcard_1);
    Sprite Endcard_2 => Manager.Resource.LoadResource<Sprite>(Sprites.Ending.Endcard_2);


    protected override void Start()
    {
        Init();
    }

    void Init()
    {
        _back = transform.GetChild(0).GetComponent<Image>();
        _front = transform.GetChild(1).GetComponent<Image>();

        StartCoroutine(Start_Routine());
    }

    IEnumerator Start_Routine()
    {
        _back.enabled = false;
        _front.enabled = false;
        yield return new WaitUntil(() => Regex.IsMatch(EndingCurrentLine, Pattern_Cutscene));
        _front.enabled = true;

        while (EndingProgress != EndingScript.Length - 1)
        {
            Match match = Regex.Match(EndingCurrentLine, Pattern_Cutscene);

            if (match.Success)
            {
                Sprites.Ending enumValue = Enum.Parse<Sprites.Ending>(match.Groups["sprite"].Value);
                Sprite sprite = Manager.Resource.LoadResource<Sprite>(enumValue);
                _front.sprite = sprite;
            }

            yield return new WaitForFixedUpdate();
        }
        _front.sprite = ThankYouForPlaying;
        transform.localScale = Vector3.one * 400f / 375f;

        yield return new WaitForSeconds(4f);

        _front.sprite = Endcard_1;
        transform.localScale = Vector3.one;

        yield return new WaitForSeconds(2f);

        _back.sprite = _front.sprite;
        _back.enabled = true;

        _front.SetTransparency(1f);
        _front.sprite = Endcard_2;
        foreach (int i in Count(50))
        {
            _front.AddTransparency(-0.02f);
            yield return new WaitForFixedUpdate();
        }
    }
}
