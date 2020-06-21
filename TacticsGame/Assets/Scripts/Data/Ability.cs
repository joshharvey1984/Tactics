using System.Collections.Generic;
using TacticsGame.Battle.Units;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data {
    public abstract class Ability {
        public abstract string Name { get; set;}
        public abstract string Description { get; set;}
        public enum AbilityTypes {
            Active,
            Passive
        }
        public abstract AbilityTypes AbilityType { get; set; }
        public abstract Sprite Icon { get; set; }
        protected readonly AbilityPause AbilityPause;

        protected Ability() {
            AbilityPause = GameObject.Find("GameManager").GetComponent<AbilityPause>();
        }
        
        public abstract void Execute();

        protected void AddStatusEffect(string statusEffect, int numTurns) {
            var statusEffectObject = AssetDatabase.LoadAssetAtPath($"Assets/Scripts/Data/StatusEffects/{statusEffect}.asset", typeof(StatusEffect)) as StatusEffect;
            Unit.SelectedUnit.currentStatusEffects.Add((statusEffectObject, numTurns));
        }

        public static void EndAbility() {
            Unit.SelectedUnit.EndTurn();
        }
    }
}
