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
        public enum TargetingTypes {
            Enemy,
            Self
        }
        public abstract TargetingTypes TargetingType { get; set; }

        public enum SpecialTargeting {
            None = default,
            Cone
        }
        public virtual SpecialTargeting SpecialTarget { get; set; }
        protected readonly AbilityPause AbilityPause;

        protected Ability() {
            AbilityPause = GameObject.Find("GameManager").GetComponent<AbilityPause>();
        }
        
        public abstract void Execute();

        protected void AddStatusEffect(string statusEffect) {
            var statusEffectObject = AssetDatabase
                .LoadAssetAtPath($"Assets/Scripts/Data/StatusEffects/{statusEffect}.asset", typeof(StatusEffect)) as StatusEffect;
            Unit.SelectedUnit.AddStatusEffect(statusEffectObject);
        }

        public abstract void EndAbility();

        public virtual void Targeting() { }
        public virtual void TileWatchTrigger(Unit triggeredUnit) { }
    }
}
