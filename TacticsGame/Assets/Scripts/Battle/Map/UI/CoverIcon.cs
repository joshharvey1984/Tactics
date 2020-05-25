using UnityEngine;

namespace TacticsGame.Battle.Map.UI {
    public class CoverIcon : MonoBehaviour {
        private Vector3 _position;
        private MeshRenderer _renderer;
        private float _targetYPos;

        private void Awake() {
            _position = transform.position;
            _renderer = GetComponent<MeshRenderer>();
        }

        private void Start() {
            _targetYPos = 0.5F;
        }

        private void Update() {
            if (transform.position.y < _targetYPos) {
                _position.y += 4.0F * Time.deltaTime;
                transform.position = _position;
            }
        }

        public void HighlightIcon(bool shouldHighlight) {
            _renderer.material.color =
                shouldHighlight ? new Color32(121, 187, 191, 255) : new Color32(121, 187, 191, 50);
        }

        public void SetCoverIcon(Texture coverTexture) {
            _renderer.material.mainTexture = coverTexture;
        }
    }
}