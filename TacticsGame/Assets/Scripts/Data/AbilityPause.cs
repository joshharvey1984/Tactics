using System.Collections;
using System.Reflection;
using UnityEngine;

namespace TacticsGame.Data {
    public class AbilityPause : MonoBehaviour
    {
        public void StartPause(float seconds, Ability ability, string returnMethod) {
            StartCoroutine(Pause(seconds, ability, returnMethod));
        }
        private IEnumerator Pause(float seconds, Ability ability, string returnMethod) {
            yield return new WaitForSeconds(seconds);
            var methodInfo = ability.GetType().GetMethod(returnMethod);
            methodInfo.Invoke(ability, null);
        }
    }
}
