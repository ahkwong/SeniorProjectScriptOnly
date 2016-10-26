using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Entities;
using Senior.Globals;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public class Idle: AIBehavior
    {
        public float idleTime = 3f;
        public List<Entity> enemies = new List<Entity>();
        public override AIBehaviorType Type
        {
            get { return AIBehaviorType.Idle; }
        }

        public override void Enable()
        {
            base.Enable();
            StartCoroutine(Linger());
        }

        public override void Disable()
        {
            base.Disable();
            //StopAllCoroutines();
        }

        private IEnumerator Linger()
        {
            yield return new WaitForSeconds(idleTime);
            Disable();
        } 

        public override bool IsValid()
        {
            base.IsValid();

            if (enemies.Count <= 0)
                return true;

            return false;
        }

        //When an enemy comes into range, add it into the enemies list
        private void OnTriggerEnter(Collider other)
        {
            Entity entity = other.gameObject.GetComponent<Entity>();

            if (entity != null && owner != null)
            {
                if ((owner.enemyFactions & entity.currentFaction) == entity.currentFaction)
                {
                    if (!enemies.Contains(entity))
                        enemies.Add(entity);
                }
            }
        }

        // When an enemy exits the trigger, remove it from the list
        void OnTriggerExit(Collider other)
        {
            Entity entitiy = other.gameObject.GetComponent<Entity>();

            if (entitiy != null && owner != null)
            {
                if ((owner.enemyFactions & entitiy.currentFaction) == entitiy.currentFaction)
                    enemies.Remove(entitiy);
            }
        }
    }
}