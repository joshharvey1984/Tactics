using TacticsGame.Battle.Core;
using UnityEngine;

namespace TacticsGame.Battle.Map.UI {
    public static class MovementUI {
        private static readonly GameManager GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        private static readonly MouseHoverTile MouseHoverTile = GameObject.Find("GameManager").GetComponent<MouseHoverTile>();
        private static readonly CoverIcons CoverIcons = GameObject.Find("Map").GetComponent<CoverIcons>();
        
        public static void DrawMovementUI(Unit unit) {
            var moveTiles = MoveGrid.MoveGridTiles(unit.currentTile, unit.movePoints);
            MoveGridLine.DrawMoveGrid(moveTiles);
            SubscribeToHoverTile();
        }
        
        private static void SubscribeToHoverTile() {
            MouseHoverTile.OnHoverTileChanged += UpdateUi;
            CoverIcons.SubscribeToHoverTile();
        }

        private static void UnsubscribeToHoverTile() {
            MouseHoverTile.OnHoverTileChanged -= UpdateUi;
            CoverIcons.UnsubscribeToHoverTile();
        }
        
        private static void UpdateUi(object sender, MouseHoverTile.OnHoverTileChangedArgs args) {
            if (args.HoverTileUi == null) {
                RouteLine.DestroyMoveLine();
                return;
            }
            UpdateMovementUi(MapTile.GetMapTileFromUi(args.HoverTileUi));
        }

        private static void UpdateMovementUi(MapTile hoverTile) {
            if (hoverTile.MoveNum > 0) {
                var rTiles = MoveRoute.GetMoveRoute(GameManager.currentUnit.currentTile, hoverTile);
                RouteLine.DrawMoveLine(rTiles);
            }
            else {
                RouteLine.DestroyMoveLine();
            }
        }
    }
}