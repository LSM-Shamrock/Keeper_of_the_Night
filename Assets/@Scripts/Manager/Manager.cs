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
    public static InputManager Input
    {
        get
        {
            if (Instance._input == null)
                Instance._input = new InputManager();
            return Instance._input;
        }
    }

    private SpeechManager _speech = new SpeechManager();
    public static SpeechManager Speech
    {
        get
        {
            if (Instance._speech == null)
                Instance._speech = new SpeechManager();
            return Instance._speech;
        }
    }

    private GameManager _game = new GameManager();
    public static GameManager Game
    {
        get
        {
            if (Instance._game == null)
                Instance._game = new GameManager();
            return Instance._game;
        }
    }

    private ObjectManager _object = new ObjectManager();
    public static ObjectManager Object
    {
        get
        {
            if (Instance._object == null)
                Instance._object = new ObjectManager();
            return Instance._object;
        }
    }
}
