using Senior.Items;
using Seniors.Skills.Projectiles;
using UnityEngine;

namespace Seniors.Skills.Hau
{
    public class HauSkill2 : Skill
    {
        public GameObject HealingBomb;
        public float bombDamage;
        public float buffDuration;
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;

            if (!IsDisabled)
            {
                IsDisabled = true;
                anim.SetTrigger("Skill2");
            }
        }
        public override void RaiseEvent(string eventName)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Healing Bomb")) return;

            switch (eventName)
            {
                case "TossBomb":
                    var bombGo = TrashMan.spawn(HealingBomb, owner.transform.position, Quaternion.identity);
                    Bomb bomb = bombGo.GetComponent<Bomb>();
                    bomb.damage = bombDamage;
                    bomb.buffDuration = buffDuration;
                    bomb.Initialize(owner, owner.alliedFactions);
                    OnCast();
                    break;
            }
        }
    }
}