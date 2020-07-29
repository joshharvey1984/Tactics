using System;
using System.Linq;
using TacticsGame.Battle.Map;
using TacticsGame.Battle.UI;
using TacticsGame.Battle.Units;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TacticsGame.Battle.Core {
    public class GameManager : MonoBehaviour {
        public int currentRound;
        public int currentGangTurn;
        public string[] playerNames = {"Josh", "Dave"};
        public int[] activeUnits = { 0, 0 };
        private float _timer = 60;
        private bool _countdown = true;
        private bool _startBattle = false;

        private InfoBar _infoBar;
        private static PlayerControls _playerControls;

        private void Awake() {
            _infoBar = FindObjectOfType<InfoBar>();
            _infoBar.SetPlayerNames(playerNames);
            _playerControls = GameObject.Find("GameManager").GetComponent<PlayerControls>();
            SubscribeToTab();
        }

        public void SubscribeToTab() {
            _playerControls.OnTabPress += SwitchUnit;
        }
        
        public void UnsubscribeToTab() {
            _playerControls.OnTabPress -= SwitchUnit;
        }
        
        private void StartBattle() {
            currentRound = 1;
            currentGangTurn = Random.Range(0, 2);
            RefreshActiveUnitNumbers();
            StartUnitTurn(FindNextUnit());
        }

        public void StartUnitTurn(Unit unit) {
            _infoBar.SetPlayerTurn(currentRound, playerNames[currentGangTurn]);
            Unit.ActiveUnit = unit;
            ResetTimer();
            MoveGrid.ResetMovedGrid();
            unit.StartTurn();
        }

        public void EndUnitTurn() {
            MoveGrid.ResetMovedGrid();
            RefreshActiveUnitNumbers();
            if (activeUnits[NextGangTurn()] > 0) currentGangTurn = NextGangTurn();
            if (activeUnits[currentGangTurn] == 0) NextRound();
            StartUnitTurn(FindNextUnit());
        }

        private int NextGangTurn() => currentGangTurn == 0 ? 1 : 0;

        private Unit FindNextUnit() => Unit.All.FirstOrDefault(unit => unit.gang == currentGangTurn && !unit.turnTaken);

        private void RefreshActiveUnitNumbers() {
            activeUnits[0] = Unit.All.Count(unit => unit.gang == 0 && !unit.turnTaken);
            activeUnits[1] = Unit.All.Count(unit => unit.gang == 1 && !unit.turnTaken);
        }

        private void SwitchUnit(object sender, EventArgs args) {
            if (activeUnits[currentGangTurn] == 1) return;
            if (Unit.ActiveUnit.moveTaken) return;
            var currentUnits = Unit.All.Where(unit => unit.gang == currentGangTurn && !unit.turnTaken).ToList();
            var unitIndex = currentUnits.IndexOf(Unit.ActiveUnit);
            unitIndex++;
            if (unitIndex >= currentUnits.Count) unitIndex = 0;
            MoveGrid.ResetMovedGrid();
            StartUnitTurn(currentUnits[unitIndex]);
        }
        
        private void NextRound() {
            currentRound++;
            foreach (var unit in Unit.All) unit.turnTaken = false;
            RefreshActiveUnitNumbers();
            currentGangTurn = NextGangTurn();
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
