using System;
using UnityEngine;
using UnityEngine.Audio;

namespace TacticsGame.Battle.Music {
    public class MixLevels : MonoBehaviour {
        public AudioMixer masterMixer;

        private void Start() {
            SetMusicLvl(0.1F);
        }

        public void SetMusicLvl(float musicLvl) {
            masterMixer.SetFloat("musicVol", musicLvl);
        }
    }
}