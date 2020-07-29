using System.Collections.Generic;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Map;
using TacticsGame.Battle.Units;
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
            EnemyFire,
            EnemyWatch,
            Self,
            Throw
        }
        public abstract TargetingTypes TargetingType { get; set; }

        public enum SpecialTargeting {
            None = default,
            Cone,
            Throw
        }
        public virtual SpecialTargeting SpecialTarget { get; set; }
        protected readonly AbilityPause AbilityPause;
        public virtual List<CombatHitModifier> CombatHitModifiers { get; set; }

        protected Ability() {
            AbilityPause = GameObject.Find("GameManager").GetComponent<AbilityPause>();
        }
        
        public virtual void Execute() { }

        protected void AddStatusEffect(StatusEffect statusEffect) {
            Unit.ActiveUnit.AddStatusEffect(statusEffect);
        }
        
        protected void AddStatusEffectToTarget(StatusEffect statusEffect) {
            Unit.ActiveUnit.targetUnit.AddStatusEffect(statusEffect);
        }
        
        public virtual void EndAbility() { }
        public virtual void Targeting() { }
        public virtual void TileWatchTrigger(Unit triggeredUnit) { }
        public virtual void ExplodeEffect(MapTile landingTile) { }
        public virtual void PassiveEffect(Unit effectedUnit) { }
    }
}
