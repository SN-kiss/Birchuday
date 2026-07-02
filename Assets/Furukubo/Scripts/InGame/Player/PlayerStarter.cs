using System.Collections;
using UnityEngine;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class PlayerStarter : MonoBehaviour
    {
        [SerializeField] private float _waitTime;
        [SerializeField] private PlayerBodyMove _move;
        [SerializeField] private ParticleSystem _ps;
        [SerializeField] private LineRenderer _lr;
        [SerializeField] private SpriteRenderer[] _srs;

        private void Awake()
        {
            ActiveAll(false);
        }

        private void Start()
        {
            if (StageEntryInfo.Instance != null)
            {
                switch (StageEntryInfo.Instance.State)
                {
                    case StageEntryState.First:
                        Debug.Log("<color=yellow>Fisrt</color>");
                        FirstStart();
                        break;

                    case StageEntryState.Clear:
                        Debug.Log("<color=yellow>Clear</color>");
                        ClearStart();
                        break;

                    case StageEntryState.Miss:
                        Debug.Log("<color=yellow>Retry</color>");
                        MissStart();
                        break;
                }
            }
            else
            {
                Debug.Log("<color=yellow>StageEntryInfo.Instance == null => First</color>");
                FirstStart();
            }
        }

        private void FirstStart()
        {
            ActiveAll(true);
        }

        private void ClearStart()
        {
            ActiveAll(true);
        }

        private void MissStart()
        {
            if (_ps != null) _ps.Play(true);

            StartCoroutine(RetryStartCoroutine());

            IEnumerator RetryStartCoroutine()
            {
                yield return new WaitForSeconds(_waitTime);

                ActiveAll(true);
            }
        }

        private void ActiveAll(bool value)
        {
            if (_move != null) _move.SetIgnoreInput(!value);

            if (_lr != null) _lr.enabled = value;

            if (_srs != null)
            {
                foreach (var sr in _srs)
                {
                    if (sr != null) sr.enabled = value;
                }
            }
        }
    }
}