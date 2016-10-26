using Assets.Scripts.Entities;
using Seniors.Skills.Buffs;
using Seniors.Skills.Projectiles;
using UnityEngine;

namespace Senior.Items
{
    public class DemonicEssence : Item
    {
        public GameObject DemonicBomb;
        public float bombDamage;
        public float buffDamage;
        public float duration;
        public override void OnHit(Entity target, float damage)
        {
            var bombGo = TrashMan.spawn(DemonicBomb, target.transform.position, Quaternion.identity);
            Bomb bomb = bombGo.GetComponent<Bomb>();
            bomb.damage = bombDamage;
            bomb.buffDamage = buffDamage;
            bomb.lifeSpan = duration;
            bomb.Initialize(owner, owner.enemyFactions);
            OnUse();

        }
    }
}