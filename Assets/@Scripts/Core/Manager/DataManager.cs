using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    public string name;
    public int startHP;
    public float startSize;
    public EnemyData(string name, int startHP, float startSize)
    {
        this.name = name;
        this.startHP = startHP;
        this.startSize = startSize;
    }
}

public class CharacterData
{
    public string name;
    public string description;
    public string specialDescription;
    public int maxHealth;
    public CharacterData(string name, string description, string specialDescription, int maxHealth)
    {
        this.name = name;
        this.description = description;
        this.specialDescription = specialDescription;
        this.maxHealth = maxHealth;
    }
    public override string ToString() => name;
}

public class DataManager 
{
    // character data
    public readonly Dictionary<Characters, CharacterData> characterDatas = new()
    {
        { Characters.Sleepground, new(
            "잠뜰",
            "월광검으로 근거리 공격", 
            "월광검 방어막", 
            200) },
        { Characters.Rather, 
            new(
            "라더",
            "물로 곡선형 공격", 
            "물감옥 생성", 
            200) },
        { Characters.Dino, new(
            "공룡",
            "월광건으로 장거리공격", 
            "야괴로 변신해 흡혈", 
            100) },
    };


    // enemy data
    public readonly Dictionary<Enemys, EnemyData> enemyDatas = new()
    {
        { Enemys.Shadow, new("영도", 100, 25.0f) },
        { Enemys.VoidCavity, new("허강", 12, 31.4f) },
        { Enemys.CrazyLaughMask, new("광소탈", 18, 31.9f) },
        { Enemys.MotherSpiritSnake, new("모령사", 23, 43.2f) },
        { Enemys.Bird, new("새", 20, 31.0f) },
        { Enemys.SadEyes, new("비안", 20, 31.4f) },
        { Enemys.DreamGhost, new("몽귀", 1, 50.0f) },
        { Enemys.ThePiedPiper, new("하민우", 18, 36.6f) },
        { Enemys.Fire, new("화이독", 23, 40.5f) },
        { Enemys.Red, new("홍난구", 25, 54.9f) },
        { Enemys.SnowLady, new("설희", 23, 50.0f) },
        { Enemys.BossDino, new("공룡", 100, 76.8f) },
    };


    // enemy hidden name data
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


    // ending data
    public readonly string patternDialogue = @"^(?<speaker>.+)\((?<name>.+)\)\s+(?<line>.+)$";
    public readonly string patternCutscene = @"^\*(?<sprite>.*)$";
    public readonly string[] endingScript = new[]
    {
        string.Empty,
        nameof(Characters.Dino) +       "(공룡) 너는 돌아가면 또다시 기억을 잃고 행복하게 살 수 있겠지..",
        nameof(Characters.Dino) +       "(공룡) 하지만 난 아니야",
        nameof(Characters.Dino) +       "(공룡) 내아들을 보고도 알아보지 못한 사실이 얼마나..",
        string.Empty,
        nameof(Characters.Rather) +     "(라더) 야 이 썩을놈아!",
        nameof(Characters.Heptagram) +  "(각별) 안돼,안돼!",
        nameof(Characters.Dino) +       "(공룡) 나만 이렇게 당하고 갈순 없잖아",
        nameof(Characters.Heptagram) +  "(각별) 형, 그아이 한테서 나와",
        nameof(Characters.Heptagram) +  "(각별) 그 아이는 잘못 없잖아",
        nameof(Characters.Dino) +       "(공룡) 얘가 그때 니가 동희대신 살렸던 아이",
        nameof(Characters.Dino) +       "(공룡) 맞지?",
        nameof(Characters.Heptagram) +  "(각별) 제발 정신 차려",
        nameof(Characters.Dino) +       "(공룡) 이 아이만 없었더라면",
        nameof(Characters.Dino) +       "(공룡) 우리 동희는 살 수 있었다고!!",
        nameof(Characters.Rather) +     "(라더) 저,저놈이..",
        nameof(Characters.Rather) +     "(라더) 이런,뭐라도 좀 해봐!",
        nameof(Characters.Heptagram) +  "(각별) 살아있는 인간..",
        nameof(Characters.Heptagram) +  "(각별) 이 방법밖에 없는 것인가..",
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        "*" + nameof(Sprites.Ending.EndCutscene_1),
        "*" + nameof(Sprites.Ending.EndCutscene_2),
        nameof(Characters.Heptagram) +  "(각별) 미안해",
        "*" + nameof(Sprites.Ending.EndCutscene_3),
        "*" + nameof(Sprites.Ending.EndCutscene_4),
        nameof(Characters.Dino) +       "(공룡) 너를 지키지 못한 내가 밉구나",
        "*" + nameof(Sprites.Ending.EndCutscene_5),
        "*" + nameof(Sprites.Ending.EndCutscene_6),
        "*" + nameof(Sprites.Ending.EndCutscene_7),
        "*" + nameof(Sprites.Ending.EndCutscene_8),
        string.Empty,
    };
}
