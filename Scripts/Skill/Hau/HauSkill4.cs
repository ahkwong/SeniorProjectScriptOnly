using Seniors.Skills.Projectiles;
using UnityEngine;

namespace Seniors.Skills.Hau
{
    public class HauSkill4 : Skill
    {
        public GameObject InfernoBomb;
        public float bombDamage;
        public float buffDamage;
        public float buffDuration;
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;

            if (!IsDisabled)
            {
                IsDisabled = true;
                anim.SetTrigger("Skill4");
            }
        }
        public override void RaiseEvent(string eventName)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Fire Bomb")) return;

            switch (eventName)
            {
                case "TossBomb":
                    var bombGo = TrashMan.spawn(InfernoBomb, owner.transform.position, Quaternion.identity);
                    Bomb bomb = bombGo.GetComponent<Bomb>();
                    bomb.damage = bombDamage;
                    bomb.buffDamage = buffDamage;
                    bomb.buffDuration = buffDuration;
                    bomb.Initialize(owner, owner.enemyFactions);
                    OnCast();
                    break;
            }
        }
    }
}