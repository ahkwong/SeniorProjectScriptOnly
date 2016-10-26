using Assets.Scripts.Entities;
using Seniors.Skills;
using Seniors.Skills.Buffs;
using Seniors.Skills.Projectiles;
using UnityEngine;

namespace Senior.Items
{
    public class IceEssence : Item
    {
        public GameObject IceBomb;
        public float bombDamage;
        public float stunDuration;
        public override void OnHit(Entity target, float damage)
        {
            var bombGo = TrashMan.spawn(IceBomb, target.transform.position, Quaternion.identity);
            Bomb bomb = bombGo.GetComponent<Bomb>();
            bomb.damage = bombDamage;
            bomb.buffDuration = stunDuration;
            bomb.Initialize(owner, owner.enemyFactions);
            OnUse();

        }
    }
}