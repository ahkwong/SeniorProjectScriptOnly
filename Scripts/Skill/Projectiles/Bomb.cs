using Assets.Scripts.Entities;
using Seniors.Skills.Buffs;
using UnityEngine;

namespace Seniors.Skills.Projectiles
{
    public class Bomb : Projectile
    {
        public GameObject BuffToBeApplied;
        public float buffDamage;
        public float buffDuration;
        public override void OnHit(Entity target)
        {
            base.OnHit(target);

            if (BuffToBeApplied != null)
            {
                var buffGo = TrashMan.spawn(BuffToBeApplied, target.transform.position, Quaternion.identity);
                Buff buff = buffGo.GetComponent<Buff>();
                buff.damage = buffDamage;
                buff.lifeSpan = buffDuration;
                buff.Initialize(owner,target);
            }

        }
    }
}