using UnityEngine;

namespace Seniors.Skills.Poring
{
    public class AttackSlash : Skill
    {
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;

            anim.SetTrigger("Slash");
        }

        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "AttackSlash_BoxColliderActivate":
                    if (BCollider != null)
                    {
                        BCollider.enabled = true;
                    }
                    break;
                case "AttackSlash_BoxColliderDeactivate":
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