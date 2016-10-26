using Senior.Items;
using UnityEngine;

namespace Seniors.Skills.Andrew
{
    public class AndrewSkill1 : Skill
    {
        public GameObject fireEssensePrefab;
        public float bombDamage;
        public float buffDamage;
        public float buffDuration = 5;
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;

            if (!IsDisabled)
            {
                IsDisabled = true;
                OnCast();
                var fireEssenceGo = TrashMan.spawn(fireEssensePrefab, owner.transform.position, Quaternion.identity);
                FireEssence fe = fireEssenceGo.GetComponent<FireEssence>();
                fe.bombDamage = bombDamage;
                fe.buffDamage = buffDamage;
                fe.buffDuration = buffDuration;
                fe.Initialize(owner);
            }
        }
    }
}