using System.Collections.Generic;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Data {
    public class AbilityUse {
        private readonly AbilityPause _abilityPause;
        private readonly List<AbilityBehaviour> _currentlyExecutingBehaviours;
        private int _behaviourStage;
        
        public AbilityUse() {
            _abilityPause = Object.FindObjectOfType<GameManager>().GetComponent<AbilityPause>();
            _currentlyExecutingBehaviours = new List<AbilityBehaviour>();
            _behaviourStage = 1;
        }

        public void ExecuteNextStage() {
            var nextBehaviours = Unit.ActiveUnit.selectedAbility.GetAbilityBehaviours(_behaviourStage);
            foreach (var executingBehaviour in nextBehaviours) {
                if (executingBehaviour.WaitForEnd) _currentlyExecutingBehaviours.Add(executingBehaviour);
                executingBehaviour.Execute();
            }
            
            if (_currentlyExecutingBehaviours.Count == 0) _abilityPause.EndTurnPause(0.5F);
        }

        public void RemoveBehaviour(AbilityBehaviour abilityBehaviour) {
            if(_currentlyExecutingBehaviours.Contains(abilityBehaviour))
                _currentlyExecutingBehaviours.Remove(abilityBehaviour);
            if (_currentlyExecutingBehaviours.Count == 0) {
                _behaviourStage++;
                ExecuteNextStage();
            }
        }
    }
}