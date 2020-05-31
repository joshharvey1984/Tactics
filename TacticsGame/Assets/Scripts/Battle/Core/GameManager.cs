using TacticsGame.Battle.Map.UI;
using TacticsGame.Battle.UI;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Battle.Core
{
    public class GameManager : MonoBehaviour {
        
        public int CurrentRound { get; set; }
        public int CurrentTurn { get; set; }

        public GameObject abilityPanel;
        
        private void Start() {
            StartUnitTurn(Unit.All[0]);
        }

        private void StartUnitTurn(Unit unit) {
            Unit.SelectedUnit = unit;
            MovementUI.DrawMovementUI(unit);
            abilityPanel.GetComponent<AbilityPanel>().CreateAbilityButtons();
        }
    }
}
