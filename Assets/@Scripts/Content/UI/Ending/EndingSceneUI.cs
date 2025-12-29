using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class EndingSceneUI : SceneUI
{
    private AudioSource _audioSource;

    private Dictionary<Characters, Transform> _characters = new();

    private ChildKey<Transform> ParticleRoot = new(nameof(ParticleRoot));
    private ChildKey<Transform> DialogueTextbox = new(nameof(DialogueTextbox));
    private ChildKey<Text> DialogueText = new(nameof(DialogueText));
    private ChildKey<Image> EndCutsceneBack = new(nameof(EndCutsceneBack));
    private ChildKey<Image> EndCutsceneFront = new(nameof(EndCutsceneFront));

    private ActionEx _onEndingProgressChange = new();
    private int _endingProgress = 1;
    private int EndingProgress
    {
        get { return _endingProgress; }
        set { _endingProgress = value; _onEndingProgressChange.Call(); }
    }
    private string EndingCurrentLine => Manager.Data.endingScript[EndingProgress];

    private Sprite spriteThankYouForPlaying => Manager.Resource.LoadResource<Sprite>(Sprites.Ending.ThankYouForPlaying);
    private Sprite spriteEndcard1 => Manager.Resource.LoadResource<Sprite>(Sprites.Ending.Endcard_1);
    private Sprite spriteEndcard2 => Manager.Resource.LoadResource<Sprite>(Sprites.Ending.Endcard_2);

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        UpdateDialogueText();
    }

    private void Init()
    {
        BindChild(
        ParticleRoot,
        DialogueTextbox,
        DialogueText,
        EndCutsceneBack,
        EndCutsceneFront);

        _audioSource = GetComponent<AudioSource>();

        foreach (Characters character in (Characters[])Enum.GetValues(typeof(Characters)))
            _characters.Add(character, Util.FindChild(transform, character.ToString()));
        
        EndingProgress = 1;

        StartCoroutine(LoopAddProgress());
        StartCoroutine(StartSleepground());
        StartCoroutine(StartDino());
        StartCoroutine(StartHeptagram());
        StartCoroutine(StartRather());
        StartCoroutine(StartEndCutscene());
    }

    private void UpdateDialogueText()
    {
        Match matchDialogue = Regex.Match(EndingCurrentLine, Manager.Data.patternDialogue);

        Text dialogueText = GetChild(DialogueText);
        Transform dialogueTextbox = GetChild(DialogueTextbox);

        dialogueText.enabled = matchDialogue.Success;
        if (!matchDialogue.Success)
            return;

        string line = matchDialogue.Groups["line"].Value;
        string speaker = matchDialogue.Groups["speaker"].Value;
        Characters speakerCharacter = Enum.Parse<Characters>(speaker);

        dialogueText.text = line;

        dialogueTextbox.position = _characters[speakerCharacter].position + Vector3.up * 30;
        dialogueTextbox.localScale = Vector3.one * 0.75f;
        dialogueText.color = (speakerCharacter) switch
        {
            Characters.Dino => Util.StringToColor("#16c72e"),
            Characters.Heptagram => Util.StringToColor("#e8e230"),
            Characters.Rather => Util.StringToColor("#2fc9e8"),
            _ => dialogueText.color,
        };

        if (speakerCharacter == Characters.Dino)
        {
            if (EndingProgress > 4)
                dialogueTextbox.position = _characters[Characters.Sleepground].position + Vector3.up * 30;
            if (EndingProgress == 28)
            {
                dialogueTextbox.position = new Vector3(30, 60);
                dialogueTextbox.localScale = Vector3.one * 1f;
            }
        }
        if (speakerCharacter == Characters.Heptagram)
        {
            if (EndingProgress == 25)
            {
                dialogueTextbox.position = new Vector3(0, 20);
                dialogueTextbox.localScale = Vector3.one * 1f;
            }
        }
    }

    private IEnumerator LoopAddProgress()
    {
        yield return new WaitForSeconds(0.5f);

        while (EndingProgress < Manager.Data.endingScript.Length - 1)
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

    private void CreateParticle(Sprites.Ending type)
    {
        GameObject prefab = Manager.Resource.LoadResource<GameObject>(Prefabs.Ending.EndingParticle);
        GameObject go = prefab.CreateClone();
        go.transform.SetParent(GetChild(ParticleRoot));
        EndingParticle particle = go.Component<EndingParticle>();
        particle.Init(type, _characters);
        particle.OnEndingProgressChange(EndingProgress);
        _onEndingProgressChange.Add(this, () => particle.OnEndingProgressChange(EndingProgress));
    }

    private IEnumerator StartSleepground()
    {
        yield return new WaitUntil(() => EndingProgress == 20);

        Transform transform = _characters[Characters.Sleepground];
        Transform heptagram = _characters[Characters.Heptagram];

        yield return transform.MoveToTransformOverTime(0.2f, heptagram);

        yield return transform.MoveOverTime(0.3f, new Vector3(-200f, 100f));

        EndingProgress++;

        yield return transform.MoveOverTime(0.2f, new Vector3(-100f, -50f));

        EndingProgress++;
    }

    private IEnumerator StartDino()
    {
        yield return new WaitUntil(() => EndingProgress == 4);

        Transform transform = _characters[Characters.Dino];

        foreach (int i in Util.Count(25))
        {
            CreateParticle(Sprites.Ending.Particle_Purple);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.5f);

        transform.gameObject.SetActive(false);

        Transform sleepground = _characters[Characters.Sleepground];
        yield return transform.MoveToTransformOverTime(0.2f, sleepground);
        transform.AddY(25f);

        EndingProgress++;

        while (EndingProgress < 21)
        {
            transform.position = sleepground.position + Vector3.up * 25f;
            yield return null;
        }
    }

    private IEnumerator StartHeptagram()
    {
        yield return new WaitUntil(() => EndingProgress == 19);

        Transform transform = _characters[Characters.Heptagram];

        transform.gameObject.SetActive(false);

        foreach (int i in Util.Count(25))
        {
            CreateParticle(Sprites.Ending.Particle_Yellow);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.5f);

        EndingProgress++;
    }

    private IEnumerator StartRather()
    {
        yield return new WaitUntil(() => EndingProgress == 22);

        yield return new WaitForSeconds(0.5f);

        Transform transform = _characters[Characters.Rather];

        yield return transform.MoveOverTime(0.5f, new Vector3(-125f, 10f));

        yield return new WaitForSeconds(0.75f);

        EndingProgress++;

        transform.gameObject.SetActive(false);
    }

    private IEnumerator StartEndCutscene()
    {
        Image back = GetChild(EndCutsceneBack);
        Image front = GetChild(EndCutsceneFront);


        back.enabled = false;
        front.enabled = false;
        yield return new WaitUntil(() => Regex.IsMatch(EndingCurrentLine, Manager.Data.patternCutscene));
        front.enabled = true;

        while (EndingProgress != Manager.Data.endingScript.Length - 1)
        {
            Match match = Regex.Match(EndingCurrentLine, Manager.Data.patternCutscene);

            if (match.Success)
            {
                Sprites.Ending enumValue = Enum.Parse<Sprites.Ending>(match.Groups["sprite"].Value);
                Sprite sprite = Manager.Resource.LoadResource<Sprite>(enumValue);
                front.sprite = sprite;
            }

            yield return new WaitForFixedUpdate();
        }
        front.sprite = spriteThankYouForPlaying;

        Vector3 scale = Vector3.one * 400f / 375f;
        back.transform.localScale = scale;
        front.transform.localScale = scale;

        yield return new WaitForSeconds(4f);

        front.sprite = spriteEndcard1;
        back.transform.localScale = Vector3.one;
        front.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(2f);

        back.sprite = front.sprite;
        back.enabled = true;

        front.SetTransparency(1f);
        front.sprite = spriteEndcard2;
        foreach (int i in Util.Count(50))
        {
            front.AddTransparency(-0.02f);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(5f);

        Util.StartScene(Scenes.LobbyScene);
    }
}
