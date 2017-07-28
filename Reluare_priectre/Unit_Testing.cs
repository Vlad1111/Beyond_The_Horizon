using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Reluare_priectre
{
    class Unit_Testing
    {
        static public void creare_nava(int a, int b)
        {

        }

        static public void text()
        {

        }

        static public int viata_bloc(int x)
        {
            return VALOARE_CUB.val(x);
        }

        static public float Distanta_Vector2(int x, ref Vector2 v1, ref Vector2 v2)
        {
            if(x == 1)
            {
                v1 = new Vector2(100, 100);
                v2 = new Vector2(100, 200);
            }
            else if (x == 2)
            {
                v1 = new Vector2(0, 0);
                v2 = new Vector2(300, 400);
            }
            else if (x == 3)
            {
                v1 = new Vector2(0, 0);
                v2 = new Vector2(3000, 4000);
            }
            else if (x == 4)
            {
                v1 = new Vector2(100, 100);
                v2 = new Vector2(100, 100);
            }
            else if (x == 5)
            {
                v1 = new Vector2(-100, -100);
                v2 = new Vector2(200, 300);
            }
            else if (x == 6)
            {
                v1 = new Vector2(0, 0);
                v2 = new Vector2(1, 1);
            }
            return MATH.dis(v1, v2);
        }
    }
}
