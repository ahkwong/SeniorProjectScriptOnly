using Seniors.Skills.Projectiles;
using UnityEngine;

namespace Seniors.Skills.Hau
{
    public class HauAttack : Skill
    {
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;


            anim.SetTrigger("Attack");
        }
        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "Attack_BoxColliderActivate":
                    if (BCollider != null)
                    {
                        BCollider.enabled = true;
                    }
                    break;
                case "Attack_BoxColliderDeactivate":
                    if (BCollider != null)
                    {
                        BCollider.enabled = false;
                    }
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            OnHit(other);
        }

        public override void OnHit(Collider hit)
        {
            base.OnHit(hit);
            StartCoroutine(FreezeFrame());

        }
    }
}