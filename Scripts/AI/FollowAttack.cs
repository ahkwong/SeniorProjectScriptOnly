using System.Collections.Generic;
using Apex;
using Apex.LoadBalancing;
using Apex.Messages;
using Apex.Services;
using Apex.Units;
using Assets.Scripts.Entities;
using Senior.Components;
using Senior.Globals;
using UnityEngine;

namespace Assets.Scripts.AI
{
    // while in this state, the unit will follow it's target to try and attack it while the target 
    // remains inside it's aggro range.
    public class FollowAttack : AIBehavior, ILoadBalanced
    {
        private IUnitFacade _unit;
        public Transform target;
        public List<Entity> enemies = new List<Entity>();
        public float requestsPerSecond = 1f;
        private float lastRequest;
        public float minimumFollowRange = 1f;
        public override AIBehaviorType Type
        {
            get { return AIBehaviorType.FollowAttack; }
        }

        public override void Initialize(Entity owner, SkillsController skills, AIManager manager)
        {
            base.Initialize(owner, skills, manager);
            _unit = owner.GetUnitFacade();
        }

        public override void Disable()
        {
            base.Disable();
            owner.anim.SetBool("Moving", false);
        }

        public override void Enable()
        {
            base.Enable();
            LoadBalancer.defaultBalancer.Add(this, requestsPerSecond);
            owner.anim.SetBool("Moving", true);
        }

        public override bool IsValid()
        {
            if (!isInitialized)
                return false;

            if (owner.tauntTarget != null)
                return true;

            // if there is an enemy
            if (enemies.Count >= 1)
            {
                // get the distance between you and the enemy
                // cancel following when below minimum range
                if (enemies[0] != null && enemies[0].currentStatus != EntityStatusState.Dead
                    && enemies[0].currentStatus != EntityStatusState.Invis && enemies[0].gameObject.activeSelf)
                {
                    float playerDistance = Vector3.Distance(owner.transform.position, enemies[0].transform.position);
                    if (playerDistance < minimumFollowRange)
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


        public bool repeat
        {
            get { return isRunning; }
        }

        public float? ExecuteUpdate(float deltaTime, float nextInterval)
        {
            // if there are no more enemies, disable this behaviour
            if (!IsValid())
            {
                Disable();
                return null;
            }

            if (owner.tauntTarget != null)
            {
                target = owner.tauntTarget.transform;
            }
            else
            {
                // gets the closest target in the list.
                target = enemies[0].transform;
            }

            if (target != null && _unit != null)
            {
                // if there is a target, move the unit towards that target
                _unit.MoveTo(target.localPosition, false);

            }

            return null;
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