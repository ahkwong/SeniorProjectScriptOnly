﻿using Senior.Items;
using UnityEngine;

namespace Seniors.Skills.Andrew
{
    public class AndrewSkill3 : Skill
    {
        public GameObject lightingEssensePrefab;
        public float bombDamage;
        public float buffDamage;
        public float debuffDuration;
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;

            if (!IsDisabled)
            {
                IsDisabled = true;
                OnCast();
                var lightingEssenseGo = TrashMan.spawn(lightingEssensePrefab, owner.transform.position, Quaternion.identity);
                LightningEssence fe = lightingEssenseGo.GetComponent<LightningEssence>();
                fe.bombDamage = bombDamage;
                fe.buffDamage = buffDamage;
                fe.duration = debuffDuration;
                fe.Initialize(owner);
            }
        }
    }
}