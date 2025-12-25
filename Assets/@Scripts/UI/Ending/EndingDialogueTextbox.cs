using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;


public class EndingDialogueTextbox : EndingBase
{
    private Text _text;

    protected override void Start()
    {
        Init();
    }

    protected override void Update()
    {
        base.Update();
        UpdatePositionAndText();
    }

    private void Init()
    {
        _text = GetComponentInChildren<Text>();

        EndingProgress = 1;
        StartCoroutine(LoopAddProgress());
    }

    private void UpdatePositionAndText()
    {
        Match matchDialogue = Regex.Match(EndingCurrentLine, Pattern_Dialogue);
        _text.enabled = matchDialogue.Success;
        if (!matchDialogue.Success) 
            return;

        string line = matchDialogue.Groups["line"].Value;
        string speaker = matchDialogue.Groups["speaker"].Value;
        Characters speakerCharacter = Enum.Parse<Characters>(speaker);

        _text.text = line;

        transform.position = Character[speakerCharacter].position + Vector3.up * 30;
        transform.localScale = Vector3.one * 0.75f;
        _text.color = (speakerCharacter) switch
        {
            Characters.Dino => Utility.StringToColor("#16c72e"),
            Characters.Heptagram => Utility.StringToColor("#e8e230"),
            Characters.Rather => Utility.StringToColor("#2fc9e8"),
        };

        if (speakerCharacter == Characters.Dino)
        {
            if (EndingProgress > 4)
                transform.position = Character[Characters.Sleepground].position + Vector3.up * 30;
            if (EndingProgress == 28)
            {
                transform.position = new Vector3(30, 60);
                transform.localScale = Vector3.one * 1f;
            }
        }
        if (speakerCharacter == Characters.Heptagram)
        {
            if (EndingProgress == 25)
            {
                transform.position = new Vector3(0, 20);
                transform.localScale = Vector3.one * 1f;
            }
        }
    }

    private IEnumerator LoopAddProgress()
    {
        yield return new WaitForSeconds(0.5f);

        while (EndingProgress < EndingScript.Length - 1)
        {
            if (EndingCurrentLine == string.Empty)
            {
                yield return new WaitUntil(() => EndingCurrentLine != string.Empty);
                yield return new WaitForSeconds(0.1f);
            }

            if (Input.GetMouseButtonDown(0))
            {
                EndingProgress++;
                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }
    }
}
