using Senior.Inputs;
using UnityEngine;
using UnityEngine.UI;

namespace Senior.Managers
{
    public class ContinueCountdown : MonoBehaviour
    {

        private Text countdownText;
        public float defaultCountdownTimer = 10f;

        public bool IsEnabled = false;
        private bool expirationSent = false;

        void Awake()
        {
            countdownText = GetComponent<Text>();
        }


        void OnEnable()
        {
            IsEnabled = true;
        }

        void OnDisable()
        {
            IsEnabled = false;
            Reset();
        }

        void Reset()
        {
            defaultCountdownTimer = 10f;
            expirationSent = false;
        }


        void Update()
        {
            if (IsEnabled)
            {
                defaultCountdownTimer -= Time.deltaTime;

                if (defaultCountdownTimer > 0)
                    countdownText.text = Mathf.Floor(defaultCountdownTimer).ToString();

                if (defaultCountdownTimer <= 0 && !expirationSent)
                {
                    expirationSent = true;

                    countdownText.text = "Now Loading...";

                }

            }
        }
    }

}