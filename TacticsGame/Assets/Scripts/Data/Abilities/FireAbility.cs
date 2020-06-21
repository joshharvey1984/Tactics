using System.Collections.Generic;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Units;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data.Abilities {
    public sealed class FireAbility : Ability {
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override AbilityTypes AbilityType { get; set; }
        public override Sprite Icon { get; set; }

        public FireAbility() {
            Name = "Fire";
            AbilityType = AbilityTypes.Active;
            Description = "Fire weapon at selected Enemy";
            Icon = AssetDatabase.LoadAssetAtPath("Assets/Textures/Abilities/Icon_Target.png", typeof(Sprite)) as Sprite;
        }
        
        public override void Execute() {
            var selectedUnit = Unit.SelectedUnit;
            var targetUnit = selectedUnit.targetUnit;
            selectedUnit.LookAtGameObject(targetUnit.gameObject);
            var hitSuccess = Random.Range(0.01F, 1.00F);
            var damageDone = 0;
            var successfulHit = hitSuccess < CombatHitCalc.CalculateHitChance(selectedUnit, targetUnit)["HIT"];
            if (successfulHit)
                damageDone = CombatDamageCalc.DamageCalculation(selectedUnit, targetUnit)["DAMAGE"];
            selectedUnit.FireBullets(5);
            targetUnit.PopUpText(successfulHit ? damageDone.ToString() : "MISS");
            targetUnit.currentHitPoints -= damageDone;
            AbilityPause.StartPause(1.5F, this);
        }
    }
}
