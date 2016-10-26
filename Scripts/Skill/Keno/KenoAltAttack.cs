using System.Collections;
using UnityEngine;

namespace Seniors.Skills
{
    public class KenoAltAttack : Skill
    {

        public GameObject LightningBolt;
        public float LightningBoltOffset;

        public override void ActivateDown()
        {
            anim.SetTrigger("AltAttack");
        }

        public void ActivateCollider()
        {
            
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



        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "AltAttack_Lightning":
                    if (LightningBolt != null)
                    {
                        Instantiate(LightningBolt, owner.transform.position + LightningBoltOffset * owner.transform.forward, Quaternion.identity);
                    }

                    break;
                case "AltAttack_SphereColliderActivate":
                    if (SCollider != null)
                    {
                        SCollider.enabled = true;
                    }
                    break;
                case "AltAttack_SphereColliderDeactivate":
                    if (SCollider != null)
                    {
                        SCollider.enabled = false;
                    }
                    break;
            }
        }
    }
}