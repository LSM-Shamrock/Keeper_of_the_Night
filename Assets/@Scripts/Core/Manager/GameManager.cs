using System.Collections.Generic;
using UnityEngine;

public class GameManager 
{

    #region wave
    public readonly ActionEx OnWaveClear = new();
    public readonly ActionEx OnShadowStateChange = new();
    public readonly ActionEx OnDreamghostAppearance = new();
    public readonly ActionEx OnNightmareEvent = new();

    public int Wave { get; set; }
    public int RemainingWaveSecond { get; set; }
    public int RemainingWaveKill { get; set; }
    public bool IsBossDinoKilled { get; set; }

    private ShadowState _shadowState;
    public ShadowState ShadowState 
    { 
        get { return _shadowState; } 
        set 
        { 
            _shadowState = value; 
            OnShadowStateChange.Call(); 
        } 
    }

    private bool _isNightmare;
    public bool IsNightmare 
    { 
        get { return _isNightmare; } 
        set 
        { 
            _isNightmare = value; 
            if (IsNightmare) SkillCooltime = 0f;
        } 
    }
    #endregion


    #region player
    public readonly ActionEx OnDisarmSpecialSkill = new();
    public readonly ActionEx OnPlayerDie = new();
    public readonly ActionEx OnSkillCooltimeChange = new();

    public string ShoutedEnemyName 
    { 
        get; 
        set; 
    }

    public Characters SelectedCharacter 
    { 
        get; 
        set; 
    } = Characters.Sleepground;
    public Characters CurrentCharacter 
    { 
        get; 
        set; 
    }
    public CharacterData CurrentCharacterData
    {
        get
        {
            return Manager.Data.characterDatas[CurrentCharacter];
        }
    }

    public int Ice 
    { 
        get; 
        set; 
    }

    private float _skillCooltime;
    public float SkillCooltime 
    { 
        get { return _skillCooltime; } 
        set 
        { 
            _skillCooltime = Mathf.Max(value, 0f); 
            OnSkillCooltimeChange.Call(); 
        } 
    }
    public bool IsSpecialSkillInvoking 
    { 
        get; 
        set; 
    }

    private int _health;
    private int _suhyenHealth; 
    private int _dreamHealth;
    public bool IsPlayerDie
    {
        get
        {
            return Health <= 0 || SuhyenHealth <= 0 || DreamHealth <= 0;
        }
    }
    public int SuhyenMaxHealth 
    { 
        get; 
    } = 60;
    public int DreamMaxHealth
    {
        get
        {
            return CurrentCharacterData.maxHealth / 2;
        }
    }
    public int Health 
    { 
        get => _health; 
        set 
        { 
            _health = Mathf.Clamp(value, 0, CurrentCharacterData.maxHealth); 
            if (value <= 0) OnPlayerDie.Call(); 
        } 
    }
    public int SuhyenHealth 
    { 
        get => _suhyenHealth; 
        set 
        { 
            _suhyenHealth = Mathf.Clamp(value, 0, SuhyenMaxHealth); 
            if (value <= 0) OnPlayerDie.Call(); 
        } 
    }
    public int DreamHealth 
    { 
        get => _dreamHealth; 
        set 
        { 
            _dreamHealth = Mathf.Clamp(value, 0, DreamMaxHealth); 
            if (value <= 0) OnPlayerDie.Call(); 
        } 
    }
    #endregion


    public void Init()
    {
        Health = CurrentCharacterData.maxHealth;
        SuhyenHealth = SuhyenMaxHealth;
        DreamHealth = DreamMaxHealth;

        IsSpecialSkillInvoking = false;
        Ice = 0;
        ShoutedEnemyName = null;

        IsNightmare = false;
        IsBossDinoKilled = false;
    }

    public void TakeDamageToPlayer(int damage)
    {
        if (CurrentCharacter == Characters.Suhyen)
            SuhyenHealth -= damage;
        else if (IsNightmare)
            DreamHealth -= damage;
        else
            Health -= damage;
    }
}
