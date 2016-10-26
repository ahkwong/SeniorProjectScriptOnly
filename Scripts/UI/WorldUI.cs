using Assets.Scripts.Entities;
using UnityEngine;

namespace Senior.Managers
{
    public class WorldUI : MonoBehaviour
    {
        public Entity owner;
        public bool follow;

        private RectTransform trans;
        public Vector2 offset;

        private void Start()
        {
            trans = GetComponent<RectTransform>();
        }
        public void Update()
        {
            if (follow && owner != null)
            {
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, owner.transform.position);
                trans.transform.position = screenPoint + offset;
            }
        }
    }
}