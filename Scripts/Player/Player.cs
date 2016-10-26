using System;
using Assets.Scripts.Entities.Hero;
using Senior.Globals;
using Senior.Inputs;
using Senior.Items;
using Senior.Managers;
using Seniors.Skills;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Senior
{
    public class Player : MonoBehaviour
    {
    #region vars
        public delegate void PlayerAction(Player player);

        public static event PlayerAction HeroSpawned;
        public static event PlayerAction HeroDied;
        [SerializeField]
        private int playerNumber;

        public int PlayerNumber
        {
            get {return playerNumber;}
            set { playerNumber = value; }
        }
        [SerializeField]
        private PlayerState currentState = PlayerState.WaitingForCredits;

        public PlayerState CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        [SerializeField]
        private GameObject heroGO;

        public GameObject HeroGO
        {
            get { return heroGO; }
            set { heroGO = value; }
        }

        public Hero hero;

        public PlayerUI ui;

        public PlayerController control;
    #endregion

    #region In Game Initialize

        void OnEnable()
        {
            MapScript.MapCreatedSuccess += SpawnPlayer;
        }

        void Disable()
        {
            MapScript.MapCreatedSuccess -= SpawnPlayer;

        }
        void Awake()
        {
            control = GetComponent<PlayerController>();
        }


        // Spawns a Hero that is controlled by the player.
        public void SpawnPlayer(Vector3 spawnPosition)
        {
            if (HeroGO != null)
            {
                spawnPosition.x += Random.Range(0, 2);
                spawnPosition.z += Random.Range(0, 2);
                GameObject go = Instantiate(heroGO, spawnPosition, Quaternion.identity) as GameObject;
                go.transform.parent = this.transform;

                if (HeroSpawned != null)
                    HeroSpawned(this);

                CurrentState = PlayerState.Playing;
               
                ShowPlayerUI(go);
            }
        }

        // Spawns the Health/Stamina bars if a hero is spawned, otherwise
        // it spawns a insert credits text. We pass in the initialized game object 
        // because we can't access getcomponent on non initialized prefabs
        public void ShowPlayerUI(GameObject go)
        {
            if (ui != null)
            {

                hero = go.GetComponent<Hero>();
                hero.Initialize(this);
                ui.Initialize(this);
            }

        }

    #endregion

    #region Player UI

        // Whenever your health value is changed, inform the health bar
        public void OnHealthModified(Hero hero)
        {
            ui.OnHealthModified(hero);
        }

        // When you die, inform the UI to spawn the continue countdown
        public void OnDead(Hero hero)
        {
            CurrentState = PlayerState.HeroDead;
            ui.OnDead(hero);
        }

        // destroy the hero game object
        public void OnDeadFinal()
        {
            GameManager.RemovePlayerHero(this);

            CurrentState = PlayerState.WaitingForCredits;
            ui.ShowCoinText();

            if (HeroDied != null)
                HeroDied(this);

            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }          
        }

        // When you pick up an item, add the item sprite to the UI
        public void OnItemPickUp(Item item)
        {
            ui.OnItemPickUp(item);
        }

        public void SetSkillIcon(Skill skill)
        {
            ui.SetSkillIcon(skill);
        }

        public void UseSkill(Skill skill)
        {
            ui.UseSkill(skill);
        }

        public void UpdateSkill(Skill skill)
        {
            ui.UpdateSkill(skill);
        }

        #endregion

        #region PlayerInput

        private bool isAxisInUse = false;
        void Update()
        {
            if (control.StartButtonDown)
            {
                OnStartButtonPressed();
            }

            // skip the rest if the player is currently in game and playing
            if (CurrentState == PlayerState.Playing) return;

            if (control.MoveInput.x < 0)
            {
                if (!isAxisInUse)
                {
                    isAxisInUse = true;
                    OnLeftButtonPressed();
                }
            }
            else if (control.MoveInput.x > 0)
            {
                if (!isAxisInUse)
                {
                    isAxisInUse = true;
                    OnRightButtonPressed();
                }
            }
            else
            {
                isAxisInUse = false;
            }

            if (control.AttackButtonDown)
                OnConfirmButtonPressed();

            if (control.AltAttackButtonDown)
                OnCancelButtonPressed();
        }

        // Depending on the game state (ie whether you're in the main menu or when you're playing)
        // we handle what happens when a player presses start.
        private void OnStartButtonPressed()
        {
            GameManager.AddPlayerToGame(this);

#if UNITY_EDITOR
            Debug.Log(string.Format("Player {0} pressed the Start Button!", PlayerNumber));
#endif

            if (CurrentState == PlayerState.WaitingForCredits && CurrentState != PlayerState.ConfirmedCharacter)
                CurrentState = PlayerState.ChoosingCharacter;

            switch (GameManager.CurrentGameState)
            {
                case GameState.MainMenu:
                    UIManager.Instance.DisplayCharacterSelect();
                    UIManager.Instance.CharacterSelect.ActivatePlayerSelectionSprite(this);
                    break;
                case GameState.CharacterSelect:
                    UIManager.Instance.CharacterSelect.ActivatePlayerSelectionSprite(this);
                    break;
                case GameState.InGame:
                    if (CurrentState == PlayerState.ChoosingCharacter)
                    {
                        if (!ui.HeroSelect.activeInHierarchy)
                            ui.ShowHeroSelect();
                        else
                            ui.OnConfirm(this);
                    }
                    else if (currentState == PlayerState.HeroDead)
                    {
                        hero.Revive();
                        ui.ShowHeroStats();
                    }
                    //else if portraits are displayed spawn the hero
                    break;
                    case GameState.LoseScreen:
                    case GameState.WinScreen:
                        GameManager.LoadLevel("main");
                        UIManager.Instance.DisplayMainMenu();
                    break;
            }
        }

        private void OnLeftButtonPressed()
        {
            if (currentState != PlayerState.ChoosingCharacter) return;
            
                switch (GameManager.CurrentGameState)
            {
                case GameState.CharacterSelect:                   
                    UIManager.Instance.CharacterSelect.MovePlayerSelectLeft(this);                    
                    break;
                case GameState.InGame:
                    ui.OnSelectLeft(this);
                    break;
            }
        }

        private void OnRightButtonPressed()
        {
            if (currentState != PlayerState.ChoosingCharacter) return;

            switch (GameManager.CurrentGameState)
            {
                case GameState.CharacterSelect:
                    UIManager.Instance.CharacterSelect.MovePlayerSelectRight(this);                    
                    break;
                case GameState.InGame:
                    ui.OnSelectRight(this);
                    break;
            }
        }

        private void OnConfirmButtonPressed()
        {
            switch (GameManager.CurrentGameState)
            {
                case GameState.CharacterSelect:
                    if (CurrentState == PlayerState.ChoosingCharacter)
                    {
                        UIManager.Instance.CharacterSelect.ConfirmSelection(this);
                        CurrentState = PlayerState.ConfirmedCharacter;
                    }
                    else if (CurrentState == PlayerState.ConfirmedCharacter)
                        UIManager.Instance.CharacterSelect.cd.DecrementTimer();
                    break;
                case GameState.InGame:
                    if (CurrentState == PlayerState.ChoosingCharacter)
                    {
                        ui.OnConfirm(this);
                    }
                        //choose the selected thing
                    break;
            }
        }

        private void OnCancelButtonPressed()
        {
            switch (GameManager.CurrentGameState)
            {
                case GameState.CharacterSelect:
                    if (CurrentState == PlayerState.ConfirmedCharacter)
                    {
                        UIManager.Instance.CharacterSelect.CancelSelection(this);
                        CurrentState = PlayerState.ChoosingCharacter;
                    }
                    break;
            }
        }

        #endregion

    }
}