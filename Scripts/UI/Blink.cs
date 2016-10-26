using UnityEngine;
using UnityEngine.UI;

namespace Senior.Managers
{
    public class Blink : MonoBehaviour
    {
        private Text guiText;

        public void Awake()
        {
            guiText = GetComponent<Text>();
        }
        public void Update()
        {
            Color col = guiText.color;
            col.a = Mathf.Round(Mathf.PingPong(Time.time * 1.0f, 1.0f));
            guiText.color = col;

        }
    }
}