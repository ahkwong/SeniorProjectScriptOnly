using Senior.Items;
using UnityEngine;

namespace Seniors.Skills.Andrew
{
    public class AndrewSkill2 : Skill
    {
        public GameObject iceEssensePrefab;
        public float bombDamage;
        public float stunDuration;
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;

            if (!IsDisabled)
            {
                IsDisabled = true;
                OnCast();
                var fireEssenceGo = TrashMan.spawn(iceEssensePrefab, owner.transform.position, Quaternion.identity);
                IceEssence fe = fireEssenceGo.GetComponent<IceEssence>();
                fe.bombDamage = bombDamage;
                fe.stunDuration = stunDuration;
                fe.Initialize(owner);

            }
        }
    }
}