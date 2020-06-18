using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TacticsGame.Battle.Map.Enums;
using TacticsGame.Battle.Units;
using static TacticsGame.Battle.Map.Enums.CoverType;
using static TacticsGame.Battle.Map.Enums.Direction;

namespace TacticsGame.Battle.Map
{
    public class MapTile
    {
        public static readonly HashSet<MapTile> All = new HashSet<MapTile>();

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

        public bool CanMoveInto() => !TileProp && Unit.All.All(unit => unit.GetCurrentMapTile() != this);

        public void SetBlocks(Direction direction, CoverType coverType, bool moveBlocker)
        {
            if (TileProp) return;
            var reverseDirection = DirectionExtensions.Reverse(direction);
            Cover[reverseDirection] = coverType;
            MoveBlocked[reverseDirection] = moveBlocker;
        }

        public bool CanSeeOtherTile(MapTile mapTile, float distance) {
            var startPos = UiTile.transform.position;
            var endPos = mapTile.UiTile.transform.position;
            startPos.y += 0.8F;
            endPos.y += 0.8F;
            
            if (Vector3.Distance(startPos, endPos) > distance) return false;
            if (Physics.Raycast(startPos, endPos - startPos, out var hit, distance)) {
                if (hit.collider.CompareTag($"LOS")) return false;
            }
            return true;
        }

        public CoverType OfferingCoverFrom(MapTile mapTile) {
            if (mapTile.MapPosX > MapPosX) return Cover[North];
            if (mapTile.MapPosX < MapPosX) return Cover[South];
            if (mapTile.MapPosZ > MapPosZ) return Cover[West];
            if (mapTile.MapPosZ < MapPosZ) return Cover[East];
            return NoCover;
        }
    }
}