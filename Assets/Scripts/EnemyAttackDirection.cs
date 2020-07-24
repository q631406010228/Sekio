using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class EnemyAttackDirection
    {
        public static bool isRight;

        public static void JudgeDirection(int attackWay)
        {
            if (attackWay == 0 || attackWay == 1)
            {
                isRight = true;
            }
            else if (attackWay == 2)
            {
                isRight = false;
            }
        }

    }
}
