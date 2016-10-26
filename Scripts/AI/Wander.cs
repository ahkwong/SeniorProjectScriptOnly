using System.Collections;
using System.Collections.Generic;
using Apex;
using Apex.LoadBalancing;
using Apex.Messages;
using Apex.Services;
using Apex.Units;
using Apex.WorldGeometry;
using Assets.Scripts.Entities;
using Senior.Components;
using Senior.Globals;
using UnityEngine;

namespace Assets.Scripts.AI
{
    // while in this state, the unit will follow it's target to try and attack it while the target 
    // remains inside it's aggro range.
    public class Wander : AIBehavior, IHandleMessage<UnitNavigationEventMessage>
    {
        private IUnitFacade _unit;
        public Transform target;
        public List<Entity> enemies = new List<Entity>();

        /// <summary>
        /// The radius from the starting position within which to wander
        /// </summary>
        public float radius = 2.0f;

        /// <summary>
        /// The minimum distance of a wander route
        /// </summary>
        public float minimumDistance = 1.0f;

        /// <summary>
        /// The time in seconds that the unit will linger after each wander route before moving on.
        /// </summary>
        public float lingerForSeconds = 0.0f;

        /// <summary>
        /// If unable to find a spot to wander to after having tried <see cref="bailAfterFailedAttempts"/> no more attempts will be made.
        /// </summary>
        public int bailAfterFailedAttempts = 100;

        private Vector3 _startPos;


        public override AIBehaviorType Type
        {
            get { return AIBehaviorType.Wander; }
        }

        public override void Initialize(Entity owner, SkillsController skills, AIManager manager)
        {
            base.Initialize(owner,skills, manager);
            _unit = owner.GetUnitFacade();
        }

        public override void Disable()
        {
            base.Disable();
            _unit.Stop();
            StopAllCoroutines();
            owner.anim.SetBool("Moving", false);

        }

        public override void Enable()
        {
            base.Enable();
            owner.anim.SetBool("Moving", true);

            GameServices.messageBus.Subscribe(this);
            _startPos = _unit.position;

            MoveNext(false);
            if (this.lingerForSeconds == 0.0f)
            {
                MoveNext(true);
            }
        }

        public override bool IsValid()
        {
            base.IsValid();

            if (enemies.Count <= 0)
                return true;
            else
            {
                if (enemies[0].currentStatus == EntityStatusState.Dead)
                    enemies.RemoveAt(0);
            }

            return false;
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

        public void Handle(UnitNavigationEventMessage message)
        {
            if (!IsValid() || owner == null) return;

            if (message.entity != owner.gameObject || message.isHandled)
            {
                return;
            }

            if (message.eventCode == UnitNavigationEventMessage.Event.WaypointReached)
            {
                message.isHandled = true;

                Disable();
            }
            else if (message.eventCode == UnitNavigationEventMessage.Event.DestinationReached)
            {
                message.isHandled = true;

                //StartCoroutine(DelayedMove());
                Disable();
            }
            else if (message.eventCode == UnitNavigationEventMessage.Event.StoppedNoRouteExists)
            {
                message.isHandled = true;

                Disable();
            }
        }

        private IEnumerator DelayedMove()
        {
            yield return new WaitForSeconds(this.lingerForSeconds);
            MoveNext(false);
        }

        private void MoveNext(bool append)
        {
            if (!IsValid()) return;

            Vector3 pos = Vector3.zero;
            bool pointFound = false;
            int attempts = 0;

            while (!pointFound && attempts < this.bailAfterFailedAttempts)
            {
                pos = _startPos + (Random.insideUnitSphere.OnlyXZ() * Random.Range(1.0f, this.radius));

                var dir = _unit.position.DirToXZ(pos);
                if (dir.sqrMagnitude < this.minimumDistance * this.minimumDistance)
                {
                    pos = _unit.position + (dir.normalized * this.minimumDistance);
                }

                var grid = GridManager.instance.GetGrid(pos);
                if (grid != null)
                {
                    var cell = grid.GetCell(pos, true);
                    pointFound = cell.IsWalkableWithClearance(_unit);
                }
                else
                {
                    pointFound = true;
                }

                attempts++;
            }

            _unit.MoveTo(pos, append);
        }
    }
}