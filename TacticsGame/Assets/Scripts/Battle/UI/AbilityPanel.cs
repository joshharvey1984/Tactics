using TacticsGame.Battle.Map.UI;
using TacticsGame.Battle.Units;
using UnityEngine;
using UnityEngine.UI;

namespace TacticsGame.Battle.UI {
    public class AbilityPanel : MonoBehaviour {
        [SerializeField] private GameObject buttonPanel;
        [SerializeField] private GameObject moveButton;
        [SerializeField] private GameObject abilityDescription;
        
        [SerializeField] private GameObject buttonPrefab;
        
        private Ability _currentAbility;
        
        public void ChangeAbility(Ability ability) {
            _currentAbility = ability;
            gameObject.GetComponent<Image>().enabled = true;
            moveButton.SetActive(!Unit.SelectedUnit._moveTaken);
            abilityDescription.SetActive(true);
            abilityDescription.GetComponent<Text>().text = ability.description;
            MovementUI.DestroyMovementUI();
        }

        public void MoveMode() {
            gameObject.GetComponent<Image>().enabled = false;
            moveButton.SetActive(false);
            abilityDescription.SetActive(false);
            MovementUI.DrawMovementUI(Unit.SelectedUnit);
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