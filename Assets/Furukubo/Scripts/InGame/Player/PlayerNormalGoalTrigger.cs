using UnityEngine;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class PlayerNormalGoalTrigger : MonoBehaviour
    {
        [SerializeField] private PlayerType _player;

        public void OnHitGoal(Collider2D col)
        {
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

        private enum PlayerType
        {
            North,
            South
        }
    }
}