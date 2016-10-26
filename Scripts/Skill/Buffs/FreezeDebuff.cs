namespace Seniors.Skills.Buffs
{
    public class FreezeDebuff : Buff
    {

        public override void OnAdd()
        {
            if (target != null)
            {
                target.mc.CanMove = false;
                target.anim.speed = 0;
            }
        }

        public override void OnDisable()
        {
            if (target != null)
            {
                target.mc.CanMove = true;
                target.anim.speed = 1;
            }

            base.OnDisable();
        }

    }
}