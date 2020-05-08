using System.Collections.Generic;

namespace TacticsGame.Battle.Map
{
    public static class MoveGrid
    {
        public static IEnumerable<MapTile> MoveGridTiles(MapTile startTile, int numMoves)
        {
            var moveTiles = new HashSet<MapTile> {startTile};
            for (var i = 0; i < numMoves; i++) {
                var addTiles = new HashSet<MapTile>();
                foreach (var mapTile in moveTiles) {
                    addTiles.UnionWith(ValidateMoveTiles(mapTile.GetNeswTiles().Values));
                }
                moveTiles.UnionWith(addTiles);
            }

            return moveTiles;
        }

        private static IEnumerable<MapTile> ValidateMoveTiles(IEnumerable<MapTile> mapTiles)
        {
            var returnTiles = new HashSet<MapTile>();
            foreach (var mapTile in mapTiles) {
                if (mapTile != null && !mapTile.MoveBlocked) returnTiles.Add(mapTile);
            }
            
            return returnTiles;
        } 
    }
}
