using System.Collections.Generic;
using UnityEngine;

public class GameManager 
{

    public Characters selectedCharacter = Characters.Sleepground;
    public Characters currentCharacter;
    public CharacterData currentCharacterData => Manager.Data.characterDatas[currentCharacter];
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


    public readonly ActionEx onSkillCooltimeChange = new();
    private float _skillCooltime;
    public float SkillCooltime
    {
        get => _skillCooltime;
        set
        {
            _skillCooltime = Mathf.Max(value, 0f);
            onSkillCooltimeChange.Call();
        }
    }


    public readonly ActionEx onPlayerDie = new();
    private int _health;
    private int _suhyenHealth; 
    private int _dreamHealth;
    public int suhyenMaxHealth = 60;
    public int dreamMaxHealth => currentCharacterData.maxHealth / 2;
    public bool isPlayerDie => Health <= 0 || SuhyenHealth <= 0 || DreamHealth <= 0;
    public int Health
    {
        get { return _health; }
        set 
        { 
            _health = Mathf.Clamp(value, 0, currentCharacterData.maxHealth); 
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
            SuhyenHealth -= damage;
        else if (IsNightmare)
            DreamHealth -= damage;
        else
            Health -= damage;
    }
    



    public string shoutedEnemyName;
    


}
