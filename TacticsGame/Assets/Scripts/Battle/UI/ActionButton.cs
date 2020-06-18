using System.Collections.Generic;
using TacticsGame.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TacticsGame.Battle.UI {
    public class ActionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
        private static readonly List<ActionButton> All = new List<ActionButton>();
        
        private Transform _myIcon;
        public AbilityPanel abilityPanel;
        public Ability ability;

        private void Awake() {
            All.Add(this);
            _myIcon = transform.GetChild(0);
        }

        public static void DestroyAll() {
            foreach (var actionButton in All) {
                Destroy(actionButton.gameObject);
            }
            All.Clear();
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