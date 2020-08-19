using TacticsGame.Battle.Core;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Data.Abilities {
    public sealed class FireAbility : Ability {
        public override string Name { get; set; }
        public override string Description { get; set; }
        public override AbilityTypes AbilityType { get; set; }
        public override Sprite Icon { get; set; }
        public override TargetingTypes TargetingType { get; set; }

        public FireAbility() {
            Name = "Fire";
            AbilityType = AbilityTypes.Active;
            Description = "Fire weapon at selected Enemy";
            Icon = Resources.Load<Sprite>("Textures/Abilities/Icon_Target");
            TargetingType = TargetingTypes.EnemyFire;
            SpecialTarget = SpecialTargeting.None;
        }
        
        public override void Execute() {
            var selectedUnit = Unit.ActiveUnit;
            var targetUnit = selectedUnit.targetUnit;
            selectedUnit.LookAtGameObject(targetUnit.gameObject);
            var hitSuccess = Random.Range(0.01F, 1.00F);
            var damageDone = 0;
            var successfulHit = hitSuccess < CombatHitCalc.CalculateHitChance(selectedUnit, targetUnit, this)["HIT"];
            if (successfulHit)
                damageDone = CombatDamageCalc.DamageCalculation(selectedUnit, targetUnit, this)["DAMAGE"];
            selectedUnit.GetComponent<UnitAudio>().Play(selectedUnit.weapon.FireSound);
            selectedUnit.FireBullets(5);
            targetUnit.PopUpText(successfulHit ? damageDone.ToString() : "MISS");
            targetUnit.TakeDamage(damageDone);
            Object.FindObjectOfType<GameManager>().SendAbility(Name, selectedUnit, targetUnit, null);
            Object.FindObjectOfType<GameManager>().SendDamage(targetUnit, damageDone);
            AbilityPause.StartPause(1.5F, this, "EndAbility");
        }

        public override void Targeting() { }

        public override void EndAbility() {
            Unit.ActiveUnit.EndTurn();
        }

        public override void ObservedExecute() {
            var selectedUnit = Unit.ActiveUnit;
            var targetUnit = selectedUnit.targetUnit;
            selectedUnit.LookAtGameObject(targetUnit.gameObject);
            selectedUnit.GetComponent<UnitAudio>().Play(selectedUnit.weapon.FireSound);
            selectedUnit.FireBullets(5);
        }
    }
}
