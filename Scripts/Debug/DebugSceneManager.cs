using Senior;
using Senior.Globals;
using Senior.Managers;
using UnityEngine;

namespace Senior.Debugs
{
    public class DebugSceneManager : MonoBehaviour
    {
        public Player player1;
        public Player player2;
        public Player player3;
        public Player player4;

        public GameObject keno;
        public GameObject andrew;
        public GameObject hau;
        public GameObject lung;

        void Start()
        {
            GameManager.AddHeroToHeroPool(keno);
            GameManager.AddHeroToHeroPool(andrew);
            GameManager.AddHeroToHeroPool(hau);
            GameManager.AddHeroToHeroPool(lung);

            GameManager.AddPlayerToGame(player1);
            //GameManager.AddPlayerToGame(player2);
            //GameManager.AddPlayerToGame(player3);
            //GameManager.AddPlayerToGame(player4);

            GameManager.GrantPlayerHero(player1, lung);
            //GameManager.GrantPlayerHero(player2, andrew);

            player1.SpawnPlayer(Vector3.zero);
            player2.SpawnPlayer(Vector3.zero);
            player3.SpawnPlayer(Vector3.zero);
            player4.SpawnPlayer(Vector3.zero);

            GameManager.SetGameState(GameState.InGame);

        }
    }
}