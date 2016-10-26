using System.Collections;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Seniors.Skills
{
    public class SwingSword : Skill
    {
        [Header("Skill")]
        public bool isActive = false;
        public bool dashing;
        public float dashSpeed;
        public override void ActivateDown()
        {
            anim.SetTrigger("Attack");
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

        private void LookAtDirection()
        {
            if (hc.MoveDirection != Vector3.zero)
            {
                hc.LastMoveDirection = hc.MoveDirection;
                owner.transform.rotation = Quaternion.LookRotation(hc.MoveDirection);
            }
        }

        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "Attack_EnableTransition":
                    anim.SetBool("EnableTransition", true);
                    break;
                case "Attack_DisableTransition":
                    anim.SetBool("EnableTransition", false);
                    anim.SetBool("CanMove", false);
                    hc.CanMove = false;
                    LookAtDirection();
                    OnCast();
                    break;
                case "Attack_EnableMove":
                    hc.CanMove = true;
                    anim.SetBool("CanMove", true);

                    break;
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
                case "Attack_SphereColliderActivate":
                    if (SCollider != null)
                    {
                        SCollider.enabled = true;
                    }
                    break;
                case "Attack_SphereColliderDeactivate":
                    if (SCollider != null)
                    {
                        SCollider.enabled = false;
                    }
                    break;
                case "Attack_StartDash":
                    dashing = true;
                    break;
                case "Attack_EndDash":
                    dashing = false;
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