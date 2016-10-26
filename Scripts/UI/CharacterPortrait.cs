using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Senior.Managers
{
    public class CharacterPortrait : MonoBehaviour
    {
        public List<Player> PlayersCurrentlySelecting = new List<Player>();
        public bool IsConfirmed { get; set; }
        public GameObject heroPrefab;
        private Image image;
        private Player confirmedPlayer;
        private void Awake()
        {
            image = GetComponent<Image>();
            if (heroPrefab != null)
                GameManager.AddHeroToHeroPool(heroPrefab);
            Reset();
        }

        public void Reset()
        {
            PlayersCurrentlySelecting.Clear();
            IsConfirmed = false;
            if (image != null)
                image.color = Color.gray;
        }

        public void Selected(Player player)
        {
            if (!PlayersCurrentlySelecting.Contains(player))
                PlayersCurrentlySelecting.Add(player);
            image.color = Color.white;
        }

        public void Deselected(Player player)
        {
            if (PlayersCurrentlySelecting.Contains(player))
                PlayersCurrentlySelecting.Remove(player);
            
            if (!IsConfirmed && PlayersCurrentlySelecting.Count == 0)           
                image.color = Color.grey;
        }

        public void UnConfirm(Player player)
        {
            if (confirmedPlayer != player) return;
            IsConfirmed = false;
            GameManager.RemovePlayerHero(player);
            confirmedPlayer = null;
        }

        public void Confirmed(Player player)
        {
            if (confirmedPlayer != null) return;

            IsConfirmed = true;
            GameManager.GrantPlayerHero(player, heroPrefab);
            confirmedPlayer = player;
            image.color = Color.yellow;
        }
    }
}