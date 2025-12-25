using UnityEngine;

public class Manager 
{
    private static Manager _instance = new Manager();
    private static Manager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Manager();
            return _instance;
        }
    }

    private InputManager _input = new InputManager();
    public static InputManager Input => Instance._input;

    private SpeechManager _speech = new SpeechManager();
    public static SpeechManager Speech => Instance._speech;

    private GameManager _game = new GameManager();
    public static GameManager Game => Instance._game;

    private ObjectManager _object;
    public static ObjectManager Object
    {
        get
        {
            if (Instance._object == null)
            {
                GameObject go = new GameObject($"@{nameof(ObjectManager)}");
                Instance._object = go.AddComponent<ObjectManager>();
                GameObject.DontDestroyOnLoad(go);
            }
            return Instance._object;
        }
    }

    private ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource => Instance._resource;
}
