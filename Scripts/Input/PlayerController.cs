using System;
using UnityEngine;
using System.Collections;
using Senior.Globals;
using Senior.Managers;

namespace Senior.Inputs
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        private Player player;
        private string horizontalControlName;
        private string verticalControlName;
        private string attackButtonName;
        private string altAttackButtonName;
        private string skillOneButtonName;
        private string skillTwoButtonName;
        private string skillThreeButtonName;
        private string skillFourButtonName;
        private string startButtonName;

        public Vector2 MoveInput { get; private set; }
        public bool AttackButtonDown { get; private set; }
        public bool AttackButtonUp { get; private set; }
        public bool AltAttackButtonDown { get; private set; }
        public bool AltAttackButtonUp { get; private set; }
        public bool SkillOneButtonDown { get; private set; }
        public bool SkillOneButtonUp { get; private set; }
        public bool SkillTwoButtonDown { get; private set; }
        public bool SkillTwoButtonUp { get; private set; }
        public bool SkillThreeButtonDown { get; private set; }
        public bool SkillThreeButtonUp { get; private set; }
        public bool SkillFourButtonDown { get; private set; }
        public bool SkillFourButtonUp { get; private set; }
        public bool StartButtonDown { get; private set; }

        private void Awake()
        {
            player = GetComponent<Player>();

            InitializePlayerControls(player.PlayerNumber);
        }

        public void InitializePlayerControls(int playerNumber)
        {
            horizontalControlName = "Horizontal_P" + playerNumber;
            verticalControlName = "Vertical_P" + playerNumber;
            attackButtonName = "Attack_P" + playerNumber;
            altAttackButtonName = "AltAttack_P" + playerNumber;
            skillOneButtonName = "SkillOne_P" + playerNumber;
            skillTwoButtonName = "SkillTwo_P" + playerNumber;
            skillThreeButtonName = "SkillThree_P" + playerNumber;
            skillFourButtonName = "SkillFour_P" + playerNumber;
            startButtonName = "Start_P" + playerNumber;

#if UNITY_EDITOR
            Debug.Log(string.Format("Player {0} initialized.", playerNumber));
#endif

        }

        private void Update()
        {
            MoveInput = new Vector2(Input.GetAxisRaw(horizontalControlName), Input.GetAxisRaw(verticalControlName));
            AttackButtonDown = Input.GetButtonDown(attackButtonName);
            AttackButtonUp = Input.GetButtonUp(attackButtonName);

            AltAttackButtonDown = Input.GetButtonDown(altAttackButtonName);
            AltAttackButtonUp = Input.GetButtonUp(altAttackButtonName);

            SkillOneButtonDown = Input.GetButtonDown(skillOneButtonName);
            SkillOneButtonUp = Input.GetButtonUp(skillOneButtonName);

            SkillTwoButtonDown = Input.GetButtonDown(skillTwoButtonName);
            SkillTwoButtonUp = Input.GetButtonUp(skillTwoButtonName);

            SkillThreeButtonDown = Input.GetButtonDown(skillThreeButtonName);
            SkillThreeButtonUp = Input.GetButtonUp(skillThreeButtonName);

            SkillFourButtonDown = Input.GetButtonDown(skillFourButtonName);
            SkillFourButtonUp = Input.GetButtonUp(skillFourButtonName);

            StartButtonDown = Input.GetButtonDown(startButtonName);
            
        }

        
    }
}