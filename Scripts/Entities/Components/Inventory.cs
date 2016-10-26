using System.Collections.Generic;
using Assets.Scripts.Entities;
using Senior.Items;
using UnityEngine;

namespace Senior.Components
{
    public class Inventory : MonoBehaviour
    {
         public List<Item> items = new List<Item>();

        private Quaternion rotation;

        void Awake()
        {
            rotation = transform.rotation;
        }

        // this is done to cancel the rotation so that attached particles won't spaz out
        void Update()
        {
            transform.rotation = rotation;
        }

        public void Equip(Item item)
        {
            if (!item.canOwnMultiple)
            {
                if (items.Contains(item))
                    return;
            }

            item.transform.SetParent(this.transform);
            items.Add(item);

        }

        public void UnEquip(Item item)
        {
            items.Remove(item);
        }

        public void OnHit(Entity target, float damage)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].OnHit(target, damage);
            }
        }

        public void OnDamage(Entity dealer, float damage)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].OnDamage(dealer, damage);
            }
        }
    }
}