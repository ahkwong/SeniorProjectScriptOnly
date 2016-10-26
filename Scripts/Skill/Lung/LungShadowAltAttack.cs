using UnityEngine;

namespace Seniors.Skills.Lung
{
    public class LungShadowAltAttack : Skill
    {
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;


            anim.SetTrigger("AltAttack");
        }
        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "AltAttack_BoxColliderActivate":
                    if (BCollider != null)
                    {
                        BCollider.enabled = true;
                    }
                    break;
                case "AltAttack_BoxColliderDeactivate":
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