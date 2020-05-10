using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TacticsGame.Battle.Map
{
    public class MapTile
    {
        private static readonly HashSet<MapTile> All = new HashSet<MapTile>();
        public static MapTile MouseHoverTile { get; set; } = null;
        
        public int MapPosX { get; }
        public int MapPosZ { get; }

        public GameObject UiTile { get; set; }
        public int MoveNum { get; set; } = -1;
        public GameObject Unit { get; set; }

        public MapTile(int mapPosX, int mapPosZ)
        {
            MapPosX = mapPosX;
            MapPosZ = mapPosZ;
            All.Add(this);
        }

        public IEnumerable<MapTile> GetAdjacentTiles() => 
            All.Where(mapTile => mapTile.MapPosX >= MapPosX - 1 && mapTile.MapPosX <= MapPosX + 1)
                .Where(mapTile => mapTile.MapPosZ >= MapPosZ - 1 && mapTile.MapPosZ <= MapPosZ + 1)
                .ToArray();

        public string GetTileName() => $"{MapPosX} - {MapPosZ}";
        
        public Vector3 GetWorldPosition() => new Vector3(MapPosX, 0, MapPosZ);
        
        public static MapTile GetTile(int posX, int posZ) =>
            All.FirstOrDefault(mapTile => mapTile.MapPosX == posX && mapTile.MapPosZ == posZ);

        public Dictionary<string, MapTile> GetNeswTiles()
        {
            var returnDict = new Dictionary<string, MapTile>
            {
                {"North", GetTile(MapPosX + 1, MapPosZ)},
                {"South", GetTile(MapPosX - 1, MapPosZ)},
                {"East", GetTile(MapPosX, MapPosZ - 1)},
                {"West", GetTile(MapPosX, MapPosZ + 1)}
            };
            return returnDict;
        }

        public bool CanMoveInto()
        {
            return !Unit;
        }

        public static MapTile GetMapTileFromUi(GameObject hoverTile) => All.First(tile => tile.UiTile == hoverTile);

    }
}