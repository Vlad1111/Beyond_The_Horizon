using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Reluare_priectre
{
    class VERIFICARE
    {
        static public int NAVA(Nava AUX, int verificare)
        {
            Vector2[] co = new Vector2[2500];
            int sf = 1, inc = 0;
            co[0] = new Vector2(18, 18);
            int[,] mat = new int[50, 50];
            mat[18, 18] = 1;

            while (inc <= sf)
            {
                int x, y;
                x = (int)co[inc].X;
                y = (int)co[inc].Y;


                for (int d = 0; d < 4; d++)
                {
                    int xx, yy;
                    xx = Game1.d1[d] + x;
                    yy = Game1.d2[d] + y;

                    if (xx >= 0 && xx < 37 && yy >= 0 && yy < 37)
                        if (AUX.comp[xx, yy] != 0)
                            if (mat[xx, yy] != 1)
                            {
                                co[sf++] = new Vector2(xx, yy);
                                mat[xx, yy] = 1;
                            }
                }
                inc++;
            }

            if (verificare != 0)
                for (int i = 0; i < 37; i++)
                    for (int j = 0; j < 37; j++)
                        if (mat[i, j] == 0 && AUX.comp[i, j] != 0)
                        {
                            AUX.eng_m -= Game1.comp[AUX.comp[i, j]].eng;
                            AUX.pow -= Game1.comp[AUX.comp[i, j]].pow;
                            AUX.comp[i, j] = 0;
                            AUX.viata[i, j] = 0;
                            AUX.nr_c--;
                        }
            return sf;
        }

        static public Nava LASER_NAVA(Nava aux, Proiectil P)
        {
            float dist = MATH.dis(aux.poz, P.poz);
            float ung = aux.rot + MATH.ung(aux.poz, P.poz);

            int x, y;
            y = -(int)(Math.Cos(ung) * dist / 20) + 18;
            x = -(int)(Math.Sin(ung) * dist / 20) + 18;
            if (x >= 0 && x < 37 && y >= 0 && y < 37)
                if (aux.comp[x, y] != 0)
                {
                    P.t = 0;
                    aux.viata[x, y] -= P.pow;
                    if (aux.viata[x, y] <= 0)
                    {
                        aux.eng_m -= Game1.comp[aux.comp[x, y]].eng;
                        aux.pow -= Game1.comp[aux.comp[x, y]].pow;
                        aux.viata[x, y] = 0;
                        aux.comp[x, y] = 0;
                        aux.nr_c--;
                        NAVA(aux, 1);
                    }
                }
            return aux;
        }
    }
}
