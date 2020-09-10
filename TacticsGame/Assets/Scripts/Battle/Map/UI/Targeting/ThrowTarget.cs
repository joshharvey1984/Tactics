using System;
using System.Collections.Generic;
using System.Linq;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Network;
using TacticsGame.Battle.Units;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TacticsGame.Battle.Map.UI.Targeting {
    public class ThrowTarget {
        private static MouseHoverTile _mouseHoverTile;
        private static PlayerControls _playerControls;
        private GameObject _arcTarget;
        
        private Color _blastColor = new Color(0.0f, 1f, 1f, 0.65f);
        private Color _shrapnelColor = new Color(0.0f, 1f, 1f, 0.35f);

        private int _throwRange, _blastAreaOfEffect, _shrapnelAreaOfEffect;
        
        public ThrowTarget(int throwRange, int blastAreaOfEffect, int shrapnelAreaOfEffect) {
            _mouseHoverTile = GameObject.Find("GameManager").GetComponent<MouseHoverTile>();
            _playerControls = GameObject.Find("GameManager").GetComponent<PlayerControls>();
            _throwRange = throwRange;
            _blastAreaOfEffect = blastAreaOfEffect;
            _shrapnelAreaOfEffect = shrapnelAreaOfEffect;
        }

        public void SubscribeToHoverTile() {
            _mouseHoverTile.OnHoverTileChanged += RefreshArc;
            _playerControls.OnLeftClick += ThrowSelect;
        }

        private void UnsubscribeToHoverTile() {
            _mouseHoverTile.OnHoverTileChanged -= RefreshArc;
            _playerControls.OnLeftClick -= ThrowSelect;
        }

        private void RefreshArc(object sender, MouseHoverTile.OnHoverTileChangedArgs args) {
            if (args.HoverTileUi == null) {
                Object.Destroy(_arcTarget);
                MapTile.UnhighlightAllTiles();
                Unit.ActiveUnit.selectedMapTile = null;
                return;
            }

            if (Vector3.Distance(args.HoverTileUi.transform.position,
                Unit.ActiveUnit.GetCurrentMapTile().UiTile.transform.position) > _throwRange) {
                return;
            }

            Unit.ActiveUnit.selectedMapTile = MapTile.GetMapTileFromUi(args.HoverTileUi);
            Object.Destroy(_arcTarget);
            MapTile.UnhighlightAllTiles();
            Unit.ActiveUnit.LookAtGameObject(args.HoverTileUi);
            _arcTarget = new GameObject { name = "ThrowTarget" };
            var lineRenderer = _arcTarget.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 16;
            var startPos = Unit.ActiveUnit.transform.position;
            startPos.y += 0.9F;
            lineRenderer.SetPositions(CalculateArcArray(startPos, args.HoverTileUi.transform.position));
            lineRenderer.useWorldSpace = false;
            lineRenderer.startColor = new Color32(121, 187, 191, 200);
            lineRenderer.endColor = new Color32(121, 187, 191, 200);
            lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            lineRenderer.startWidth = 0.05F;
            lineRenderer.endWidth = 0.05F;
            var areaOfEffectTiles = FindAOETiles(args.HoverTileUi, _blastAreaOfEffect, _shrapnelAreaOfEffect);
            MapTile.HighlightMultipleTiles(areaOfEffectTiles.Item1, _blastColor);
            MapTile.HighlightMultipleTiles(areaOfEffectTiles.Item2, _shrapnelColor);
        }

        public static (List<MapTile>, List<MapTile>) FindAOETiles(GameObject aimTile, int blastAOE, int shrapnelAOE) {
            var blastTiles = GrenadeGrid.GridTiles(MapTile.GetMapTileFromUi(aimTile), blastAOE);
            var shrapnelTiles = GrenadeGrid.GridTiles(MapTile.GetMapTileFromUi(aimTile), shrapnelAOE)
                .Except(blastTiles).ToList();
            
            return (blastTiles, shrapnelTiles);
        }
        

        public static Vector3[] CalculateArcArray(Vector3 start, Vector3 end) {
            var arcArray = new Vector3[16];
            for (var i = 0; i <= 15; i++) {
                var t = i / (float) 15;
                arcArray[i] = Parabola(start, end, 1.5F, t);
            }

            return arcArray;
        }

        private static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t) {
            Func<float, float> f = x => -4 * height * x * x + 4 * height * x;
            var mid = Vector3.Lerp(start, end, t);
            return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }

        private void ThrowSelect(object sender, EventArgs args) {
            if (Unit.ActiveUnit.selectedMapTile == null) return;
            Object.Destroy(_arcTarget);
            MapTile.UnhighlightAllTiles();
            UnsubscribeToHoverTile();
            GameObject.Find("GameManager").GetComponent<GameManager>().SendAbility(new AbilityAction());
        }
    }
}