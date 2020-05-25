using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using TacticsGame.Battle.Map.Enums;
using static TacticsGame.Battle.Map.Enums.CoverType;
using static TacticsGame.Battle.Map.Enums.Direction;

namespace TacticsGame.Battle.Map
{
    public class MapTile
    {
        public static readonly HashSet<MapTile> All = new HashSet<MapTile>();
        public static MapTile MouseHoverTile { get; set; }
        public static MapTile SelectedTile { get; set; }
        
        public int MapPosX { get; }
        public int MapPosZ { get; }
        public Dictionary<Direction, CoverType> Cover { get; set; }
        public Dictionary<Direction, bool> MoveBlocked { get; set; }

        public GameObject UiTile { get; set; }
        public int MoveNum { get; set; } = -1;
        public Prop TileProp { get; set; }
        
        public List<GameObject> CoverIcons = new List<GameObject>();

        public MapTile(int mapPosX, int mapPosZ)
        {
            MapPosX = mapPosX;
            MapPosZ = mapPosZ;
            Cover = new Dictionary<Direction, CoverType>()
            {
                {North, NoCover},
                {South, NoCover},
                {East, NoCover},
                {West, NoCover}
            };
            
            MoveBlocked = new Dictionary<Direction, bool>()
            {
                {North, false},
                {South, false},
                {East, false},
                {West, false}
            };
            All.Add(this);
        }
        
        public static MapTile GetMapTileFromUi(GameObject hoverTile) => All.First(tile => tile.UiTile == hoverTile);
        
        public static MapTile GetMapTileFromPos(int posX, int posZ) =>
            All.SingleOrDefault(mapTile => mapTile.MapPosX == posX && mapTile.MapPosZ == posZ);

        public IEnumerable<MapTile> GetAdjacentTiles() => 
            All.Where(mapTile => mapTile.MapPosX >= MapPosX - 1 && mapTile.MapPosX <= MapPosX + 1)
                .Where(mapTile => mapTile.MapPosZ >= MapPosZ - 1 && mapTile.MapPosZ <= MapPosZ + 1)
                .ToArray();

        public string GetTileName() => $"{MapPosX} - {MapPosZ}";
        
        public Vector3 GetWorldPosition() => new Vector3(MapPosX, 0, MapPosZ);
        
        public Dictionary<Direction, MapTile> GetNSEWTiles()
        {
            return new Dictionary<Direction, MapTile>
            {
                {North, GetMapTileFromPos(MapPosX + 1, MapPosZ)},
                {South, GetMapTileFromPos(MapPosX - 1, MapPosZ)},
                {East, GetMapTileFromPos(MapPosX, MapPosZ - 1)},
                {West, GetMapTileFromPos(MapPosX, MapPosZ + 1)}
            };
        }

        public bool CanMoveInto()
        {
            if (TileProp) return false;
            return true;
        }

        public void SetBlocks(Direction direction, CoverType coverType, bool moveBlocker)
        {
            if (TileProp) return;
            var reverseDirection = DirectionExtensions.Reverse(direction);
            Cover[reverseDirection] = coverType;
            MoveBlocked[reverseDirection] = moveBlocker;
        }
    }
}