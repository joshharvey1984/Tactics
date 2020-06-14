using System.Collections.Generic;
using TacticsGame.Battle.Units;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TacticsGame.Battle.UI {
    public class TargetButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler 
    {
        private static readonly List<TargetButton> All = new List<TargetButton>();

        public TargetPanel targetPanel;
        public Unit targetUnit;
        
        private void Awake() {
            All.Add(this);
        }
        
        public static void DestroyAll() {
            foreach (var targetButton in All) {
                Destroy(targetButton.gameObject);
            }
            All.Clear();
        }
        
        private void OnClick() {
            targetPanel.ChangeTarget(targetUnit);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            
        }

        public void OnPointerExit(PointerEventData eventData) {
            
        }

        public void OnPointerClick(PointerEventData eventData) {
            OnClick();
        }
    }
}
