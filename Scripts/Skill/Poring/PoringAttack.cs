using UnityEngine;

namespace Seniors.Skills.Poring
{
    public class PoringAttack : Skill
    {
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;

                anim.SetTrigger("Stab");        
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
    }
}