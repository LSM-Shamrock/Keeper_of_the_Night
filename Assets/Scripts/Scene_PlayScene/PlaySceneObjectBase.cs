using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShadowState 
{ None, Killed, Giantization }

public class PlaySceneObjectBase : ObjectBase
{
    // 플레이어 캐릭터
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


    // 플레이어 캐릭터 스킬 
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


    // 신호, 변수
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


    #region 야괴 이름 외치기
    protected static string shoutedEnemyName;
    protected readonly List<string> hiddenSurnames = new()
    {
        "김", "이", "박", "최", "정",
        "강", "조", "윤", "장", "임",
        "한", "오", "서", "신", "권",
        "황", "안", "송", "전", "홍",
    };
    protected readonly List<string> hiddenMainames = new()
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
    #endregion

}
