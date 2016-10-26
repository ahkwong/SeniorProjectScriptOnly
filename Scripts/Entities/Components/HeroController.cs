using Assets.Scripts.Entities.Components;
using Assets.Scripts.Entities.Hero;
using Senior.Components;
using UnityEngine;

namespace Senior.Inputs
{
    [RequireComponent(typeof(Stats))]
    [RequireComponent(typeof(Rigidbody))]
    public class HeroController : MonoBehaviour, IMovementController
    {
        private Stats stats;
        private IPlayerController playerController;
        public SkillsController skills;
        private Rigidbody rb;
        private Animator anim;
        private Hero hero;
        public bool RotateBasedOnMovement { get; set; }
        public bool OnlyRotate { get; set; }
        public bool CanMove { get; set; }
        public Vector3 MoveDirection { get; set; }
        private Vector3 lastMoveDirection;

        public Vector3 LastMoveDirection
        {
            get {return lastMoveDirection;}
            set { lastMoveDirection = value.normalized; }
        }

        public void Start()
        {
            hero = GetComponent<Hero>();
            playerController = GetComponentInParent<IPlayerController>();
            skills = GetComponentInChildren<SkillsController>();
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            stats = GetComponent<Stats>();
            RotateBasedOnMovement = true;
            skills.Initialize(this, hero, anim, rb);
            CanMove = true;

        }

        public void Update()
        {
            if (playerController != null)
            {
                if (playerController.AttackButtonDown)
                    skills.AttackDown();
                if (playerController.AttackButtonUp)
                    skills.AttackUp();
                if (playerController.AltAttackButtonDown)
                    skills.AltAttackDown();
                if (playerController.AltAttackButtonUp)
                    skills.AltAttackUp();
                if (playerController.SkillOneButtonDown)
                    skills.SkillOneDown();
                if (playerController.SkillOneButtonUp)
                    skills.SkillOneUp();
                if (playerController.SkillTwoButtonDown)
                    skills.SkillTwoDown();
                if (playerController.SkillTwoButtonUp)
                    skills.SkillTwoUp();
                if (playerController.SkillThreeButtonDown)
                    skills.SkillThreeDown();
                if (playerController.SkillThreeButtonUp)
                    skills.SkillThreeUp();
                if (playerController.SkillFourButtonDown)
                    skills.SkillFourDown();
                if (playerController.SkillFourButtonUp)
                    skills.SkillFourUp();
            }

            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
            viewPos.x = Mathf.Clamp01(viewPos.x);
            viewPos.y = Mathf.Clamp(viewPos.y,0,.9f);
            Vector3 pos = Camera.main.ViewportToWorldPoint(viewPos);
            pos.y = 0;
            transform.position = pos;
        }

        public void FixedUpdate()
        {
            GetDirection();

            if (playerController != null && CanMove)
                Move();
        }


        private void GetDirection()
        {
            var moveDirection = new Vector3(playerController.MoveInput.x, 0, playerController.MoveInput.y);
            moveDirection = Camera.main.transform.TransformDirection(moveDirection);
            moveDirection.y = 0;
            MoveDirection = moveDirection;
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
                        Time.deltaTime*stats.RotationSpeedBase);
                }
            }
            // if the player is only allowed to rotate, then don't move
            if (!OnlyRotate)
                rb.MovePosition(rb.position + MoveDirection.normalized * stats.MovementSpeedBase * Time.deltaTime);
        }

        public void AnimationEvent(string eventName)
        {
            skills.RaiseEvent(eventName);
        }

    }
}