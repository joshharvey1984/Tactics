using UnityEngine;
using UnityEngine.Audio;

namespace TacticsGame.Battle.Music {
    public class MusicLayers : MonoBehaviour {
        public AudioMixer layersMixer;
        public AudioMixerSnapshot stealth;
        public AudioMixerSnapshot tension;
        public AudioMixerSnapshot danger;
        
        private void Start() {
            SetVolume(-15);
            stealth.TransitionTo(0);
        }

        public void SetStealth() {
            stealth.TransitionTo(0.5F);
        }

        public void SetTension() {
            tension.TransitionTo(0.5F);
        }

        public void SetDanger() {
            danger.TransitionTo(0.5F);
        }

        public void SetVolume(float volume) {
            layersMixer.SetFloat("MasterVol", volume);
        }
    }
}