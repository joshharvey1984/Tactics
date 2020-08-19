using TacticsGame.Battle.Map;
using UnityEngine;

namespace TacticsGame.Data.Equipments.Utilities {
    public abstract class Grenade : Equipment {
        public abstract int BlastAreaOfEffect { get; set; }
        public virtual int ShrapnelAreaOfEffect { get; set; }
        public virtual int BlastDamage { get; set; }
        public virtual int ShrapnelDamage { get; set; }
        public abstract GameObject GrenadePrefab { get; set; }
        public abstract GameObject ExplodePrefab { get; set; }
        public abstract AudioClip ExplosionAudio { get; set; }
    }
}