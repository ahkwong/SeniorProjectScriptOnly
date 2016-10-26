using Apex;
using Assets.Scripts.Entities;
using Senior.Components;
using Senior.Globals;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public abstract class AIBehavior : ExtendedMonoBehaviour
    {
        protected Entity owner;
        protected AIManager manager;
        protected SkillsController skills;
        protected bool isRunning = false;
        public int weight = 0;                  // the weight of this behavior when randomly choosing
        protected bool isInitialized = false;

        public virtual AIBehaviorType Type
        {
            get {  return AIBehaviorType.None;}
        }

        // initialize the behavior
        public virtual void Initialize(Entity owner, SkillsController skills, AIManager manager)
        {
            this.owner = owner;
            this.manager = manager;
            this.skills = skills;
            isInitialized = true;
        }

        public virtual void Update()
        {
            if (isRunning)
            {
                if (!IsValid())
                    Disable();
            }
        }

        // is valid
        public virtual bool IsValid()
        {
            if (!isInitialized)
                return false;

            return false;
        }

        // enable the behavior
        public virtual void Enable()
        {
            isRunning = true;
        }

        // disable the behavior
        public virtual void Disable()
        {
            isRunning = false;
            manager.FindNewBehaviour();
        }
    }
}