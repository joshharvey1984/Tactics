using System.Collections;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Data {
    public class AbilityPause : MonoBehaviour
    {
        public void EndTurnPause(float seconds) {
            StartCoroutine(Pause(seconds));
        }
        
        private static IEnumerator Pause(float seconds) {
            yield return new WaitForSeconds(seconds);
            Unit.ActiveUnit.EndTurn();
        }
    }

    /*public class AbilityStagePause : MonoBehaviour {
        private Ability Ability { get; }
        private int AbilityStage { get; set; }
        public AbilityStagePause(Ability ability, int abilityStage, float seconds) {
            Ability = ability;
            AbilityStage = abilityStage;
            StartCoroutine(Pause(seconds));
        }    
        
        private IEnumerator Pause(float seconds) {
            yield return new WaitForSeconds(seconds);
            AbilityStage++;
            new AbilityUse(Ability, AbilityStage);
        }
    }*/
}
