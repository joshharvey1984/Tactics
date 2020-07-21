using System;
using UnityEngine;

namespace TacticsGame.Battle.Core {
    public class PlayerControls : MonoBehaviour
    {
        public event EventHandler OnRightClick;
        public event EventHandler OnLeftClick;
        
        private void Update() {
            if (Input.GetKeyUp(KeyCode.Mouse1)) RightClick();
            if (Input.GetKeyDown(KeyCode.Mouse0)) LeftClick();
        }

        private void LeftClick() {
            OnLeftClick?.Invoke(this, EventArgs.Empty);
        }

        private void RightClick() {
            OnRightClick?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
