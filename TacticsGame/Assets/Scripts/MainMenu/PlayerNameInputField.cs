using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace TacticsGame.MainMenu {
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour {
        private const string PlayerNamePrefKey = "PlayerName";
        private void Start() {
            var defaultName = string.Empty;
            var inputField = GetComponent<InputField>();

            if (inputField != null) {
                if (PlayerPrefs.HasKey(PlayerNamePrefKey)) {
                    defaultName = PlayerPrefs.GetString(PlayerNamePrefKey);
                    inputField.text = defaultName;
                }
            }
        }

        public void SetPlayerName(string value) {
            PlayerPrefs.SetString(PlayerNamePrefKey, value);
        }
    }
}