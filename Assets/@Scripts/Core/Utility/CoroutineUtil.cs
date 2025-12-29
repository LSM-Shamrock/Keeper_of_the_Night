using System;
using System.Collections;
using UnityEngine;

public static class CoroutineUtil
{
    public static IEnumerator RunForSec(float sec, Action<float> action)
    {
        for (float s = 0.0f; s < sec; s += Time.deltaTime)
        {
            action?.Invoke(s);
            yield return null;
        }
    }
    public static void StartRunForSec(MonoBehaviour obj, float sec, Action<float> action)
    {
        obj.StartCoroutine(RunForSec(sec, action));
    }

    public static IEnumerator WaitAndRun(float sec, Action action)
    {
        yield return new WaitForSeconds(sec);
        action?.Invoke();
    }
    public static void StartWaitAndRun(MonoBehaviour obj, float sec, Action action)
    {
        obj.StartCoroutine(WaitAndRun(sec, action));
    }


    public static IEnumerator LoopWaitAndRun(float interval, Action action)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            action?.Invoke();
        }
    }
    public static IEnumerator LoopWaitAndRun(float interval, int count, Action action, Action endCallback=null)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(interval);
            action?.Invoke();
        }
        endCallback?.Invoke();
    }
    public static IEnumerator LoopWaitAndRun(float interval, Func<bool> checker, Action action, Action endCallback=null)
    {
        while (checker.Invoke())
        {
            yield return new WaitForSeconds(interval);
            action?.Invoke();
        }
        endCallback?.Invoke();
    }

    public static IEnumerator LoopRunAndWait(float interval, Action action)
    {
        while (true)
        {
            action?.Invoke();
            yield return new WaitForSeconds(interval);
        }
    }
    public static IEnumerator LoopRunAndWait(float interval, int count, Action action, Action endCallback=null)
    {
        for (int i = 0; i < count; i++)
        {
            action?.Invoke();
            yield return new WaitForSeconds(interval);
        }
        endCallback?.Invoke();
    }
    public static IEnumerator LoopRunAndWait(float interval, Func<bool> checker, Action action, Action endCallback=null)
    {
        while (checker.Invoke())
        {
            action?.Invoke();
            yield return new WaitForSeconds(interval);
        }
        endCallback?.Invoke();
    }

}
