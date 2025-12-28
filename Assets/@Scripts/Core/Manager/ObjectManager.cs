using System.Collections;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private Transform _character;
    private Transform _moonlightswordShield;
    private Transform _waterPrison;
    public Transform Character
    {
        get
        {
            if (_character == null)
                _character = Utility.FindGameObject(PlaySceneObjects.Character).transform;
            return _character;
        }
    }
    public Transform MoonlightswordShield
    {
        get
        {
            if (_moonlightswordShield == null)
                _moonlightswordShield = Utility.FindGameObject(PlaySceneObjects.MoonlightswordShield).transform;
            return _moonlightswordShield;
        }
    }
    public Transform WaterPrison
    {
        get
        {
            if (_waterPrison == null)
                _waterPrison = Utility.FindGameObject(PlaySceneObjects.WaterPrison).transform;
            return _waterPrison;
        }
    }


    private Camera _mainCamera;
    public Camera MainCamera
    {
        get
        {
            if (_mainCamera == null)
                _mainCamera = Camera.main;
            return _mainCamera;
        }
    }


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
