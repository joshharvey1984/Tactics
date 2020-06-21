using System;
using System.Collections.Generic;
using TacticsGame.Battle.Units;
using static TacticsGame.Battle.Map.Enums.CoverType;

namespace TacticsGame.Battle.Core {
    public static class CombatHitCalc {
        private static Unit _attackingUnit;
        private static Unit _defendingUnit;
        
        public static Dictionary<string, float> CalculateHitChance(Unit attackingUnit, Unit defendingUnit) {
            _attackingUnit = attackingUnit;
            _defendingUnit = defendingUnit;
            var basicHit = (Convert.ToSingle(_attackingUnit.aim) / Convert.ToSingle(_defendingUnit.defence)) / 2.5F;
            var coverBonus = CoverBonus();
            //var aimTimeBonus = (Convert.ToSingle(attackingUnit.movePoints) * 2) / 100;
            var hunkerDown = 0.2F;
            var toHit = basicHit + coverBonus;
            return new Dictionary<string, float>
            {
                {"Aim vs Defence", basicHit},
                //{"Weapon Range", rangeMod},
                {"Cover Bonus", 0 - coverBonus},
                //{"Aim Time Bonus", aimTimeBonus},
                {"Hunker Down", 0 - hunkerDown},
                //{"Flush Out", flushOut},
                {"HIT", toHit}
            };
        }
        
        private static float CoverBonus() {
            if (_defendingUnit.GetCurrentMapTile().OfferingCoverFrom(_attackingUnit.GetCurrentMapTile()) ==
                HalfCover) return -0.2F;
            if (_defendingUnit.GetCurrentMapTile().OfferingCoverFrom(_attackingUnit.GetCurrentMapTile()) ==
                FullCover) return -0.4F;
            return 0;
        }
    }
}
