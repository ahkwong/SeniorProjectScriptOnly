using Assets.Scripts.Entities.Hero;
using Senior.Inputs;
using UnityEngine;

namespace Seniors.Skills
{
    public class KenoSkill3 : Skill
    {
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;
            if (!IsDisabled)
            {
                IsDisabled = true;
                OnCast();
                anim.SetTrigger("Skill3");
            }
        }

        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "Skill3_Launch":
                    ShootProjectile();
                    break;
            }
        }
    }
}