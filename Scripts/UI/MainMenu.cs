using System;
using Senior.Globals;
using Senior.Inputs;
using UnityEngine;
using UnityEngine.UI;

namespace Senior.Managers
{
    public class MainMenu : MonoBehaviour
    {
        private void OnEnable()
        {
           // PlayerController.StartButtonPressed += StartGame;
        }

        private void OnDisable()
        {
           // PlayerController.StartButtonPressed -= StartGame;
        }

        private void StartGame(int playerNumber)
        {
            //if (!GameManager.Instance.PlayerIsInGame(playerNumber))
            //    GameManager.PlayersInGame.Add(playerNumber);

            //UIManager.Instance.DisplayCharacterSelect();
            //UIManager.Instance.CharacterSelect.ActivatePlayerSelectionSprite(playerNumber);

        }
    }
}