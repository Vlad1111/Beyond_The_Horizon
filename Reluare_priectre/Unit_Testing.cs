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
        static public float Incadrarea_unghi_la_primul_cerc(float x)
        {
            return MATH.prim_cadran(x);
        }

        static public float special_sqrt(float x)
        {
            return MATH.sqrt(x);
        }

        static public void crafting(int a1, int a2, int[] comp)
        {
            CRAFTING.ver(a1, a2, comp);
            for (int i = 0; i < comp.Length; i++)
                comp[i] -= Game1.inventar[i];
        }

    }
}
