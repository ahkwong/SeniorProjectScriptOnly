using Seniors.Skills.Projectiles;
using UnityEngine;

namespace Seniors.Skills.Hau
{
    public class HauSkill3 : Skill
    {
        public GameObject IceNovaBomb;
        public float bombDamage;
        public float buffDuration;
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;

            if (!IsDisabled)
            {
                IsDisabled = true;
                anim.SetTrigger("Skill3");
            }
        }
        public override void RaiseEvent(string eventName)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Ice Bomb")) return;

            switch (eventName)
            {
                case "TossBomb":
                    var bombGo = TrashMan.spawn(IceNovaBomb, owner.transform.position, Quaternion.identity);
                    Bomb bomb = bombGo.GetComponent<Bomb>();
                    bomb.damage = bombDamage;
                    bomb.buffDuration = buffDuration;
                    bomb.Initialize(owner, owner.enemyFactions);
                    OnCast();
                    break;
            }
        }
    }
}