using System;
using System.Collections;
using UnityEngine;

public static class CoroutineUtil
{
    public static void StartRunForSec(MonoBehaviour obj, float sec, Action<float> action) 
      => obj.StartCoroutine(RunForSec(sec, action));
    public static void StartWaitAndRun(MonoBehaviour obj, float sec, Action action) 
      => obj.StartCoroutine(WaitAndRun(sec, action));
    public static void StartWaitAndRunForCount(MonoBehaviour obj, float interval, int count, Action action) 
      => obj.StartCoroutine(WaitAndRunForCount(interval, count, action));
    public static void StartRunAndWaitForCount(MonoBehaviour obj, float interval, int count, Action action) 
      => obj.StartCoroutine(RunAndWaitForCount(interval, count, action));
    public static void StartWaitAndRunLoop(MonoBehaviour obj, float interval, Action action) 
      => obj.StartCoroutine(WaitAndRunLoop(interval, action));
    public static void StartRunAndWaitLoop(MonoBehaviour obj, float interval, Action action) 
      => obj.StartCoroutine(RunAndWaitLoop(interval, action));
    public static void StartWaitAndRunWhile(MonoBehaviour obj, float interval, Func<bool> checker, Action action) 
        => obj.StartCoroutine(WaitAndRunWhile(interval, checker, action));
    public static void StartRunAndWaitWhile(MonoBehaviour obj, float interval, Func<bool> checker, Action action) 
      => obj.StartCoroutine(RunAndWaitWhile(interval, checker, action));

    public static IEnumerator RunForSec(float sec, Action<float> action)
    {
        for (float s = 0.0f; s < sec; s += Time.deltaTime)
        {
            action?.Invoke(s);
            yield return null;
        }
    }
    public static IEnumerator WaitAndRun(float sec, Action action)
    {
        yield return new WaitForSeconds(sec);
        action?.Invoke();
    }
    public static IEnumerator WaitAndRunForCount(float interval, int count, Action action)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(interval);
            action?.Invoke();
        }
    }
    public static IEnumerator RunAndWaitForCount(float interval, int count, Action action)
    {
        for (int i = 0; i < count; i++)
        {
            action?.Invoke();
            yield return new WaitForSeconds(interval);
        }
    }
    public static IEnumerator WaitAndRunLoop(float interval, Action action)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            action?.Invoke();
        }
    }
    public static IEnumerator RunAndWaitLoop(float interval, Action action)
    {
        while (true)
        {
            action?.Invoke();
            yield return new WaitForSeconds(interval);
        }
    }
    public static IEnumerator WaitAndRunWhile(float interval, Func<bool> checker, Action action)
    {
        while (checker.Invoke())
        {
            yield return new WaitForSeconds(interval);
            action?.Invoke();
        }
    }
    public static IEnumerator RunAndWaitWhile(float interval, Func<bool> checker, Action action)
    {
        while (checker.Invoke())
        {
            action?.Invoke();
            yield return new WaitForSeconds(interval);
        }
    }
}
