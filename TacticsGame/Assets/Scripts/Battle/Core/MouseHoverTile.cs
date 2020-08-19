using System;
using UnityEngine;

namespace TacticsGame.Battle.Core {
    public class MouseHoverTile : MonoBehaviour {
        private const int WallBitMask = ~(1 << 8);
        private Camera _mainCamera;
        private static GameObject _hoverTileObject;

        public event EventHandler<OnHoverTileChangedArgs> OnHoverTileChanged;

        public class OnHoverTileChangedArgs : EventArgs {
            public GameObject HoverTileUi;
        }

        private void Awake() {
            OnHoverTileChanged += _OnHoverTileChanged;
        }

        private static void _OnHoverTileChanged(object sender, OnHoverTileChangedArgs e) {
            _hoverTileObject = e.HoverTileUi;
        }

        private void Update() {
            var mousePosition = Input.mousePosition;
            if (_mainCamera == null) GetMainCam();
            var ray = _mainCamera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out var hit, 200.0f, WallBitMask)) {
                if (hit.collider.gameObject != _hoverTileObject)
                    OnHoverTileChanged?.Invoke(this,
                        new OnHoverTileChangedArgs {HoverTileUi = hit.collider.gameObject});
            }
            else {
                OnHoverTileChanged?.Invoke(this, new OnHoverTileChangedArgs {HoverTileUi = null});
            }
        }

        private void GetMainCam() {
            if (!Camera.current) _mainCamera = Camera.main;
        }
    }
}