

using Senior.Managers;
using Seniors.Skills.Projectiles;
using UnityEngine;
using UnityEngine.UI;

namespace Seniors.Skills.Andrew
{
    public class AndrewAltAttack : Skill
    {
        public float maxChargeTime;
        public GameObject chargePrefab;
        private GameObject chargeInstance;
        public float baseSpeed = 2;
        public float damageMultiplier = 2;
        public float speedMultiplier = 10;
        private float chargeTime = 0;
        public override void Start()
        {
            
        }
        public override void ActivateDown()
        {
            if (sc.IsBusy) return;
            base.ActivateDown();
            anim.SetTrigger("AltAttack");
            anim.SetBool("AltHold", true);

            sc.IsBusy = true;
            hc.OnlyRotate = true;
            hc.RotateBasedOnMovement = true;
            owner.channelFill.fillAmount = 0;

            owner.channelBarGO.SetActive(true);
            chargeInstance = TrashMan.spawn(chargePrefab, owner.transform.position, Quaternion.identity);
        }

        public override void Update()
        {
            base.Update();

            if (buttonHold)
            {
                owner.channelFill.fillAmount = buttonHoldTimePressed / maxChargeTime;
            }

        }

        public override void ActivateUp()
        {
            chargeTime = buttonHoldTimePressed;
            base.ActivateUp();

            anim.SetBool("AltHold", false);
            owner.channelBarGO.SetActive(false);
            owner.channelFill.fillAmount = 0;
            
            if (chargeInstance != null)
                Destroy(chargeInstance);


        }

        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "AltAttack_Shoot":
                    ShootProjectile();
                    break;
                case "AltAttack_NotBusy":
                    sc.IsBusy = false;
                    hc.OnlyRotate = false;
                    hc.RotateBasedOnMovement = false;
                    break;
            }
        }

        public override void ShootProjectile()
        {
            if (projectilePrefab != null)
            {
                OnCast();
                var pro = TrashMan.spawn(projectilePrefab, owner.transform.position + (projectileOffset * owner.transform.forward) + (0.4f * owner.transform.up),
                    owner.transform.rotation);

                Projectile proj = pro.GetComponent<Projectile>();
                if (proj != null)
                {
                    proj.damage = damage * (1 + damageMultiplier * (chargeTime / maxChargeTime));
                    proj.speed = baseSpeed + speedMultiplier * chargeTime / maxChargeTime;
                    proj.Initialize(owner, owner.enemyFactions);
                }
            }
        }
    }
}