using UnityEngine;
using Random = UnityEngine.Random;

namespace TacticsGame.Battle.Units {
    public class UnitVoice : MonoBehaviour {
        [SerializeField] private AudioSource audioSource;
        
        private Unit _unit;
        private bool _speaking;

        private void Awake() {
            _unit = GetComponent<Unit>();
        }

        public void PlayVoiceClip(string folder, float volume) {
            if (audioSource.isPlaying) return;
            _speaking = true;
            audioSource.volume = volume;
            audioSource.PlayOneShot(GetRandomClip(folder));
        }

        private AudioClip GetRandomClip(string folder) {
            var path = _unit.VoicePath + folder;
            var clips = Resources.LoadAll(path, typeof(AudioClip));
            return clips[Random.Range(0, clips.Length)] as AudioClip;
        }

        private void Update() {
            if (_speaking) CheckForEndVoiceClip();
        }

        private void CheckForEndVoiceClip() {
            if (audioSource.isPlaying) return;
            _speaking = false;
            _unit.EndSpeak();
        }
    }
}