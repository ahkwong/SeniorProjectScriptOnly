
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Senior;
using Senior.Components;
using Senior.Globals;
using Senior.Inputs;
using Senior.Items;
using Seniors.Skills;
using UnityEngine;

namespace Assets.Scripts.Entities.Hero
{
    [RequireComponent(typeof(HeroController))]

    public class Hero : Entity
    {
        public Sprite Portrait;

        public void Initialize(Player player)
        {
            playerOwner = player;
        }

        public override void Awake()
        {
            base.Awake();
            mc = GetComponent<HeroController>();
            StatsComponent.Level = 1;           
        }


        public override void Update()        
        {
            base.Update();
        }

        public void Revive()
        {
            anim.SetTrigger("Revive");
            FullHeal();
            currentStatus = EntityStatusState.Normal;
            collider.enabled = true;
            if (mc != null)
                mc.CanMove = true;
        }

        // the hero is dead
        public override void Die()
        {  
            base.Die();
            //Turn into spirit          
            if (playerOwner)
                playerOwner.OnDead(this);
        }

        // the hero is damaged by a certain amount
        public override void Damage(Entity dealer, float damage)
        {
            base.Damage(dealer,damage);
            InventoryComponent.OnDamage(dealer, damage);
            if (playerOwner)
                playerOwner.OnHealthModified(this);
        }

        // the hero is healed by a certain amount
        public override void Heal(float heal)
        {
            base.Heal(heal);
            if (playerOwner)
                playerOwner.OnHealthModified(this);

        }

        // the hero is fully healed
        public override void FullHeal()
        {
            base.FullHeal();

            if (playerOwner)
                playerOwner.OnHealthModified(this);
        }

        // the hero collides with an object
        public override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);

            //Pick up Items automatically when you touch them
            Item item = collision.gameObject.GetComponent<Item>();
            if (item != null)
            {
                PickUpItem(item);
            }
        }

        // the hero picks up an item
        public override void PickUpItem(Item item)
        {
            base.PickUpItem(item);
            if (playerOwner)
                playerOwner.OnItemPickUp(item);
        }

        // Is used to notify the UI that a skill has been used, and the cd should start
        public override void UseSkill(Skill skill)
        {
            if (skill.showInUi)
                playerOwner.UseSkill(skill);
        }

        // Is used to update the skill cd for the ui
        public override void UpdateSkill(Skill skill)
        {
            if (skill.showInUi)
                playerOwner.UpdateSkill(skill);
        }

        // called when a skill has hit an enemy, incokes any OnHit events ex. from items
        public override void OnHit(Entity entitiy, float damage)
        {
            InventoryComponent.OnHit(entitiy,damage);
            BuffManager.OnHit(entitiy, damage);
            
        }

        // called when casting a skill, invokes and OnCast events ex. from items
        public override void OnCast()
        {
            
        }
    }
}