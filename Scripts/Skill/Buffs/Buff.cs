using Assets.Scripts.Entities;
using Senior.Globals;
using UnityEngine;

namespace Seniors.Skills.Buffs
{
    public abstract class Buff : MonoBehaviour
    {
        public BuffType type;
        public Entity target;
        public Entity owner;
        public float lifeSpan = 1f;
        public float period = 1f;
        public bool canStack = false;
        public float damage;
        //TODO: make a buff manager
        private float nextActionTime = 0f;

        public virtual void Initialize(Entity origin, Entity target)
        {
            this.target = target;
            owner = origin;
            target.BuffManager.AddBuff(this);
            OnAdd();
        }

        public virtual void OnAdd()
        {
            
        }

        public virtual void OnEnable()
        {
            TrashMan.despawnAfterDelay(gameObject, lifeSpan);
        }

        public virtual void Update()
        {
            if (Time.time > nextActionTime)
            {
                nextActionTime = Time.time + period;
                OnTick();
            }
        }

        // when the player hits a target
        public virtual void OnHit(Entity target, float damage)
        {
        }

        public virtual void OnTick()
        {
        }

        public virtual void OnDisable()
        {
            if (owner != null)
                owner.BuffManager.RemoveBuff(this);
        }
    }
}