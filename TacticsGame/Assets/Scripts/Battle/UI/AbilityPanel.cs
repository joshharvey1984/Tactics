using TacticsGame.Battle.Core;
using TacticsGame.Battle.Map.UI;
using TacticsGame.Battle.Units;
using UnityEngine;
using UnityEngine.UI;

namespace TacticsGame.Battle.UI {
    public class AbilityPanel : MonoBehaviour {
        private GameManager _gameManager;
        
        [SerializeField] private GameObject buttonPanel;
        [SerializeField] private GameObject moveButton;
        [SerializeField] private GameObject executeButton;
        [SerializeField] private GameObject abilityDescription;
        
        [SerializeField] private GameObject buttonPrefab;

        [SerializeField] private GameObject targetPanel;
        private TargetPanel _targetPanel;
        
        private Ability _currentAbility;

        private void Awake() {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _targetPanel = targetPanel.GetComponent<TargetPanel>();
        }
        
        public void ChangeAbility(Ability ability) {
            _currentAbility = ability;
            ActivatePanel();
            abilityDescription.GetComponent<Text>().text = ability.description;
            executeButton.transform.GetChild(0).GetComponent<Text>().text = ability.name;
            MovementUI.DestroyMovementUI();
        }

        public void MoveMode() {
            DeactivatePanel();
            MovementUI.DrawMovementUI(Unit.SelectedUnit);
        }

        private void ActivatePanel() {
            gameObject.GetComponent<Image>().enabled = true;
            moveButton.SetActive(!Unit.SelectedUnit.moveTaken);
            abilityDescription.SetActive(true);
            executeButton.SetActive(true);
            _targetPanel.MovePanel(210);
        }

        private void DeactivatePanel() {
            gameObject.GetComponent<Image>().enabled = false;
            moveButton.SetActive(false);
            abilityDescription.SetActive(false);
            executeButton.SetActive(false);
            _targetPanel.MovePanel(80);
        }

        public void ExecuteAbility() {
            DeactivatePanel();
            ActionButton.DestroyAll();
            _gameManager.EndUnitTurn();
        } 
        
        public void CreateAbilityButtons() {
            foreach (var ability in Unit.SelectedUnit.abilities) {
                var btn = Instantiate(buttonPrefab, buttonPanel.transform);
                var actionButton = btn.GetComponent<ActionButton>();
                actionButton.SetIcon(ability.icon);
                actionButton.abilityPanel = this;
                actionButton.ability = ability;
            }
        }
    }
}