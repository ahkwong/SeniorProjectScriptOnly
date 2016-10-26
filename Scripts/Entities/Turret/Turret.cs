using Seniors.Skills.Projectiles;
using UnityEngine;

namespace Assets.Scripts.Entities.Turret
{
    public class Turret : Entity
    {
        public bool canTakeDamage;
        public float lifeSpan = 10f;
        public GameObject bombPrefab;
        private bool isInitialized = false;
        protected float countupTimer = 0;

        public void Initialize(Entity owner, int duration, bool canTakeDamage)
        {
            enemyFactions = owner.enemyFactions;
            alliedFactions = owner.alliedFactions;
            currentFaction = owner.currentFaction;
            this.canTakeDamage = canTakeDamage;
            lifeSpan = duration;
            this.entityOwner = owner;
            countupTimer = 0;
            isInitialized = true;
        }


        public override void Start()
        {
            FullHeal();
        }

        public override void Update()
        {
            if (isInitialized)
            {
                countupTimer += Time.deltaTime;

                if (countupTimer >= lifeSpan)
                    TrashMan.despawn(gameObject);

            }
        }

        public void OnDisable()
        {
            if (bombPrefab != null && isInitialized)
            {
                var bomb = TrashMan.spawn(bombPrefab, transform.position, Quaternion.identity);

                Bomb bo = bomb.GetComponent<Bomb>();
                if (bo != null)
                    bo.Initialize(this, enemyFactions);
            }
        }
    }
}