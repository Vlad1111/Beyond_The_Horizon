using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Reluare_priectre
{
    class MATH
    {
        static public int semn(float x)
        {
            if (x == 0)
                return 0;
            if (x > 0)
                return 1;
            return -1;
        }

        static public float dis(Vector2 A, Vector2 B)
        {
            return (float)Math.Sqrt((A.X - B.X) * (A.X - B.X) + (A.Y - B.Y) * (A.Y - B.Y));
        }
        static public float dis_prt(Vector2 A, Vector2 B)
        {
            return (A.X - B.X) * (A.X - B.X) + (A.Y - B.Y) * (A.Y - B.Y);
        }

        static public float ung(Vector2 A, Vector2 B)
        {
            return (float)Math.Atan2(A.X - B.X, A.Y - B.Y);
        }

        static public float sqrt(float x)
        {
            return (float)Math.Sqrt(x * semn(x)) * semn(x);
        }
    }
}
