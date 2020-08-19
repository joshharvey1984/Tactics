using UnityEngine;

namespace TacticsGame.MainMenu {
    public class DoNotDestroy : MonoBehaviour {
        private void Awake() {
            DontDestroyOnLoad(gameObject);
        }
    }
}