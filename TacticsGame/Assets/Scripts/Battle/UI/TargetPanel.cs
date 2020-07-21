using System.Linq;
using TacticsGame.Battle.Units;
using UnityEngine;
using static TacticsGame.Data.Ability.TargetingTypes;

namespace TacticsGame.Battle.UI {
    public class TargetPanel : MonoBehaviour {
        private Transform _transform;
        private AbilityPanel _abilityPanel;
        [SerializeField] private GameObject targetButtonPrefab;

        private void Awake() {
            _transform = gameObject.transform;
            _abilityPanel = FindObjectOfType<AbilityPanel>();
        }

        public void MovePanel(float yPos) {
            var targetPanelPos = _transform.position;
            targetPanelPos.y = yPos;
            _transform.position = targetPanelPos;
        }

        private static void ChangeTarget(object sender, TargetButton.OnTargetButtonClickArgs e) {
            Unit.ActiveUnit.targetUnit = e.SelectedUnit;
        }

        public void ChangedAbility(object sender, AbilityButton.OnAbilityButtonClickArgs e) {
            if (e.SelectedAbility.TargetingType == EnemyFire || e.SelectedAbility.TargetingType == EnemyWatch) {
                if (!Unit.ActiveUnit.targetUnit) {
                    Unit.ActiveUnit.targetUnit = Unit.ActiveUnit.EnemiesInLineOfSight()[0];
                }
                
                TargetButton.All.First(targetButton => targetButton.targetUnit == Unit.ActiveUnit.targetUnit)
                    .ChangeColour(new Color32(191, 30, 46, 255));
            }
            else {
                UpdateTargetPanel();
            }
            
        }

        public void UpdateTargetPanel() {
            TargetButton.DestroyAll();
            foreach (var unit in Unit.ActiveUnit.EnemiesInLineOfSight()) {
                var btn = Instantiate(targetButtonPrefab, transform);
                var targetButton = btn.GetComponent<TargetButton>();
                targetButton.targetUnit = unit;
                targetButton.OnTargetButtonClick += ChangeTarget;
                targetButton.OnTargetButtonClick += _abilityPanel.ChangedTarget;
            }
        }
    }
}
