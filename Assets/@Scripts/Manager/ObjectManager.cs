using System.Collections;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public void Despawn(GameObject obj)
    {
        GameObject.Destroy(obj.gameObject);
    }

    private IEnumerator WaitAndDespawn(GameObject obj, float sec)
    {
        yield return new WaitForSeconds(sec);
        Despawn(obj);
    }

    public void DespawnAfterSec(GameObject obj, float sec)
    {
        StartCoroutine(WaitAndDespawn(obj, sec));
    }
}
