using System.Collections.Generic;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Units;
using UnityEngine;
using UnityEngine.UI;

namespace TacticsGame.Battle.UI {
    public class DamagePanel : MonoBehaviour
    {
        [SerializeField] private Text damageValue;
        [SerializeField] private List<Text> damageModifiers;
        [SerializeField] private List<Text> damageModValues;

        public void ActivatePanel() {
            foreach (var modText in damageModifiers) modText.text = "";
            foreach (var modValue in damageModValues) modValue.text = "";
            
            var attackingUnit = Unit.SelectedUnit;
            var combatCalc = CombatDamageCalc.DamageCalculation(attackingUnit, attackingUnit.targetUnit,
                attackingUnit.selectedAbility);
            damageValue.text = combatCalc["DAMAGE"].ToString();
            var textCount = 0;
            foreach (var kv in combatCalc)
            {
                if (kv.Key == "DAMAGE") continue;
                if (kv.Value == 0.0F) continue;
                damageModifiers[textCount].text = kv.Key;
                damageModValues[textCount].text = kv.Value.ToString();
                damageModValues[textCount].color = kv.Value > 0 ? Color.green : Color.red;
                textCount++;
            }
            gameObject.SetActive(true);
        }
        
        public void DeactivatePanel() => gameObject.SetActive(false);
    }
}
