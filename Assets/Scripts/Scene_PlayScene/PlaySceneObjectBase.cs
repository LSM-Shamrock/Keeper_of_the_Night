using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShadowState 
{ None, Killed, Giantization }

public class PlaySceneObjectBase : ObjectBase
{
    // �÷��̾� ĳ����
    static Transform s_character;
    protected static Transform Character
    {
        get
        {
            if (s_character == null)
                s_character = Utile.FindGameObject(PlaySceneObjects.Character).transform;
            return s_character;
        }
    }

    protected static Vector3 characterMoveDirection;

    protected static void TakeDamageToPlayer(int damage)
    {
        if (currentCharacter == Sprites.Characters.Suhyen)
        {
            suhyenHealth -= damage;
        }
        else if (isNightmare)
        {
            healthInDream -= damage;
        }
        else
        {
            remainingHealth -= damage;
        }
    }


    // �÷��̾� ĳ���� ��ų 
    static Transform s_moonlightswordShield;
    protected static Transform MoonlightswordShield
    {
        get
        {
            if (s_moonlightswordShield == null)
                s_moonlightswordShield = Utile.FindGameObject(PlaySceneObjects.MoonlightswordShield).transform;
            return s_moonlightswordShield;
        }
    }

    static Transform s_waterPrison;
    protected static Transform WaterPrison
    {
        get
        {
            if (s_waterPrison == null)
                s_waterPrison = Utile.FindGameObject(PlaySceneObjects.WaterPrison).transform;
            return s_waterPrison;
        }
    }

    static Transform s_bossDinoBlackBall;
    protected static Transform BossDinoBlackBall
    {
        get
        {
            if (s_bossDinoBlackBall == null)
                s_bossDinoBlackBall = Utile.FindGameObject(PlaySceneObjects.BossDinoBlackBall).transform;
            return s_bossDinoBlackBall;
        }
    }


    // ��ȣ, ����
    protected static readonly SignalOfMonoBehaviour onWaveClear = new(); 
    protected static readonly SignalOfMonoBehaviour onDisarmSpecialSkill = new(); 
    protected static readonly SignalOfMonoBehaviour onDreamghostAppearance = new();
    protected static readonly SignalOfMonoBehaviour onNightmareEvent = new(); 
    protected static Sprites.Characters currentCharacter;
    protected static bool isSpecialSkillInvoking;
    protected static float specialSkillCooltime;
    protected static int remainingHealth;
    protected static int suhyenHealth;
    protected static int healthInDream;
    protected static int ice;
    protected static int remainingWaveSecond;
    protected static int remainingWaveKill;
    protected static ShadowState shadowState;
    protected static bool isNightmare;
    protected static bool isBossDinoKilled;


    #region �߱� �̸� ��ġ��
    protected static string shoutedEnemyName;
    protected readonly List<string> hiddenSurnames = new()
    {
        "��", "��", "��", "��", "��",
        "��", "��", "��", "��", "��",
        "��", "��", "��", "��", "��",
        "Ȳ", "��", "��", "��", "ȫ",
    };
    protected readonly List<string> hiddenMainames = new()
    {
        "����", "����", "����", "����", "�ÿ�",
        "����", "�ֿ�", "��ȣ", "�ؿ�", "����",
        "����", "����", "����", "����", "����",
        "����", "����", "�ǿ�", "����", "�ؼ�",
        "����", "����", "����", "����", "����",
        "����", "����", "����", "����", "����",
        "�ϸ�", "����", "����", "�μ�", "ä��",
        "����", "����", "�þ�", "����", "����",
    };
    #endregion

}
