using Assets.Scripts.Entities;
using UnityEngine;

namespace Seniors.Skills.Lung
{
    public class LungAltAttack : Skill
    {
        public bool dashing;
        public float dashSpeed;
        public float backstabMultiplier = 3;
        public GameObject backstabHitPrefab;
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;


            anim.SetTrigger("AltAttack");
        }

        public override void FixedUpdate()
        {
            if (dashing)
                Dash();
        }

        private void Dash()
        {
            rb.AddForce(rb.position + hc.LastMoveDirection * dashSpeed);
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
                    dashing = true;
                    break;
                case "AltAttack_BoxColliderDeactivate":
                    if (BCollider != null)
                    {
                        BCollider.enabled = false;
                    }
                    dashing = false;
                    break;
                case "AltAttack_Shoot":
                    ShootProjectile();
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Vector3 positionFromTarget = (other.transform.position - owner.transform.position).normalized;

            if (Vector3.Dot(positionFromTarget, other.transform.forward) > 0)
            {
                //Hero is behind target
                BackstabOnHit(other);
            }
            else
            {
                OnHit(other);

            }
        }

        public  void BackstabOnHit(Collider hit)
        {
            Entity entity = hit.gameObject.GetComponent<Entity>();
            if (entity != null)
            {
                if ((owner.enemyFactions & entity.currentFaction) == entity.currentFaction)
                {
                    owner.OnHit(entity, damage * backstabMultiplier);
                    entity.Damage(owner, damage * backstabMultiplier);

                    TrashMan.spawn(backstabHitPrefab, entity.transform.position, Quaternion.identity);
                }
            }

        }
    }
}