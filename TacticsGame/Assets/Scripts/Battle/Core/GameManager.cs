using System;
using System.Collections;
using System.Linq;
using Photon.Pun;
using TacticsGame.Battle.Map;
using TacticsGame.Battle.UI;
using TacticsGame.Battle.Units;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TacticsGame.Battle.Core {
    public class GameManager : MonoBehaviour {
        public int gangNumber;
        public string opponent;
        public bool isMaster;
        
        public int currentRound;
        public int currentGangTurn;
        public string[] playerNames;
        public int[] activeUnits = { 0, 0 };
        private float _timer = 60;
        private bool _countdown = true;

        private InfoBar _infoBar;
        private static PlayerControls _playerControls;
        private PhotonView _photonView;
        private PlayerGang _playerGang;
        private FogOfWar _fogOfWar;

        public event EventHandler OnNewRound;

        private void Awake() {
            _infoBar = FindObjectOfType<InfoBar>();
            _playerControls = GameObject.Find("GameManager").GetComponent<PlayerControls>();
            _photonView = GetComponent<PhotonView>();
            _playerGang = GetComponent<PlayerGang>();
            _fogOfWar = FindObjectOfType<FogOfWar>();
        }

        public void SubscribeToTab() {
            _playerControls.OnTabPress += SwitchUnit;
        }
        
        public void UnsubscribeToTab() {
            _playerControls.OnTabPress -= SwitchUnit;
        }

        private void StartBattle() {
            _infoBar.SetPlayerNames(new[]{ PhotonNetwork.NickName, PhotonNetwork.PlayerListOthers[0].NickName });
            currentRound = 1;
            RefreshActiveUnitNumbers();
            _fogOfWar.DrawAllFogOfWar();
            if (isMaster) {
                currentGangTurn = Random.Range(0, 2);
                SendGangTurn(currentGangTurn);
            }
        }

        public void StartUnitTurn(Unit unit) {
            _infoBar.SetPlayerTurn(currentRound, playerNames[currentGangTurn]);
            Unit.ActiveUnit = unit;
            ResetTimer();
            MoveGrid.ResetMovedGrid();
            unit.StartTurn();
        }

        [PunRPC]
        public void EndUnitTurn(int unitId) {
            MoveGrid.ResetMovedGrid();
            Unit.GetUnitById(unitId).turnTaken = true;
            RefreshActiveUnitNumbers();
            if ((activeUnits[currentGangTurn] + activeUnits[NextGangTurn()]) == 0) NextRound();
            else {
                if (activeUnits[NextGangTurn()] > 0) currentGangTurn = NextGangTurn();
                if (currentGangTurn == gangNumber) StartUnitTurn(FindNextUnit());
            }
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
            OnNewRound?.Invoke(this, EventArgs.Empty);
            currentGangTurn = NextGangTurn();
            _infoBar.SetPlayerTurn(currentRound, playerNames[currentGangTurn]);
            if (currentGangTurn == gangNumber) StartUnitTurn(FindNextUnit());
        }

        private void ResetTimer() {
            _timer = 60;
            StartTimer();
        }

        public void StartTimer() => _countdown = true;
        public void StopTimer() => _countdown = false;

        private void Update() {
            if (_countdown) {
                _timer -= Time.deltaTime;
                _infoBar.UpdateTimer(_timer);
                if (_timer <= 0) Unit.ActiveUnit.EndTurn();
            }
            
        }
        
        [PunRPC]
        private void SendGangTurn(int gangTurn) {
            _photonView.RPC("GetGangTurn", RpcTarget.All, gangTurn);
        }

        [PunRPC]
        public void SendEndUnitTurn(int unitId) {
            _photonView.RPC("EndUnitTurn", RpcTarget.All, unitId);
        }
        
        [PunRPC]
        private void GetGangTurn(int gangTurn) {
            StartCoroutine(Pause(gangTurn));
        }

        private IEnumerator Pause(int gangTurn) {
            yield return new WaitForSeconds(0.1F);
            currentGangTurn = gangTurn;
            _playerGang.UnitVisibilityUpdate();
            _fogOfWar.UpdateFogOfWar();
            if (gangTurn == gangNumber) StartUnitTurn(FindNextUnit());
        }
        
        [PunRPC]
        public void GetGangNumber(int receivedGangNumber) {
            gangNumber = receivedGangNumber;
            _playerGang.SetGangNumber(receivedGangNumber);
            _photonView.RPC("BattleReady", RpcTarget.All);
        }
        
        [PunRPC]
        private void BattleReady() {
            StartBattle();
        }

        [PunRPC]
        public void SendUnitMove(Unit unit, MapTile moveTile) {
            var unitId = unit.unitId;
            var moveTileXy = new int[] { moveTile.MapPosX, moveTile.MapPosZ };
            _photonView.RPC("GetUnitMove", RpcTarget.Others, unitId, moveTileXy);
        }

        [PunRPC]
        public void GetUnitMove(int unitId, int[] moveTileXy) {
            var moveUnit = Unit.GetUnitById(unitId);
            var moveTile = MapTile.GetMapTileFromPos(moveTileXy[0], moveTileXy[1]);
            var rTiles = MoveRoute.GetMoveRoute(moveUnit.GetCurrentMapTile(), moveTile);
            Unit.GetUnitById(unitId).MoveUnit(rTiles);
        }

        [PunRPC]
        public void SendAbility(string abilityName, Unit unit, Unit targetUnit, MapTile targetTile) {
            var unitId = unit.unitId;
            var targetUnitId = -1;
            if (targetUnit != null) targetUnitId = targetUnit.unitId;
            var targetTileXy = new [] { -1, -1 };
            if (targetTile != null) {
                targetTileXy[0] = targetTile.MapPosX;
                targetTileXy[1] = targetTile.MapPosZ;
            }
            _photonView.RPC("GetAbility", RpcTarget.Others, abilityName, unitId, targetUnitId, targetTileXy);
        }

        [PunRPC]
        public void GetAbility(string abilityName, int unitId, int targetUnitId, int[] targetTileXy) {
            var unit = Unit.GetUnitById(unitId);
            Unit.ActiveUnit = unit;
            unit.selectedAbility = unit.abilities.First(ability => ability.Name == abilityName);
            if (targetUnitId > -1) unit.targetUnit = Unit.GetUnitById(targetUnitId);
            unit.selectedAbility.ObservedExecute();
        }
        
        [PunRPC]
        public void SendDamage(Unit targetUnit, int damage) {
            var unitId = targetUnit.unitId;
            _photonView.RPC("GetDamage", RpcTarget.Others, unitId, damage);
        }
        
        [PunRPC]
        public void GetDamage(int targetUnitId, int damage) {
            var unit = Unit.GetUnitById(targetUnitId);
            unit.PopUpText(damage > 0 ? damage.ToString() : "MISS");
            unit.TakeDamage(damage);
        }
    }
}
