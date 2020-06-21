using TacticsGame.Battle.Units;
using UnityEngine;
using UnityEngine.UI;

namespace TacticsGame.Battle.UI {
    public class TextPopup : MonoBehaviour
    {
        private Text _text;
        private Transform _transform;
        private Unit _unit;
        private Camera _mainCamera;
        private float _yPos;
    
        public void TrueStart(Unit unit, string text)
        {
            _unit = unit;
            _text = gameObject.GetComponent<Text>();
            _text.text = text;
            _transform = transform;
            _mainCamera = Camera.main;
            _yPos = _mainCamera.WorldToScreenPoint(_unit.transform.position).y;
        }

        private void Update()
        {
            var pos = _mainCamera.WorldToScreenPoint(_unit.transform.position);
            pos.y += 1;
            transform.position = pos;
        
            var col = _text.color;
            col.a -= 0.35F * Time.deltaTime;
            _text.color = col;

            if (col.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
