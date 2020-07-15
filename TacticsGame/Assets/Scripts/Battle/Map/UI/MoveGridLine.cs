using System.Collections.Generic;
using TacticsGame.Battle.Map.Enums;
using UnityEngine;
using static TacticsGame.Battle.Map.Enums.Direction;

namespace TacticsGame.Battle.Map.UI {
    public static class MoveGridLine {
        private static GameObject _moveGrid;
        private static GameObject _gridLine;

        public static void DestroyMoveGrid() {
            if (_moveGrid) Object.Destroy(_moveGrid);
        }

        public static void DrawMoveGrid(List<MapTile> moveTiles) {
            _moveGrid = new GameObject { name = "MoveGrid" };
            foreach (var mapTile in moveTiles) {
                if (!moveTiles.Contains(mapTile.GetAdjacentTiles(false)[North]))
                    DrawLine(mapTile.GetWorldPosition(), North);
                if (!moveTiles.Contains(mapTile.GetAdjacentTiles(false)[South]))
                    DrawLine(mapTile.GetWorldPosition(), South);
                if (!moveTiles.Contains(mapTile.GetAdjacentTiles(false)[East]))
                    DrawLine(mapTile.GetWorldPosition(), East);
                if (!moveTiles.Contains(mapTile.GetAdjacentTiles(false)[West]))
                    DrawLine(mapTile.GetWorldPosition(), West);
            }
        }

        private static void DrawLine(Vector3 pos, Direction direction) {
            _gridLine = new GameObject();
            _gridLine.transform.SetParent(_moveGrid.transform);
            _gridLine.transform.position = pos;
            var lr = _gridLine.AddComponent<LineRenderer>();
            lr.useWorldSpace = false;
            lr.startColor = new Color32(121, 187, 191, 255);
            lr.endColor = new Color32(121, 187, 191, 255);
            lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            lr.startWidth = 0.035f;
            lr.endWidth = 0.035f;
            lr.positionCount = 2;

            if (direction == North) {
                lr.SetPosition(0, new Vector3(0.5F, 0.1F, 0.5F));
                lr.SetPosition(1, new Vector3(0.5F, 0.1F, -0.5F));
            }

            if (direction == South) {
                lr.SetPosition(0, new Vector3(-0.5F, 0.1F, 0.5F));
                lr.SetPosition(1, new Vector3(-0.5F, 0.1F, -0.5F));
            }

            if (direction == East) {
                lr.SetPosition(0, new Vector3(0.5F, 0.1F, -0.5F));
                lr.SetPosition(1, new Vector3(-0.5F, 0.1F, -0.5F));
            }

            if (direction == West) {
                lr.SetPosition(0, new Vector3(0.5F, 0.1F, 0.5F));
                lr.SetPosition(1, new Vector3(-0.5F, 0.1F, 0.5F));
            }
        }
    }
}