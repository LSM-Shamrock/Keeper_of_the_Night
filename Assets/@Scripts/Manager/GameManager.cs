using System.Collections.Generic;
using UnityEngine;

public class GameManager 
{

    public Characters selectedCharacter = Characters.Sleepground;
    public string characterDescription = "월광검으로 근거리 공격";
    public string specialDescription = "월광검 방어막";
    public Characters currentCharacter;
    public Vector3 characterMoveDirection;
    public int wave;
    public int remainingWaveSecond;
    public int remainingWaveKill;
    public bool isBossDinoKilled;
    public bool isSpecialSkillInvoking;
    public int ice;

    public readonly ActionEx onWaveClear = new();
    public readonly ActionEx onDisarmSpecialSkill = new();
    public readonly ActionEx onDreamghostAppearance = new();
    public readonly ActionEx onNightmareEvent = new();


    public readonly ActionEx onNightMareChange = new();
    private bool _isNightmare;
    public bool IsNightmare
    {
        get { return _isNightmare; }
        set { _isNightmare = value; onNightMareChange.Call(); }
    }

    public readonly ActionEx onShadowStateChange = new();
    private ShadowState _shadowState;
    public ShadowState ShadowState
    {
        get { return _shadowState; }
        set { _shadowState = value; onShadowStateChange.Call(); }
    }


    private float _specialSkillCooltime;
    public float SpecialSkillCooltime
    {
        get => _specialSkillCooltime;
        set => _specialSkillCooltime = Mathf.Max(value, 0f);
    }


    public readonly ActionEx onPlayerDie = new();
    private int _health;
    private int _suhyenHealth; 
    private int _dreamHealth;
    public int maxHealth = 200;
    public int suhyenMaxHealth = 60;
    public int dreamMaxHealth => maxHealth / 2;
    public int Health
    {
        get { return _health; }
        set 
        { 
            _health = Mathf.Clamp(value, 0, maxHealth); 
            if (value <= 0) onPlayerDie.Call(); 
        }
    }
    public int SuhyenHealth
    {
        get { return _suhyenHealth; } 
        set
        {
            _suhyenHealth = Mathf.Clamp(value, 0, suhyenMaxHealth);
            if (value <= 0) onPlayerDie.Call();
        }
    }
    public int DreamHealth
    {
        get { return _dreamHealth; }
        set
        {
            _dreamHealth = Mathf.Clamp(value, 0, dreamMaxHealth);
            if (value <= 0) onPlayerDie.Call();
        }
    }
    public void TakeDamageToPlayer(int damage)
    {
        if (currentCharacter == Characters.Suhyen)
        {
            SuhyenHealth -= damage;
            return;
        }
        if (IsNightmare)
        {
            DreamHealth -= damage;
            return;
        }
        Health -= damage;
    }




    public string shoutedEnemyName;
    public readonly List<string> hiddenSurnames = new()
    {
        "김", "이", "박", "최", "정",
        "강", "조", "윤", "장", "임",
        "한", "오", "서", "신", "권",
        "황", "안", "송", "전", "홍",
    };
    public readonly List<string> hiddenMainames = new()
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
