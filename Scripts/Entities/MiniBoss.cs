using System.Collections;
using Assets.Scripts.AI;
using Assets.Scripts.Entities.Components;
using Senior.Globals;
using Senior.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Entities
{
    public class MiniBoss : Entity
    {
        //Monster Controller
        //AI which keep track of states
        //UI which shows health bar

        private Image HealthFill;
        public HealthBarWorldUI HealthPrefab;
        private GameObject healthGO;

        private AIManager aiManger;
        private MonsterState currentState = MonsterState.Idle;

        public MonsterState CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public float healAmount = 20f;

        public override void Awake()
        {
            base.Awake();
            mc = GetComponent<MonsterController>();
            aiManger = GetComponentInChildren<AIManager>();
        }

        public override void Die()
        {
            foreach (var players in GameManager.PlayersInGame)
            {
                players.hero.Heal(healAmount);
            }

            TrashMan.despawn(healthGO);
            base.Die();
            StartCoroutine(RemoveBody());
        }

        public IEnumerator RemoveBody()
        {
            yield return new WaitForSeconds(3f);
            TrashMan.despawn(gameObject);
        }

        public override void Start()
        {
            base.Start();
            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

            HealthBarWorldUI health = Instantiate(HealthPrefab, screenPoint, Quaternion.identity) as HealthBarWorldUI;
            healthGO = health.gameObject;
            healthGO.SetActive(true);
            health.GetComponent<RectTransform>().SetParent(UIManager.Instance.WorldUi.transform);
            health.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            health.offset = new Vector2(0, 40);
            health.owner = this;
            HealthFill = health.HealthFill;
            healthGO.SetActive(false);
        }

        public override void Damage(Entity dealer, float damage)
        {
            if (healthGO)
                healthGO.gameObject.SetActive(true);
            if (aiManger.currentBehavior != AIBehaviorType.NormalAttack)
                anim.SetTrigger("GotHit");
            base.Damage(dealer, damage);
            Debug.Log(name + " has been hit for " + damage + " damage!");
            HealthFill.fillAmount = StatsComponent.HealthCurrent/(float) StatsComponent.HealthMax;
        }

        public override void Heal(float heal)
        {
            base.Heal(heal);

            HealthFill.fillAmount = StatsComponent.HealthCurrent/(float) StatsComponent.HealthMax;


            if (StatsComponent.HealthCurrent == StatsComponent.HealthMax)
                healthGO.gameObject.SetActive(false);
        }

        public override void FullHeal()
        {
            base.FullHeal();
            //healthGO.gameObject.SetActive(false);
        }


    }
}