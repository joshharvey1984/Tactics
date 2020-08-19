using Photon.Pun;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TacticsGame.MainMenu {
    public class BattleSetup : MonoBehaviour {

        private InfoBar _infoBar;
        private GameManager _gameManager;
        private PhotonView _photonView;
        private PlayerGang _playerGang;

        public void BattleSetupOnLoad() {
            _gameManager = FindObjectOfType<GameManager>();
            _photonView = _gameManager.gameObject.GetComponent<PhotonView>();
            _playerGang = FindObjectOfType<PlayerGang>();

            if (!PhotonNetwork.IsMasterClient) return;
            _gameManager.isMaster = true;
            if (Random.value >= .5) {
                _gameManager.gangNumber = 1;
                _playerGang.SetGangNumber(1);
                SendGangNumber(0);
            }
            else {
                _gameManager.gangNumber = 0;
                _playerGang.SetGangNumber(0);
                SendGangNumber(1);
            }
        }

        [PunRPC]
        private void SendGangNumber(int gangNumber) {
            _photonView.RPC("GetGangNumber", RpcTarget.Others, gangNumber);
        }
    }
}