using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TacticsGame.Battle.Map.UI {
    public static class RouteLine {
        private static GameObject _moveLineObject;
        private static GameObject _moveCircle;

        public static void DrawMoveLine(List<MapTile> routeTiles) {
            DestroyMoveLine();

            _moveLineObject = new GameObject();

            for (var i = 1; i < routeTiles.Count; i++) {
                DrawLine(routeTiles[i], routeTiles[i - 1], 255, 255);
            }

            DrawHoverCircle(routeTiles[routeTiles.Count - 1]);
        }

        public static void DestroyMoveLine() {
            if (_moveLineObject) Object.Destroy(_moveLineObject);
            if (_moveCircle) Object.Destroy(_moveCircle);
        }

        private static void DrawLine(MapTile startTile, MapTile nextTile, byte startAlpha, byte endAlpha) {
            var drawLine = new GameObject();
            drawLine.transform.SetParent(_moveLineObject.transform);
            var lr = drawLine.AddComponent<LineRenderer>();
            lr.useWorldSpace = false;
            lr.startColor = new Color32(121, 187, 191, startAlpha);
            lr.endColor = new Color32(121, 187, 191, endAlpha);
            lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            lr.startWidth = 0.035f;
            lr.endWidth = 0.035f;
            lr.SetPosition(0, startTile.UiTile.transform.position);
            lr.SetPosition(1, nextTile.UiTile.transform.position);
        }

        private static void DrawHoverCircle(MapTile hoverTile) {
            const int segments = 360;
            _moveCircle = new GameObject();
            var uPos = hoverTile.UiTile.transform.position;
            _moveCircle.transform.position = new Vector3(uPos.x, uPos.y + 0.01F, uPos.z);
            var line = _moveCircle.AddComponent<LineRenderer>();
            line.useWorldSpace = false;
            line.startWidth = 0.025F;
            line.endWidth = 0.025F;
            line.startColor = new Color32(121, 187, 191, 255);
            line.endColor = new Color32(121, 187, 191, 255);
            line.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            line.positionCount = segments + 1;

            const int pointCount = segments + 1;
            var points = new Vector3[pointCount];

            for (var i = 0; i < pointCount; i++) {
                var rad = Mathf.Deg2Rad * (i * 360f / segments);
                points[i] = new Vector3(Mathf.Sin(rad) * 0.4F, 0, Mathf.Cos(rad) * 0.4F);
            }

            line.SetPositions(points);
        }
    }
}