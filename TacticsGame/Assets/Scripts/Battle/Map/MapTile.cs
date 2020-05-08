using System.Collections.Generic;
using System.Linq;

namespace TacticsGame.Battle.Map
{
    public class MapTile
    {
        private static readonly HashSet<MapTile> All = new HashSet<MapTile>();
        
        private readonly int _mapPosX;
        private readonly int _mapPosZ;
        
        public bool MoveBlocked { get; set; }

        public MapTile(int mapPosX, int mapPosZ)
        {
            _mapPosX = mapPosX;
            _mapPosZ = mapPosZ;
            All.Add(this);
        }

        public IEnumerable<MapTile> FindAdjacentTiles()
        {
            return All
                .Where(mapTile => mapTile._mapPosX >= _mapPosX - 1 && mapTile._mapPosX <= _mapPosX + 1)
                .Where(mapTile => mapTile._mapPosZ >= _mapPosZ - 1 && mapTile._mapPosZ <= _mapPosZ + 1)
                .ToArray();
        }

        public string TileName()
        {
            return $"{_mapPosX} - {_mapPosZ}";
        }
    }
}