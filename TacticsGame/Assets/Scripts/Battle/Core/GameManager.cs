using TacticsGame.Battle.Map.UI;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Battle.Core
{
    public class GameManager : MonoBehaviour {
        
        public int CurrentRound { get; set; }
        public int CurrentTurn { get; set; }
        public Unit currentUnit;
        
        public enum ControlState {
            Movement
        }

        public ControlState playerControlState;

        private void Start() {
            StartUnitTurn(Unit.All[0]);
        }

        private void StartUnitTurn(Unit unit) {
            currentUnit = unit;
            MovementUI.DrawMovementUI(unit);
        }
    }
}
