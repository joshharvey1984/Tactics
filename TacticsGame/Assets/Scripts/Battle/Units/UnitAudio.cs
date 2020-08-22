using System.Collections;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Battle.Units {
    public class UnitAudio : MonoBehaviour {
        [SerializeField] private AudioSource[] audioSources;
        
        public bool sprinting = false;

        public void Play(AudioClip clipToPlay, float volume = 0.5F, float pitch = 1.0F, int sourceNum = 0) {
            audioSources[sourceNum].volume = volume;
            audioSources[sourceNum].pitch = pitch;
            audioSources[sourceNum].PlayOneShot(clipToPlay);
        }

        private void StopAllSounds() {
            foreach (var audioSource in audioSources) audioSource.Stop();
        }

        public void SprintSound() {
            sprinting = true;
            Play(PathToAudioClip("Audio/SFX/Movement/ClotheRustle/rustle 1"), volume: 0.2F, sourceNum: 1);
            StartCoroutine(Footsteps("Audio/SFX/Movement/Footsteps/Concrete"));
        }

        public void StopSprintSound() {
            StopAllSounds();
            sprinting = false;
        }
        
        public AudioClip PathToAudioClip(string audioClipPath) => Resources.Load(audioClipPath, typeof(AudioClip)) as AudioClip;

        private AudioClip GetRandomSound(string folder) {
            var clips = Resources.LoadAll(folder, typeof(AudioClip));
            /*var clipPath = new string[clips.Length];
            for (var i = 0; i < clips.Length; i++) {
                clipPath[i] = AssetDatabase.GUIDToAssetPath(clips[i]);
            }*/

            return clips[Random.Range(0, clips.Length)] as AudioClip;
        }

        private IEnumerator Footsteps(string clipFolder) {
            if (sprinting) {
                var clip = GetRandomSound(clipFolder);
                Play(clip, 0.03F, 0.5F);
                yield return new WaitForSeconds(0.33F);
                StartCoroutine(Footsteps(clipFolder));
            }
        }
    }
}