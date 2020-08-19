using System;
using System.Collections.Generic;
using TacticsGame.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TacticsGame.Battle.UI {
    public class AbilityButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
        private static readonly List<AbilityButton> All = new List<AbilityButton>();

        private UIAudio _uiAudio;
        
        private Image _myIcon;
        private Ability _ability;

        public event EventHandler<OnAbilityButtonClickArgs> OnAbilityButtonClick;
        public class OnAbilityButtonClickArgs : EventArgs {
            public Ability SelectedAbility;
        }

        private void Awake() {
            All.Add(this);
            _myIcon = transform.GetChild(0).gameObject.GetComponent<Image>();
            _uiAudio = FindObjectOfType<UIAudio>();
        }

        public static void DestroyAll() {
            foreach (var actionButton in All) {
                Destroy(actionButton.gameObject);
            }
            All.Clear();
        }

        private void OnDeHover() {
            if (_myIcon != null) transform.localScale = Vector3.one;
        }

        private void OnHover() {
            if (_myIcon != null) transform.localScale = Vector3.one * 1.2f;
            _uiAudio.Play(_uiAudio.PathToAudioClip("Audio/UI/AbilityButton"), 0.125F);
        }
        
        private void OnClick() => OnAbilityButtonClick?.Invoke(this, new OnAbilityButtonClickArgs{ SelectedAbility = _ability });
        public void AssignAbility(Ability ability) => _ability = ability;
        public void SetIcon(Sprite icon) => _myIcon.sprite = icon;
        public void OnPointerEnter(PointerEventData eventData) => OnHover();
        public void OnPointerExit(PointerEventData eventData) => OnDeHover();
        public void OnPointerClick(PointerEventData eventData) => OnClick();
    }
}