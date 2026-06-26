using UnityEngine;

namespace InGame.Player
{
    public class PlayerSpriteChange : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _tearParticle;
        [SerializeField] private SpriteRenderer _bodySr;
        [SerializeField] private SpriteRenderer _lipSr;
        [SerializeField] private SpriteRenderer _bodyLightSr;
        [SerializeField] private Sprite[] _bodySprites;
        [SerializeField] private Sprite[] _lipSprites;

        public void ChangeSprites(int health)
        {
            if (_bodySr == null) return;
            if (_lipSr == null) return;
            if (_bodySprites == null) return;
            if (_lipSprites == null) return;
            if (health < 0 || _bodySprites.Length <= health) return;
            if (health < 0 || _lipSprites.Length <= health) return;

            _bodySr.sprite = _bodySprites[health];
            _lipSr.sprite = _lipSprites[health];

            if (health <= 1) _tearParticle.Play(true);
            else _tearParticle.Stop(true);

            if(_bodyLightSr != null) _bodyLightSr.enabled = 2 <= health;
        }
    }
}