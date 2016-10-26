using Assets.Scripts.Entities;
using Seniors.Skills.Buffs;
using UnityEngine;

namespace Seniors.Skills.Hau
{
    public class HauAltAttack : Skill
    {
        public GameObject TauntPrefab;
        private GameObject tauntInstance;
        public GameObject tauntDebuff;

        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;

            base.ActivateDown();

            tauntInstance = TrashMan.spawn(TauntPrefab, owner.transform.position, Quaternion.identity);
            tauntInstance.transform.SetParent(owner.transform);

        }

        public override void Update()
        {
            base.Update();

            if (buttonHold)
            {
                if (buttonHoldTimePressed > .5f)
                {
                    SCollider.enabled = true;
                }
            }

        }

        public override void ActivateUp()
        {
            SCollider.enabled = false;

            base.ActivateUp();

            if (tauntInstance != null)
                TrashMan.despawn(tauntInstance);
        }

        private void OnTriggerEnter(Collider other)
        {
            Entity entity = other.gameObject.GetComponent<Entity>();

            if (entity != null)
            {
                if ((owner.enemyFactions & entity.currentFaction) == entity.currentFaction)
                {
                    var buff = TrashMan.spawn(tauntDebuff, entity.transform.position, Quaternion.identity);
                    Buff taunt = buff.GetComponent<Buff>();
                    taunt.Initialize(owner, entity);
                }
            }
        }

    }
}