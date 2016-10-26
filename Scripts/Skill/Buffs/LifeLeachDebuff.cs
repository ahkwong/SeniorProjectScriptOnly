namespace Seniors.Skills.Buffs
{
    public class LifeLeachDebuff : Buff
    {
        public override void OnTick()
        {
            target.Damage(owner, damage);
            owner.Heal(damage);
        }
    }
}