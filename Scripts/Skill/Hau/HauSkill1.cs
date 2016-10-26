using Assets.Scripts.Entities;
using Seniors.Skills.Buffs;
using UnityEngine;

namespace Seniors.Skills.Hau
{
    public class HauSkill1 : Skill
    {
        public bool dashing;
        public float dashSpeed;
        public GameObject stunDebuff;
        public float stunDuration;
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;

            if (!IsDisabled)
            {
                IsDisabled = true;
                OnCast();
                anim.SetTrigger("Skill1");

            }
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
                case "Skill1_StartDash":
                    rb.AddForce(rb.position + hc.LastMoveDirection * dashSpeed,ForceMode.Impulse);

                    if (BCollider != null)
                    {
                        BCollider.enabled = true;
                    }
                    break;
                case "Skill1_EndDash":
                    if (BCollider != null)
                    {
                        BCollider.enabled = false;
                    }
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //stun debuff
            OnHit(other);
        }

        public override void OnHit(Collider hit)
        {
            Entity entity = hit.gameObject.GetComponent<Entity>();
            if (entity != null)
            {
                if ((owner.enemyFactions & entity.currentFaction) == entity.currentFaction)
                {
                    owner.OnHit(entity, damage);
                    entity.Damage(owner, damage);
                    if (knockback)
                    {
                        Vector3 direction = (entity.transform.position - owner.transform.position).normalized;
                        entity.gameObject.GetComponent<Rigidbody>().AddForce(direction * knockbackForce, ForceMode.Impulse);
                    }


                    var buff = TrashMan.spawn(stunDebuff, hit.transform.position, Quaternion.identity);
                    Buff stun = buff.GetComponent<Buff>();
                    stun.lifeSpan = stunDuration;
                    stun.Initialize(owner, entity);

                    StartCoroutine(FreezeFrame());
                }
            }


        }
    }
}