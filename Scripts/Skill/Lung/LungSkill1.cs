using System.Collections.Generic;
using Assets.Scripts.Entities;
using Seniors.Skills.Projectiles;
using UnityEngine;

namespace Seniors.Skills.Lung
{
    public class LungSkill1 : Skill
    {
        public List<Entity> enemies = new List<Entity>();
        public GameObject SmokeBomb;
        public float buffDuration = 3;
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;


            if (!IsDisabled)
            {
                IsDisabled = true;
                OnCast();
                anim.SetTrigger("Skill1");

            }

        }

        //When an enemy comes into range, add it into the enemies list
        private void OnTriggerEnter(Collider other)
        {
            Entity entity = other.gameObject.GetComponent<Entity>();

            if (entity != null)
            {
                if ((owner.enemyFactions & entity.currentFaction) == entity.currentFaction)
                {
                    if (!enemies.Contains(entity))
                        enemies.Add(entity);
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

        private Entity GetRandomEntity()
        {
            if (enemies.Count > 0)
            {
                int randomIndex = Random.Range(0, enemies.Count);
                return enemies[randomIndex];
            }
            else
            {
                return null;
            }
        }

        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "Skill1_CastSmoke":
                    var bombGo = TrashMan.spawn(SmokeBomb, owner.transform.position, Quaternion.identity);
                    Bomb bomb = bombGo.GetComponent<Bomb>();
                    bomb.buffDuration = buffDuration;
                    bomb.Initialize(owner, owner.enemyFactions);
                    OnCast();
                    break;
                case "Skill1_Teleport":
                    Entity target = GetRandomEntity();
                    if (target != null)
                    {
                        owner.transform.position = target.transform.position - (target.transform.forward*.5f);
                        owner.transform.rotation =
                            Quaternion.LookRotation(target.transform.position - owner.transform.position);
                    }
                    break;
            }
        }
    }
}