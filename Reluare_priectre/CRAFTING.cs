using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reluare_priectre
{
    class CRAFTING
    {
        static public int ver(int a1, int a2)
        {
            if (a1 != 0 && a2 != 0)
            {
                if (a2 >= 0)
                {
                    if (Game1.inventar[Game1.COMP_A] > 0)
                    {
                        Game1.inventar[Game1.COMP_A]--;
                        Game1.inventar[Game1.NR_comp + Game1.NR_subs * Game1.NR_elem + a2 + 1]++;
                    }
                    else return 0;
                }
                else if (a2 == -1)
                {
                    int s1 = Game1.NR_comp + Game1.NR_subs * Game1.NR_elem + 1;
                    if (a1 == 2)    //10 fer + 5 carbon
                    {
                        if (Game1.inventar[s1 + 1] < 10 || Game1.inventar[s1 + 3] < 5)
                            return 0;
                        Game1.inventar[s1 + 1] -= 10;
                        Game1.inventar[s1 + 3] -= 5;
                        Game1.inventar[Game1.COMP_A]++;
                    }
                    else if (a1 == 3) //10 fer + 10 carbon + 20 uraniu + 5 sulfur
                    {
                        if (Game1.inventar[s1 + 1] < 10 || Game1.inventar[s1 + 3] < 10 || Game1.inventar[s1 + 4] < 20 || Game1.inventar[s1 + 2] < 5)
                            return 0;
                        Game1.inventar[s1 + 1] -= 10;
                        Game1.inventar[s1 + 3] -= 10;
                        Game1.inventar[s1 + 4] -= 20;
                        Game1.inventar[s1 + 2] -= 5;
                        Game1.inventar[Game1.COMP_A]++;
                    }
                    else if (a1 == 4) // 10 fer + 10 uraniu + 10 sulfir
                    {
                        if (Game1.inventar[s1 + 1] < 10 || Game1.inventar[s1 + 2] < 10 || Game1.inventar[s1 + 4] < 10)
                            return 0;
                        Game1.inventar[s1 + 1] -= 10;
                        Game1.inventar[s1 + 2] -= 10;
                        Game1.inventar[s1 + 4] -= 10;
                        Game1.inventar[Game1.COMP_A]++;
                    }
                    else if (a1 == 5)    // 10 fer + 10 carbon + 2 uraniu
                    {
                        if (Game1.inventar[s1 + 1] < 10 || Game1.inventar[s1 + 3] < 10 || Game1.inventar[s1 + 4] < 2)
                            return 0;
                        Game1.inventar[s1 + 1] -= 10;
                        Game1.inventar[s1 + 3] -= 10;
                        Game1.inventar[s1 + 4] -= 2;
                        Game1.inventar[Game1.COMP_A]++;
                    }
                    else if (a1 == 6) //30 fer + 10 carbon
                    {
                        if (Game1.inventar[s1 + 1] < 30 || Game1.inventar[s1 + 3] < 10)
                            return 0;
                        Game1.inventar[s1 + 1] -= 30;
                        Game1.inventar[s1 + 3] -= 10;
                        Game1.inventar[Game1.COMP_A]++;
                    }
                    else if (a1 == 7) //1 Placaj1 + 5 fer + 10 carbon
                    {
                        if (Game1.inventar[2] < 1 || Game1.inventar[s1 + 1] < 5 || Game1.inventar[s1 + 3] < 10)
                            return 0;
                        Game1.inventar[2] -= 1;
                        Game1.inventar[s1 + 1] -= 5;
                        Game1.inventar[s1 + 3] -= 10;
                        Game1.inventar[Game1.COMP_A]++;
                    }
                    else if (a1 == 8) //3 Propulsoare1 + 10 fer + 10 carbon
                    {
                        if (Game1.inventar[3] < 3 || Game1.inventar[s1 + 1] < 10 || Game1.inventar[s1 + 3] < 10)
                            return 0;
                        Game1.inventar[3] -= 3;
                        Game1.inventar[s1 + 1] -= 10;
                        Game1.inventar[s1 + 3] -= 10;
                        Game1.inventar[Game1.COMP_A]++;
                    }
                    else if (a1 == 9) // 2 Generatoare1 + 5 fer + 10 carbon + 10 uraniu
                    {
                        if (Game1.inventar[4] < 2 || Game1.inventar[s1 + 1] < 5 || Game1.inventar[s1 + 3] < 10 || Game1.inventar[s1 + 4] < 10)
                            return 0;
                        Game1.inventar[4] -= 2;
                        Game1.inventar[s1 + 1] -= 5;
                        Game1.inventar[s1 + 3] -= 10;
                        Game1.inventar[s1 + 4] -= 10;
                        Game1.inventar[Game1.COMP_A]++;
                    }
                    else if (a1 == 10) // 1 Tureta1 + 10 fer + 5 carbon + 5 uraniu
                    {
                        if (Game1.inventar[5] < 1 || Game1.inventar[s1 + 1] < 10 || Game1.inventar[s1 + 3] < 5 || Game1.inventar[s1 + 4] < 5)
                            return 0;
                        Game1.inventar[5] -= 1;
                        Game1.inventar[s1 + 1] -= 10;
                        Game1.inventar[s1 + 3] -= 5;
                        Game1.inventar[s1 + 4] -= 5;
                        Game1.inventar[s1 + 2] -= 5;
                        Game1.inventar[Game1.COMP_A]++;
                    }
                    else if (a1 == 11) // 4 Placaj2 + 10 fer + 10 sulfur + 5 carbon
                    {
                        if (Game1.inventar[7] < 4 || Game1.inventar[s1 + 1] < 10 || Game1.inventar[s1 + 3] < 5 || Game1.inventar[s1 + 2] < 10)
                            return 0;
                        Game1.inventar[7] -= 4;
                        Game1.inventar[s1 + 1] -= 10;
                        Game1.inventar[s1 + 3] -= 5;
                        Game1.inventar[s1 + 2] -= 10;
                        Game1.inventar[Game1.COMP_A]++;
                    }
                    else if (a1 == 12) //4 Generatoare2 + 5 fer + 10 uraniu
                    {
                        if (Game1.inventar[9] < 4 || Game1.inventar[s1 + 1] < 5 || Game1.inventar[s1 + 4] < 10)
                            return 0;
                        Game1.inventar[9] -= 4;
                        Game1.inventar[s1 + 1] -= 5;
                        Game1.inventar[s1 + 4] -= 10;
                        Game1.inventar[Game1.COMP_A]++;
                    }
                    else if (a1 == 13) // 2 Tureta2 + 5 uraniu + 5 carbon + 5 fer
                    {
                        if (Game1.inventar[10] < 2 || Game1.inventar[s1 + 1] < 5 || Game1.inventar[s1 + 3] < 5 || Game1.inventar[s1 + 4] < 5)
                            return 0;
                        Game1.inventar[10] -= 2;
                        Game1.inventar[s1 + 1] -= 5;
                        Game1.inventar[s1 + 4] -= 5;
                        Game1.inventar[s1 + 3] -= 5;
                        Game1.inventar[Game1.COMP_A]++;
                    }
                    else if (a1 == 14) // 4 Propulsoare2 + 10 uraniu + 10 fer + 10 sulf
                    {
                        if (Game1.inventar[8] < 4 || Game1.inventar[s1 + 1] < 10 || Game1.inventar[s1 + 4] < 10 || Game1.inventar[s1 + 2] < 10)
                            return 0;
                        Game1.inventar[8] -= 4;
                        Game1.inventar[s1 + 1] -= 10;
                        Game1.inventar[s1 + 4] -= 20;
                        Game1.inventar[s1 + 2] -= 10;
                        Game1.inventar[Game1.COMP_A]++;
                    }
                    else if (a1 == 15) //
                    {
                        if (Game1.inventar[11] < 3 || Game1.inventar[s1 + 1] < 20 || Game1.inventar[s1 + 3] < 10)
                            return 0;
                        Game1.inventar[11] -= 3;
                        Game1.inventar[s1 + 1] -= 20;
                        Game1.inventar[s1 + 3] -= 10;
                        Game1.inventar[Game1.COMP_A]++;
                    }
                    else if (a1 == 16) //
                    {
                        if (Game1.inventar[11] < 3 || Game1.inventar[s1 + 1] < 5 || Game1.inventar[s1 + 3] < 20 || Game1.inventar[s1 + 4] < 20 || Game1.inventar[s1 + 2] < 10)
                            return 0;
                        Game1.inventar[11] -= 3;
                        Game1.inventar[s1 + 1] -= 5;
                        Game1.inventar[s1 + 3] -= 20;
                        Game1.inventar[s1 + 4] -= 20;
                        Game1.inventar[s1 + 2] -= 10;
                        Game1.inventar[Game1.COMP_A]++;
                    }
                }
            }
            return 1;
        }
    }
}
