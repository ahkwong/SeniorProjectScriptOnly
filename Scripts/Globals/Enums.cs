namespace Senior.Globals
{
    public enum GameState
    {
        MainMenu,
        CharacterSelect,
        InGame,
        Loading,
        LoseScreen,
        WinScreen
    }

    public enum PlayerState
    {
        ChoosingCharacter,
        ConfirmedCharacter,
        Playing,
        WaitingForCredits,
        HeroDead,
    }

    public enum SkillTargeting
    {
        Self,
        Auto
    }

    public enum Faction
    {
        Neutral = 1,    
        Player = 2,
        BorosLegion = 4,
        Conclave = 8,
        Demon = 16
    }

    public enum BuffType
    {
        Buff,
        Debuff
    }

    public enum SkillType
    {
        BasicAttack,            // has no cooldown and does not appear on the UI
        UiSkill,                // has a cooldown and does appear on the UI
    }

    public enum TurrateState
    {
        Follow,
        Attack
    }

    public enum MonsterState
    {
        Idle,
        Patrol,
        Stunned,
        Attacking,
    }

    public enum AIBehaviorType
    {
        None,
        Wander,
        FollowAttack,
        Idle,
        NormalAttack
    }

    public enum EntityStatusState
    {
        Normal,
        Stunned,
        Invis,
        Dead
    }

}