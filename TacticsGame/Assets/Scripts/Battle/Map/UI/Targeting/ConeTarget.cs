using System;
using System.Collections.Generic;
using System.Linq;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Network;
using TacticsGame.Battle.Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TacticsGame.Battle.Map.UI.Targeting {
    public class ConeTarget {
        private static MouseHoverTile _mouseHoverTile;
        private static PlayerControls _playerControls;
        private static GameObject _coneTarget;

        public void SubscribeToHoverTile() {
            _mouseHoverTile.OnHoverTileChanged += RefreshCone;
            _playerControls.OnLeftClick += ConeSelect;
        }

        private void UnsubscribeToHoverTile() {
            _mouseHoverTile.OnHoverTileChanged -= RefreshCone;
            _playerControls.OnLeftClick -= ConeSelect;
        }

        public ConeTarget() {
            _mouseHoverTile = GameObject.Find("GameManager").GetComponent<MouseHoverTile>();
            _playerControls = GameObject.Find("GameManager").GetComponent<PlayerControls>();
        }

        private void DrawCone(int length, int degrees) {
            var mesh = new Mesh();

            var vertices = new Vector3[3];
            var triangles = new int[3];

            var width = (length / (90 / degrees)) / 2;

            vertices[0] = new Vector3(0, 0, 0);
            vertices[1] = new Vector3(-width, 0, Convert.ToSingle(length));
            vertices[2] = new Vector3(width, 0, Convert.ToSingle(length));

            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            _coneTarget.AddComponent<MeshFilter>();
            _coneTarget.GetComponent<MeshFilter>().mesh = mesh;

            var lineRenderer = _coneTarget.AddComponent<LineRenderer>();
            lineRenderer.useWorldSpace = false;
            lineRenderer.startColor = new Color32(121, 187, 191, 200);
            lineRenderer.endColor = new Color32(121, 187, 191, 200);
            lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            lineRenderer.startWidth = 0.05F;
            lineRenderer.endWidth = 0.05F;
            lineRenderer.positionCount = 3;
            lineRenderer.SetPositions(vertices);
            lineRenderer.loop = true;
        }

        private void RefreshCone(object sender, MouseHoverTile.OnHoverTileChangedArgs args) {
            Object.Destroy(_coneTarget);
            MapTile.UnhighlightAllTiles();
            if (args.HoverTileUi == null) return;

            Unit.ActiveUnit.LookAtGameObject(args.HoverTileUi);
            Unit.ActiveUnit.selectedMapTile = MapTile.GetMapTileFromUi(args.HoverTileUi);
            var conePos = Unit.ActiveUnit.transform.position;
            var coneRot = Unit.ActiveUnit.transform.rotation;
            conePos.y += 0.1F;
            _coneTarget = new GameObject {name = "ConeTarget"};
            _coneTarget.transform.position = conePos;
            _coneTarget.transform.rotation = coneRot;
            DrawCone(13, 90);
            MapTile.HighlightMultipleTiles(FindTilesInCone(), Color.white);
        }

        private List<MapTile> FindTilesInCone() {
            var vert = _coneTarget.GetComponent<MeshFilter>().mesh.vertices;
            var vPos0 = new Vector3(_coneTarget.transform.TransformPoint(vert[0]).x,
                _coneTarget.transform.TransformPoint(vert[0]).z);
            var vPos1 = new Vector3(_coneTarget.transform.TransformPoint(vert[1]).x,
                _coneTarget.transform.TransformPoint(vert[1]).z);
            var vPos2 = new Vector3(_coneTarget.transform.TransformPoint(vert[2]).x,
                _coneTarget.transform.TransformPoint(vert[2]).z);

            return MapTile.All
                .Where(tile => PointInTriangle(new Vector3(tile.MapPosX, tile.MapPosZ), vPos0, vPos1, vPos2))
                .Where(tile => Unit.ActiveUnit.GetCurrentMapTile().CanSeeOtherTile(tile, 20.0F, true))
                .ToList();
        }

        private static float Sign(Vector3 p1, Vector3 p2, Vector3 p3) =>
            (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);

        private static bool PointInTriangle(Vector3 pt, Vector3 v1, Vector3 v2, Vector3 v3) {
            bool b1, b2, b3;

            b1 = Sign(pt, v1, v2) < 0.0f;
            b2 = Sign(pt, v2, v3) < 0.0f;
            b3 = Sign(pt, v3, v1) < 0.0f;

            return b1 == b2 && b2 == b3;
        }

        private void ConeSelect(object sender, EventArgs args) {
            Unit.ActiveUnit.aoeTargetTiles.Clear();
            Unit.ActiveUnit.aoeTargetTiles.Add(Unit.ActiveUnit.selectedAbility, FindTilesInCone());
            Object.Destroy(_coneTarget);
            MapTile.UnhighlightAllTiles();
            UnsubscribeToHoverTile();
            GameObject.Find("GameManager").GetComponent<GameManager>().SendAbility(new AbilityAction());
        }
    }
}