using System;
using UnityEngine;

namespace TacticsGame.Battle.Core {
    public class PlayerControls : MonoBehaviour
    {
        public event EventHandler OnRightClick;
        public event EventHandler OnLeftClick;
        public event EventHandler OnTabPress;
        
        private void Update() {
            if (Input.GetKeyUp(KeyCode.Mouse1)) RightClick();
            if (Input.GetKeyDown(KeyCode.Mouse0)) LeftClick();
            if (Input.GetKeyUp(KeyCode.Tab)) TabPress();
        }

        private void LeftClick() {
            OnLeftClick?.Invoke(this, EventArgs.Empty);
        }

        private void RightClick() {
            OnRightClick?.Invoke(this, EventArgs.Empty);
        }

        private void TabPress() {
            OnTabPress?.Invoke(this, EventArgs.Empty);
        }
    }
}
