using System.Collections.Generic;
using Assets.Scripts.Entities;
using Senior.Globals;
using Seniors.Skills.Projectiles;
using UnityEngine;

namespace Seniors.Skills.Andrew
{
    public class AndrewAttack : Skill
    {
        public List<Entity> enemies = new List<Entity>(); 
        public Entity target = null;
        public float kickRange = 1f;
        public GameObject KickBomb;
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;

            if (target != null)
            {
                Vector3 directionToTarget = target.transform.position - transform.position;
                if (directionToTarget.magnitude > 1f)
                    anim.SetTrigger("Attack");
                else
                    anim.SetTrigger("SecAttack");
            }
            else
            {
                anim.SetTrigger("Attack");
            }
        }

        public override void Update()
        {
            if (hc.CanMove && !sc.IsBusy)
            {
                // if there is an enemy in range, turn to look at it
                if (enemies.Count > 0)
                {
                    target = FindNearestTarget();
                    if (target != null)
                    {
                        Vector3 directionToTarget = target.transform.position - transform.position;
                        hc.RotateBasedOnMovement = false;
                        owner.transform.rotation = Quaternion.Slerp(transform.rotation,
                            Quaternion.LookRotation(directionToTarget),
                            Time.deltaTime*stats.RotationSpeedBase);
                    }
                }
                else
                {
                    hc.RotateBasedOnMovement = true;
                }
            }
        }

        // from the enemies list, find the closest target to look at
        private Entity FindNearestTarget()
        {
            float closestDist = Mathf.Infinity;
            Entity closestEntitiy = null;
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] != null && enemies[i].currentStatus != EntityStatusState.Dead &&
                    enemies[i].gameObject.activeSelf)
                {
                    float dist = (transform.position - enemies[i].transform.position).sqrMagnitude;
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closestEntitiy = enemies[i];
                    }
                }
                else
                {
                    enemies.RemoveAt(i);
                    break;
                }
            }
            return closestEntitiy;
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

        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "Attack_Shoot":
                    ShootProjectile();
                    break;
                case "Attack_Kick":
                    var bombGO = TrashMan.spawn(KickBomb, owner.transform.position + 1*owner.transform.forward,
                        Quaternion.identity);
                    Bomb bomb = bombGO.GetComponent<Bomb>();
                    bomb.damage = damage;
                    bomb.Initialize(owner, owner.enemyFactions);
                    break;
            }
        }

    }
}