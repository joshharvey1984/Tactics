using System.Linq;
using TacticsGame.Battle.Map;
using TacticsGame.Battle.UI;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Battle.Core {
    public class GameManager : MonoBehaviour {
        public int currentRound;
        public int currentGangTurn;
        public string[] playerNames = {"Josh", "Dave"};
        private float _timer = 60;
        private bool _countdown = true;
        private bool _startBattle = false;

        private InfoBar _infoBar;

        private void Awake() {
            _infoBar = FindObjectOfType<InfoBar>();
            _infoBar.SetPlayerNames(playerNames);
        }
        
        private void StartBattle() {
            currentRound = 1;
            currentGangTurn = 0;
            StartUnitTurn(FindNextUnit());
        }

        private void StartUnitTurn(Unit unit) {
            _infoBar.SetPlayerTurn(currentRound, playerNames[currentGangTurn]);
            Unit.ActiveUnit = unit;
            ResetTimer();
            unit.StartTurn();
        }

        public void EndUnitTurn() {
            MoveGrid.ResetMovedGrid();
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

        private void ResetTimer() {
            _timer = 60;
            StartTimer();
        }

        public void StartTimer() => _countdown = true;
        public void StopTimer() => _countdown = false;

        private void Update() {
            if (!_startBattle) {
                _startBattle = true;
                StartBattle();
            }
            
            if (_countdown) {
                _timer -= Time.deltaTime;
                _infoBar.UpdateTimer(_timer);
                if (_timer <= 0) Unit.ActiveUnit.EndTurn();
            }
            
        }
    }
}
