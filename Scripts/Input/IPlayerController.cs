using UnityEngine;

namespace Senior.Inputs
{
    public interface IPlayerController
    {
        Vector2 MoveInput { get; }
        bool AttackButtonDown { get; }
        bool AttackButtonUp { get; }
        bool AltAttackButtonDown { get; }
        bool AltAttackButtonUp { get; }
        bool SkillOneButtonDown { get; }
        bool SkillOneButtonUp { get; }
        bool SkillTwoButtonDown { get; }
        bool SkillTwoButtonUp { get; }
        bool SkillThreeButtonDown { get; }
        bool SkillThreeButtonUp { get; }
        bool SkillFourButtonDown { get; }
        bool SkillFourButtonUp { get; }
    }
}