using Apex;
using Apex.Steering;
using Assets.Scripts.AI;
using Assets.Scripts.Entities.Monsters;
using Senior.Components;
using Senior.Globals;
using UnityEngine;

namespace Assets.Scripts.Entities.Components
{
    public class MonsterController : MonoBehaviour, IMovementController
    {
        private Stats stats;
        private IMovable unit;
        private Entity self;
        public SkillsController skills;
        private Rigidbody rb;
        private Animator anim;
        private AIManager ai;

        public Entity target;

        public bool RotateBasedOnMovement { get; set; }
        public bool OnlyRotate { get; set; }
        private bool canMove = true;

        public bool CanMove
        {
            get
            {
                return canMove;
            }
            set
            {
                if (self.currentStatus == EntityStatusState.Dead)
                    return;
                
                canMove = value;
                if (value == false)
                {
                    ai.Disable();
                }
                else
                {
                    ai.Enable();
                }
            }
        }

        public Vector3 MoveDirection { get; set; }
        public Vector3 LastMoveDirection { get; set; }

        public void Awake()
        {
            unit = this.As<IMovable>();
        }

        public void Start()
        {
            self = GetComponent<Entity>();
            skills = GetComponentInChildren<SkillsController>();
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            stats = GetComponent<Stats>();
            ai = GetComponentInChildren<AIManager>();

            skills.Initialize(this,self, anim, rb);
            ai.Initialize(self, skills);
        }

        public void Move()
        {
        }

        public void FixedUpdate()
        {
        }

        public void Update()
        {
        }

        public void AnimationEvent(string eventName)
        {
            skills.RaiseEvent(eventName);
        }

    }
}