using System.Collections.Generic;
using Assets.Scripts.Entities;
using Senior.Components;
using Senior.Globals;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public class AIManager : MonoBehaviour
    {
        public float decisionInterval = 5f;
        private bool isEnabled = false;
        private float timer = 0f;
        private Dictionary<AIBehaviorType, AIBehavior> states = new Dictionary<AIBehaviorType, AIBehavior>();
        public AIBehaviorType currentBehavior = AIBehaviorType.None;
        // Enable the AI
        public void Enable()
        {
            isEnabled = true;
            FindNewBehaviour();
        }

        // Disable the AI
        public void Disable()
        {
            isEnabled = false;
            currentBehavior = AIBehaviorType.None;
            foreach (KeyValuePair<AIBehaviorType, AIBehavior> behavior in states)
            {
                behavior.Value.Disable();
            }
        }

        // get all behaviour components
        public void Awake()
        {
            AIBehavior[] behaviors = GetComponentsInChildren<AIBehavior>();

            for (int i = 0; i < behaviors.Length; i++)
            {
                states.Add(behaviors[i].Type, behaviors[i]);
            }
        }

        // initialize all behaviors
        public void Initialize(Entity owner, SkillsController skills)
        {
            foreach (KeyValuePair<AIBehaviorType, AIBehavior> behavior in states)
            {
                behavior.Value.Initialize(owner,skills, this);
            }

            Enable();
        }

        public void FindNewBehaviour()
        {
            if (!isEnabled) return;

            // find the suitable behavior
            AIBehavior behavior = GetDecision();

            // enable the behavior
            if (behavior != null)
            {
                //AIBehavior pastBehavior;
                // disable the past behavior
                //if (states.TryGetValue(currentBehavior, out pastBehavior))
                //{
                 //   states[currentBehavior].Disable();
                //}

                behavior.Enable();

                currentBehavior = behavior.Type;
            }
            else
            {
                currentBehavior = AIBehaviorType.None;
            }
        }
        // in intervals choose which behavior to do
        public void Update()
        {
            if (isEnabled)
            {
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    // find the suitable behavior
                    if (currentBehavior == AIBehaviorType.None)
                        FindNewBehaviour();

                    // reset timer
                    timer = decisionInterval;
                }
            }
        }

        // enable a valid behavior
        private AIBehavior GetDecision()
        {
            int totalWeight = 0;
            List<AIBehavior> validBehaviors = new List<AIBehavior>();

            foreach (KeyValuePair<AIBehaviorType, AIBehavior> behavior in states)
            {
                if (behavior.Value.IsValid())
                {
                    totalWeight += behavior.Value.weight;
                    validBehaviors.Add(behavior.Value);
                }
            }

            int randomNumber = Random.Range(0, totalWeight);

            for (int i = 0; i < validBehaviors.Count; i++)
            {
                if (randomNumber < validBehaviors[i].weight)
                    return validBehaviors[i];
                randomNumber -= validBehaviors[i].weight;
            }

            return null;
        }
    }
}