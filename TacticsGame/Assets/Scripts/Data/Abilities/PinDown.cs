using System.Collections.Generic;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Units;
using TacticsGame.Data.StatusEffects;
using UnityEngine;

namespace TacticsGame.Data.Abilities {
    public sealed class PinDown : Ability {
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override AbilityTypes AbilityType { get; set; }
        public override Sprite Icon { get; set; }
        public override TargetingTypes TargetingType { get; set; }
        public override List<AbilityBehaviour> AbilityBehaviours { get; set; }
        public override List<AbilityCalculation> AbilityCalculations { get; set; }

        public PinDown() {
            Name = "Pin Down";
            AbilityType = AbilityTypes.Active;
            Description = "Pin down an enemy fighter and reduce their aim. If the enemy moves in line of sight then perform a reaction shot.";
            Icon = Resources.Load<Sprite>("Textures/Abilities/Icon_PinDown");
            TargetingType = TargetingTypes.Single;
            CombatHitModifiers = new List<CombatHitModifier> {
                new CombatHitModifier {
                    Name = "Reaction Shot", 
                    Description = "Reaction Shot", 
                    ModifierValue = -0.2F
                }
            };
            ActiveUnitStatusEffect = new StatusEffects.PinDown();
            TargetUnitStatusEffect = new PinnedDown();
            AbilityBehaviours = new List<AbilityBehaviour> {
                new UnitVoiceClip(1, "LockedOnTarget"),
                new UnitAnimation(1, "AimWeapon"),
                new AddStatusEffect(1, new StatusEffects.PinDown()),
                new AddStatusEffectToTarget(1, new PinnedDown())
            };
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