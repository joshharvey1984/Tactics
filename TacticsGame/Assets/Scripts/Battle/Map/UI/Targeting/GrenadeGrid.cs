using System.Collections.Generic;
using System.Linq;
using TacticsGame.Battle.Map.Enums;

namespace TacticsGame.Battle.Map.UI.Targeting {
    public static class GrenadeGrid {
        public static List<MapTile> GridTiles(MapTile startTile, int areaOfEffect) {
            var gridTiles = new List<MapTile> {startTile};
            for (var i = 0; i < areaOfEffect; i++) {
                var addTiles = new HashSet<MapTile>();
                foreach (var mapTile in gridTiles) {
                    addTiles.UnionWith(ValidateBlastTiles(mapTile.GetAdjacentTiles(true)));
                }

                foreach (var addTile in addTiles.Where(addTile => !gridTiles.Contains(addTile))) {
                    gridTiles.Add(addTile);
                }
            }

            return gridTiles;
        }

        private static IEnumerable<MapTile> ValidateBlastTiles(Dictionary<Direction, MapTile> mapTiles) {
            var returnTiles = new HashSet<MapTile>();
            foreach (var mapTile in mapTiles) {
                if (mapTile.Value == null) continue;
                returnTiles.Add(mapTile.Value);
            }

            return returnTiles;
        }
    }
}