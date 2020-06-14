using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TacticsGame.Battle.Map
{
    public static class MoveGrid
    {
        public static List<MapTile> MoveGridTiles(MapTile startTile, int numMoves)
        {
            var moveTiles = new List<MapTile> {startTile};
            for (var i = 0; i < numMoves; i++) {
                var addTiles = new HashSet<MapTile>();
                foreach (var mapTile in moveTiles) {
                   addTiles.UnionWith(ValidateMoveTiles(mapTile.GetNSEWTiles().Values, i));
                }

                foreach (var addTile in addTiles.Where(addTile => !moveTiles.Contains(addTile))) {
                    moveTiles.Add(addTile);
                }
                
            }

            return moveTiles;
        }

        private static IEnumerable<MapTile> ValidateMoveTiles(IEnumerable<MapTile> mapTiles, int moveNum)
        {
            var returnTiles = new HashSet<MapTile>();
            foreach (var mapTile in mapTiles)
            {
                if (mapTile == null || !mapTile.CanMoveInto()) continue;
                if (mapTile.MoveNum == -1) mapTile.MoveNum = moveNum + 1;
                returnTiles.Add(mapTile);
            }
            
            return returnTiles;
        }

        public static void ResetMovedGrid() {
            foreach (var mapTile in MapTile.All) mapTile.MoveNum = -1;
        }
    }
}
