using UnityEngine;

public class Manager 
{
    private static Manager _instance;
    private static Manager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Manager();
            return _instance;
        }
    }

    private InputManager _input;
    public static InputManager Input
    {
        get
        {
            if (Instance._input == null)
                Instance._input = new InputManager();
            return Instance._input;
        }
    }

    private SpeechManager _speech;
    public static SpeechManager Speech
    {
        get
        {
            if (Instance._speech == null)
                Instance._speech = new SpeechManager();
            return Instance._speech;
        }
    }

    private GameManager _game;
    public static GameManager Game
    {
        get
        {
            if (Instance._game == null)
                Instance._game = new GameManager();
            return Instance._game;
        }
    }
}
