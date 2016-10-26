using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Hero;
using Seniors.Skills;
using UnityEngine;

namespace Senior.Items
{
    public abstract class Item : MonoBehaviour
    {
        public string itemName;
        public string description;
        public Sprite icon;
        public Entity owner;
        public bool limitedUses = false;
        public int numberOfUses = 1;
        public bool limitedTime = false;
        public float lifespan = 1;
        public bool canOwnMultiple = true;
        public bool hasDurability = false;
        public float healthMax = 100;
        public float healthCurrent;

        // sets the lifespan of the item
        public virtual void OnEnable()
        {
            healthCurrent = healthMax;
            if (limitedTime)
                TrashMan.despawnAfterDelay(gameObject, lifespan);
        }

        public virtual void Initialize(Entity owner)
        {
            this.owner = owner;
            this.owner.InventoryComponent.Equip(this);
            OnEquip();
        }

        // when the item is initially equipped
        public virtual void OnEquip()
        {
            
        }

        // when the player hits a target
        public virtual void OnHit(Entity target, float damage)
        {
        }

        // When the player got damaged
        public virtual void OnDamage(Entity dealer, float damage)
        {
            if (hasDurability)
            {
                healthCurrent -= damage;

                if (healthCurrent <= 0)
                    TrashMan.despawn(gameObject);
            }
        }

        // when the player casts a spell
        public virtual void OnCast(Skill skill)
        {
            
        }

        public virtual void Repair()
        {
            healthCurrent = healthMax;
        }

        // when the item is used
        public virtual void OnUse()
        {
            if (limitedUses)
            {
                numberOfUses -= 1;
                if (numberOfUses <= 0)
                    TrashMan.despawn(gameObject);
            }
        }

        // when the item is destroyed, call to unquip it
        public virtual void OnDisable()
        {
            if (owner)
                owner.InventoryComponent.UnEquip(this);
        }
    }
}