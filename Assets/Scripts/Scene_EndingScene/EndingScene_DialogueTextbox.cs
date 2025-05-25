using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;


public class EndingScene_DialogueTextbox : EndingSceneObjectBase
{
    Text _text;
    Image _image;

    protected override void Start()
    {
        Init();
    }

    protected override void Update()
    {
        base.Update();
        Update_PositionAndText();
    }

    void Init()
    {
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<Text>();

        EndingProgress = 1;
        StartCoroutine(Loop_AddProgress());
    }

    void Update_PositionAndText()
    {
        Match matchDialogue = Regex.Match(EndingCurrentLine, Pattern_Dialogue);

        if (!matchDialogue.Success)
        {
            _text.enabled = false;
            _image.enabled = false;
        }
        else
        {
            _text.enabled = true;
            _image.enabled = true;

            string speaker = matchDialogue.Groups["speaker"].Value;
            string line = matchDialogue.Groups["line"].Value;

            Sprites.Characters speakerCharacter = Enum.Parse<Sprites.Characters>(speaker);

            Transform sleepground = Character[Sprites.Characters.Sleepground];
            Transform dino = Character[Sprites.Characters.Dino];
            Transform heptagram = Character[Sprites.Characters.Heptagram];
            Transform rather = Character[Sprites.Characters.Rather];

            float size = 0.75f;
            Color color = new();
            Vector3 position = new();
            switch (speakerCharacter)
            {
                case Sprites.Characters.Dino:

                    color = Utile.StringToColor("#16c72e");
                    position = (EndingProgress > 4 ? sleepground : dino).position;
                    position.y += 30;
                    if (EndingProgress == 28)
                    {
                        position = new Vector3(30, 60);
                        size = 1f;
                    }
                    break;

                case Sprites.Characters.Heptagram:

                    color = Utile.StringToColor("#e8e230");
                    position = heptagram.position;
                    position.y += 30;
                    if (EndingProgress == 25)
                    {
                        position = new Vector3(0, 20);
                        size = 1f;
                    }
                    break;

                case Sprites.Characters.Rather:

                    color = Utile.StringToColor("#2fc9e8");
                    position = rather.position;
                    position.y += 30;
                    break;
            }
            transform.position = position;
            transform.localScale = Vector3.one * size;
            _text.color = color;
            _text.text = line;
        }
    }

    IEnumerator Loop_AddProgress()
    {
        yield return WaitUntil(() => !IsMouseClicked);
        yield return WaitForSeconds(0.5f);
        while (true)
        {
            if (EndingProgress < EndingScript.Length - 1)
            {
                if (EndingCurrentLine == string.Empty)
                {
                    yield return WaitUntil(() => EndingCurrentLine != string.Empty);
                    yield return WaitForSeconds(0.1f);
                }

                if (IsMouseClicked)
                {
                    EndingProgress++;
                    yield return WaitUntil(() => !IsMouseClicked);
                    yield return WaitForSeconds(0.1f);
                }
            }
            yield return waitForFixedUpdate;
        }
    }
}
