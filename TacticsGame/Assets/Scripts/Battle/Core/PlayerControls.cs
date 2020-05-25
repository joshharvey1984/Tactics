using System;
using UnityEngine;

namespace TacticsGame.Battle.Core {
    public class PlayerControls : MonoBehaviour
    {
        public event EventHandler OnRightClick;
        
        private void Update() {
            if (Input.GetKeyUp(KeyCode.Mouse1)) RightClick();
        }

        private void RightClick() {
            OnRightClick?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
