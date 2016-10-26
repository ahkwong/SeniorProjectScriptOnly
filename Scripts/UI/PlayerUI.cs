using System.Collections.Generic;
using Assets.Scripts.Entities.Hero;
using Senior.Items;
using Seniors.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace Senior.Managers
{
    public class PlayerUI : MonoBehaviour
    {
        public GameObject HeroStats;
        public GameObject CoinText;
        public GameObject ContinueText;
        public GameObject HeroSelect;

        public Image HeroPorait;

        public Text HealthText;
        public Image HealthFill;

        public List<Image> SkillIcons;
        public List<Image> SkillIconFill; 
        public List<Text> SkillCdText; 
        private Player owner;

        public void Awake()
        {
            ShowCoinText();
        }

        public void ShowHeroStats()
        {
            CoinText.SetActive(false);
            HeroStats.SetActive(true);
            ContinueText.SetActive(false);
            HeroSelect.SetActive(false);
        }

        public void ShowCoinText()
        {
            CoinText.SetActive(true);
            HeroStats.SetActive(false);
            ContinueText.SetActive(false);
            HeroSelect.SetActive(false);
        }

        public void ShowContinueText()
        {
            CoinText.SetActive(false);
            HeroStats.SetActive(false);
            ContinueText.SetActive(true);
            HeroSelect.SetActive(false);
        }

        public void ShowHeroSelect()
        {
            CoinText.SetActive(false);
            HeroStats.SetActive(false);
            ContinueText.SetActive(false);
            HeroSelect.SetActive(true);
        }

        public void Initialize(Player player)
        {
            if (player != null)
            {
                owner = player;
                if (player.HeroGO != null)
                {
                    Hero hero = player.HeroGO.GetComponent<Hero>();
                    if (hero.Portrait != null)
                        HeroPorait.sprite = hero.Portrait;

                    for (int i = 0; i < SkillIconFill.Count; i++)
                    {
                        SkillIconFill[i].gameObject.SetActive(false);
                    }

                    for (int i = 0; i < SkillCdText.Count; i++)
                    {
                        SkillCdText[i].gameObject.SetActive(false);
                    }

                    DeadCountdown dc = ContinueText.GetComponent<DeadCountdown>();
                    if (dc != null)
                        dc.Initialize(player);
                    ShowHeroStats();
                }

            }
        }

        // whenever the hero's health is modified, change the health text/bar
        public void OnHealthModified(Hero hero)
        {
            SetHealthText(hero.StatsComponent.HealthCurrent, hero.StatsComponent.HealthMax);
        }

        public void SetHealthText(float current, float max)
        {
            string health = string.Format("{0}/{1}", current, max);
            HealthText.text = health;
            HealthFill.fillAmount = current/(float)max;
        }

        public void SetSkillIcon(Skill skill)
        {
            if (SkillIcons != null)
            {
                SkillIcons[skill.SkillIndex].sprite = skill.SkillIcon;
            }
        }

        public void UseSkill(Skill skill)
        {
            SkillIconFill[skill.SkillIndex].gameObject.SetActive(true);
            SkillCdText[skill.SkillIndex].gameObject.SetActive(true);

        }

        public void UpdateSkill(Skill skill)
        {
            if (skill.CoolDownTimer > 0)
            {
                SkillIconFill[skill.SkillIndex].fillAmount = skill.CoolDownTimer/ skill.CoolDown;
                SkillCdText[skill.SkillIndex].text = Mathf.Floor(skill.CoolDownTimer + 1).ToString();
            }
            else
            {
                SkillIconFill[skill.SkillIndex].gameObject.SetActive(false);
                SkillCdText[skill.SkillIndex].gameObject.SetActive(false);
            }
        }

        //Show item icon in player ui
        public void OnItemPickUp(Item item)
        {
        }

        // When a player dies, show the countdown timer to allow them to continue with the same hero
        public void OnDead(Hero hero)
        {
            ShowContinueText();
        }

        public void  OnSelectLeft(Player player)
        {
            InGameHeroSelect hs = HeroSelect.GetComponent<InGameHeroSelect>();

            if (hs != null)
            {
                hs.OnSelectLeft(player);
            }
        }

        public void OnSelectRight(Player player)
        {
            InGameHeroSelect hs = HeroSelect.GetComponent<InGameHeroSelect>();

            if (hs != null)
            {
                hs.OnSelectRight(player);
            }
        }

        public void OnConfirm(Player player)
        {
            InGameHeroSelect hs = HeroSelect.GetComponent<InGameHeroSelect>();

            if (hs != null)
            {
                hs.OnConfirm(player);
            }
        }
    }
}