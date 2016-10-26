using UnityEngine;

namespace Senior.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        public GameObject PlayerOne;
        public GameObject PlayerTwo;
        public GameObject PlayerThree;
        public GameObject PlayerFour;


        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void SpawnHero(int playerNumber, int heroNumber)
        {
            
        }
    }
}