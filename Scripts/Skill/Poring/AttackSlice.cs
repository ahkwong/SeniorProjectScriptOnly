using UnityEngine;

namespace Seniors.Skills.Poring
{
    public class AttackSlice : Skill
    {
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;

            anim.SetTrigger("Slice");
        }

        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "AttackSlice_BoxColliderActivate":
                    if (BCollider != null)
                    {
                        BCollider.enabled = true;
                    }
                    break;
                case "AttackSlice_BoxColliderDeactivate":
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