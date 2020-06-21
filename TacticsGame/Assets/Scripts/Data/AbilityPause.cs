using System.Collections;
using UnityEngine;

namespace TacticsGame.Data {
    public class AbilityPause : MonoBehaviour
    {
        public void StartPause(float seconds, Ability ability) {
            StartCoroutine(Pause(seconds, ability));
        }
        public IEnumerator Pause(float seconds, Ability ability) {
            yield return new WaitForSeconds(seconds);
            Ability.EndAbility();
        }
    }
}
