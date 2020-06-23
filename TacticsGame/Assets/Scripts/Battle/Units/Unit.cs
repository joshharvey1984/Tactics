﻿using System;
using System.Collections.Generic;
using System.Linq;
using TacticsGame.Battle.Core;
using TacticsGame.Battle.Effects;
using TacticsGame.Battle.Map;
using TacticsGame.Battle.Map.Enums;
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
        public Ability selectedAbility;

        private static AbilityPanel _abilityPanel;
        private static TargetPanel _targetPanel;
        public GameObject popupTextPrefab;
        private GameObject _canvas;
        private GameManager _gameManager;

        public GameObject muzzle;
        private MuzzleFlashGenerator _muzzleFlashGenerator;
        private BulletTracerGenerator _bulletTracerGenerator;

        public int gang;
        public string unitName;
        
        public bool moveTaken;
        public bool turnTaken;

        public int movePoints = 8;
        public int hitPoints = 100;
        public int aim = 10;
        public int defence = 10;
        
        public int currentHitPoints = 100;
        public List<StatusEffect> currentStatusEffects = new List<StatusEffect>();

        private void Awake() {
            All.Add(this);
            
            _transform = gameObject.transform;
            _unitMovement = GetComponent<UnitMovement>();
            _unitMovement.unit = this;

            var objectRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            _meshRenderers = objectRenderers.ToList();

            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _canvas = GameObject.Find("Canvas");
            _abilityPanel = GameObject.Find("AbilityPanel").GetComponent<AbilityPanel>();
            _targetPanel = GameObject.Find("TargetPanel").GetComponent<TargetPanel>();

            _muzzleFlashGenerator = muzzle.GetComponent<MuzzleFlashGenerator>();
            _bulletTracerGenerator = muzzle.GetComponent<BulletTracerGenerator>();
            
            abilities.Add(new FireAbility());
            abilities.Add(new HunkerDown());
        }

        public void MoveUnit(List<MapTile> tiles) {
            _moveTiles = tiles;
            _moveNum = 0;
            _unitMovement.SetAnimation("Crouch", false);
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
            _abilityPanel.CreateAbilityButtons();
            _targetPanel.UpdateTargetPanel();
        }

        public void ExecuteAbility() {
            selectedAbility.Execute();
        }

        public MapTile GetCurrentMapTile() => MapTile.GetMapTileFromPos(Convert.ToInt32(_transform.position.x), 
            Convert.ToInt32(_transform.position.z));

        public void StartTurn() {
            StatusEffectDurationUpdate();
            MovementUI.DrawMovementUI(this);
            _abilityPanel.CreateAbilityButtons();
            _targetPanel.UpdateTargetPanel();
        }

        private void StatusEffectDurationUpdate() {
            foreach (var currentStatusEffect in currentStatusEffects) {
                currentStatusEffect.turnLength--;
            }
            currentStatusEffects.RemoveAll(effect => effect.turnLength <= 0);
        }

        public void EndTurn() {
            selectedAbility = null;
            targetUnit = null;
            turnTaken = true;
            _gameManager.EndUnitTurn();
        }

        public List<Unit> EnemiesInLineOfSight() => All.Where(unit => unit.gang != gang)
                .Where(unit => GetCurrentMapTile().CanSeeOtherTile(unit.GetCurrentMapTile(), 20.0F))
                .ToList();

        public void FireBullets(int numBullets) {
            _muzzleFlashGenerator.toFire = numBullets;
            _bulletTracerGenerator.toFire = numBullets;
        }

        public void PopUpText(string text) {
            var popUpText = Instantiate(popupTextPrefab, _canvas.transform);
            popUpText.GetComponent<TextPopup>().TrueStart(this, text);
        }
        
        public void LookAtGameObject(GameObject gameObj) => gameObject.transform.LookAt(gameObj.transform);
        public void PlayAnimation(string anim) => _unitMovement.SetAnimation(anim, true);
    }
}
