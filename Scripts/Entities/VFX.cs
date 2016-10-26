using Senior.Globals;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Entities
{
    public class VFX : MonoBehaviour
    {
        public float lifeTime = 1;

        void Start()
        {
            Destroy(this);
        }
    }
}