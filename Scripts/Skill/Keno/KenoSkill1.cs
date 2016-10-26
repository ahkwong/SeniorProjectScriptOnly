using Assets.Scripts.Entities.Hero;
using Senior.Inputs;
using Seniors.Skills.Buffs;
using Seniors.Skills.Projectiles;
using UnityEngine;

namespace Seniors.Skills
{
    public class KenoSkill1 : Skill
    {
        public GameObject SparksPrefab;
        public GameObject TempestShieldPrefab;
        public float duration;

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

        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "Skill1_Sparks":
                    Instantiate(SparksPrefab, owner.transform.position, Quaternion.identity);
                    break;
                case "Skill1_Shield":
                    var shieldGO = TrashMan.spawn(TempestShieldPrefab, owner.transform.position, Quaternion.identity);
                    TempestShield tshield = shieldGO.GetComponent<TempestShield>();
                    tshield.damage = damage;
                    tshield.lifeSpan = duration;
                    tshield.Initialize(owner,owner);
                    break;
            }
        }
    }
}