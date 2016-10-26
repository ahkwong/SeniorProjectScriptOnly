using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Senior.Globals;
using Senior.Inputs;

namespace Senior.Managers
{
    public class UICharacterSelect : MonoBehaviour
    {
        public delegate void Select(Player player);

        //TODO: Instantiate stuff during awake so that you wont get null errors
        public RectTransform[] PlayerSelectionSprites;
        public CharacterPortrait[] CharacterPortraits;
        //TODO: Make this so that when icons overlap they do not hide each other.
        private Vector2[] PlayerSelctionPositions =
        {
            new Vector2(-247, 215),
            new Vector2(-78, 215),
            new Vector2(83, 215),
            new Vector2(241, 215)
        };

        private Dictionary<Player,int> playerSelectionIndex = new Dictionary<Player, int>(); //player..CurrentSelection

        public CharacterCountdown cd;

        private void Awake()
        {
            Initialize();
        }


        private void OnEnable()
        {
            CharacterCountdown.CountdownExpiration += FinalizePlayerHeroes;
        }

        private void OnDisable()
        {
            CharacterCountdown.CountdownExpiration -= FinalizePlayerHeroes;
        }

        private void Initialize()
        {
            Reset();
        }

        public void Reset()
        {
            //Set All Sprites Inactive
            foreach (var sprite in PlayerSelectionSprites)
            {
                sprite.gameObject.SetActive(false);
            }

            foreach (var portrait in CharacterPortraits)
            {
                portrait.Reset();
            }
            playerSelectionIndex.Clear();
        }

        public void ActivatePlayerSelectionSprite(Player player)
        {
            if (playerSelectionIndex.ContainsKey(player)) return;
            int index = player.PlayerNumber - 1;
            int unComfirmedChar = GetUnConfirmedCharacter();
            playerSelectionIndex.Add(player, unComfirmedChar);
            PlayerSelectionSprites[index].anchoredPosition = PlayerSelctionPositions[unComfirmedChar];
            CharacterPortraits[unComfirmedChar].Selected(player);
            PlayerSelectionSprites[index].gameObject.SetActive(true);

        }

        //TODO Make this a delegate
        public void MovePlayerSelectRight(Player player)
        {
            int characterSelectIndex = player.PlayerNumber - 1;
            int prevIndex = playerSelectionIndex[player];
            int newIndex = SearchRight(player);
            PlayerSelectionSprites[characterSelectIndex].anchoredPosition = PlayerSelctionPositions[newIndex];

            CharacterPortraits[prevIndex].Deselected(player);
            CharacterPortraits[newIndex].Selected(player);

            playerSelectionIndex[player] = newIndex;

        }

        // This method listens to the Player Events when a player moves left.
        public void MovePlayerSelectLeft(Player player)
        {
            int characterSelectIndex = player.PlayerNumber - 1;
            int prevIndex = playerSelectionIndex[player];
            int newIndex = SearchLeft(player);

            // Moves the Player Marker to the next index
            PlayerSelectionSprites[characterSelectIndex].anchoredPosition = PlayerSelctionPositions[newIndex];

            CharacterPortraits[prevIndex].Deselected(player);
            CharacterPortraits[newIndex].Selected(player);

            // Registers the new selection to the player selection index
            playerSelectionIndex[player] = newIndex;
        }

        public void ConfirmSelection(Player player)
        {
            int index = playerSelectionIndex[player];
            CharacterPortraits[index].Confirmed(player);
            
            //Update Positions of players that had selected this hero before it being confirmed.
            //Bug: the PlayersCurrntlySelecting is not functioning
            foreach (var p in CharacterPortraits[index].PlayersCurrentlySelecting.ToList())
            {
                if (p != player)
                   MoveCharactersToUnconfirmedPositions(p, DoNothing);
            }
            
        }

        public void DoNothing(Player player) { }
        

        public void CancelSelection(Player player)
        {
            int index = playerSelectionIndex[player];
            CharacterPortraits[index].UnConfirm(player);
            CharacterPortraits[index].Selected(player);
        }

        public int SearchRight(Player player)
        {

            int index = playerSelectionIndex[player];

            if (index == CharacterPortraits.Length - 1) return index;

            for (int i = index + 1; i < CharacterPortraits.Length; i++)
            {
                if (!CharacterPortraits[i].IsConfirmed)
                {
                    return i;
                }
            }

            return index;
        }

        public int SearchLeft(Player player)
        {
            int index = playerSelectionIndex[player];

            if (index == 0) return index;

            for (int i = index - 1; i >= 0; i--)
            {
                if (!CharacterPortraits[i].IsConfirmed)
                {
                    return i;
                }
            }

            return index;
        }

        private int GetUnConfirmedCharacter()
        {

            for (int i = 0; i < CharacterPortraits.Length; i++)
            {
                if (!CharacterPortraits[i].IsConfirmed)
                {
                    return i;
                }
            }
            return 0;

        }

        void MoveCharactersToUnconfirmedPositions(Player player, Select select)
        {
            int index = GetUnConfirmedCharacter();
            if (playerSelectionIndex[player] > index)
            {
                MovePlayerSelectLeft(player);
                select(player);
            }
            else if (playerSelectionIndex[player] < index)
            {
                MovePlayerSelectRight(player);
                select(player);
            }
            else
            {
                select(player);
            }
        }

        private void FinalizePlayerHeroes()
        {
            if (GameManager.AllPlayersInGameAreConfirmed()) return;
            foreach (var player in GameManager.PlayersInGame)
            {
                if (player.CurrentState != PlayerState.ConfirmedCharacter)
                {
                    //If the character the player is currently selecting is not confirmed, confirm it for the player.
                    if (!CharacterPortraits[playerSelectionIndex[player]].IsConfirmed)
                        ConfirmSelection(player);
                    else                   
                        MoveCharactersToUnconfirmedPositions(player,ConfirmSelection);
                                     
                }
            }
        }
    }
}