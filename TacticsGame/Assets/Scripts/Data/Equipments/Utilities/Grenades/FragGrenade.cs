using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TacticsGame.Data.Equipments.Utilities.Grenades {
    public class FragGrenade : Grenade {
        public sealed override string Name { get; set; }
        public sealed override int Value { get; set; }
        public sealed override List<Ability> AddedAbilities { get; set; }
        public sealed override int BlastAreaOfEffect { get; set; }
        public sealed override int ShrapnelAreaOfEffect { get; set; }
        public sealed override int BlastDamage { get; set; }
        public sealed override int ShrapnelDamage { get; set; }
        public sealed override GameObject GrenadePrefab { get; set; }
        public sealed override GameObject ExplodePrefab { get; set; }


        public FragGrenade() {
            Name = "Frag Grenade";
            Value = 100;
            BlastAreaOfEffect = 1;
            ShrapnelAreaOfEffect = 3;
            BlastDamage = 60;
            ShrapnelDamage = 30;
            AddedAbilities = new List<Ability> { new Abilities.FragGrenade() };
            GrenadePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Battle/Weapons/frag_grenade.prefab",
                typeof(GameObject)) as GameObject;
            ExplodePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Battle/Effects/SmallExplosionEffect.prefab",
                typeof(GameObject)) as GameObject;
        }
    }
}