using System.Linq;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Map.UI;
using TacticsGame.Battle.Units;
using TacticsGame.Data;
using UnityEngine;
using UnityEngine.UI;

namespace TacticsGame.Battle.UI {
    public class AbilityPanel : MonoBehaviour {
        [SerializeField] private GameObject buttonPanel;
        [SerializeField] private GameObject moveButton;
        [SerializeField] private GameObject executeButton;
        [SerializeField] private GameObject abilityDescription;
        
        [SerializeField] private GameObject buttonPrefab;

        public HitPanel _hitPanel;
        private DamagePanel _damagePanel;
        
        private TargetPanel _targetPanel;

        private void Awake() {
            _targetPanel = FindObjectOfType<TargetPanel>();
            _hitPanel = Resources.FindObjectsOfTypeAll<HitPanel>()[0];
            _damagePanel = Resources.FindObjectsOfTypeAll<DamagePanel>()[0];
        }

        private void ChangeAbility(object sender, AbilityButton.OnAbilityButtonClickArgs e) {
            MovementUI.DestroyMovementUI();
            Unit.SelectedUnit.selectedAbility = e.SelectedAbility;
            ActivatePanel();
            abilityDescription.GetComponent<Text>().text = e.SelectedAbility.Description;
            executeButton.transform.GetChild(0).GetComponent<Text>().text = e.SelectedAbility.Name;
        }

        public void MoveMode() {
            Unit.SelectedUnit.selectedAbility = null;
            Unit.SelectedUnit.targetUnit = null;
            DeactivatePanel();
            _targetPanel.UpdateTargetPanel();
            MovementUI.DrawMovementUI(Unit.SelectedUnit);
        }

        private void ActivatePanel() {
            gameObject.GetComponent<Image>().enabled = true;
            moveButton.SetActive(!Unit.SelectedUnit.moveTaken);
            abilityDescription.SetActive(true);
            executeButton.SetActive(true);
            _targetPanel.MovePanel(210);
            if (Unit.SelectedUnit.selectedAbility.TargetingType == Ability.TargetingTypes.Enemy) ActivateCombatPanels();
            else DeactivateCombatPanels();
        }

        private void DeactivatePanel() {
            gameObject.GetComponent<Image>().enabled = false;
            moveButton.SetActive(false);
            abilityDescription.SetActive(false);
            executeButton.SetActive(false);
            _targetPanel.MovePanel(80);
            DeactivateCombatPanels();
        }

        private void ActivateCombatPanels() {
            _hitPanel.ActivatePanel();
            _damagePanel.ActivatePanel();
        }
        
        private void DeactivateCombatPanels() {
            _hitPanel.DeactivatePanel();
            _damagePanel.DeactivatePanel();
        }

        public void ExecuteAbility() {
            DeactivatePanel();
            AbilityButton.DestroyAll();
            TargetButton.DestroyAll();
            Unit.SelectedUnit.ExecuteAbility();
        } 
        
        public void CreateAbilityButtons() {
            AbilityButton.DestroyAll();
            foreach (var ability in Unit.SelectedUnit.abilities) {
                if (ability.TargetingType == Ability.TargetingTypes.Enemy && Unit.SelectedUnit.EnemiesInLineOfSight().Count == 0)
                    continue;
                var btn = Instantiate(buttonPrefab, buttonPanel.transform);
                var actionButton = btn.GetComponent<AbilityButton>();
                actionButton.OnAbilityButtonClick += ChangeAbility;
                actionButton.OnAbilityButtonClick += _targetPanel.ChangedAbility;
                actionButton.SetIcon(ability.Icon);
                actionButton.AssignAbility(ability);
            }
        }

        public void ChangedTarget(object sender, TargetButton.OnTargetButtonClickArgs e) {
            if (Unit.SelectedUnit.selectedAbility == null || 
                Unit.SelectedUnit.selectedAbility.TargetingType != Ability.TargetingTypes.Enemy) 
                ChangeAbility(sender, new AbilityButton.OnAbilityButtonClickArgs 
                    {SelectedAbility = Unit.SelectedUnit.abilities[0]});
        }
    }
}