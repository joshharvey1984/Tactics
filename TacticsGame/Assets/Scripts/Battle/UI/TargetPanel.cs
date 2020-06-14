using System.Collections.Generic;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Battle.UI {
    public class TargetPanel : MonoBehaviour {
        private Transform _transform;

        [SerializeField] private GameObject targetButtonPrefab;

        private Unit _currentTarget;
        
        
        private void Awake() {
            _transform = gameObject.transform;
        }

        public void MovePanel(float yPos) {
            var targetPanelPos = _transform.position;
            targetPanelPos.y = yPos;
            _transform.position = targetPanelPos;
        }

        public void ChangeTarget(Unit unit) {
            _currentTarget = unit;
        }

        public void UpdateTargetPanel(List<Unit> units = null) {
            TargetButton.DestroyAll();
            foreach (var unit in units) {
                var btn = Instantiate(targetButtonPrefab, transform);
                var targetButton = btn.GetComponent<TargetButton>();
                targetButton.targetUnit = unit;
                targetButton.targetPanel = this;
            }
        }
    }
}
