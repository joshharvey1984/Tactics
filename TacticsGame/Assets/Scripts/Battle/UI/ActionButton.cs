using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TacticsGame.Battle.UI {
    public class ActionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
        private Button _btn;
        private Transform _myIcon;
        public AbilityPanel abilityPanel;
        public Ability ability;

        private void Awake() {
            _btn = GetComponent<Button>();
            _myIcon = transform.GetChild(0);
        }

        public void SetIcon(Sprite icon) => _myIcon.gameObject.GetComponent<Image>().sprite = icon;

        private void OnDeHover() {
            if (_myIcon != null) _myIcon.localScale = Vector3.one;
        }

        private void OnHover() {
            if (_myIcon != null) _myIcon.localScale = Vector3.one * 1.2f;
        }

        private void OnClick() {
            abilityPanel.ChangeAbility(ability);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            OnHover();
        }

        public void OnPointerExit(PointerEventData eventData) {
            OnDeHover();
        }

        public void OnPointerClick(PointerEventData eventData) {
            OnClick();
        }
    }
}