using System.Collections.Generic;
using System.Linq;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Map;
using TacticsGame.Battle.Map.UI.Targeting;
using TacticsGame.Battle.Units;
using UnityEngine;

namespace TacticsGame.Data {
    public abstract class Ability {
        public abstract string Name { get; set;}
        public abstract string Description { get; set;}
        public abstract AbilityTypes AbilityType { get; set; }
        public abstract Sprite Icon { get; set; }
        public abstract TargetingTypes TargetingType { get; set; }
        public abstract List<AbilityBehaviour> AbilityBehaviours { get; set; }
        public abstract List<AbilityCalculation> AbilityCalculations { get; set; }
        




        public List<CombatHitModifier> CombatHitModifiers { get; set; }
        protected StatusEffect ActiveUnitStatusEffect { get; set; }
        protected StatusEffect TargetUnitStatusEffect { get; set; }
        protected internal Equipment EquipmentObject { get; set; }

        public enum AbilityTypes {
            Active,
            Passive
        }
        public enum TargetingTypes {
            None = default,
            Single,
            Cone,
            Point
        }
        
        public void Targeting() {
            if (TargetingType == TargetingTypes.Cone) {
                var coneTarget = new ConeTarget();
                coneTarget.SubscribeToHoverTile();
            }

            if (TargetingType == TargetingTypes.Point) {
                var throwTarget = new ThrowTarget(10, 1, 3);
                throwTarget.SubscribeToHoverTile();
            }
        }

        public virtual void PassiveEffect(Unit effectedUnit) { }
        public virtual void TileWatchTrigger(Unit triggeredUnit) { }
        public virtual void ExplodeEffect(MapTile landingTile) { }

        public IEnumerable<AbilityBehaviour> GetAbilityBehaviours(int executionStage) {
            return AbilityBehaviours.Where(x => x.ExecutionStage == executionStage);
        }

        #region OldCode
        
        /*protected Ability() {
            AbilityPause = GameObject.Find("GameManager").GetComponent<AbilityPause>();
        }*/

        /*public virtual void Execute(bool observed = false) {
            VoiceLine(PreVoiceClip != "" ? PreVoiceClip : "Acknowledgment");
            if (PreAnimationClip == "AimWeapon") Unit.ActiveUnit.LookAtGameObject(Unit.ActiveUnit.targetUnit.gameObject);
            if (PreAnimationClip == "Crouch") ActiveUnitAnimation(PreAnimationClip);
            
            if (TargetingType == TargetingTypes.EnemyWatch) {
                Unit.ActiveUnit.watchingUnit = Unit.ActiveUnit.targetUnit;
                Unit.ActiveUnit.watchingTiles.Add(this, Unit.ActiveUnit.AllTilesInSight());
            }
            
            if (TargetingType == TargetingTypes.EnemyFire) {
                AbilityPause.StartPause(0.7F, this, "FireWeapon");
            }

            if (TargetingType == TargetingTypes.AOEDamage) {
                AbilityPause.StartPause(0.7F, this, "FireWeaponAOE");
            }

            if (ActiveUnitStatusEffect != null || TargetUnitStatusEffect != null) {
                AbilityPause.StartPause(0.7F, this, "AddStatusEffect");
            }
            
            if (TargetingType == TargetingTypes.Throw) {
                Unit.ActiveUnit.selectedEquipment = EquipmentObject;
                Unit.ActiveUnit.ThrowAnimation(Unit.ActiveUnit.selectedEquipment as Grenade);
            }
        }
        
        public void FireWeapon() {
            Unit.ActiveUnit.FireWeapon();
            var damageDealt = DamageDealt();
            Unit.ActiveUnit.targetUnit.TakeDamage(damageDealt);
            SendDamage(damageDealt);
            AbilityPause.StartPause(1.5F, this, "EndAbility");
        }

        public void FireWeaponAOE() {
            Unit.ActiveUnit.FireWeapon();
            var aoeTiles = new List<MapTile>();
            foreach (var activeUnitWatchingTile in Unit.ActiveUnit.watchingTiles.Where(activeUnitWatchingTile => activeUnitWatchingTile.Key.Name == Name)) {
                aoeTiles = activeUnitWatchingTile.Value;
            }

            foreach (var aoeTile in aoeTiles.Where(aoeTile => aoeTile.GetUnitInTile())) {
                aoeTile.GetUnitInTile().TakeDamage(15);
            }
            
            Unit.ActiveUnit.watchingTiles.Clear();
            AbilityPause.StartPause(1.5F, this, "EndAbility");
        }

        private int DamageDealt() => ToHitCalculation() ? DamageCalculation() : 0;
        
        public void SendDamage(int damage) {
            Object.FindObjectOfType<GameManager>().SendDamage(Unit.ActiveUnit.targetUnit, damage);
        }

        public void AddStatusEffect() {
            if (ActiveUnitStatusEffect != null) {
                Unit.ActiveUnit.PopUpText(ActiveUnitStatusEffect.Name);
                Unit.ActiveUnit.AddStatusEffect(ActiveUnitStatusEffect);
            }
            if (TargetUnitStatusEffect != null) {
                Unit.ActiveUnit.PopUpText(TargetUnitStatusEffect.Name);
                Unit.ActiveUnit.AddStatusEffect(TargetUnitStatusEffect);
            }
            
            AbilityPause.StartPause(1.5F, this, "EndAbility");
        }

        private void VoiceLine(string folder) {
            Unit.ActiveUnit.Speak(folder);
        }

        private void ActiveUnitAnimation(string animation) {
            Unit.ActiveUnit.PlayAnimation(animation);
        }

        public virtual void EndAbility() {
            Unit.ActiveUnit.EndTurn();
        }

        
        public virtual void TileWatchTrigger(Unit triggeredUnit) { }
        public virtual void ExplodeEffect(MapTile landingTile) { }
        public virtual void PassiveEffect(Unit effectedUnit) { }

        private bool ToHitCalculation() {
            var hitSuccess = Random.Range(0.01F, 1.00F);
            return hitSuccess < CombatHitCalc
                .CalculateHitChance(Unit.ActiveUnit, Unit.ActiveUnit.targetUnit, this)["HIT"];
        }

        private int DamageCalculation() {
            return CombatDamageCalc
                .DamageCalculation(Unit.ActiveUnit, Unit.ActiveUnit.targetUnit, this)["DAMAGE"];
        }*/
        
        #endregion


       
    }
}
