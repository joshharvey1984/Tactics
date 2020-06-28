using System;
using UnityEngine;
using UnityEngine.UI;

namespace TacticsGame.Battle.UI {
    public class InfoBar : MonoBehaviour {
        [SerializeField] private Text player0Panel;
        [SerializeField] private Text player1Panel;
        [SerializeField] private Text timerPanel;
        [SerializeField] private Text roundPanel;
        [SerializeField] private Text turnPanel;
        
        public void SetPlayerTurn(int roundNum, string playerTurn) {
            turnPanel.text = $"{playerTurn}'s Turn";
            roundPanel.text = $"Round: {roundNum}";
        }

        public void SetPlayerNames(string[] names) {
            player0Panel.text = names[0];
            player1Panel.text = names[1];
        }

        public void UpdateTimer(float timer) {
            timerPanel.text = Convert.ToInt32(timer).ToString();
        }
    }
}
