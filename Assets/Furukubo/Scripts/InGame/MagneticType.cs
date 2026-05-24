using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public enum MagneticType
    {
        Both,
        North,
        South,
    }

    //North o South
    //Both o North
    //Both o South
    //Both o Both
    //North x North
    //South x South

    public static class MagnetJudgement
    {
        public static bool IsAttachable(MagneticType a, MagneticType b)
        {
            if (a == MagneticType.Both || b == MagneticType.Both)
            {
                return true;
            }
            else if (a == b)
            {
                return false;
            }

            return true;
        }
    }
}