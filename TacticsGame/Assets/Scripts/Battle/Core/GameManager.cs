using System.Linq;
using TacticsGame.Battle.Map;
using TacticsGame.Battle.Map.UI;
using TacticsGame.Battle.UI;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Battle.Core
{
    public class GameManager : MonoBehaviour {

        public int currentRound;
        public int currentGangTurn;

        public GameObject abilityPanel;
        public GameObject targetPanel;
        
        private void Start() {
            currentRound = 1;
            currentGangTurn = 0;
            StartUnitTurn(FindNextUnit());
        }

        private void StartUnitTurn(Unit unit) {
            Unit.SelectedUnit = unit;
            unit.StartTurn();
        }

        public void EndUnitTurn() {
            MoveGrid.ResetMovedGrid();
            Unit.SelectedUnit.turnTaken = true;
            currentGangTurn++;
            if (currentGangTurn > 1) currentGangTurn = 0;
            if (FindNextUnit() == null) {
                NextRound();
            }
            else {
                StartUnitTurn(FindNextUnit());
            }
        }

        private Unit FindNextUnit() => Unit.All.FirstOrDefault(unit => unit.gang == currentGangTurn && !unit.turnTaken);

        private void NextRound() {
            currentRound++;
            foreach (var unit in Unit.All) unit.turnTaken = false;
            StartUnitTurn(FindNextUnit());
        }
    }
}
