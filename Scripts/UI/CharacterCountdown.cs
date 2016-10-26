using Senior.Globals;
using Senior.Inputs;
using UnityEngine;
using UnityEngine.UI;

namespace Senior.Managers
{
    public class CharacterCountdown : MonoBehaviour
    {
        public delegate void CountdownAction();

        public static event CountdownAction CountdownExpiration;

        private Text countdownText;
        public float defaultCountdownTimer = 30f;

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
            defaultCountdownTimer = 30f;
            expirationSent = false;
        }

        // Decrement timer when all players in the character selection screen are confirmed
        public void DecrementTimer()
        {
            if (GameManager.AllPlayersInGameAreConfirmed())
                defaultCountdownTimer -= 1;
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
                    if (CountdownExpiration != null)
                        CountdownExpiration();
                    expirationSent = true;

                    countdownText.text = "Now Loading...";

                }
                //have callback or delay so that players can see what heroes they random.

                if (defaultCountdownTimer <= -2)
                {
                    UIManager.Instance.DisplayInGameStuff();
                    GameManager.LoadLevel("LevelZero");
                    //Loading Screen
                    IsEnabled = false;
                    Reset();
                }
            }
        }
    }
}