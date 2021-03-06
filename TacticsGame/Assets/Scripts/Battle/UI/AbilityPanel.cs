﻿using TacticsGame.Battle.Map.UI;
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

        private HitPanel _hitPanel;
        private DamagePanel _damagePanel;
        
        private TargetPanel _targetPanel;

        private UIAudio _uiAudio;

        private void Awake() {
            _targetPanel = FindObjectOfType<TargetPanel>();
            _hitPanel = Resources.FindObjectsOfTypeAll<HitPanel>()[0];
            _damagePanel = Resources.FindObjectsOfTypeAll<DamagePanel>()[0];
            _uiAudio = FindObjectOfType<UIAudio>();
        }

        private void ChangeAbility(object sender, AbilityButton.OnAbilityButtonClickArgs e) {
            MovementUI.DestroyMovementUI();
            Unit.ActiveUnit.selectedAbility = e.SelectedAbility;
            ActivatePanel();
            abilityDescription.GetComponent<Text>().text = e.SelectedAbility.Description;
            executeButton.transform.GetChild(0).GetComponent<Text>().text = e.SelectedAbility.Name;
        }

        public void MoveMode() {
            Unit.ActiveUnit.selectedAbility = null;
            Unit.ActiveUnit.targetUnit = null;
            DeactivatePanel();
            _targetPanel.UpdateTargetPanel();
            MovementUI.DrawMovementUI(Unit.ActiveUnit);
            _uiAudio.Play(_uiAudio.PathToAudioClip("Audio/UI/MoveButton"));
        }

        private void ActivatePanel() {
            gameObject.GetComponent<Image>().enabled = true;
            moveButton.SetActive(!Unit.ActiveUnit.moveTaken);
            abilityDescription.SetActive(true);
            executeButton.SetActive(true);
            _targetPanel.MovePanel(210);
            if (Unit.ActiveUnit.selectedAbility.TargetingType == Ability.TargetingTypes.Single) ActivateCombatPanels();
            else DeactivateCombatPanels();
            _uiAudio.Play(_uiAudio.PathToAudioClip("Audio/UI/AbilityPanelActive"));
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
            Unit.ActiveUnit.ExecuteAbility();
        } 
        
        public void CreateAbilityButtons() {
            AbilityButton.DestroyAll();
            foreach (var ability in Unit.ActiveUnit.abilities) {
                if (ability.AbilityType == Ability.AbilityTypes.Passive) continue;
                if (ability.TargetingType == Ability.TargetingTypes.Single 
                    && Unit.ActiveUnit.EnemiesInLineOfSight().Count == 0)
                    continue;
                var btn = Instantiate(buttonPrefab, buttonPanel.transform);
                var actionButton = btn.GetComponent<AbilityButton>();
                actionButton.OnAbilityButtonClick += _targetPanel.ChangedAbility;
                actionButton.OnAbilityButtonClick += ChangeAbility;
                actionButton.SetIcon(ability.Icon);
                actionButton.AssignAbility(ability);
            }
        }

        public void ChangedTarget(object sender, TargetButton.TargetButtonClickArgs e) {
            if (Unit.ActiveUnit.selectedAbility == null || 
                Unit.ActiveUnit.selectedAbility.TargetingType != Ability.TargetingTypes.Single)
                ChangeAbility(sender, new AbilityButton.OnAbilityButtonClickArgs 
                    { SelectedAbility = Unit.ActiveUnit.abilities[0] });
        }
    }
}