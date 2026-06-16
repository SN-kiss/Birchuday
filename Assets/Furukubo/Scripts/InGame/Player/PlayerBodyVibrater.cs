using System.Collections;
using UnityEngine;

namespace InGame.Player
{
    public class PlayerBodyVibrater : MonoBehaviour
    {
        [SerializeField] private int _count;
        [SerializeField] private float _interval;
        [SerializeField] private float _speed;
        [SerializeField] private Vector2 _scale;

        public IEnumerator VibrateCoroutine()
        {
            for (int i = _count - 1; 0 <= i; i--)
            {
                Vector2 dir = OriginalCalculateUtils.AngleToDirection(Random.Range(0f, 360f));
                float ratio = (float)i / _count;
                float x = dir.x * Random.Range(0f, _scale.x) * ratio;
                float y = dir.y * Random.Range(0f, _scale.y) * ratio;

                transform.localPosition = new Vector2(x, y);

                yield return new WaitForSeconds(_interval);
            }

            transform.localPosition = Vector2.zero;
        }
    }
}