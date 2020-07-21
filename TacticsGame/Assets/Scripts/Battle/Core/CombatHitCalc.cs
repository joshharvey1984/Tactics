using System;
using System.Collections.Generic;
using TacticsGame.Battle.Units;
using TacticsGame.Data;
using static TacticsGame.Battle.Map.Enums.CoverType;

namespace TacticsGame.Battle.Core {
    public static class CombatHitCalc {
        private static Unit _attackingUnit;
        private static Unit _defendingUnit;
        private static Ability _ability;

        public static Dictionary<string, float> CalculateHitChance(Unit attUnit, Unit defUnit, Ability ability) {
            _attackingUnit = attUnit;
            _defendingUnit = defUnit;
            _ability = ability;

            var hitModifiers = new List<CombatHitModifier>();

            var basicHit = (Convert.ToSingle(_attackingUnit.aim) / Convert.ToSingle(_defendingUnit.defence)) / 2.5F;
            hitModifiers.Add(new CombatHitModifier {
                Name = "Basic Hit", Description = "Aim vs Defence", ModifierValue = basicHit
            });
            
            var coverBonus = CoverBonus();
            hitModifiers.Add(new CombatHitModifier {
                Name = "Cover Bonus", Description = "Cover Bonus", ModifierValue = coverBonus
            });
            //var aimTimeBonus = (Convert.ToSingle(attackingUnit.movePoints) * 2) / 100;
            
            foreach (var statusEffect in defUnit.currentStatusEffects) hitModifiers.AddRange(statusEffect.CombatHitModifiers);
            foreach (var statusEffect in attUnit.currentStatusEffects) hitModifiers.AddRange(statusEffect.CombatHitModifiers);
            if (_ability.CombatHitModifiers != null) hitModifiers.AddRange(_ability.CombatHitModifiers);
            
            var toHit = 0.0F;
            var returnDict = new Dictionary<string, float>();
            foreach (var combatHitModifier in hitModifiers) {
                returnDict.Add(combatHitModifier.Description, combatHitModifier.ModifierValue);
                toHit += combatHitModifier.ModifierValue;
            }
            
            returnDict.Add("HIT", toHit);
            return returnDict;
        }
        
        private static float CoverBonus() {
            if (_defendingUnit.GetCurrentMapTile().OfferingCoverFrom(_attackingUnit.GetCurrentMapTile()) ==
                HalfCover) return -0.15F;
            if (_defendingUnit.GetCurrentMapTile().OfferingCoverFrom(_attackingUnit.GetCurrentMapTile()) ==
                FullCover) return -0.3F;
            return 0;
        }
    }

    public struct CombatHitModifier {
        public string Name { get; set; }
        public string Description { get; set; }
        public float ModifierValue { get; set; }
    }
}