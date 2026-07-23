using UnityEngine;
using UnityEngine.Events;

namespace InGame.Gimmick
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class MultiFrags : MonoBehaviour
    {
        [SerializeField] private bool _isLockFragsWhenAllActived;
        [SerializeField, Min(1)] private int _fragCount;
        [SerializeField] private UnityEvent _onWhileAllActive;
        [SerializeField] private UnityEvent _onWhileAllPassive;

        private bool _isFragsLocked;
        private bool[] _frags;

        private void Awake()
        {
            _frags = new bool[_fragCount];
        }

        private void FixedUpdate()
        {
            if (IsAllActive())
            {
                _onWhileAllActive?.Invoke();
            }
            else
            {
                _onWhileAllPassive?.Invoke();
            }
        }

        private bool IsAllActive()
        {
            foreach (bool frag in _frags) if (!frag) return false;

            return true;
        }

        public void SetFragActiveAt(int index)
        {
            if (_isFragsLocked) return;

            if (_frags == null) return;

            int length = _frags.Length;

            if(length == 0) return;
            if (index < 0 || length <= index) return;

            _frags[index] = true;

            if(_isLockFragsWhenAllActived) _isFragsLocked = IsAllActive();
        }

        public void SetFragPassiveAt(int index)
        {
            if (_isFragsLocked) return;

            if (_frags == null) return;

            int length = _frags.Length;

            if (length == 0) return;
            if (index < 0 || length <= index) return;

            _frags[index] = false;
        }

        public void SetAllFragsActive()
        {
            if(_isFragsLocked) return;
            if (_frags == null) return;

            for (int i = 0; i < _frags.Length; i++)
            {
                _frags[i] = true;
            }
        }

        public void SetAllFragsPassive()
        {
            if (_isFragsLocked) return;
            if (_frags == null) return;

            for (int i = 0; i < _frags.Length; i++)
            {
                _frags[i] = false;
            }
        }
    }
}