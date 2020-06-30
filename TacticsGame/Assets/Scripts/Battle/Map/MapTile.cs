using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TacticsGame.Battle.Map.Enums;
using TacticsGame.Battle.Units;
using static TacticsGame.Battle.Map.Enums.CoverType;
using static TacticsGame.Battle.Map.Enums.Direction;

namespace TacticsGame.Battle.Map {
    public class MapTile {
        public static readonly HashSet<MapTile> All = new HashSet<MapTile>();

        public int MapPosX { get; }
        public int MapPosZ { get; }
        public Dictionary<Direction, CoverType> Cover { get; set; }
        public Dictionary<Direction, bool> MoveBlocked { get; set; }

        public GameObject UiTile { get; set; }
        public int MoveNum { get; set; } = -1;
        public Prop TileProp { get; set; }

        public List<GameObject> CoverIcons = new List<GameObject>();

        public MapTile(int mapPosX, int mapPosZ) {
            MapPosX = mapPosX;
            MapPosZ = mapPosZ;
            Cover = new Dictionary<Direction, CoverType>() {
                {North, NoCover},
                {South, NoCover},
                {East, NoCover},
                {West, NoCover}
            };

            MoveBlocked = new Dictionary<Direction, bool>() {
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

        public static MapTile GetMapTileFromPos(float posX, float posZ) {
            var convertedPosX = Convert.ToInt32(posX);
            var convertedPosZ = Convert.ToInt32(posZ);
            return GetMapTileFromPos(convertedPosX, convertedPosZ);
        }

        public string GetTileName() => $"{MapPosX} - {MapPosZ}";
        public Vector3 GetWorldPosition() => new Vector3(MapPosX, 0, MapPosZ);

        public Dictionary<Direction, MapTile> GetAdjacentTiles(bool diagonals) {
            var returnDir = new Dictionary<Direction, MapTile> {
                {North, GetMapTileFromPos(MapPosX + 1, MapPosZ)},
                {South, GetMapTileFromPos(MapPosX - 1, MapPosZ)},
                {East, GetMapTileFromPos(MapPosX, MapPosZ - 1)},
                {West, GetMapTileFromPos(MapPosX, MapPosZ + 1)}
            };
            if (diagonals) {
                returnDir.Add(NorthEast, GetMapTileFromPos(MapPosX + 1, MapPosZ - 1));
                returnDir.Add(NorthWest, GetMapTileFromPos(MapPosX + 1, MapPosZ + 1));
                returnDir.Add(SouthEast, GetMapTileFromPos(MapPosX - 1, MapPosZ - 1));
                returnDir.Add(SouthWest, GetMapTileFromPos(MapPosX - 1, MapPosZ + 1));
            }

            return returnDir;
        }

        public Dictionary<Direction, MapTile> GetMovableAdjacentTiles() {
            var adjacentTiles = GetAdjacentTiles(false);
            return adjacentTiles
                .Where(tile => !MoveBlocked[tile.Key])
                .ToDictionary(tile => tile.Key, tile => tile.Value);
        }

        public bool CanMoveIntoFrom(Direction direction) {
            if (TileProp) return false;
            foreach (var unit in Unit.All) {
                if (unit.GetCurrentMapTile() == this) return false;
            }
            return true;
        }

        public List<Direction> FindDirectionFromOtherTile(MapTile otherTile) {
            var returnList = new List<Direction>();
            if (otherTile.MapPosX > MapPosX) returnList.Add(South);
            if (otherTile.MapPosX < MapPosX) returnList.Add(North);
            if (otherTile.MapPosZ > MapPosZ) returnList.Add(East);
            if (otherTile.MapPosZ < MapPosZ) returnList.Add(West);
            return returnList;
        }
        
        public void SetBlocks(Direction direction, CoverType coverType, bool moveBlocker) {
            Cover[direction] = coverType;
            MoveBlocked[direction] = moveBlocker;
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
            var bestCover = mapTile.MapPosX > MapPosX ? Cover[North] : Cover[South];
            if (mapTile.MapPosZ > MapPosZ) {
                if (Cover[West] > bestCover) bestCover = Cover[West];
            }
            else if (Cover[East] > bestCover) bestCover = Cover[East];

            return bestCover;
        }
    }
}