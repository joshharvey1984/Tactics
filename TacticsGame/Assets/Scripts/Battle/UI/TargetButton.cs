using System;
using System.Collections.Generic;
using TacticsGame.Battle.Units;
using TacticsGame.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TacticsGame.Battle.UI {
    public class TargetButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
        public static readonly List<TargetButton> All = new List<TargetButton>();

        [SerializeField] private GameObject hitChanceText;
        [SerializeField] private GameObject selectBox;
        
        
        public Unit targetUnit;
        
        public event EventHandler<TargetButtonClickArgs> OnTargetButtonClick;
        public class TargetButtonClickArgs : EventArgs {
            public Unit SelectedUnit;
        }

        private void Awake() {
            All.Add(this);
        }

        public static void DestroyAll() {
            foreach (var targetButton in All) {
                Destroy(targetButton.gameObject);
            }
            All.Clear();
        }

        public void TargetSelected() {
            ChangeColour(new Color32(191, 30, 46, 255));
            selectBox.SetActive(true);
        }

        public void SetHitChance(float hitChance) => hitChanceText.GetComponent<Text>().text = hitChance + "%";

        private void OnClick() {
            OnTargetButtonClick?.Invoke(this, new TargetButtonClickArgs{ SelectedUnit = targetUnit });
        } 
        
        private void OnHover() {
            ChangeColour(new Color32(191, 30, 46, 255));
            hitChanceText.SetActive(true);
        }

        private void DeHover() {
            if (Unit.ActiveUnit.targetUnit != targetUnit)
                ChangeColour(new Color32(138, 31, 43, 215));
            hitChanceText.SetActive(false);
        }

        private void ChangeColour(Color32 color) => gameObject.GetComponent<Image>().color = color;

        public void OnPointerEnter(PointerEventData eventData) => OnHover();
        public void OnPointerExit(PointerEventData eventData) => DeHover();
        public void OnPointerClick(PointerEventData eventData) => OnClick();
    }
}
