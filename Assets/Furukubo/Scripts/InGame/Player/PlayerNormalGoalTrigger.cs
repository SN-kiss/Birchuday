using UnityEngine;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class PlayerNormalGoalTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerType _player;

        private bool _ignore;

        public void OnHitGoal(Collider2D col)
        {
            if (_ignore)
            {
                return;
            }

            if (col.TryGetComponent(out NormalGoal goal))
            {
                if (_player == PlayerType.North)
                {
                    goal.OnGoalPlayerNorth();
                }
                else
                {
                    goal.OnGoalPlayerSouth();
                }
            }
        }

        public void SetIgnoreGoal(bool value) => _ignore = value;

        private enum PlayerType
        {
            North,
            South
        }
    }
}