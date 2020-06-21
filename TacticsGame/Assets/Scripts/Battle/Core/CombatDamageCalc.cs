using System.Collections.Generic;
using TacticsGame.Battle.Units;
using TacticsGame.Data;
using UnityEngine;

namespace TacticsGame.Battle.Core {
    public class CombatDamageCalc : MonoBehaviour
    {
        
        private static Unit _attackingUnit;
        private static Unit _defendingUnit;
        
        public static Dictionary<string, int> DamageCalculation(Unit attackingUnit, Unit defendingUnit)
        {
            _attackingUnit = attackingUnit;
            _defendingUnit = defendingUnit;
            var baseDamage = _attackingUnit.weapon.damage;
            var armourReduction = _defendingUnit.armour.damageReduction;
            //var flushOut = 0;
            //if (ability.name == "Flush Out") flushOut = ability.damageModifer;
            var totalDamage = baseDamage - armourReduction;
            return new Dictionary<string, int>
            {
                {"Base Damage", baseDamage}, 
                {"Armour Reduction", 0 - armourReduction}, 
                //{"Flush Out", flushOut},
                {"DAMAGE", totalDamage}
            };
        }
    }
}
