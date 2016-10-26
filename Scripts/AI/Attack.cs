using System.Collections;
using System.Collections.Generic;
using Apex.Services;
using Assets.Scripts.Entities;
using Senior.Globals;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public class Attack : AIBehavior
    {
        public List<Entity> enemies = new List<Entity>();
        public float minimumAttackRange = 1f;
        public float waitBetweenAttacks = 3f;
        public bool rangeAttacker = false;
        public override AIBehaviorType Type
        {
            get { return AIBehaviorType.NormalAttack; }
        }
        public override bool IsValid()
        {
            if (!isInitialized)
                return false;

            if (owner.tauntTarget != null)
            {
                float pDistance = Vector3.Distance(owner.transform.position, owner.tauntTarget.transform.position);
                if (pDistance > minimumAttackRange)
                    return false;
                
                return true;
            }

            // if there is an enemy
            if (enemies.Count >= 1)
            {
                // get the distance between you and the enemy
                // cancel following when below minimum range
                if (enemies[0] != null && enemies[0].currentStatus != EntityStatusState.Dead
                    && enemies[0].currentStatus != EntityStatusState.Invis && enemies[0].gameObject.activeSelf)
                {
                    float playerDistance = Vector3.Distance(owner.transform.position, enemies[0].transform.position);
                    if (playerDistance > minimumAttackRange)
                        return false;
                }
                else
                {
                    enemies.RemoveAt(0);
                    return false;
                }

                return true;
            }

            return false;
        }

        public override void Enable()
        {
            base.Enable();
            StartCoroutine(DoAttack());
        }

        public override void Disable()
        {
            StopAllCoroutines();
            base.Disable();           
        }

        public override void Update()
        {
            base.Update();
            if (owner != null)
            {
                if (owner.mc.CanMove && rangeAttacker)
                {
                    // if there is an enemy in range, turn to look at it
                    if (enemies.Count > 0)
                    {
                        var target = FindNearestTarget();
                        if (target != null)
                        {
                            Vector3 directionToTarget = target.transform.position - transform.position;
                            owner.transform.rotation = Quaternion.Slerp(transform.rotation,
                                Quaternion.LookRotation(directionToTarget),
                                Time.deltaTime*owner.StatsComponent.RotationSpeedBase);
                        }
                    }

                }
            }
        }

        private Entity FindNearestTarget()
        {
            float closestDist = Mathf.Infinity;
            Entity closestEntitiy = null;

            if (owner.tauntTarget != null)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (owner.tauntTarget == enemies[i])
                        return owner.tauntTarget;
                }

            }

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

        private IEnumerator DoAttack()
        {
            skills.RandomAttack();
            yield return new WaitForSeconds(waitBetweenAttacks);
            Disable();
        }

        //When an enemy comes into range, add it into the enemies list
        private void OnTriggerEnter(Collider other)
        {
            Entity entity = other.gameObject.GetComponent<Entity>();

            if (entity != null && owner != null)
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

            if (entitiy != null && owner != null)
            {
                if ((owner.enemyFactions & entitiy.currentFaction) == entitiy.currentFaction)
                    enemies.Remove(entitiy);
            }
        }
    }
}