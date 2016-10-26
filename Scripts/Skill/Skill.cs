using System.Collections;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Components;
using Assets.Scripts.Entities.Hero;
using Senior.Components;
using Senior.Inputs;
using Seniors.Skills.Projectiles;
using UnityEngine;

namespace Seniors.Skills
{
    public abstract class Skill : MonoBehaviour
    {
        public string SkillName = string.Empty;
        public Sprite SkillIcon = null;
        public int SkillIndex = 0;
        public bool showInUi = true;
        public float CoolDown = 0;
        public float CoolDownTimer = 0;
        public bool buttonHold = false;
        public float buttonHoldTimePressed = 0f;
        //type target, projectile aoe
        public float damage = 0;
        public bool knockback = false;
        public float knockbackForce = 1f;
        [Header("Target")]
        public bool isTarget = false;
        public Vector3 targetLocation;
        [Header("Projectile")]
        public bool isProjectile = false;
        public GameObject projectilePrefab;
        public float projectileOffset = 1f;

        protected bool IsDisabled = false;
        protected Animator anim;
        protected IMovementController hc;
        protected Rigidbody rb;
        protected Stats stats;
        protected Entity owner;
        protected SkillsController sc;
        [Header("Colldrs")]
        public BoxCollider BCollider;

        public SphereCollider SCollider;


        public virtual void Awake()
        {
        }

        public virtual void Start()
        {
        }

        public virtual void Update()
        {
            if (buttonHold)
            {
                buttonHoldTimePressed += Time.deltaTime;
            }

            if (IsDisabled)
            {
                CoolDownTimer -= Time.deltaTime;

                //if (countdownTimer > 1)
                owner.UpdateSkill(this);
                if (CoolDownTimer <= 0)
                {
                    IsDisabled = false;
                    Reset();
                }
            }
        }

        public virtual void FixedUpdate()
        {
        }


        public virtual void ShootProjectile()
        {
            if (projectilePrefab != null)
            {
                OnCast();
                var pro = TrashMan.spawn(projectilePrefab, owner.transform.position + (projectileOffset * owner.transform.forward) + (0.4f * owner.transform.up),
                    owner.transform.rotation);

                Projectile proj = pro.GetComponent<Projectile>();
                if (proj != null)
                {
                    proj.Initialize(owner, owner.enemyFactions);
                    proj.damage = damage;
                }
            }
        }

        public virtual void OnHit(Collider hit)
        {
            Entity entity = hit.gameObject.GetComponent<Entity>();
            if (entity != null)
            {
                if ((owner.enemyFactions & entity.currentFaction) == entity.currentFaction)
                {
                    owner.OnHit(entity, damage);
                    entity.Damage(owner,damage);
                    if (knockback)
                    {
                        Vector3 direction = (entity.transform.position - owner.transform.position).normalized;
                        entity.gameObject.GetComponent<Rigidbody>().AddForce(direction * knockbackForce, ForceMode.Impulse);
                    }
                }
            }
            
        }

        public virtual void OnCast()
        {
            owner.UseSkill(this);
        }

        public virtual void Initialize(SkillsController sc, IMovementController hc, Entity hero, Animator anim, Rigidbody rb)
        {
            this.hc = hc;
            this.owner = hero;
            this.anim = anim;
            this.rb = rb;
            this.stats = hero.StatsComponent;
            this.sc = sc;
            if (hero is Hero)
                this.owner.playerOwner.SetSkillIcon(this);
            Reset();
        }

        public virtual void Reset()
        {
            CoolDownTimer = CoolDown;

        }

        public virtual void ActivateDown()
        {
            IsDisabled = true;
            buttonHold = true;
            buttonHoldTimePressed = 0f;
        }

        public virtual void ActivateUp()
        {
            buttonHold = false;
            buttonHoldTimePressed = 0f;
        }

        public virtual void Deactivate()
        {
        }


        public virtual void RaiseEvent(string eventName)
        {           
        }

        //FreezeFrame
        protected IEnumerator FreezeFrame()
        {
            // if frozen = false, froze = true you know just in case
            anim.speed = 0;
            yield return new WaitForSeconds(0.12f);
            anim.speed = 1;
        }

    }
}