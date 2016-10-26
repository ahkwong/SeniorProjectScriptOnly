using Assets.Scripts.Entities;
using UnityEngine;

namespace Seniors.Skills.Buffs
{
    public class StealthBuff : Buff
    {
        public int heal = 0;
        public override void OnAdd()
        {
            // become invisible by turning off your collider
            target.GoInvisible();
        }
        public override void OnTick()
        {
            target.Heal(heal);
        }

        public override void OnHit(Entity target, float damage)
        {
            // When a player hits someone, then not invis anymore
            //TrashMan.despawn(gameObject);
        }

        public override void OnDisable()
        {
            target.UnInvisible();

            base.OnDisable();
        }
    }
}