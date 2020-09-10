using TacticsGame.Battle.Core;
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

        private void ChangeTarget(object sender, TargetButton.TargetButtonClickArgs e) {
            Unit.ActiveUnit.targetUnit = e.SelectedUnit;
            UpdateTargetPanel();
        }

        public void ChangedAbility(object sender, AbilityButton.OnAbilityButtonClickArgs e) {
            if (e.SelectedAbility.TargetingType == Single) {
                if (!Unit.ActiveUnit.targetUnit) {
                    Unit.ActiveUnit.targetUnit = Unit.ActiveUnit.EnemiesInLineOfSight()[0];
                }
                UpdateTargetPanel();
            }
            else {
                TargetButton.DestroyAll();
            }
        }

        public void UpdateTargetPanel() {
            TargetButton.DestroyAll();
            foreach (var unit in Unit.ActiveUnit.EnemiesInLineOfSight()) {
                var btn = Instantiate(targetButtonPrefab, transform);
                var targetButton = btn.GetComponent<TargetButton>();
                targetButton.targetUnit = unit;
                if (Unit.ActiveUnit.targetUnit == unit) targetButton.TargetSelected();
                if (Unit.ActiveUnit.selectedAbility == null)
                    Unit.ActiveUnit.selectedAbility = Unit.ActiveUnit.abilities[0];
                var hitChance = CombatHitCalc.CalculateHitChance(Unit.ActiveUnit, unit, Unit.ActiveUnit.selectedAbility)["HIT"];
                targetButton.SetHitChance(hitChance * 100);
                targetButton.OnTargetButtonClick += ChangeTarget;
                targetButton.OnTargetButtonClick += _abilityPanel.ChangedTarget;
            }
        }
    }
}
