using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathThumbnail : BaseController
{
    protected override void Start()
    {
        StartCoroutine(ShowWhenPlayerDie());
    }
    IEnumerator ShowWhenPlayerDie()
    {
        Image image = GetComponent<Image>();
        image.enabled = false;
        yield return WaitUntil(() => Manager.Game.remainingHealth > 0 && Manager.Game.suhyenHealth > 0 && Manager.Game.healthInDream > 0);
        yield return WaitUntil(() => Manager.Game.remainingHealth <= 0 || Manager.Game.suhyenHealth <= 0 || Manager.Game.healthInDream <= 0);
        image.enabled = true;
    }
}
