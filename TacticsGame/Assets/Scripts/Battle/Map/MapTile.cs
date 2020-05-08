using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        public IEnumerable<MapTile> GetAdjacentTiles() => 
            All.Where(mapTile => mapTile._mapPosX >= _mapPosX - 1 && mapTile._mapPosX <= _mapPosX + 1)
                .Where(mapTile => mapTile._mapPosZ >= _mapPosZ - 1 && mapTile._mapPosZ <= _mapPosZ + 1)
                .ToArray();

        public string GetTileName() => $"{_mapPosX} - {_mapPosZ}";
        
        public Vector3 GetWorldPosition() => new Vector3(_mapPosX, 0, _mapPosZ);
        
        public static MapTile GetTile(int posX, int posZ) =>
            All.FirstOrDefault(mapTile => mapTile._mapPosX == posX && mapTile._mapPosZ == posZ);

        public Dictionary<string, MapTile> GetNeswTiles()
        {
            var returnDict = new Dictionary<string, MapTile>
            {
                {"North", GetTile(_mapPosX + 1, _mapPosZ)},
                {"South", GetTile(_mapPosX - 1, _mapPosZ)},
                {"East", GetTile(_mapPosX, _mapPosZ - 1)},
                {"West", GetTile(_mapPosX, _mapPosZ + 1)}
            };
            return returnDict;
        }

    }
}