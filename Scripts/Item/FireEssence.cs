using Assets.Scripts.Entities;
using Seniors.Skills.Buffs;
using Seniors.Skills.Projectiles;
using UnityEngine;

namespace Senior.Items
{
    public class FireEssence : Item
    {
        public GameObject FireBomb;
        public float bombDamage;
        public float buffDamage;
        public float buffDuration;
        public override void OnHit(Entity target, float damage)
        {
            var bombGo = TrashMan.spawn(FireBomb, target.transform.position, Quaternion.identity);
            Bomb bomb = bombGo.GetComponent<Bomb>();
            bomb.damage = bombDamage;
            bomb.buffDamage = buffDamage;
            bomb.buffDuration = buffDuration;
            bomb.Initialize(owner, owner.enemyFactions);
            OnUse();

        }
    }
}