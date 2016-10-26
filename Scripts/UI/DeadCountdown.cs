using UnityEngine;
using UnityEngine.UI;

namespace Senior.Managers
{
    public class DeadCountdown : MonoBehaviour
    {
        public delegate void CountdownAction(Player player);

        public static event CountdownAction DeadCountdownExpiration;


        public float defaultCountdownTimer = 10f;

        public Text countdownText;
        private bool IsEnabled = false;
        private bool expirationSent = false;

        private Player owner;

        public void Initialize(Player player)
        {
            owner = player;
        }

        void OnEnable()
        {
            IsEnabled = true;
            Reset();

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

                if (defaultCountdownTimer > 1)
                    countdownText.text = "CONTINUE? " + Mathf.Floor(defaultCountdownTimer).ToString();

                if (defaultCountdownTimer <= 0 && !expirationSent)
                {
                    if (DeadCountdownExpiration != null)
                        DeadCountdownExpiration(owner);

                    owner.OnDeadFinal();
                    GameManager.RemovePlayerFromGame(owner);
                    expirationSent = true;
                    IsEnabled = false;

                }
            }
        }
    }
}