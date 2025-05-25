using System.Collections.Generic;
using UnityEngine;


public abstract class EndingSceneObjectBase : ObjectBase
{

    protected readonly static Dictionary<Sprites.Characters, Transform> Character = new();


    protected static int EndingProgress = 1;
    protected static string EndingCurrentLine => EndingScript[EndingProgress];
    
    protected static readonly string Pattern_Dialogue = @"^(?<speaker>.+)\((?<name>.+)\)\s+(?<line>.+)$";
    protected static readonly string Pattern_Cutscene = @"^\*(?<sprite>.*)$";

    protected static readonly string[] EndingScript = new[]
    {
        string.Empty,
        nameof(Sprites.Characters.Dino) +       "(����) �ʴ� ���ư��� �Ǵٽ� ����� �Ұ� �ູ�ϰ� �� �� �ְ���..",
        nameof(Sprites.Characters.Dino) +       "(����) ������ �� �ƴϾ�",
        nameof(Sprites.Characters.Dino) +       "(����) ���Ƶ��� ���� �˾ƺ��� ���� ����� �󸶳�..",
        string.Empty,                                           
        nameof(Sprites.Characters.Rather) +     "(���) �� �� �������!",
        nameof(Sprites.Characters.Heptagram) +  "(����) �ȵ�,�ȵ�!",
        nameof(Sprites.Characters.Dino) +       "(����) ���� �̷��� ���ϰ� ���� ���ݾ�",
        nameof(Sprites.Characters.Heptagram) +  "(����) ��, �׾��� ���׼� ����",
        nameof(Sprites.Characters.Heptagram) +  "(����) �� ���̴� �߸� ���ݾ�",
        nameof(Sprites.Characters.Dino) +       "(����) �갡 �׶� �ϰ� ������ ��ȴ� ����",
        nameof(Sprites.Characters.Dino) +       "(����) ����?",
        nameof(Sprites.Characters.Heptagram) +  "(����) ���� ���� ����",
        nameof(Sprites.Characters.Dino) +       "(����) �� ���̸� ���������",
        nameof(Sprites.Characters.Dino) +       "(����) �츮 ����� �� �� �־��ٰ�!!",
        nameof(Sprites.Characters.Rather) +     "(���) ��,������..",
        nameof(Sprites.Characters.Rather) +     "(���) �̷�,���� �� �غ�!",
        nameof(Sprites.Characters.Heptagram) +  "(����) ����ִ� �ΰ�..",
        nameof(Sprites.Characters.Heptagram) +  "(����) �� ����ۿ� ���� ���ΰ�..",
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,                                           
        "*" + nameof(Sprites.Ending.EndCutscene_1),                   
        "*" + nameof(Sprites.Ending.EndCutscene_2),                   
        nameof(Sprites.Characters.Heptagram) +  "(����) �̾���",
        "*" + nameof(Sprites.Ending.EndCutscene_3),                   
        "*" + nameof(Sprites.Ending.EndCutscene_4),                   
        nameof(Sprites.Characters.Dino) +       "(����) �ʸ� ��Ű�� ���� ���� �ӱ���",
        "*" + nameof(Sprites.Ending.EndCutscene_5),
        "*" + nameof(Sprites.Ending.EndCutscene_6),
        "*" + nameof(Sprites.Ending.EndCutscene_7),
        "*" + nameof(Sprites.Ending.EndCutscene_8),
        string.Empty,
    };

    
}
