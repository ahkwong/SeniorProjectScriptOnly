using Assets.Scripts.Entities;
using Seniors.Skills.Buffs;
using Seniors.Skills.Projectiles;
using UnityEngine;

namespace Senior.Items
{
    public class LightningEssence : Item
    {
        public GameObject LightningBomb;
        public float bombDamage;
        public float buffDamage;
        public float duration;
        public override void OnHit(Entity target, float damage)
        {
            var bombGo = TrashMan.spawn(LightningBomb, target.transform.position, Quaternion.identity);
            Bomb bomb = bombGo.GetComponent<Bomb>();
            bomb.damage = bombDamage;
            bomb.buffDamage = buffDamage;
            bomb.buffDuration = duration;
            bomb.Initialize(owner, owner.enemyFactions);
            OnUse();

        }
    }
}