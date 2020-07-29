using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TacticsGame.Battle.Map.Enums;
using TacticsGame.Battle.Units;
using static TacticsGame.Battle.Map.Enums.CoverType;
using static TacticsGame.Battle.Map.Enums.Direction;
using Object = UnityEngine.Object;

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

        private MapTile GetAdjacentTile(Direction direction) {
            if (direction == North) return GetMapTileFromPos(MapPosX + 1, MapPosZ);
            if (direction == South) return GetMapTileFromPos(MapPosX - 1, MapPosZ);
            if (direction == East) return GetMapTileFromPos(MapPosX, MapPosZ - 1);
            if (direction == West) return GetMapTileFromPos(MapPosX, MapPosZ + 1);
            if (direction == SouthWest) return GetMapTileFromPos(MapPosX - 1, MapPosZ + 1);
            if (direction == SouthEast) return GetMapTileFromPos(MapPosX - 1, MapPosZ - 1);
            if (direction == NorthWest) return GetMapTileFromPos(MapPosX + 1, MapPosZ + 1);
            if (direction == NorthEast) return GetMapTileFromPos(MapPosX + 1, MapPosZ - 1);
            return null;
        }

        public Dictionary<Direction, MapTile> GetAdjacentTiles(bool diagonals) {
            var returnDir = new Dictionary<Direction, MapTile> {
                {North, GetAdjacentTile(North)},
                {South, GetAdjacentTile(South)},
                {East, GetAdjacentTile(East)},
                {West, GetAdjacentTile(West)}
            };
            if (diagonals) {
                returnDir.Add(NorthEast, GetAdjacentTile(NorthEast));
                returnDir.Add(NorthWest, GetAdjacentTile(NorthWest));
                returnDir.Add(SouthEast, GetAdjacentTile(SouthEast));
                returnDir.Add(SouthWest, GetAdjacentTile(SouthWest));
            }

            return returnDir;
        }

        public Dictionary<Direction, MapTile> GetMovableAdjacentTiles(bool diagonals = false) {
            var adjacentTiles = GetAdjacentTiles(diagonals);
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
        
        public bool CanSeeOtherTile(MapTile mapTile, float distance, bool cornerPeak = false, bool cornerSpot = false) {
            var startPositions = new List<Vector3> { UiTile.transform.position };
            if (cornerPeak)
                startPositions.AddRange(CornerPeakFrom().Select(tile => tile.UiTile.transform.position));
            
            var endPositions = new List<Vector3> { mapTile.GetWorldPosition() };
            if (cornerSpot)
                endPositions.AddRange(mapTile.CornerPeakFrom().Select(tile => tile.UiTile.transform.position));
            
            foreach (var startPosition in startPositions) {
                foreach (var endPosition in endPositions) {
                    var endPos = endPosition;
                    endPos.y += 0.8F;
                    var losPos = startPosition;
                    losPos.y += 0.8F;
                    if (Vector3.Distance(losPos, endPos) > distance) continue;
                    if (Physics.Linecast(losPos, endPos, out var hit)) {
                        if (hit.collider.CompareTag($"LOS")) continue;
                    }

                    return true;
                }
            }

            return false;
        }

        public CoverType OfferingCoverFrom(MapTile mapTile) {
            var bestCover = mapTile.MapPosX > MapPosX ? Cover[North] : Cover[South];
            if (mapTile.MapPosZ > MapPosZ) {
                if (Cover[West] > bestCover) bestCover = Cover[West];
            }
            else if (Cover[East] > bestCover) bestCover = Cover[East];

            return bestCover;
        }

        public List<MapTile> CornerPeakFrom() {
            var peakList = new List<MapTile>();

            foreach (var cardinal in DirectionExtensions.GetCardinals()) {
                foreach (var rightAngleDirection in DirectionExtensions.GetRightAngleDirections(cardinal)) {
                    if (MoveBlocked[cardinal] && !GetAdjacentTile(rightAngleDirection).MoveBlocked[cardinal]) {
                        peakList.Add(GetAdjacentTile(DirectionExtensions
                            .GetDiagonalFromCardinals(cardinal, rightAngleDirection)));
                    }
                }
            }

            return peakList;
        }

        public void HighlightTile(Color color = default) {
            UiTile.GetComponent<MeshRenderer>().material.color = color;
            UiTile.GetComponent<MeshRenderer>().enabled = true;
        }
        public void UnhighlightTile() => UiTile.GetComponent<MeshRenderer>().enabled = false;
        public static void HighlightMultipleTiles(IEnumerable<MapTile> mapTiles, Color color) {
            foreach (var mapTile in mapTiles) mapTile.HighlightTile(color);
        }
        public static void UnhighlightAllTiles() {
            foreach (var mapTile in All) mapTile.UnhighlightTile();
        }

        public void CreateProp(GameObject prop) {
            var newProp = Object.Instantiate(prop, GetWorldPosition(), Quaternion.identity);
            newProp.transform.Rotate(new Vector3(-90, 0));
        }

        public Unit GetUnitInTile() => Unit.All.FirstOrDefault(unit => unit.GetCurrentMapTile() == this);
    }
}