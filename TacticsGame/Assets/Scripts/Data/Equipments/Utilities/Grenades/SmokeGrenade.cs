using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data.Equipments.Utilities.Grenades {
    sealed class SmokeGrenade : Grenade {
        public override string Name { get; set; }
        public override int Value { get; set; }
        public override List<Ability> AddedAbilities { get; set; }
        public override int BlastAreaOfEffect { get; set; }
        public override GameObject GrenadePrefab { get; set; }
        public override GameObject ExplodePrefab { get; set; }
        public override AudioClip ExplosionAudio { get; set; }

        public SmokeGrenade() {
            Name = "Smoke Grenade";
            Value = 50;
            BlastAreaOfEffect = 1;
            AddedAbilities = new List<Ability> { new Abilities.SmokeGrenade() };
            GrenadePrefab = Resources.Load("Prefabs/Battle/Weapons/smoke_grenade.prefab",
                typeof(GameObject)) as GameObject;
            ExplodePrefab = Resources.Load("Prefabs/Battle/Effects/muzzleFlash.prefab",
                typeof(GameObject)) as GameObject;
        }
    }
}  