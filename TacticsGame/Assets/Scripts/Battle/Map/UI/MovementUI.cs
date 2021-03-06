﻿using System;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Battle.Map.UI {
    public static class MovementUI {
        private static readonly MouseHoverTile MouseHoverTile = GameObject.Find("GameManager").GetComponent<MouseHoverTile>();
        private static readonly PlayerControls PlayerControl = GameObject.Find("GameManager").GetComponent<PlayerControls>();
        private static readonly CoverIcons CoverIcons = GameObject.Find("Map").GetComponent<CoverIcons>();
        private static readonly GameManager GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        private static MapTile _hoverTile;
        private static Unit _currentUnit;

        public static void DrawMovementUI(Unit unit) {
            _currentUnit = unit;
            var moveTiles = MoveGrid.MoveGridTiles(unit.GetCurrentMapTile(), unit.movePoints);
            MoveGridLine.DrawMoveGrid(moveTiles);
            SubscribeToHoverTile();
        }

        public static void DestroyMovementUI() {
            UnsubscribeToHoverTile();
            RouteLine.DestroyMoveLine();
            MoveGridLine.DestroyMoveGrid();
            CoverIcons.DestroyAll();
        }
        
        private static void SubscribeToHoverTile() {
            MouseHoverTile.OnHoverTileChanged += UpdateUi;
            CoverIcons.SubscribeToHoverTile();
            PlayerControl.OnRightClick += MoveTileSelected;
            PlayerControl.OnLeftClick += SwitchActiveUnit;
        }

        private static void UnsubscribeToHoverTile() {
            MouseHoverTile.OnHoverTileChanged -= UpdateUi;
            CoverIcons.UnsubscribeToHoverTile();
            PlayerControl.OnRightClick -= MoveTileSelected;
            PlayerControl.OnLeftClick -= SwitchActiveUnit;
        }
        
        private static void UpdateUi(object sender, MouseHoverTile.OnHoverTileChangedArgs args) {
            if (args.HoverTileUi == null) {
                RouteLine.DestroyMoveLine();
                _hoverTile = null;
                return;
            }
            _hoverTile = MapTile.GetMapTileFromUi(args.HoverTileUi);
            UpdateMovementUi();
        }

        private static void UpdateMovementUi() {
            if (_hoverTile.MoveNum > 0 && !_hoverTile.TileProp) {
                var rTiles = MoveRoute.GetMoveRoute(_currentUnit.GetCurrentMapTile(), _hoverTile);
                RouteLine.DrawMoveLine(rTiles);
            }
            else {
                RouteLine.DestroyMoveLine();
            }
        }

        private static void MoveTileSelected(object sender, EventArgs args) {
            if (_hoverTile == null || _hoverTile.MoveNum < 1) return;
            _currentUnit.moveTaken = true;
            DestroyMovementUI();
            var rTiles = MoveRoute.GetMoveRoute(_currentUnit.GetCurrentMapTile(), _hoverTile);
            _currentUnit.MoveUnit(rTiles);
            GameManager.SendUnitMove(_currentUnit, _hoverTile);
        }
        
        private static void SwitchActiveUnit(object sender, EventArgs args) {
            if (_hoverTile == null) return;
            if (_hoverTile.GetUnitInTile() == null) return;
            if (_hoverTile.GetUnitInTile() == Unit.ActiveUnit) return;
            if (_hoverTile.GetUnitInTile().gang == GameManager.currentGangTurn && !_hoverTile.GetUnitInTile().turnTaken)
                GameManager.StartUnitTurn(_hoverTile.GetUnitInTile());
        }
    }
}