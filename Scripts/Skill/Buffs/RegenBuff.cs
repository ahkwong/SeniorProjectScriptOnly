namespace Seniors.Skills.Buffs
{
    public class RegenBuff : Buff
    {
        public int heal = 0;
        public override void OnTick()
        {
            target.Heal(heal);
        }
    }
}