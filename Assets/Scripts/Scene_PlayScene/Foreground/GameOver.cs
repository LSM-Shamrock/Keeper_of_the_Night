using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : PlaySceneObjectBase
{
    protected override void Start()
    {
        StartCoroutine(ShowWhenPlayerDie());
    }
    private void FixedUpdate()
    {
        Update_HealthMin();
    }

    void StopCodeOfAnotherObject()
    {
        PlaySceneObjectBase[] codes = FindObjectsByType<PlaySceneObjectBase>(FindObjectsSortMode.None);
        foreach (var code in codes)
        {
            if (code == this) 
                continue;
            code.enabled = false;
            code.StopAllCoroutines();
        }
    }

    IEnumerator ShowWhenPlayerDie()
    {
        yield return WaitUntil(() => remainingHealth > 0 && suhyenHealth > 0 && healthInDream > 0);
        yield return WaitUntil(() => remainingHealth <= 0 || suhyenHealth <= 0 || healthInDream <= 0);
        yield return WaitForSeconds(0.1f); 
        StopCodeOfAnotherObject();

        Image image = GetComponent<Image>();
        Color color = image.color;
        color.a = 0.5f;
        image.color = color;
        image.enabled = true;

        Vector3 position = transform.position;
        position.x = 300f;
        transform.position = position;
        for (int i = 12; i > 0; i--)
        {
            transform.position += Vector3.right * -25f;
            yield return waitForFixedUpdate;
        }
    }

    void Update_HealthMin()
    {
        if (remainingHealth < 0) 
            remainingHealth = 0;
        if (suhyenHealth < 0)
            suhyenHealth = 0;
        if (healthInDream < 0)
            healthInDream = 0;
    }
}
