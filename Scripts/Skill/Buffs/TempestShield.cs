using System.Collections.Generic;
using Assets.Scripts.Entities;
using UnityEngine;

namespace Seniors.Skills.Buffs
{
    public class TempestShield : Buff
    {
        public float knockbackForce;

        public List<Entity> enemies = new List<Entity>();

        public override void OnTick()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                Hit(enemies[i]);
            }
        }

        public void OnTriggerEnter(Collider collision)
        {
            Entity entity = collision.gameObject.GetComponent<Entity>();
            if (entity != null)
            {
                if ((owner.enemyFactions & entity.currentFaction) == entity.currentFaction)
                {
                    if (!enemies.Contains(entity))
                        enemies.Add(entity);

                    Hit(entity);
                }
            }
        }

        private void Hit(Entity entity)
        {

            entity.Damage(owner,damage);
            //owner.OnHit(entity, damage);
            if (entity != null && target != null)
            {
                Vector3 direction = (entity.transform.position - target.transform.position).normalized;
                entity.gameObject.GetComponent<Rigidbody>().AddForce(direction*knockbackForce, ForceMode.Impulse);
            }
        }

        void OnTriggerExit(Collider other)
        {
            Entity entitiy = other.gameObject.GetComponent<Entity>();

            if (entitiy != null)
            {
                if ((owner.enemyFactions & entitiy.currentFaction) == entitiy.currentFaction)
                    enemies.Remove(entitiy);
            }
        }
    }
}