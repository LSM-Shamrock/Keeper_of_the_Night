using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected virtual void Start() { }

    protected float DistanceTo(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance;
    }

    // 변수
    protected static int wave;
    protected static Sprites.Characters selectedCharacter = Sprites.Characters.Sleepground;
    protected static string characterDescription = "월광검으로 근거리 공격";
    protected static string specialDescription = "월광검 방어막";
    protected static int characterMaxHealth = 200;


    // 흐름
    protected float FixedDeltaTime => Time.fixedDeltaTime;
    protected readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    protected WaitForSeconds WaitForSeconds(float seconds)
    {
        return new WaitForSeconds(seconds);
    }
    protected WaitUntil WaitUntil(Func<bool> predicate)
    {
        return new WaitUntil(predicate);
    }
    protected WaitWhile WaitWhile(Func<bool> predicate)
    {
        return new WaitWhile(predicate);
    }
    protected IEnumerable<int> Count(int count)
    {
        for (int i = 0; i < count; i++)
            yield return i;
    }

    // 입력 
    protected bool IsContactMousePointer { get; private set; }
    protected bool IsMouseClicked { get; private set; }
    protected bool IsPressedW { get; private set; }
    protected bool IsPressedA { get; private set; }
    protected bool IsPressedS { get; private set; }
    protected bool IsPressedD { get; private set; }
    protected bool IsPressedN { get; private set; }
    protected bool IsPressedT { get; private set; }
    protected bool IsPressedEnter { get; private set; }
    public virtual void OnPointerEnter(PointerEventData eventData) 
    { 
        IsContactMousePointer = true; 
    }
    public virtual void OnPointerExit(PointerEventData eventData) 
    { 
        IsContactMousePointer = false; 
    }
    protected virtual void Update()
    {
        IsMouseClicked = Input.GetMouseButton(0);
        IsPressedW = Input.GetKey(KeyCode.W);
        IsPressedA = Input.GetKey(KeyCode.A);
        IsPressedS = Input.GetKey(KeyCode.S);
        IsPressedD = Input.GetKey(KeyCode.D);
        IsPressedN = Input.GetKey(KeyCode.N);
        IsPressedT = Input.GetKey(KeyCode.T);
        IsPressedEnter = Input.GetKey(KeyCode.Return);
    }


    // 말하기
    static Transform SpeechbubbleRoot
    {
        get
        {
            if (s_speechbubbleRoot == null)
            {
                var prefab = Utile.LoadResource<GameObject>(Prefabs.Core.SpeechbubbleCanvas);
                var go = prefab.CreateClone();
                var canvas = go.Component<Canvas>();
                canvas.worldCamera = Camera.main;

                s_speechbubbleRoot = go.transform;
            }
            return s_speechbubbleRoot;
        }
    }
    static Transform s_speechbubbleRoot;
    static Dictionary<Transform, Speechbubble> s_speechbubbles = new();
    static Inputbox s_inputbox;
    static Inputbox Inputbox
    {
        get
        {
            if (s_inputbox == null)
            {
                var prefab = Utile.LoadResource<GameObject>(Prefabs.Core.InputboxCanvas);
                var go = prefab.CreateClone();
                var inputbox = go.Component<Inputbox>();
                inputbox.Init();
                s_inputbox = inputbox;
            }
            return s_inputbox;
        }
    }
    protected void Speech(string text)
    {
        Speechbubble speechbubble;
        if (s_speechbubbles.TryGetValue(transform, out var saved) && saved != null)
        {
            speechbubble = saved;
        }
        else
        {
            var prefab = Utile.LoadResource<GameObject>(Prefabs.Core.Speechbubble);
            var go = Instantiate(prefab, SpeechbubbleRoot);
            speechbubble = go.Component<Speechbubble>();
            speechbubble.Init(transform);
            s_speechbubbles[transform] = speechbubble;
        }
        speechbubble.Show(text);
    }
    protected void EraseSpeachbubble()
    {
        var speechbubble = s_speechbubbles[transform];
        speechbubble.Hide();
    }
    protected IEnumerator SpeechForSeconds(string text, float seconds)
    {
        Speech(text);
        yield return WaitForSeconds(seconds);
        EraseSpeachbubble();
    }
    protected IEnumerator SpeechAndWaitInput(string text, Action<string> action)
    {
        Speech(text);

        yield return Inputbox.ShowAndWaitInput(action);

        EraseSpeachbubble();
    }
}