using Assets.Scripts.Entities;
using Seniors.Skills.Buffs;
using Seniors.Skills.Projectiles;
using UnityEngine;

namespace Seniors.Skills.Lung
{
    public class LungSkill2 : Skill
    {
        public GameObject stealthBuff;
        public GameObject SmokeBomb;
        public float buffDuration = 3;
        public float stealthDuration = 5;
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;


            if (!IsDisabled)
            {
                IsDisabled = true;
                OnCast();
                anim.SetTrigger("Skill2");

            }

        }

        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "Skill2_CastSmoke":
                    var bombGo = TrashMan.spawn(SmokeBomb, owner.transform.position, Quaternion.identity);
                    Bomb bomb = bombGo.GetComponent<Bomb>();
                    bomb.buffDuration = buffDuration;
                    bomb.Initialize(owner, owner.enemyFactions);
                    OnCast();
                    break;
                case "Skill2_GoInvis":
                    var buffGO = TrashMan.spawn(stealthBuff, owner.transform.position, Quaternion.identity);
                    Buff buff = buffGO.GetComponent<Buff>();
                    buff.lifeSpan = stealthDuration;
                    buff.Initialize(owner,owner);
                    break;
            }
        }
    }
}