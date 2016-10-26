using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Hero;
using Senior.Globals;
using UnityEngine;

namespace Seniors.Skills.Projectiles
{


    public class Projectile : MonoBehaviour
    {
        public Entity owner;
        private Rigidbody rb;
        private BoxCollider bc;
        public bool isHealing = false;
        public float damage;
        public float speed;
        public float lifeSpan;
        public bool piercing = false;
        public bool isMoving = true;
        public bool knockback = false;
        public float knockbackForce = 1f;
        [HideInInspector]
        public Faction targetFaction;
        protected bool isInitialized = false;
        protected float countupTimer = 0;

        public virtual void Initialize(Entity owner, Faction targetFaction)
        {
            this.owner = owner;
            this.targetFaction = targetFaction;
            isInitialized = true;
            countupTimer = 0;    
            OnEnable();

        }

        public virtual void OnEnable()
        {
            if (!isInitialized) return;

            if (isMoving)
            {
                rb = GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;

                bc = GetComponent<BoxCollider>();
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                rb.velocity = transform.TransformDirection(Vector3.forward*speed);
            }
        }

        public virtual void Update()
        {
            if (isInitialized)
            {
                countupTimer += Time.deltaTime;

                if (countupTimer >= lifeSpan)
                    TrashMan.despawn(gameObject);

            }
        }
        // when the projectile can pierce
        public virtual void OnTriggerEnter(Collider collision)
        {
            Entity entity = collision.gameObject.GetComponent<Entity>();
            if (entity != null)
            {
                if ((targetFaction & entity.currentFaction) == entity.currentFaction)
                {
                    OnHit(entity);
                    if (!piercing)
                    {
                        TrashMan.despawn(gameObject);
                    }
                }
            }
        }

        public virtual void OnHit(Entity target)
        {
            if (isHealing)
                target.Heal(damage);
            else
            {
                target.Damage(owner, damage);
                if (owner != null)
                    owner.OnHit(target, damage);
            }
            if (knockback)
            {
                if (target != null && owner != null)
                {
                    Vector3 direction = (target.transform.position - owner.transform.position).normalized;
                    target.gameObject.GetComponent<Rigidbody>().AddForce(direction * knockbackForce, ForceMode.Impulse);
                }              
            }
        }

        public virtual void OnDisable()
        {
            isInitialized = false;
        }
    }
}