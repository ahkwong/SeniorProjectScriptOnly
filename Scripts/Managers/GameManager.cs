using System;
using System.Collections.Generic;
using Assets.Scripts.Entities.Hero;
using Senior.Globals;
using Senior.Inputs;
using UnityEngine;

namespace Senior.Managers
{
    public class GameManager : MonoBehaviour
    {
        public delegate void GameManagerAction();

        public static event GameManagerAction InactiveHeroListModified;

        public static GameManager Instance { get; private set; }
        public static GameState CurrentGameState { get; private set; }
        public static List<Player> PlayersInGame = new List<Player>();
        public static List<GameObject> HeroPool = new List<GameObject>();
        public static List<GameObject> HeroesNotInGame = new List<GameObject>();

        public static int NumberOfPlayersInGame
        {
            get { return PlayersInGame.Count; }
        }

        private void Awake()
        {
            // First we check if there are any other instances conflicting
            if (Instance != null && Instance != this)
            {
                // If that is the case, we destroy other instances
                Destroy(gameObject);
            }

            // Here we save our singleton instance
            Instance = this;

            // Furthermore we make sure that we don't destroy between scenes (this is optional)
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public static void SetGameState(GameState state)
        {
            CurrentGameState = state;
        }

        public static void AddPlayerToGame(Player player)
        {
            if (!PlayerIsInGame(player))
                PlayersInGame.Add(player);
        }

        public static void RemovePlayerFromGame(Player player)
        {
            if (PlayerIsInGame(player))
                PlayersInGame.Remove(player);

            // if there are no players from the game go to game over/high score screen
            CheckIfAllAreDead();
        }

        public static void AddHeroToHeroPool(GameObject hero)
        {
            if (HeroPool.Contains(hero)) return;

            HeroPool.Add(hero);
            HeroesNotInGame.Add(hero);

            if (InactiveHeroListModified != null)
                InactiveHeroListModified();
        }

        public static void GrantPlayerHero(Player player, GameObject hero)
        {
            if (PlayerIsInGame(player))
            {
                if (HeroPool.Contains(hero))
                {
                    player.HeroGO = hero;

                    if (HeroesNotInGame.Contains(hero))
                        HeroesNotInGame.Remove(hero);

                    if (InactiveHeroListModified != null)
                        InactiveHeroListModified();
                }
            }
        }

        public static void RemovePlayerHero(Player player)
        {
            if (PlayerIsInGame(player))
            {
                HeroesNotInGame.Add(player.HeroGO);
                player.HeroGO = null;

                if (InactiveHeroListModified != null)
                    InactiveHeroListModified();
            }
        }

        public static void CheckIfAllAreDead()
        {
            if (PlayersInGame.Count <= 0)
            {
                LoadLevel("lose");
                UIManager.Instance.DisplayLoseScreen();
            }
        }

        // player defeated the skeleton king
        public static void Win()
        {
            LoadLevel("win");
            UIManager.Instance.DisplayWinScreen();

        }


        public static bool PlayerIsInGame(Player player)
        {
            return PlayersInGame.Contains(player);
        }

        public static bool AllPlayersInGameAreConfirmed()
        {
            foreach (var p in PlayersInGame)
            {
                if (p.CurrentState != PlayerState.ConfirmedCharacter)
                    return false;
            }
            return true;
        }       
        //TODO: Create loading screen
        //TODO: This is a trash method
        public static void LoadLevel(string levelName)
        {
            if (levelName == Application.loadedLevelName) return;

            Application.LoadLevel(levelName);

        }


    }
}