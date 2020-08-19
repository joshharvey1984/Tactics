using UnityEditor;
using UnityEngine;

namespace TacticsGame.Battle.UI {
    public class UIAudio : MonoBehaviour {
        private AudioSource[] _audioSources;

        private void Awake() {
            _audioSources = GetComponents<AudioSource>();
        }
        
        public void Play(AudioClip clipToPlay, float volume = 0.5F, float pitch = 1.0F, int sourceNum = 0) {
            _audioSources[sourceNum].volume = volume;
            _audioSources[sourceNum].pitch = pitch;
            _audioSources[sourceNum].PlayOneShot(clipToPlay);
        }
        
        public AudioClip PathToAudioClip(string audioClipPath) => Resources.Load(audioClipPath, typeof(AudioClip)) as AudioClip;

        
    }
}