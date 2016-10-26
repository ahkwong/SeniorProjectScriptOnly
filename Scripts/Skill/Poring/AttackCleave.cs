using UnityEngine;

namespace Seniors.Skills.Poring
{
    public class AttackCleave : Skill
    {
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;

            anim.SetTrigger("Cleave");
        }

        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "AttackCleave_BoxColliderActivate":
                    if (BCollider != null)
                    {
                        BCollider.enabled = true;
                    }
                    break;
                case "AttackCleave_BoxColliderDeactivate":
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
    }
}