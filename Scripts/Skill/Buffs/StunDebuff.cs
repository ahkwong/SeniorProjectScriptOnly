using UnityEngine.UI;

namespace Seniors.Skills.Buffs
{
    public class StunDebuff : Buff
    {
        public override void OnAdd()
        {
            if (target != null)
            {
                target.mc.CanMove = false;
                target.anim.SetBool("Stunned", true);
            }
        }

        public override void OnDisable()
        {
            if (target != null)
            {
                target.mc.CanMove = true;
                target.anim.SetBool("Stunned", false);
            }

            base.OnDisable();
        }
    }
}