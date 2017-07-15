using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reluare_priectre
{
    class VALOARE_CUB
    {
        static public int val(int x)
        {
            if (x <= 0)
                return 0;
            if (x <= Game1.NR_comp)
                return Game1.comp[x].v;
            else if (x - Game1.NR_comp <= Game1.NR_elem * Game1.NR_subs)
            {
                int y = (x - Game1.NR_comp - 1) / Game1.NR_subs + 1;
                if (y == 1)
                    return 100;
                else if (y == 2)
                    return 200;
                else if (y == 3)
                    return 50;
                else if (y == 4)
                    return 5;
                else if (y == 5 || y == 6 || y == 7 || y == 12)
                    return 0;
                else if (y == 8)
                    return 200;
                else if (y == 9)
                    return 100;
                else if (y == 10 || y == 15 || y == 16)
                    return 400;
                else if (y == 11)
                    return 50;
                else if (y == 13)
                    return 400;
                else if (y == 14)
                    return 100;
                else if (y == 17 || y == 18 || y == 19 || y == 20)
                    return 150;
            }
            return 0;
        }
    }
}
