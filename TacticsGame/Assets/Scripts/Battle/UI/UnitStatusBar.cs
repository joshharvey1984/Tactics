using System.Collections;
using TacticsGame.Battle.Units;
using UnityEngine;
using UnityEngine.UI;

namespace TacticsGame.Battle.UI {
    public class UnitStatusBar : MonoBehaviour {
        private Slider _slider;
        private bool _slideAnim;
        private int _health;
        public Unit unit;
        private Camera _mainCamera;
        public GameObject statusIcon;
        
        private void Start() {
            _slider = gameObject.GetComponent<Slider>();
            _mainCamera = Camera.main;
        }

        public void LoseHealth(int health) {
            _health = health;
            _slideAnim = true;
        }

        public void UpdateStatusIcons() {
            if (unit.currentStatusEffects.Count > 0) {
                statusIcon.GetComponent<Image>().sprite = unit.currentStatusEffects[0].icon;
                statusIcon.GetComponent<Image>().enabled = true;
            }
            else {
                statusIcon.GetComponent<Image>().enabled = false;
            }
        }

        private void Update() {
            var pos = _mainCamera.WorldToScreenPoint(unit.transform.position);
            pos.y += 110;
            transform.position = pos;
            
            if (_slideAnim) {
                StartCoroutine(SlideHealth());
                _slideAnim = false;
            }
        }

        private IEnumerator SlideHealth() {
            for (var i = 0; i < _health; i++) {
                _slider.value -= 1;
                yield return new WaitForSeconds(0.05F);
            }
        }
    }
}