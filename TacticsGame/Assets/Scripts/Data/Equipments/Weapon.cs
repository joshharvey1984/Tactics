using UnityEngine;

namespace TacticsGame.Data.Equipments {
    public abstract class Weapon : Equipment {
        public abstract bool Melee { get; set; }
        public abstract bool Ranged { get; set; }
        public abstract int Damage { get; set; }
        public virtual int RangeDropOff { get; set; }
        public abstract GameObject WeaponModel { get; set; }
        public abstract Quaternion SlotRotation { get; set; }
        public abstract AudioClip FireSound { get; set; }
    }
}