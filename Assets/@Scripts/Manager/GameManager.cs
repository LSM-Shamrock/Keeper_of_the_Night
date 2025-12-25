using System.Collections.Generic;
using UnityEngine;

public class GameManager 
{
    public int wave;
    public Sprites.Characters selectedCharacter = Sprites.Characters.Sleepground;
    public string characterDescription = "월광검으로 근거리 공격";
    public string specialDescription = "월광검 방어막";
    public int characterMaxHealth = 200;




    private Transform _character;
    private Transform _moonlightswordShield;
    private Transform _waterPrison;
    private Transform _bossDinoBlackBall;
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

    public Vector3 characterMoveDirection;
    public void TakeDamageToPlayer(int damage)
    {
        if (currentCharacter == Sprites.Characters.Suhyen)
            suhyenHealth -= damage;
        else if (isNightmare)
            healthInDream -= damage;
        else
            remainingHealth -= damage;
    }




    // 신호, 변수
    public readonly ActionEx onWaveClear = new ActionEx();
    public readonly ActionEx onDisarmSpecialSkill = new ActionEx();
    public readonly ActionEx onDreamghostAppearance = new ActionEx();
    public readonly ActionEx onNightmareEvent = new ActionEx();


    public Sprites.Characters currentCharacter;
    public bool isSpecialSkillInvoking;
    public float specialSkillCooltime;


    public ActionEx onPlayerDie { get; private set; } = new ActionEx();

    private int _remainingHealth;
    public int remainingHealth
    {
        get { return _remainingHealth; }
        set
        {
            _remainingHealth = Mathf.Clamp(value, 0, characterMaxHealth);

            if (value <= 0)
                onPlayerDie.Call();
        }
    }


    private int _suhyenHealth;
    public int suhyenHealth
    {
        get { return _suhyenHealth; }
        set
        {
            _suhyenHealth = Mathf.Max(value, 0);

            if (value <= 0)
                onPlayerDie.Call();
        }
    }

    private int _healthInDream;
    public int healthInDream
    {
        get { return _healthInDream; }
        set
        {
            _healthInDream = Mathf.Clamp(value, 0, characterMaxHealth / 2);

            if (value <= 0)
                onPlayerDie.Call();
        }
    }

    public int ice;

    public int remainingWaveSecond;
    public int remainingWaveKill;

    public ActionEx onShadowStateChange { get; private set; } = new ActionEx();
    private ShadowState _shadowState;
    public ShadowState shadowState
    {
        get { return _shadowState; }
        set
        {
            _shadowState = value;
            onShadowStateChange.Call();
        }
    }

    public ActionEx onNightMareChange { get; private set; } = new ActionEx();
    private bool _isNightmare;
    public bool isNightmare
    {
        get { return _isNightmare; }
        set
        {
            _isNightmare = value;
            onNightMareChange.Call();
        }
    }

    public bool isBossDinoKilled;






    // 야괴 이름 외치기
    public string shoutedEnemyName;
    public readonly List<string> hiddenSurnames = new List<string>()
    {
        "김", "이", "박", "최", "정",
        "강", "조", "윤", "장", "임",
        "한", "오", "서", "신", "권",
        "황", "안", "송", "전", "홍",
    };
    public readonly List<string> hiddenMainames = new List<string>()
    {
        "서준", "하준", "도윤", "민준", "시우",
        "예준", "주원", "지호", "준우", "유준",
        "은우", "지후", "서진", "도현", "선우",
        "우진", "시윤", "건우", "연우", "준서",
        "서윤", "하윤", "서연", "지우", "하은",
        "지유", "지안", "서아", "수아", "지아",
        "하린", "다은", "서현", "민서", "채원",
        "소율", "윤서", "시아", "예린", "소윤",
    };
}
