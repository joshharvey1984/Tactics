using System.Collections.Generic;
using System.Linq;
using TacticsGame.Battle.Map.Enums;
using TacticsGame.Battle.Map.UI;
using UnityEngine;

namespace TacticsGame.Battle.Map {
    public static class MoveGrid {
        public static List<MapTile> MoveGridTiles(MapTile startTile, int numMoves) {
            var moveTiles = new List<MapTile> {startTile};
            for (var i = 0; i < numMoves; i++) {
                var addTiles = new HashSet<MapTile>();
                foreach (var mapTile in moveTiles) {
                    addTiles.UnionWith(ValidateMoveTiles(mapTile.GetMovableAdjacentTiles(), i));
                }

                foreach (var addTile in addTiles.Where(addTile => !moveTiles.Contains(addTile))) {
                    moveTiles.Add(addTile);
                }
            }

            return moveTiles;
        }

        private static IEnumerable<MapTile> ValidateMoveTiles(Dictionary<Direction, MapTile> mapTiles, int moveNum) {
            var returnTiles = new HashSet<MapTile>();
            foreach (var mapTile in mapTiles) {
                if (mapTile.Value == null || !mapTile.Value.CanMoveIntoFrom(mapTile.Key)) continue;
                if (mapTile.Value.MoveNum == -1) mapTile.Value.MoveNum = moveNum + 1;
                returnTiles.Add(mapTile.Value);
            }

            return returnTiles;
        }

        public static void ResetMovedGrid() {
            foreach (var mapTile in MapTile.All) mapTile.MoveNum = -1;
            MovementUI.DestroyMovementUI();
        }
    }
}