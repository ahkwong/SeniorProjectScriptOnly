using System.Collections;
using Assets.Scripts.Entities;
using Seniors.Skills.Buffs;
using UnityEngine;

namespace Seniors.Skills.Lung
{
    public class LungSkill3 : Skill
    {
        public bool dashing;
        public float dashSpeed;
        public float backstabMultiplier = 3;
        public GameObject backstabHitPrefab;
        public Buff BuffToBeApplied;
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;


            if (!IsDisabled)
            {
                IsDisabled = true;
                OnCast();
                anim.SetTrigger("Skill3");

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
                case "Skill3_BoxColliderActivate":
                    if (BCollider != null)
                    {
                        BCollider.enabled = true;
                    }
                    //dashing = true;
                    rb.AddForce(rb.position + hc.LastMoveDirection * dashSpeed, ForceMode.Impulse);
                    owner.collider.enabled = false;

                    break;
                case "Skill3_BoxColliderDeactivate":
                    if (BCollider != null)
                    {
                        BCollider.enabled = false;
                    }
                    //dashing = false;
                    break;
                case "Skill3_Pause":
                    StartCoroutine(Pause());
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

        public override void OnHit(Collider hit)
        {
            Entity entity = hit.gameObject.GetComponent<Entity>();
            if (entity != null)
            {
                if ((owner.enemyFactions & entity.currentFaction) == entity.currentFaction)
                {
                    Buff buff = Instantiate(BuffToBeApplied, entity.transform.position, Quaternion.identity) as Buff;
                    buff.Initialize(owner, entity);
                    owner.OnHit(entity, damage);
                    entity.Damage(owner, damage);
                    if (knockback)
                    {
                        Vector3 direction = (entity.transform.position - owner.transform.position).normalized;
                        entity.gameObject.GetComponent<Rigidbody>().AddForce(direction * knockbackForce, ForceMode.Impulse);
                    }
                }
            }

        }

        public void BackstabOnHit(Collider hit)
        {
            Entity entity = hit.gameObject.GetComponent<Entity>();
            if (entity != null)
            {
                if ((owner.enemyFactions & entity.currentFaction) == entity.currentFaction)
                {
                    Buff buff = Instantiate(BuffToBeApplied, entity.transform.position, Quaternion.identity) as Buff;
                    buff.Initialize(owner, entity);
                    owner.OnHit(entity, damage * backstabMultiplier);
                    entity.Damage(owner, damage * backstabMultiplier);

                    Instantiate(backstabHitPrefab, entity.transform.position, Quaternion.identity);
                }
            }

        }

        IEnumerator Pause()
        {
            anim.enabled = false;
            yield return new WaitForSeconds(.8f);
            anim.enabled = true;
            owner.collider.enabled = true;

        }
    }
}