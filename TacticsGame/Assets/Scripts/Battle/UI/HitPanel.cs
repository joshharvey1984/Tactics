using System.Collections.Generic;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Units;
using UnityEngine;
using UnityEngine.UI;

namespace TacticsGame.Battle.UI {
    public class HitPanel : MonoBehaviour
    {
        [SerializeField] private Text toHitValue;
        [SerializeField] private List<Text> aimModifiers;
        [SerializeField] private List<Text> aimModValues;

        public void ActivatePanel() {
            foreach (var modText in aimModifiers) modText.text = "";
            foreach (var modValue in aimModValues) modValue.text = "";
            
            var attackingUnit = Unit.ActiveUnit;
            var combatCalc = CombatHitCalc.CalculateHitChance(attackingUnit, attackingUnit.targetUnit,
                attackingUnit.selectedAbility);
            toHitValue.text = combatCalc["HIT"] * 100 + "%";
            var textCount = 0;
            foreach (var kv in combatCalc)
            {
                if (kv.Key == "HIT") continue;
                if (kv.Value == 0.0F) continue;
                aimModifiers[textCount].text = kv.Key;
                aimModValues[textCount].text = kv.Value * 100 + "%";
                aimModValues[textCount].color = kv.Value > 0 ? Color.green : Color.red;
                textCount++;
            }
            gameObject.SetActive(true);
        }
        
        public void DeactivatePanel() => gameObject.SetActive(false);
    }
}
