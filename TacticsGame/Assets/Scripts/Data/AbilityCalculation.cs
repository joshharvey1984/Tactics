using TacticsGame.Battle.Core;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Data {
    public abstract class AbilityCalculation {
        public abstract void Calculate();
        protected static bool HitSuccess() {
            return Random.Range(0.01F, 1.00F) < CombatHitCalc
                .CalculateHitChance(Unit.ActiveUnit, Unit.ActiveUnit.targetUnit, Unit.ActiveUnit.selectedAbility)["HIT"];
        }
        protected static int DamageAmount() {
            return Unit.ActiveUnit.damageCalculation = CombatDamageCalc
                .DamageCalculation(Unit.ActiveUnit, Unit.ActiveUnit.targetUnit, Unit.ActiveUnit.selectedAbility)[
                    "DAMAGE"];
        }
    }
    
    internal sealed class CalculationDamage : AbilityCalculation {
        public override void Calculate() {
            Unit.ActiveUnit.damageCalculation = HitSuccess() ? DamageAmount() : 0;
        }
    }
}