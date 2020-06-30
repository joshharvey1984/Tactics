using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TacticsGame.Battle.Map {
    public static class MoveRoute {
        public static List<MapTile> GetMoveRoute(MapTile startTile, MapTile endTile) {
            var tileRoute = GetTileRoute(startTile, endTile);
            tileRoute.Reverse();

            var lerpyTileRoute = new List<MapTile>();

            for (var i = 0; i < tileRoute.Count; i++) {
                for (var j = tileRoute.Count - 1; j > 0; j--) {
                    if (CheckRouteBlocked(AllLerpedTiles(tileRoute[i], tileRoute[j]))) {
                        lerpyTileRoute.Add(tileRoute[i]);
                        lerpyTileRoute.Add(tileRoute[j]);
                        i = j;
                        break;
                    }
                }

                lerpyTileRoute.Add(tileRoute[i]);
            }

            return lerpyTileRoute;
        }

        private static List<MapTile> GetTileRoute(MapTile startTile, MapTile endTile) {
            var tileRoute = new List<MapTile> {endTile};
            var checkTile = endTile;
            for (var i = 0; i < endTile.MoveNum; i++) {
                foreach (var tile in checkTile.GetAdjacentTiles(false)) {
                    if (tile.Value == null || tile.Value.MoveNum != (checkTile.MoveNum - 1) ||
                        !tile.Value.CanMoveIntoFrom(tile.Key)) continue;
                    tileRoute.Add(tile.Value);
                    checkTile = tile.Value;
                }
            }

            tileRoute.Add(startTile);
            return tileRoute;
        }

        private static bool CheckRouteBlocked(List<MapTile> route) {
            var direction = route[route.Count - 1].FindDirectionFromOtherTile(route[0]);
            for (var i = 0; i < route.Count - 1; i++) {
                if (route[i].MoveBlocked[direction[0]]) return false;
                if (route[i].MoveBlocked[direction[direction.Count - 1]]) return false;
            }
            return true;
        }

        private static float Lerp(int start, int end, float t) => start + t * (end - start);

        private static MapTile LerpTile(MapTile start, MapTile end, float t) =>
            MapTile.GetMapTileFromPos(Convert.ToInt32(Lerp(start.MapPosX, end.MapPosX, t)),
                Convert.ToInt32(Lerp(start.MapPosZ, end.MapPosZ, t)));

        private static List<MapTile> AllLerpedTiles(MapTile startTile, MapTile endTile) {
            var returnSet = new List<MapTile>();
            for (var i = 0; i < 20; i++) {
                var t = Convert.ToSingle(i) / 20.0F;
                var lerpedTile = LerpTile(startTile, endTile, t);
                if (!returnSet.Contains(lerpedTile)) returnSet.Add(lerpedTile);
            }

            return returnSet;
        }
    }
}