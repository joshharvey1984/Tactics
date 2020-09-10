using System.Linq;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Data {
    public abstract class AbilityBehaviour {
        public abstract int ExecutionStage { get; set; }
        public abstract bool WaitForEnd { get; set; }
        public abstract void Execute();
    }
    
    internal sealed class DamageUnitsInAOE : AbilityBehaviour {
        public override int ExecutionStage { get; set; }
        public override bool WaitForEnd { get; set; }

        public DamageUnitsInAOE(int executionStage, bool waitForEnd = false) {
            ExecutionStage = executionStage;
            WaitForEnd = waitForEnd;
        }

        public override void Execute() {
            var aoeTiles = Unit.ActiveUnit.aoeTargetTiles.First(x => x.Key == Unit.ActiveUnit.selectedAbility);
            foreach (var mapTile in aoeTiles.Value) {
                mapTile.GetUnitInTile()?.TakeDamage(40);
            }
        }
    }
    
    internal sealed class UnitTakeDamage : AbilityBehaviour {
        public override int ExecutionStage { get; set; }
        public override bool WaitForEnd { get; set; }

        public UnitTakeDamage(int executionStage, bool waitForEnd = false) {
            ExecutionStage = executionStage;
            WaitForEnd = waitForEnd;
        }

        public override void Execute() {
            Unit.ActiveUnit.targetUnit.TakeDamage(Unit.ActiveUnit.damageCalculation);
        }
    }

    internal sealed class FireWeapon : AbilityBehaviour {
        public override int ExecutionStage { get; set; }
        public override bool WaitForEnd { get; set; }

        public FireWeapon(int executionStage, bool waitForEnd = false) {
            ExecutionStage = executionStage;
            WaitForEnd = waitForEnd;
        }

        public override void Execute() {
            Unit.ActiveUnit.FireWeapon();
        }
    }

    internal sealed class AddStatusEffect : AbilityBehaviour {
        public override int ExecutionStage { get; set; }
        public override bool WaitForEnd { get; set; }
        private StatusEffect StatusEffect { get; set; }

        public AddStatusEffect(int executionStage, StatusEffect statusEffect, bool waitForEnd = false) {
            ExecutionStage = executionStage;
            StatusEffect = statusEffect;
            WaitForEnd = waitForEnd;
        }

        public override void Execute() {
            Unit.ActiveUnit.AddStatusEffect(StatusEffect);
        }
    }

    internal sealed class AddStatusEffectToTarget : AbilityBehaviour {
        public override int ExecutionStage { get; set; }
        public override bool WaitForEnd { get; set; }
        private StatusEffect StatusEffect { get; set; }

        public AddStatusEffectToTarget(int executionStage, StatusEffect statusEffect, bool waitForEnd = false) {
            ExecutionStage = executionStage;
            StatusEffect = statusEffect;
            WaitForEnd = waitForEnd;
        }

        public override void Execute() {
            Unit.ActiveUnit.targetUnit.AddStatusEffect(StatusEffect);
        }
    }

    internal sealed class UnitVoiceClip : AbilityBehaviour {
        public override int ExecutionStage { get; set; }
        public override bool WaitForEnd { get; set; }
        private string VoiceClip { get; set; }

        public UnitVoiceClip(int executionStage, string voiceClip, bool waitForEnd = true) {
            ExecutionStage = executionStage;
            VoiceClip = voiceClip;
            WaitForEnd = waitForEnd;
        }

        public override void Execute() {
            Unit.ActiveUnit.Speak(VoiceClip);
        }
    }

    internal sealed class UnitAnimation : AbilityBehaviour {
        public override int ExecutionStage { get; set; }
        public override bool WaitForEnd { get; set; }
        private string AnimationName { get; set; }

        public UnitAnimation(int executionStage, string animationName, bool waitForEnd = false) {
            ExecutionStage = executionStage;
            AnimationName = animationName;
            WaitForEnd = waitForEnd;
        }

        public override void Execute() {
            if (AnimationName == "AimWeapon") {
                Unit.ActiveUnit.LookAtGameObject(Unit.ActiveUnit.targetUnit
                    ? Unit.ActiveUnit.targetUnit.gameObject
                    : Unit.ActiveUnit.selectedMapTile.UiTile);
                return;
            }

            Unit.ActiveUnit.PlayAnimation(AnimationName);
        }
    }
    
    internal sealed class ThrowGrenade : AbilityBehaviour {
        public override int ExecutionStage { get; set; }
        public override bool WaitForEnd { get; set; }

        public ThrowGrenade(int executionStage, bool waitForEnd = false) {
            ExecutionStage = executionStage;
            WaitForEnd = waitForEnd;
        }

        public override void Execute() {
            Unit.ActiveUnit.ThrowAnimation(Unit.ActiveUnit.selectedAbility.EquipmentObject);
        }
    }
}