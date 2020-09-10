using System.Collections.Generic;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Data.Abilities {
    public sealed class Overwatch : Ability {
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override AbilityTypes AbilityType { get; set; }
        public override Sprite Icon { get; set; }
        public override TargetingTypes TargetingType { get; set; }
        public override List<AbilityBehaviour> AbilityBehaviours { get; set; }
        public override List<AbilityCalculation> AbilityCalculations { get; set; }

        public Overwatch() {
            Name = "Overwatch";
            AbilityType = AbilityTypes.Active;
            Description = "Fire at the first enemy unit to move in line of sight";
            Icon = Resources.Load<Sprite>("Textures/Abilities/Icon_Eye");
            TargetingType = TargetingTypes.Cone;
            CombatHitModifiers = new List<CombatHitModifier> {
                new CombatHitModifier {
                    Name = "Reaction Shot", 
                    Description = "Reaction Shot", 
                    ModifierValue = -0.2F
                }
            };
            ActiveUnitStatusEffect = new StatusEffects.Overwatch();
        }
        
        public override void TileWatchTrigger(Unit triggeredUnit) {
            var selectedUnit = triggeredUnit;
            var targetUnit = Unit.ActiveUnit;
            selectedUnit.LookAtGameObject(targetUnit.gameObject);
            var hitSuccess = Random.Range(0.01F, 1.00F);
            var damageDone = 0;
            var successfulHit = hitSuccess < CombatHitCalc.CalculateHitChance(selectedUnit, targetUnit, this)["HIT"];
            if (successfulHit)
                damageDone = CombatDamageCalc.DamageCalculation(selectedUnit, targetUnit, this)["DAMAGE"];
            selectedUnit.FireBullets(5);
            targetUnit.PopUpText(successfulHit ? damageDone.ToString() : "MISS");
            targetUnit.TakeDamage(damageDone);
        }
    }
}