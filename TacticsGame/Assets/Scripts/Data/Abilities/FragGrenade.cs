using System.Collections.Generic;
using TacticsGame.Battle.Map;
using TacticsGame.Battle.Map.UI.Targeting;
using UnityEngine;

namespace TacticsGame.Data.Abilities {
    public sealed class FragGrenade : Ability {
        public sealed override string Name { get; set; }
        public sealed override string Description { get; set; }
        public sealed override AbilityTypes AbilityType { get; set; }
        public sealed override Sprite Icon { get; set; }
        public sealed override TargetingTypes TargetingType { get; set; }
        public override List<AbilityBehaviour> AbilityBehaviours { get; set; }
        public override List<AbilityCalculation> AbilityCalculations { get; set; }


        public FragGrenade(Equipment fragGrenade) {
            Name = "Frag Grenade";
            AbilityType = AbilityTypes.Active;
            Description = "Throw a frag grenade.";
            Icon = Resources.Load<Sprite>("Textures/Abilities/Icon_Bomb");
            TargetingType = TargetingTypes.Point;
            EquipmentObject = fragGrenade;
            AbilityBehaviours = new List<AbilityBehaviour> {
                new UnitVoiceClip(1, "Grenade"),
                new ThrowGrenade(1)
            };
        }

        public override void ExplodeEffect(MapTile landingTile) {
            var grenade = new Equipments.Utilities.Grenades.FragGrenade();
            var aoeTile = ThrowTarget.FindAOETiles(landingTile.UiTile, grenade.BlastAreaOfEffect, grenade.ShrapnelAreaOfEffect);
            foreach (var mapTile in aoeTile.Item1) {
                if (mapTile.GetUnitInTile()) {
                    mapTile.GetUnitInTile().PopUpText(grenade.BlastDamage.ToString());
                    mapTile.GetUnitInTile().TakeDamage(grenade.BlastDamage);
                }
            }
            foreach (var mapTile in aoeTile.Item2) {
                if (mapTile.GetUnitInTile()) {
                    mapTile.GetUnitInTile().PopUpText(grenade.ShrapnelDamage.ToString());
                    mapTile.GetUnitInTile().TakeDamage(grenade.ShrapnelDamage);
                }
            }
            landingTile.UiTile.AddComponent<AudioSource>().PlayOneShot(grenade.ExplosionAudio);
        }
    }
}