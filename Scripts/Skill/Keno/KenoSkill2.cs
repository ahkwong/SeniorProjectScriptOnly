using Assets.Scripts.Entities.Hero;
using Senior.Inputs;
using UnityEngine;

namespace Seniors.Skills
{
    public class KenoSkill2 : Skill
    {
        public float rollforce = 1f;
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;

            if (!IsDisabled)
            {
                //todo: become invulnerable throughout duration
                IsDisabled = true;
                OnCast();
                anim.SetTrigger("Skill2");
                rb.AddForce(hc.LastMoveDirection * rollforce, ForceMode.Impulse);
            }
        }
    }
}