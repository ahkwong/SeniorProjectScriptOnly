using Assets.Scripts.Entities;
using Senior.Items;
using Seniors.Skills.Buffs;
using UnityEngine;

namespace Seniors.Skills.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class Arrow : Projectile
    {
        public GameObject FireTrail;
        public GameObject IceTrail;
        public GameObject LightningTrail;
        public GameObject DemonicTrail;

        private bool FireActive = false;
        private bool IceActive = false;
        private bool LightningActive = false;
        private bool DemonicActive = false;

        //because im lazy
        public Buff brnDebuff;
        public Buff iceDebuff;
        public Buff lightningDebuff;
        public Buff demonicDebuff;

        public bool Onehit = false;

        public override void OnEnable()
        {
            if (!isInitialized) return;

            base.OnEnable();
            if (FireTrail != null)
                FireTrail.SetActive(false);
            if (IceTrail != null)
                IceTrail.SetActive(false);
            if (LightningTrail != null)
                LightningTrail.SetActive(false);
            if (DemonicTrail != null)
                DemonicTrail.SetActive(false);
            if (owner.InventoryComponent != null)
            {
                for (int i = 0; i < owner.InventoryComponent.items.Count; i++)
                {
                    if (owner.InventoryComponent.items[i] is FireEssence)
                    {
                        FireTrail.SetActive(true);
                        FireActive = true;
                    }
                    else if (owner.InventoryComponent.items[i] is IceEssence)
                    {
                        IceTrail.SetActive(true);
                        IceActive = true;
                    }
                    else if (owner.InventoryComponent.items[i] is LightningEssence)
                    {
                        LightningTrail.SetActive(true);
                        LightningActive = true;
                    }
                    else if (owner.InventoryComponent.items[i] is DemonicEssence)
                    {
                        DemonicTrail.SetActive(true);
                        DemonicActive = true;
                    }
                }
            }
        }

        // when the projectile can pierce
        public override void OnTriggerEnter(Collider collision)
        {
            Entity entity = collision.gameObject.GetComponent<Entity>();
            if (entity != null)
            {
                if ((owner.enemyFactions & entity.currentFaction) == entity.currentFaction)
                {
                    OnHit(entity);
                    if (Onehit)
                        TrashMan.despawn(gameObject);
                }
            }
        }

        public override void OnHit(Entity target)
        {
            base.OnHit(target);
            // if using piercing arrow, then it applies the debuffs for each it hits
            if (piercing)
            {
                if (FireActive && brnDebuff != null)
                {
                    Buff burn = Instantiate(brnDebuff, target.transform.position, Quaternion.identity) as Buff;
                    burn.Initialize(owner, target);
                }

                if (IceActive && iceDebuff != null)
                {
                    Buff ice = Instantiate(iceDebuff, target.transform.position, Quaternion.identity) as Buff;
                    ice.Initialize(owner, target);
                }

                if (LightningActive && lightningDebuff != null)
                {
                    Buff light = Instantiate(lightningDebuff, target.transform.position, Quaternion.identity) as Buff;
                    light.Initialize(owner, target);
                }

                if (DemonicActive && demonicDebuff != null)
                {
                    Buff demon = Instantiate(demonicDebuff, target.transform.position, Quaternion.identity) as Buff;
                    demon.Initialize(owner, target);
                }

            }
        }
    }
}