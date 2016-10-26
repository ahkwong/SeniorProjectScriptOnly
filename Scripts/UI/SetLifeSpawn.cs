using UnityEngine;

namespace Senior.Managers
{
    public class SetLifeSpawn : MonoBehaviour
    {
        public float timer;

        void OnEnable()
        {

            TrashMan.despawnAfterDelay(gameObject, timer);
        }
    }
}