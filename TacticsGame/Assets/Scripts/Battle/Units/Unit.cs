using System;
using System.Collections.Generic;
using System.Linq;
using TacticsGame.Battle.Map;
using TacticsGame.Battle.Map.UI;
using TacticsGame.Battle.UI;
using TacticsGame.Data;
using TacticsGame.Data.Abilities;
using UnityEngine;

namespace TacticsGame.Battle.Units {
    public class Unit : MonoBehaviour
    {
        public static readonly List<Unit> All = new List<Unit>();
        public static Unit SelectedUnit;

        private Transform _transform;
        private List<SkinnedMeshRenderer> _meshRenderers;
        [SerializeField] private GameObject weaponGameObject;

        public Weapon weapon;
        public Armour armour;
        public List<Ability> abilities = new List<Ability>();

        private UnitMovement _unitMovement;
        private List<MapTile> _moveTiles;
        private int _moveNum;

        public Unit targetUnit;

        private static AbilityPanel _abilityPanel;
        private static TargetPanel _targetPanel;

        public int gang;
        public string unitName;
        
        public bool moveTaken;
        public bool turnTaken;

        public int movePoints = 8;
        public int aim = 10;
        public int defence = 10;

        private void Awake() {
            All.Add(this);
            
            _transform = gameObject.transform;
            _unitMovement = GetComponent<UnitMovement>();
            _unitMovement.unit = this;

            var objectRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            _meshRenderers = objectRenderers.ToList();
            
            _abilityPanel = GameObject.Find("AbilityPanel").GetComponent<AbilityPanel>();
            _targetPanel = GameObject.Find("TargetPanel").GetComponent<TargetPanel>();
            
            abilities.Add(new FireAbility());
        }

        public void MoveUnit(List<MapTile> tiles) {
            _moveTiles = tiles;
            _moveNum = 0;
            MoveNextTile();
        }

        public void MoveNextTile() {
            _moveNum++;
            if (_moveNum == _moveTiles.Count - 1) EndMoveTurn();
            else _unitMovement.SetNextTile(_moveTiles[_moveNum]);
        }

        private void EndMoveTurn() {
            _unitMovement.EndMove();
            moveTaken = true;
            _targetPanel.UpdateTargetPanel(EnemiesInLineOfSight());
        }

        public void ExecuteAbility(Ability ability, Unit aTargetUnit = null) {
            targetUnit = aTargetUnit; 
            ability.Execute();
        }

        public MapTile GetCurrentMapTile() => MapTile.GetMapTileFromPos(Convert.ToInt32(_transform.position.x), 
            Convert.ToInt32(_transform.position.z));

        public void StartTurn() {
            MovementUI.DrawMovementUI(this);
            _abilityPanel.CreateAbilityButtons();
            _targetPanel.UpdateTargetPanel(EnemiesInLineOfSight());
        }

        private List<Unit> EnemiesInLineOfSight() => All.Where(unit => unit.gang != gang)
                .Where(unit => GetCurrentMapTile().CanSeeOtherTile(unit.GetCurrentMapTile(), 20.0F))
                .ToList();

        private void DrawUnit() {
            foreach (var meshRenderer in _meshRenderers) {
                meshRenderer.enabled = true;
            }
        }
        
        private void HideUnit() {
            foreach (var meshRenderer in _meshRenderers) {
                meshRenderer.enabled = false;
            }
        }
    }
}
