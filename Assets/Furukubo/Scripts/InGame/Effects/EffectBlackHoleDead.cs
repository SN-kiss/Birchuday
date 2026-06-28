using System;
using System.Collections;
using UnityEngine;

public class EffectBlackHoleDead : MonoBehaviour
{
    [SerializeField] private float _speedMax;
    [SerializeField] private float _speedMin;
    [SerializeField] private float _angleSpeedMax;
    [SerializeField] private float _angleSpeedMin;
    [SerializeField] private float _rotateSpeedMax;
    [SerializeField] private float _rotateSpeedMin;

    public event Action<EffectBlackHoleDead> OnReleaseToPool;

    public void StartBlackHoleAttracted(Vector2 posS, Vector2 velo, Vector2 posG, float rotS)
    {
        StartCoroutine(BlackHoleAttractedCoroutine());

        IEnumerator BlackHoleAttractedCoroutine()
        {
            float rot = rotS;
            float startRad = (posS - posG).magnitude;
            float rad = startRad;
            float ang = OriginalCalculateUtils.DirectionToAngle((posS - posG).normalized);
            float sign = CalcSign(posS, velo, posG);

            while (0f < rad)
            {
                float dt = Time.deltaTime;

                float sqrRatio = rad / startRad;
                float ratio = sqrRatio * sqrRatio;

                rad -= dt * Mathf.Lerp(_speedMax, _speedMin, ratio);
                ang += dt * 360f * Mathf.Lerp(_angleSpeedMax, _angleSpeedMin, ratio) * sign;
                rot += dt * 360f * Mathf.Lerp(_rotateSpeedMax, _rotateSpeedMin, ratio) * sign;

                transform.position = posG + OriginalCalculateUtils.AngleToDirection(ang) * rad;
                transform.localEulerAngles = new Vector3(0f, 0f, rot);

                transform.localScale = new Vector3(sqrRatio, sqrRatio, 1f);

                yield return null;
            }

            if (OnReleaseToPool == null)
            {
                Destroy(gameObject);
            }
            else
            {
                OnReleaseToPool?.Invoke(this);
            }
        }
    }

    private float CalcSign(Vector2 posS, Vector2 velo, Vector2 posG)
    {
        float curAng = OriginalCalculateUtils.DirectionToAngle((posS - posG).normalized);
        float nextAng = OriginalCalculateUtils.DirectionToAngle((posS + velo - posG).normalized);

        return 0f <= Mathf.DeltaAngle(curAng, nextAng) ? 1f : -1f;
    }
}
