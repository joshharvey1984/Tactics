namespace TacticsGame.Data.Equipments {
    public abstract class Armour : Equipment {
        public abstract int DamageReduction { get; set; }
        public abstract int MoveReduction { get; set; }
    }
}