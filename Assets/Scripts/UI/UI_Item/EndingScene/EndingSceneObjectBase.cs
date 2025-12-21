using System.Collections.Generic;
using UnityEngine;


public abstract class EndingSceneObjectBase : BaseController
{

    protected readonly static Dictionary<Sprites.Characters, Transform> Character = new();


    protected static int EndingProgress = 1;
    protected static string EndingCurrentLine => EndingScript[EndingProgress];
    
    protected static readonly string Pattern_Dialogue = @"^(?<speaker>.+)\((?<name>.+)\)\s+(?<line>.+)$";
    protected static readonly string Pattern_Cutscene = @"^\*(?<sprite>.*)$";

    protected static readonly string[] EndingScript = new[]
    {
        string.Empty,
        nameof(Sprites.Characters.Dino) +       "(공룡) 너는 돌아가면 또다시 기억을 잃고 행복하게 살 수 있겠지..",
        nameof(Sprites.Characters.Dino) +       "(공룡) 하지만 난 아니야",
        nameof(Sprites.Characters.Dino) +       "(공룡) 내아들을 보고도 알아보지 못한 사실이 얼마나..",
        string.Empty,                                           
        nameof(Sprites.Characters.Rather) +     "(라더) 야 이 썩을놈아!",
        nameof(Sprites.Characters.Heptagram) +  "(각별) 안돼,안돼!",
        nameof(Sprites.Characters.Dino) +       "(공룡) 나만 이렇게 당하고 갈순 없잖아",
        nameof(Sprites.Characters.Heptagram) +  "(각별) 형, 그아이 한테서 나와",
        nameof(Sprites.Characters.Heptagram) +  "(각별) 그 아이는 잘못 없잖아",
        nameof(Sprites.Characters.Dino) +       "(공룡) 얘가 그때 니가 동희대신 살렸던 아이",
        nameof(Sprites.Characters.Dino) +       "(공룡) 맞지?",
        nameof(Sprites.Characters.Heptagram) +  "(각별) 제발 정신 차려",
        nameof(Sprites.Characters.Dino) +       "(공룡) 이 아이만 없었더라면",
        nameof(Sprites.Characters.Dino) +       "(공룡) 우리 동희는 살 수 있었다고!!",
        nameof(Sprites.Characters.Rather) +     "(라더) 저,저놈이..",
        nameof(Sprites.Characters.Rather) +     "(라더) 이런,뭐라도 좀 해봐!",
        nameof(Sprites.Characters.Heptagram) +  "(각별) 살아있는 인간..",
        nameof(Sprites.Characters.Heptagram) +  "(각별) 이 방법밖에 없는 것인가..",
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,                                           
        "*" + nameof(Sprites.Ending.EndCutscene_1),                   
        "*" + nameof(Sprites.Ending.EndCutscene_2),                   
        nameof(Sprites.Characters.Heptagram) +  "(각별) 미안해",
        "*" + nameof(Sprites.Ending.EndCutscene_3),                   
        "*" + nameof(Sprites.Ending.EndCutscene_4),                   
        nameof(Sprites.Characters.Dino) +       "(공룡) 너를 지키지 못한 내가 밉구나",
        "*" + nameof(Sprites.Ending.EndCutscene_5),
        "*" + nameof(Sprites.Ending.EndCutscene_6),
        "*" + nameof(Sprites.Ending.EndCutscene_7),
        "*" + nameof(Sprites.Ending.EndCutscene_8),
        string.Empty,
    };

    
}
