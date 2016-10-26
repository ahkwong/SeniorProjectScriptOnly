namespace Seniors.Skills.Buffs
{
    public class BurnDebuff : Buff
    {
        public override void OnTick()
        {
            target.Damage(owner, damage);
        }
    }
}