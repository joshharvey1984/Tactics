using TacticsGame.Battle.Map.Enums;
using UnityEngine;

namespace TacticsGame.Battle.Map.UI
{
    public class CoverIcon : MonoBehaviour
    {
        private Vector3 _position;

        private void Start()
        {
            _position = transform.position;
        }

        private void Update()
        {
            if (transform.position.y < 0.5F) {
                _position.y += 4.0F * Time.deltaTime;
                transform.position = _position;
            }
        }
    }
}
