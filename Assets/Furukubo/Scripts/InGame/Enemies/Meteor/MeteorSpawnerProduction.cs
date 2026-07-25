using UnityEngine;

namespace InGame.Gimmick
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class MeteorSpawnerProduction : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _scaleCurve;
        [SerializeField] private AnimationCurve _offsetCurve;
        [SerializeField] private Transform _trSpr;
        [SerializeField] private SpriteRenderer _srSpr;
        [SerializeField] private ParticleSystem _ps;

        public void SetSpawnTimeIntervalRatio(float ratio, float shotAngle)
        {
            if (_trSpr == null) return;
            if(_srSpr == null) return;

            Vector2 dir = -OriginalCalculateUtils.AngleToDirection(shotAngle);
            float offset = _offsetCurve?.Evaluate(ratio) ?? ratio;
            _trSpr.localPosition = Vector2.LerpUnclamped(dir, Vector2.zero, offset);

            _trSpr.localEulerAngles = new Vector3(0f, 0f, Mathf.LerpUnclamped(360f, 0f, offset));

            _srSpr.color = Color.Lerp(Color.black, Color.white, ratio);

            float scale = _scaleCurve?.Evaluate(ratio) ?? ratio;
            _trSpr.localScale = new Vector3(scale, scale, 1f);
        }

        public void PlaySpawnParticle()
        {
            if (_ps == null) return;
            _ps.Play(true);
        }
    }
}