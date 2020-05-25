using System.Collections.Generic;
using TacticsGame.Battle.Map;
using UnityEngine;

namespace TacticsGame.Battle.Units {
    public class Unit : MonoBehaviour
    {
        public static readonly List<Unit> All = new List<Unit>();

        private UnitMovement _unitMovement;
        private List<MapTile> _moveTiles;
        private int _moveNum;

        public MapTile currentTile;

        public int movePoints = 8;

        private void Awake() {
            All.Add(this);
            _unitMovement = GetComponent<UnitMovement>();
            _unitMovement.unit = this;
        }

        public void MoveUnit(List<MapTile> tiles) {
            _moveTiles = tiles;
            MoveNextTile();
        }

        public void MoveNextTile() {
            _moveNum++;
            if (_moveNum == _moveTiles.Count - 1) EndMoveTurn();
            else _unitMovement.SetNextTile(_moveTiles[_moveNum]);
        }

        private void EndMoveTurn() {
            _unitMovement.EndMove();
        }
    }
}
