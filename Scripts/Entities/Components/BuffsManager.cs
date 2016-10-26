using System.Collections.Generic;
using Seniors.Skills.Buffs;
using UnityEngine;

namespace Assets.Scripts.Entities.Components
{
    public class BuffsManager : MonoBehaviour
    {
        public List<Buff> buffs = new List<Buff>();
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
        public void AddBuff(Buff buff)
        {
            if (!buff.canStack)
            {
                if (buffs.Contains(buff))
                    return;
            }

            buff.transform.SetParent(this.transform);
            buff.OnTick();
            buffs.Add(buff);
        }

        public void OnHit(Entity target, float damage)
        {
            for (int i = 0; i < buffs.Count; i++)
            {
                buffs[i].OnHit(target, damage);
            }
        }

        public void RemoveBuff(Buff buff)
        {
            buffs.Remove(buff);
        }
    }
}