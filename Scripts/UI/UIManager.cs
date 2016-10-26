using Senior.Globals;
using UnityEngine;

namespace Senior.Managers
{
    public class UIManager : MonoBehaviour
    {
        public GameObject MainMenu;
        public UICharacterSelect CharacterSelect;
        public GameObject PlayerUi;
        public GameObject WorldUi;
        public GameObject LoseScreen;
        public GameObject WinScreen;
        public static UIManager Instance { get; private set; }

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

        public void DisplayMainMenu()
        {
            MainMenu.SetActive(true);
            CharacterSelect.gameObject.SetActive(false);
            PlayerUi.SetActive(true);
            WorldUi.SetActive(false);
            LoseScreen.SetActive(false);
            WinScreen.SetActive(false);
            GameManager.SetGameState(GameState.MainMenu);
        }

        public void DisplayCharacterSelect()
        {
            MainMenu.SetActive(false);
            CharacterSelect.gameObject.SetActive(true);
            CharacterSelect.Reset();
            PlayerUi.SetActive(false);
            WorldUi.SetActive(false);
            LoseScreen.SetActive(false);
            WinScreen.SetActive(false);
            GameManager.SetGameState(GameState.CharacterSelect);
        }

        public void DisplayLoadingGraphic()
        {
            MainMenu.SetActive(false);
            CharacterSelect.gameObject.SetActive(false);
            PlayerUi.SetActive(false);
            WorldUi.SetActive(false);
            LoseScreen.SetActive(false);
            WinScreen.SetActive(false);
            GameManager.SetGameState(GameState.Loading);
        }

        public void DisplayInGameStuff()
        {
            MainMenu.SetActive(false);
            CharacterSelect.gameObject.SetActive(false);
            PlayerUi.SetActive(true);
            WorldUi.SetActive(true);
            for (int i = WorldUi.transform.childCount - 1; i >= 0; --i)
            {
                var child = WorldUi.transform.GetChild(i).gameObject;
                Destroy(child);
            }
            LoseScreen.SetActive(false);
            WinScreen.SetActive(false);
            GameManager.SetGameState(GameState.InGame);
        }

        public void DisplayLoseScreen()
        {
            // should have score and shit
            MainMenu.SetActive(false);
            CharacterSelect.gameObject.SetActive(false);
            PlayerUi.SetActive(false);
            WorldUi.SetActive(false);
            LoseScreen.SetActive(true);
            WinScreen.SetActive(false);
            GameManager.SetGameState(GameState.LoseScreen);
        }

        public void DisplayWinScreen()
        {
            // should have score and shit
            MainMenu.SetActive(false);
            CharacterSelect.gameObject.SetActive(false);
            PlayerUi.SetActive(false);
            WorldUi.SetActive(false);
            LoseScreen.SetActive(false);
            WinScreen.SetActive(true);
            GameManager.SetGameState(GameState.WinScreen);
        }
    }
}