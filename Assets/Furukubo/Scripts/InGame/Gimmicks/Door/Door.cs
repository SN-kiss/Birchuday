using UnityEditor;
using UnityEngine;

namespace InGame.Gimmick
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class Door : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField, Range(0, 1f)] private float _timeDoorOpen;
        [SerializeField, Range(0, 1f)] private float _timeSpriteChangeThreashoud;
        [SerializeField, Min(1f)] private float _doorRadius;
        [SerializeField, Min(1f)] private float _openRadius;
        [SerializeField, Min(0.05f)] private float _openSpeed;
        [SerializeField] private AnimationCurve _openCurve;

        [Header("References")]
        [SerializeField] private Sprite _sprClose;
        [SerializeField] private Sprite _sprOpen;
        [Header("Door Left")]
        [SerializeField] private Rigidbody2D _rbLeft;
        [SerializeField] private BoxCollider2D _colLeft;
        [SerializeField] private SpriteRenderer _srLeft;
        [Header("Door Right")]
        [SerializeField] private Rigidbody2D _rbRight;
        [SerializeField] private BoxCollider2D _colRight;
        [SerializeField] private SpriteRenderer _srRight;

        public void OnOpenUpdate()
        {
            if (1f <= _timeDoorOpen) return;

            _timeDoorOpen = Mathf.Clamp01(_timeDoorOpen + _openSpeed * Time.fixedDeltaTime);
            float curvedTime = _openCurve?.Evaluate(_timeDoorOpen) ?? 0f;

            UpdateDoorPositions(curvedTime);
        }

        public void OnCloseUpdate()
        {
            if (_timeDoorOpen <= 0f) return;

            _timeDoorOpen = Mathf.Clamp01(_timeDoorOpen - _openSpeed * Time.fixedDeltaTime);
            float curvedTime = _openCurve?.Evaluate(_timeDoorOpen) ?? 0f;
            
            UpdateDoorPositions(curvedTime);
        }

        private void UpdateDoorPositions(float t)
        {
            if (_rbLeft != null)
            {
                Vector2 leftClosePos = transform.TransformPoint(new Vector2(-_doorRadius * 0.5f, 0f));
                Vector3 leftOpenPos = transform.TransformPoint(new Vector2(-(_doorRadius * 0.5f + _openRadius), 0f));
                Vector2 left = Vector2.Lerp(leftClosePos, leftOpenPos, t);
                _rbLeft.MovePosition(left);
            }

            if (_srLeft != null)
            {
                _srLeft.sprite = t < _timeSpriteChangeThreashoud ? _sprClose : _sprOpen;
            }

            if (_rbRight != null)
            {
                Vector2 rightClosePos = transform.TransformPoint(new Vector2(_doorRadius * 0.5f, 0f));
                Vector3 rightOpenPos = transform.TransformPoint(new Vector2(_doorRadius * 0.5f + _openRadius, 0f));
                Vector2 right = Vector2.Lerp(rightClosePos, rightOpenPos, t);
                _rbRight.MovePosition(right);
            }

            if (_srRight != null)
            {
                _srRight.sprite = t < _timeSpriteChangeThreashoud ? _sprClose : _sprOpen;
            }
        }

        private void OnValidateDoorsWhileEditor()
        {
            float curvedTime = _openCurve?.Evaluate(_timeDoorOpen) ?? 0f;

            if (_srLeft != null)
            {
                _srLeft.size = new Vector2(_doorRadius, 1f);

                _srLeft.sprite = curvedTime < _timeSpriteChangeThreashoud ? _sprClose : _sprOpen;
            }

            if (_colLeft != null)
            {
                _colLeft.size = new Vector2(_doorRadius, 1f);
            }

            if (_rbLeft != null)
            {
                Vector2 leftClosePos = transform.TransformPoint(new Vector2(-_doorRadius * 0.5f, 0f));
                Vector3 leftOpenPos = transform.TransformPoint(new Vector2(-(_doorRadius * 0.5f + _openRadius), 0f));
                Vector2 left = Vector2.Lerp(leftClosePos, leftOpenPos, curvedTime);
                _rbLeft.transform.position = left;
            }

            if (_srRight != null)
            {
                _srRight.size = new Vector2(_doorRadius, 1f);

                _srRight.sprite = curvedTime < _timeSpriteChangeThreashoud ? _sprClose : _sprOpen;
            }

            if (_colRight != null)
            {
                _colRight.size = new Vector2(_doorRadius, 1f);
            }

            if (_rbRight != null)
            {
                Vector2 rightClosePos = transform.TransformPoint(new Vector2(_doorRadius * 0.5f, 0f));
                Vector3 rightOpenPos = transform.TransformPoint(new Vector2(_doorRadius * 0.5f + _openRadius, 0f));
                Vector2 right = Vector2.Lerp(rightClosePos, rightOpenPos, curvedTime);
                _rbRight.transform.position = right;
            }
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            EditorApplication.delayCall += () =>
            {
                OnValidateDoorsWhileEditor();
            };
#endif
        }
    }
}