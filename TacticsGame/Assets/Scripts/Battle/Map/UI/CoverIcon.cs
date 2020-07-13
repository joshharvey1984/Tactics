using System.Collections.Generic;
using UnityEngine;

namespace TacticsGame.Battle.Map.UI {
    public class CoverIcon : MonoBehaviour {
        private MeshRenderer _renderer;
        public MapTile mapTile;

        private void Awake() {
            _renderer = GetComponent<MeshRenderer>();
        }
        public void HighlightIcon(bool shouldHighlight) {
            _renderer.material.color = shouldHighlight ? new Color32(121, 187, 191, 255) 
                    : new Color32(121, 187, 191, 50);
        }

        public void SetCoverIcon(Texture coverTexture) {
            _renderer.material.mainTexture = coverTexture;
        }
    }
}