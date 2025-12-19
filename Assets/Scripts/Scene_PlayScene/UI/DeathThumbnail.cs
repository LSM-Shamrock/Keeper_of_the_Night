using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathThumbnail : PlaySceneObjectBase
{
    protected override void Start()
    {
        StartCoroutine(ShowWhenPlayerDie());
    }
    IEnumerator ShowWhenPlayerDie()
    {
        Image image = GetComponent<Image>();
        image.enabled = false;
        yield return WaitUntil(() => remainingHealth > 0 && suhyenHealth > 0 && healthInDream > 0);
        yield return WaitUntil(() => remainingHealth <= 0 || suhyenHealth <= 0 || healthInDream <= 0);
        image.enabled = true;
    }
}
