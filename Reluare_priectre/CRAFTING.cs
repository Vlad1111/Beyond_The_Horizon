using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reluare_priectre
{
    class CRAFTING
    {
        static public void ver(int a1, int a2, int[] vec)
        {
            int nr_comp = Game1.NR_comp;
            int nr_item = Game1.NR_item;
            int nr_subs = Game1.NR_subs;
            int nr_elem = Game1.NR_elem;
            if (a1 == 1)
            {
                int[,] crft = new int[nr_comp + 1, nr_comp + nr_elem * nr_subs + nr_item + 5];
              
                crft[2, 2] = -1;
                crft[2, nr_comp + nr_elem * nr_subs + 1 + 1] = 50;
                crft[2, nr_comp + nr_elem * nr_subs + 1 + 3] = 25;

                crft[3, 3] = -1;
                crft[3, nr_comp + nr_elem * nr_subs + 1 + 1] = 25;
                crft[3, nr_comp + nr_elem * nr_subs + 1 + 2] = 60;
                crft[3, nr_comp + nr_elem * nr_subs + 1 + 4] = 30;

                crft[4, 4] = -1;
                crft[4, nr_comp + nr_elem * nr_subs + 1 + 1] = 30;
                crft[4, nr_comp + nr_elem * nr_subs + 1 + 2] = 60;
                crft[4, nr_comp + nr_elem * nr_subs + 1 + 4] = 70;

                crft[5, 5] = -1;
                crft[5, nr_comp + nr_elem * nr_subs + 1 + 1] = 70;
                crft[5, nr_comp + nr_elem * nr_subs + 1 + 2] = 50;
                crft[5, nr_comp + nr_elem * nr_subs + 1 + 3] = 20;

                crft[6, 6] = -1;
                crft[6, nr_comp + nr_elem * nr_subs + 1 + 1] = 100;
                crft[6, nr_comp + nr_elem * nr_subs + 1 + 3] = 60;

                crft[7, 7] = -1;
                crft[7, nr_comp + nr_elem * nr_subs + 1 + 1] = 50;
                crft[7, nr_comp + nr_elem * nr_subs + 1 + 3] = 100;
                crft[7, 2] = 5;

                crft[8, 8] = -1;
                crft[8, nr_comp + nr_elem * nr_subs + 1 + 1] = 25;
                crft[8, nr_comp + nr_elem * nr_subs + 1 + 2] = 120;
                crft[8, nr_comp + nr_elem * nr_subs + 1 + 3] = 20;
                crft[8, nr_comp + nr_elem * nr_subs + 1 + 4] = 20;
                crft[8, 3] = 10;

                crft[9, 9] = -1;
                crft[9, nr_comp + nr_elem * nr_subs + 1 + 1] = 25;
                crft[9, nr_comp + nr_elem * nr_subs + 1 + 2] = 50;
                crft[9, nr_comp + nr_elem * nr_subs + 1 + 3] = 10;
                crft[9, nr_comp + nr_elem * nr_subs + 1 + 4] = 100;
                crft[9, 3] = 10;

                crft[10, 10] = -1;
                crft[10, nr_comp + nr_elem * nr_subs + 1 + 1] = 20;
                crft[10, nr_comp + nr_elem * nr_subs + 1 + 2] = 40;
                crft[10, nr_comp + nr_elem * nr_subs + 1 + 3] = 50;
                crft[10, nr_comp + nr_elem * nr_subs + 1 + 4] = 40;
                crft[10, 5] = 6;

                crft[11, 11] = -1;
                crft[10, nr_comp + nr_elem * nr_subs + 1 + 0] = 10;
                crft[11, nr_comp + nr_elem * nr_subs + 1 + 1] = 50;
                crft[11, nr_comp + nr_elem * nr_subs + 1 + 2] = 70;
                crft[11, nr_comp + nr_elem * nr_subs + 1 + 3] = 50;
                crft[11, 7] = 15;

                crft[12, 12] = -1;
                crft[12, nr_comp + nr_elem * nr_subs + 1 + 0] = 20;
                crft[12, nr_comp + nr_elem * nr_subs + 1 + 1] = 40;
                crft[12, nr_comp + nr_elem * nr_subs + 1 + 2] = 150;
                crft[12, nr_comp + nr_elem * nr_subs + 1 + 3] = 50;
                crft[12, nr_comp + nr_elem * nr_subs + 1 + 4] = 70;
                crft[12, 9] = 15;

                crft[13, 13] = -1;
                crft[13, nr_comp + nr_elem * nr_subs + 1 + 0] = 25;
                crft[13, nr_comp + nr_elem * nr_subs + 1 + 1] = 100;
                crft[13, nr_comp + nr_elem * nr_subs + 1 + 2] = 200;
                crft[13, nr_comp + nr_elem * nr_subs + 1 + 3] = 70;
                crft[13, nr_comp + nr_elem * nr_subs + 1 + 4] = 50;
                crft[13, 10] = 15;

                crft[14, 14] = -1;
                crft[14, nr_comp + nr_elem * nr_subs + 1 + 0] = 50;
                crft[14, nr_comp + nr_elem * nr_subs + 1 + 1] = 100;
                crft[14, nr_comp + nr_elem * nr_subs + 1 + 2] = 250;
                crft[14, nr_comp + nr_elem * nr_subs + 1 + 3] = 50;
                crft[14, nr_comp + nr_elem * nr_subs + 1 + 4] = 150;
                crft[14, 8] = 25;

                crft[15, 15] = -1;
                crft[15, nr_comp + nr_elem * nr_subs + 1 + 0] = 100;
                crft[15, nr_comp + nr_elem * nr_subs + 1 + 1] = 300;
                crft[15, nr_comp + nr_elem * nr_subs + 1 + 2] = 50;
                crft[15, nr_comp + nr_elem * nr_subs + 1 + 3] = 200;
                crft[15, nr_comp + nr_elem * nr_subs + 1 + 4] = 70;
                crft[15, 11] = 30;

                crft[16, 16] = -1;
                crft[16, nr_comp + nr_elem * nr_subs + 1 + 0] = 100;
                crft[16, nr_comp + nr_elem * nr_subs + 1 + 1] = 150;
                crft[16, nr_comp + nr_elem * nr_subs + 1 + 2] = 300;
                crft[16, nr_comp + nr_elem * nr_subs + 1 + 3] = 50;
                crft[16, nr_comp + nr_elem * nr_subs + 1 + 4] = 200;
                crft[16, 13] = 30;

                for (int nr = 1; nr < nr_comp + nr_elem * nr_subs + nr_item + 1; nr++)
                    vec[nr] = Game1.inventar[nr] - crft[a2, nr];
            }
            else if (a1 == 2)
            {
                int[,] crft = new int[nr_elem * nr_subs + 1, nr_comp + nr_elem * nr_subs + nr_item + 5];

                if ((a2 >= 1 && a2 <= 20) || (a2 >= 36 && a2 <= 45) || (a2 >= 51 && a2 <= 55) || (a2 >= 66 && a2 <= 70))
                {
                    crft[a2, a2 + nr_comp] = 1;
                    crft[a2, nr_comp + nr_elem * nr_subs + (a2 - 1) % 5 + 1] = -1;
                }
                else if (a2 >= 61 && a2 <= 65)
                {
                    crft[a2, a2 + nr_comp] = -1;
                    crft[a2, nr_comp + nr_elem * nr_subs + 2] = 20;
                    crft[a2, nr_comp + nr_elem * nr_subs + 3] = 5;
                    crft[a2, nr_comp + nr_elem * nr_subs + 4] = 5;
                }
                else if (a2 >= 71 && a2 <= 75)
                {
                    crft[a2, a2 + nr_comp] = -1;
                    crft[a2, nr_comp + nr_elem * nr_subs + 2] = 15;
                    crft[a2, nr_comp + nr_elem * nr_subs + 3] = 5;
                    crft[a2, nr_comp + nr_elem * nr_subs + 4] = 5;
                }
                else if (a2 >= 76 && a2 <= 80)
                {
                    crft[a2, a2 + nr_comp] = -1;
                    crft[a2, nr_comp + nr_elem * nr_subs + 2] = 18;
                    crft[a2, nr_comp + nr_elem * nr_subs + 3] = 5;
                    crft[a2, nr_comp + nr_elem * nr_subs + 4] = 5;
                    crft[a2, nr_comp + nr_elem * nr_subs + 1] = 5;
                }
                else if (a2 >= 81 && a2 <= 110)
                {
                    crft[a2, a2 + nr_comp] = -1;
                    crft[a2, nr_comp + nr_elem * nr_subs + 1] = 2;
                    crft[a2, nr_comp + nr_elem * nr_subs + 2] = 2;
                    crft[a2, nr_comp + nr_elem * nr_subs + 3] = 2;
                    crft[a2, nr_comp + nr_elem * nr_subs + 4] = 2;
                    crft[a2, nr_comp + nr_elem * nr_subs + 5] = 2;
                }
                else if (a2 >= 46 && a2 <= 50)
                {
                    crft[a2, a2 + nr_comp] = -1;
                    crft[a2, nr_comp + nr_elem * nr_subs + (a2 - 46) % 5 + 1] = 25;
                }

                for (int nr = 1; nr < nr_comp + nr_elem * nr_subs + nr_item + 1; nr++)
                    vec[nr] = Game1.inventar[nr] - crft[a2, nr];
            }
            else if (a1 == 3)
            {
                for (int nr = 1; nr < nr_comp + nr_elem * nr_subs + nr_item + 1; nr++)
                    vec[nr] = Game1.inventar[nr];
                if (a2 == 9)
                {
                    vec[nr_comp + nr_elem * nr_subs + 9] += 1;
                    vec[nr_comp + nr_elem * nr_subs + 1] += -100;
                    vec[nr_comp + nr_elem * nr_subs + 2] += -50;
                    vec[nr_comp + nr_elem * nr_subs + 3] += -25;
                }
                else if(a2 == 10)
                {
                    vec[nr_comp + nr_elem * nr_subs + 10] += 1;
                    vec[nr_comp + nr_elem * nr_subs + 9] += -7;
                    vec[nr_comp + nr_elem * nr_subs + 1] += -50;
                    vec[nr_comp + nr_elem * nr_subs + 2] += -150;
                }
                else if (a2 == 11)
                {
                    vec[nr_comp + nr_elem * nr_subs + 11] += 1;
                    vec[nr_comp + nr_elem * nr_subs + 10] += -10;
                    vec[nr_comp + nr_elem * nr_subs + 1] += -100;
                    vec[nr_comp + nr_elem * nr_subs + 2] += -200;
                    vec[nr_comp + nr_elem * nr_subs + 4] = -300;
                    vec[nr_comp + nr_elem * nr_subs + 5] += -50;
                }
            }
        }
    }
}
