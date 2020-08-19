using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TacticsGame.MainMenu {
    public class Launcher : MonoBehaviourPunCallbacks {
        private BattleSetup _battleSetup;
        
        private const string GameVersion = "1";
        
        [SerializeField] private Text status;
        [SerializeField] private Text playerCount;
        [SerializeField] private Button joinGame;

        private bool _findingGame = false;

        private void Awake() {
            SceneManager.sceneLoaded += OnBattleLoaded;
            _battleSetup = gameObject.GetComponent<BattleSetup>();
            joinGame.gameObject.SetActive(false);
        }
        
        private void Start() {
            Connect();
        }

        private void Connect() {
            status.text = "Connecting...";
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = GameVersion;
        }

        public override void OnConnectedToMaster() {
            playerCount.text = PhotonNetwork.CountOfPlayers.ToString();
            joinGame.gameObject.SetActive(true);
            status.text = "Connected";
        }

        public void JoinGame() {
            if (!_findingGame) {
                PhotonNetwork.JoinRandomRoom();
                _findingGame = true;
                joinGame.GetComponentInChildren<Text>().text = "Cancel";
            }
            else {
                PhotonNetwork.LeaveRoom();
                _findingGame = false;
                joinGame.GetComponentInChildren<Text>().text = "Find Game";
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message) {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
            status.text = "Finding opponent...";
        }

        public override void OnJoinedRoom() {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2) {
                SetUpBattle();
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer) {
            SetUpBattle();
        }

        public void DebugMode() {
            SceneManager.LoadScene(1);
        }

        private void SetUpBattle() {
            PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName");
            SceneManager.LoadScene(1);
        }

        private void OnBattleLoaded(Scene scene, LoadSceneMode mode) {
            if (scene.name == "Battle") _battleSetup.BattleSetupOnLoad();
        }
    }
}