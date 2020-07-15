using TacticsGame.Battle.Core;
using TacticsGame.Battle.Map.UI.Targeting;
using TacticsGame.Battle.Units;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data.Abilities {
    public sealed class Overwatch : Ability {
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override AbilityTypes AbilityType { get; set; }
        public override Sprite Icon { get; set; }
        public override TargetingTypes TargetingType { get; set; }
        public override SpecialTargeting SpecialTarget { get; set; }

        public Overwatch() {
            Name = "Overwatch";
            AbilityType = AbilityTypes.Active;
            Description = "Fire at the first enemy unit to move in line of sight";
            Icon = AssetDatabase.LoadAssetAtPath("Assets/Textures/Abilities/Icon_Eye.png", typeof(Sprite)) as Sprite;
            TargetingType = TargetingTypes.Self;
            SpecialTarget = SpecialTargeting.Cone;
        }

        public override void Targeting() {
            var coneTarget = new ConeTarget();
            coneTarget.SubscribeToHoverTile();
        }

        public override void Execute() {
            Unit.SelectedUnit.PopUpText("OVERWATCH");
            AddStatusEffect("Overwatch");
            AbilityPause.StartPause(1.5F, this, "EndAbility");
        }
        
        public override void EndAbility() {
            Unit.SelectedUnit.EndTurn();
        }

        public override void TileWatchTrigger(Unit triggeredUnit) {
            var selectedUnit = triggeredUnit;
            var targetUnit = Unit.SelectedUnit;
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