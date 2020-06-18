using UnityEngine;
using System.Collections.Generic;
using TacticsGame.Data;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public new string name;
    public int value;
    public bool ballistic;
    public bool melee;
    public int damage;
    public enum IdealRange
    {
        Short,
        Medium,
        Long
    }
    public IdealRange range;
    public List<Ability> abilities;
}