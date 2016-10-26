using System.Collections.Generic;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Hero;
using Senior.Inputs;
using Seniors.Skills.Projectiles;
using UnityEngine;

namespace Seniors.Skills
{
    public class KenoSkill4 : Skill
    {
        public List<Entity> enemies = new List<Entity>();

        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;

            if (!IsDisabled)
            {
                IsDisabled = true;
                OnCast();
                anim.SetTrigger("Skill4");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Entity entitiy = other.gameObject.GetComponent<Entity>();

            if (entitiy != null)
            {
                if ((owner.enemyFactions & entitiy.currentFaction) == entitiy.currentFaction)
                {
                    if (!enemies.Contains(entitiy))
                        enemies.Add(entitiy);
                }
            }
        }

        // When an enemy exits the trigger, remove it from the list
        void OnTriggerExit(Collider other)
        {
            Entity entitiy = other.gameObject.GetComponent<Entity>();

            if (entitiy != null)
            {
                if ((owner.enemyFactions & entitiy.currentFaction) == entitiy.currentFaction)
                    enemies.Remove(entitiy);
            }
        }

        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "Skill4_Launch":
                    ShootProjectile();
                    break;
            }
        }

        public override void ShootProjectile()
        {
            if (enemies.Count > 0)
            {
                if (projectilePrefab != null)
                {
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (enemies[i] != null)
                        {
                            var pro = TrashMan.spawn(projectilePrefab, enemies[i].transform.position,
                                Quaternion.identity);
                            Projectile proj = pro.GetComponent<Projectile>();
                            if (proj != null)
                            {
                                proj.damage = damage;
                                proj.Initialize(owner, owner.enemyFactions);


                            }
                        }
                    }

                }
            }
            
        }
    }
}