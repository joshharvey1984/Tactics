using TacticsGame.Battle.Core;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Data.AbilityBehaviours {
    public class CalculateToHit : AbilityBehaviour
    {
        public bool Success { get; set; }
        public override void Execute() {
            var hitCalc = CombatHitCalc.CalculateHitChance(Unit.SelectedUnit, Unit.SelectedUnit.targetUnit);
            Success = Random.Range(1, 100) < hitCalc["HIT"];
        }
        
        
    }
}
