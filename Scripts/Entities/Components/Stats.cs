using UnityEngine;
using UnityEngine.UI;

namespace Senior.Components
{
    public class Stats : MonoBehaviour
    {
        public int Experience { get; set; }
        public int Level { get; set; }

        // The base health granted to the entity
        public float HealthBase = 100;
        public float HealthCurrent { get; set; }
        // The health granted per level, current default value is 32 (set in the editor to modify)
        public float HealthPerLevel = 32;
        // The sum of all health modifications due to items/skills/buffs/debuffs
        public float HealthModifier { get; set; }
        //The max health as the result of the formula
        public float HealthMax
        {
            get { return (HealthBase + HealthPerLevel * Level + HealthModifier); }
        }

        public float StaminaBase = 100;
        public float StaminaCurrent { get; set; }

        public float AttackSpeedBase = 5;
        public float AttackSpeedCurrent { get; set; }

        public float DamageBase = 5;
        public float DamageCurrent { get; set; }

        public float MovementSpeedBase = 5;
        public float MovementSpeedCurrent { get; set; }

        public float RotationSpeedBase = 20;
        public float RotationSpeedCurrent { get; set; }

        public void Awake()
        {
            //HealthCurrent = HealthBase;
            //StaminaCurrent = StaminaBase;
        }


    }
}