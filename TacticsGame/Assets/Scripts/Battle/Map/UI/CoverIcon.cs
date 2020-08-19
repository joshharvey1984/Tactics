using TacticsGame.Battle.UI;
using UnityEngine;

namespace TacticsGame.Battle.Map.UI {
    public class CoverIcon : MonoBehaviour {
        private MeshRenderer _renderer;
        public MapTile mapTile;
        
        private UIAudio _uiAudio;

        private void Awake() {
            _renderer = GetComponent<MeshRenderer>();
            _uiAudio = FindObjectOfType<UIAudio>();
        }
        public void HighlightIcon(bool shouldHighlight) {
             if(shouldHighlight) {
                 _renderer.material.color = new Color32(121, 187, 191, 255);
                 _uiAudio.Play(_uiAudio.PathToAudioClip("Audio/UI/CoverLightUp"), 0.1F);
             }
             else {
                 _renderer.material.color = new Color32(121, 187, 191, 50);
             }
             
            
        }

        public void SetCoverIcon(Texture coverTexture) {
            _renderer.material.mainTexture = coverTexture;
        }
    }
}