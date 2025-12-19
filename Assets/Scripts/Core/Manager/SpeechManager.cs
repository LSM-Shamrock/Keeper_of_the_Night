using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechManager 
{
    private Transform SpeechbubbleRoot
    {
        get
        {
            if (s_speechbubbleRoot == null)
            {
                var prefab = Utility.LoadResource<GameObject>(Prefabs.Core.SpeechbubbleCanvas);
                var go = prefab.CreateClone();
                var canvas = go.Component<Canvas>();
                canvas.worldCamera = Camera.main;

                s_speechbubbleRoot = go.transform;
            }
            return s_speechbubbleRoot;
        }
    }
    private Transform s_speechbubbleRoot;
    private Dictionary<Transform, Speechbubble> s_speechbubbles = new();
    private Inputbox s_inputbox;
    private Inputbox Inputbox
    {
        get
        {
            if (s_inputbox == null)
            {
                var prefab = Utility.LoadResource<GameObject>(Prefabs.Core.InputboxCanvas);
                var go = prefab.CreateClone();
                var inputbox = go.Component<Inputbox>();
                inputbox.Init();
                s_inputbox = inputbox;
            }
            return s_inputbox;
        }
    }
    public void Speech(Transform transform, string text)
    {
        Speechbubble speechbubble;
        if (s_speechbubbles.TryGetValue(transform, out var saved) && saved != null)
        {
            speechbubble = saved;
        }
        else
        {
            var prefab = Utility.LoadResource<GameObject>(Prefabs.Core.Speechbubble);
            var go = GameObject.Instantiate(prefab, SpeechbubbleRoot);
            speechbubble = go.Component<Speechbubble>();
            speechbubble.Init(transform);
            s_speechbubbles[transform] = speechbubble;
        }
        speechbubble.Show(text);
    }
    public void EraseSpeachbubble(Transform transform)
    {
        var speechbubble = s_speechbubbles[transform];
        speechbubble.Hide();
    }
    public IEnumerator SpeechForSeconds(Transform transform, string text, float seconds)
    {
        Speech(transform, text);
        yield return new WaitForSeconds(seconds);
        EraseSpeachbubble(transform);
    }
    public IEnumerator SpeechAndWaitInput(Transform transform, string text, Action<string> action)
    {
        Speech(transform, text);

        yield return Inputbox.ShowAndWaitInput(action);

        EraseSpeachbubble(transform);
    }
}
