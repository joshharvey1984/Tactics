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
        
        public Unit targetUnit;
        
        public event EventHandler<OnTargetButtonClickArgs> OnTargetButtonClick;
        public class OnTargetButtonClickArgs : EventArgs {
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

        private void OnClick() => OnTargetButtonClick?.Invoke(this, new OnTargetButtonClickArgs{ SelectedUnit = targetUnit });

        private void OnHover() {
            ChangeColour(new Color32(191, 30, 46, 255));
        }

        private void DeHover() {
            if (Unit.SelectedUnit.targetUnit != targetUnit || 
                Unit.SelectedUnit.selectedAbility.TargetingType != Ability.TargetingTypes.Enemy)
                ChangeColour(new Color32(138, 31, 43, 215));
        }
        
        public void ChangeColour(Color32 color) => gameObject.GetComponent<Image>().color = color;

        public void OnPointerEnter(PointerEventData eventData) => OnHover();
        public void OnPointerExit(PointerEventData eventData) => DeHover();
        public void OnPointerClick(PointerEventData eventData) => OnClick();
    }
}
