using System.Collections.Generic;
using UnityEngine;

public class GameManager 
{
    private float _skillCooltime;
    private int _health;
    private int _suhyenHealth; 
    private int _dreamHealth;
    private Vector3 _characterMoveDirection;
    private bool _isNightmare;
    private ShadowState _shadowState;

    public readonly ActionEx onWaveClear = new();
    public readonly ActionEx onDisarmSpecialSkill = new();
    public readonly ActionEx onDreamghostAppearance = new();
    public readonly ActionEx onNightmareEvent = new();
    public readonly ActionEx onNightMareChange = new();
    public readonly ActionEx onShadowStateChange = new();
    public readonly ActionEx onSkillCooltimeChange = new();
    public readonly ActionEx onPlayerDie = new();


    // getter only
    public CharacterData currentCharacterData => Manager.Data.characterDatas[currentCharacter];
    public int suhyenMaxHealth { get; } = 60;
    public int dreamMaxHealth => currentCharacterData.maxHealth / 2;
    public bool isPlayerDie => health <= 0 || suhyenHealth <= 0 || dreamHealth <= 0;

    // getter and setter
    public Characters selectedCharacter { get; set; } = Characters.Sleepground;
    public Characters currentCharacter { get; set; }
    public float skillCooltime 
    { 
        get => _skillCooltime; 
        set 
        { 
            _skillCooltime = Mathf.Max(value, 0f); 
            onSkillCooltimeChange.Call(); 
        } 
    }
    public int health 
    { 
        get => _health; 
        set 
        { 
            _health = Mathf.Clamp(value, 0, currentCharacterData.maxHealth); 
            if (value <= 0) onPlayerDie.Call(); 
        } 
    }
    public int suhyenHealth 
    { 
        get => _suhyenHealth; 
        set 
        { 
            _suhyenHealth = Mathf.Clamp(value, 0, suhyenMaxHealth); 
            if (value <= 0) onPlayerDie.Call(); 
        } 
    }
    public int dreamHealth 
    { 
        get => _dreamHealth; 
        set 
        { 
            _dreamHealth = Mathf.Clamp(value, 0, dreamMaxHealth); 
            if (value <= 0) onPlayerDie.Call(); 
        } 
    }
    public bool isSpecialSkillInvoking { get; set; }
    public int ice { get; set; }
    public string shoutedEnemyName { get; set; }
    
    public int wave { get; set; }
    public int remainingWaveSecond { get; set; }
    public int remainingWaveKill { get; set; }
    public ShadowState shadowState 
    { 
        get => _shadowState; 
        set 
        { 
            _shadowState = value; 
            onShadowStateChange.Call(); 
        } 
    }
    public bool isNightmare 
    { 
        get => _isNightmare; 
        set 
        { 
            _isNightmare = value; 

            if (isNightmare == true) 
                skillCooltime = 0f;

            onNightMareChange.Call();
        } 
    }
    public bool isBossDinoKilled { get; set; }



    public void Init()
    {
        health = currentCharacterData.maxHealth;
        suhyenHealth = suhyenMaxHealth;
        dreamHealth = dreamMaxHealth;

        isSpecialSkillInvoking = false;
        ice = 0;
        shoutedEnemyName = null;

        isNightmare = false;
        isBossDinoKilled = false;
    }

    public void TakeDamageToPlayer(int damage)
    {
        if (currentCharacter == Characters.Suhyen)
            suhyenHealth -= damage;
        else if (isNightmare)
            dreamHealth -= damage;
        else
            health -= damage;
    }
}
