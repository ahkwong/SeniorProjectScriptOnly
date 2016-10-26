using Senior.Items;
using UnityEngine;

namespace Seniors.Skills.Andrew
{
    public class AndrewSkill4 : Skill
    {
        public GameObject demonicEssensePrefab;
        public float bombDamage;
        public float buffDamage;
        public float debuffDuration;
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;

            if (!IsDisabled)
            {
                IsDisabled = true;
                OnCast();
                var lightingEssenseGo = TrashMan.spawn(demonicEssensePrefab, owner.transform.position, Quaternion.identity);
                DemonicEssence fe = lightingEssenseGo.GetComponent<DemonicEssence>();
                fe.bombDamage = bombDamage;
                fe.buffDamage = buffDamage;
                fe.duration = debuffDuration;
                fe.Initialize(owner);
            }
        }
    }
}