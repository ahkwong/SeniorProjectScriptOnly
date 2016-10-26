using System;
using System.Collections.Generic;
using Assets.Scripts.Entities.Components;
using Senior.Components;
using Senior.Globals;
using Senior.Inputs;
using UnityEngine;

namespace Assets.Scripts.Entities.Turret
{
    public class TurrentController : MonoBehaviour, IMovementController
    {
        private Stats stats;
        private IPlayerController playerController;
        private SkillsController skills;
        private Rigidbody rb;
        private Animator anim;
        private Entity owner;
        public bool RotateBasedOnMovement { get; set; }
        public bool OnlyRotate { get; set; }
        public bool CanMove { get; set; }
        public Vector3 MoveDirection { get; set; }
        public Vector3 LastMoveDirection { get; set; }

        public List<Entity> enemies = new List<Entity>();
        public Entity target = null;
        public float meleeDistance;
        public TurrateState currentState = TurrateState.Follow;
        public void Start()
        {
            owner = GetComponent<Entity>();
            playerController = GetComponentInParent<IPlayerController>();
            skills = GetComponentInChildren<SkillsController>();
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            stats = GetComponent<Stats>();
            RotateBasedOnMovement = true;
            skills.Initialize(this, owner, anim, rb);
            CanMove = true;
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

        public void Move()
        {
            if (anim != null)
            {
                if (!OnlyRotate)
                    anim.SetFloat("Speed", MoveDirection.normalized.sqrMagnitude);
                else
                    anim.SetFloat("Speed", 0);


            }

            //Makes sure the player faces the direction they're moving.
            if (MoveDirection != Vector3.zero)
            {
                LastMoveDirection = MoveDirection;
                if (RotateBasedOnMovement)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(MoveDirection),
                        Time.deltaTime * stats.RotationSpeedBase);
                }
            }
            // if the player is only allowed to rotate, then don't move
            if (!OnlyRotate)
                rb.MovePosition(rb.position + MoveDirection.normalized * stats.MovementSpeedBase * Time.deltaTime);
        }

        public void FixedUpdate()
        {
            GetTargetDirection();

            if (CanMove)
                Move();
        }

        private void GetTargetDirection()
        {
            // if there is an enemy in range, turn to look at it
            if (enemies.Count > 0)
            {
               target = FindNearestTarget();
                currentState = TurrateState.Attack;
            }
            else
            {
                target = owner.entityOwner;
                currentState = TurrateState.Follow;
            }

            if (target != null)
            {
                Vector3 directionToTarget = target.transform.position - transform.position;
                RotateBasedOnMovement = false;
                owner.transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(directionToTarget),
                    Time.deltaTime*stats.RotationSpeedBase);

                MoveDirection = directionToTarget;
            }
        }

        // from the enemies list, find the closest target to look at
        private Entity FindNearestTarget()
        {
            float closestDist = Mathf.Infinity;
            Entity closestEntitiy = null;
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] != null && enemies[0].currentStatus != EntityStatusState.Dead && enemies[0].gameObject.activeSelf)
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
                }           
            }
            return closestEntitiy;
        }

        public void Update()
        {
            if (target != null)
            {
                float dist = (transform.position - target.transform.position).sqrMagnitude;
                if (dist < meleeDistance)
                {
                    CanMove = false;
                    anim.SetFloat("Speed", 0);
                }
                else
                {
                    CanMove = true;
                }
                switch (currentState)
                {
                    case TurrateState.Follow:
                        break;
                    case TurrateState.Attack:
                        if (CanMove)
                            skills.AttackDown();
                        else
                            skills.AltAttackDown();
                        break;
                }
            }
            // stop moving when in melee distance
            

            
        }

        public void AnimationEvent(string eventName)
        {
            
            skills.RaiseEvent(eventName);
        
        }
    }
}