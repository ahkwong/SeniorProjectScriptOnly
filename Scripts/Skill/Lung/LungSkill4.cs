using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Entities.Turret;
using Seniors.Skills.Projectiles;
using UnityEngine;

namespace Seniors.Skills.Lung
{
    public class LungSkill4 : Skill
    {
        public int numberOfClones = 3;
        public GameObject shadowClonesPrefab;
        public int clonesDuration = 10;
        public List<Vector3> spawnLocations;
        public GameObject SmokeBomb;
        public float buffDuration = 3;
        public override void ActivateDown()
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) return;


            if (!IsDisabled)
            {
                IsDisabled = true;
                OnCast();
                anim.SetTrigger("Skill4");

            }

        }
        public override void RaiseEvent(string eventName)
        {
            switch (eventName)
            {
                case "Skill4_CastSmoke":
                    var bombGo = TrashMan.spawn(SmokeBomb, owner.transform.position, Quaternion.identity);
                    Bomb bomb = bombGo.GetComponent<Bomb>();
                    bomb.buffDuration = buffDuration;
                    bomb.Initialize(owner, owner.enemyFactions);
                    OnCast();
                    break;
                case "Skill4_Project":
                    if (spawnLocations.Count > 0)
                    {
                        for (int i = 0; i < numberOfClones; i++)
                        {
                            Vector3 location = owner.transform.position;
                            location += spawnLocations[i];

                            var clones = TrashMan.spawn(shadowClonesPrefab, location, Quaternion.identity);
                            Turret clo = clones.GetComponent<Turret>();
                            if (clo != null)
                                clo.Initialize(owner, clonesDuration, true);
                        }
                    }
                    break;
            }
        }

        private Vector3 RandomLocationAroundYou()
        {
            Vector3 location = transform.position;
            location.x += Random.Range(1, 2)*Random.Range(-1, 1);
            location.z += Random.Range(1, 2) * Random.Range(-1, 1);

            return location;;
        }

        private IEnumerator Spawn()
        {
            

            yield return null;
        }
    }
}