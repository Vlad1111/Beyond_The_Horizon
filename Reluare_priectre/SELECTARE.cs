using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Reluare_priectre
{
    class SELECTARE
    {
        static public void PLANETA()
        {
            Random ran = new Random();
            if (Game1.L_PLA[Game1.PLA_A].SALVAT == 0)
            {
                if (Game1.L_PLA[Game1.PLA_A].ID < 1000 && Game1.L_PLA[Game1.PLA_A].ID > 0)
                {
                    Game1.PLA_S = CREARE.PLANETA_FROM_IMG(Game1.game.Content.Load<Texture2D>("PLANETA_" + (Game1.L_PLA[Game1.PLA_A].ID - 1)));
                    Game1.L_PLA[Game1.PLA_A].SALVAT = 1;
                }
                else
                {
                    if (Game1.L_PLA[Game1.PLA_A].ord_elm[Game1.NR_subs + 1] == 7)
                    {
                        Game1.PLA_S = CREARE.ASTERORID();
                    }
                    else
                    {
                        if (Game1.L_PLA[Game1.PLA_A].ord_elm[Game1.NR_subs + 1] == 0)
                            Game1.PLA_S = CREARE.STATIE();
                        else Game1.PLA_S = CREARE.PLANETA();
                        Game1.PLA_S.creaturi[Game1.PLA_S.nr_creaturi] = new Creatura();
                        Game1.PLA_S.creaturi[Game1.PLA_S.nr_creaturi].viata = 100000000;
                        Game1.PLA_S.creaturi[Game1.PLA_S.nr_creaturi].inteligenta = -1;
                        Game1.PLA_S.creaturi[Game1.PLA_S.nr_creaturi].poz.X = Game1.PL_P.poz.Y;
                        Game1.PLA_S.creaturi[Game1.PLA_S.nr_creaturi].poz.Y = Game1.PL_P.poz.X;
                        Game1.PLA_S.creaturi[Game1.PLA_S.nr_creaturi].agresiune = 0;
                        Game1.PLA_S.creaturi[Game1.PLA_S.nr_creaturi].rot = new float[11];
                        Game1.PLA_S.nr_creaturi++;
                    }
                    Game1.L_PLA[Game1.PLA_A].SALVAT = 1;
                    MaxiFun.IO.Save<Planeta>(Game1.PLA_S, Game1.saveDir, "PLANETA" + Game1.L_PLA[Game1.PLA_A].ID);
                }
            }
            else
            {
                Game1.PLA_S = MaxiFun.IO.Load<Planeta>(Game1.saveDir, "PLANETA" + Game1.L_PLA[Game1.PLA_A].ID);
                // PLA_S = Newtonsoft.Json.JsonConvert.DeserializeObject<Planeta>(Unzip(File.ReadAllBytes("test.json")));

                int aux_sel = -1;
                for (int i = 0; i < Game1.PLA_S.nr_creaturi; i++)
                    if (Game1.PLA_S.creaturi[i].inteligenta == -1)
                        if (aux_sel == -1 || ran.Next(0, Game1.PLA_S.nr_creaturi + 1) == 1)
                            aux_sel = i;

                if (aux_sel != -1)
                {
                    Game1.PL_P.poz.X = Game1.PLA_S.creaturi[aux_sel].poz.Y;
                    Game1.PL_P.poz.Y = Game1.PLA_S.creaturi[aux_sel].poz.X;
                }
                else
                {
                    Game1.PL_P.poz = new Vector2(100, 100);
                }
            }
        }

        static public void INTERIOR_NAVA(Nava aux)
        {
            Planeta AUX = new Planeta();
            for (int i = 0; i < 37; i++)
                for (int j = 0; j < 37; j++)
                    if (aux.comp[i, j] != 0)
                    {
                        for (int k = 0; k <= 6; k++)
                            for (int l = 0; l <= 6; l++)
                                if(i==18 && j==18)
                                    AUX.a[i * 7 + k, j * 7 + l] = 1000;
                                else AUX.a[i * 7 + k, j * 7 + l] = 1001;
                        for (int k = 0; k < 2; k++)
                        {
                            AUX.b[i * 7, j * 7 + k] = AUX.b[i * 7 + k, j * 7] = AUX.b[i * 7 + 6 - k, j * 7] = AUX.b[i * 7, j * 7 + 6 - k] = 1;
                            AUX.b[i * 7 + 6, j * 7 + k] = AUX.b[i * 7 + k, j * 7 + 6] = AUX.b[i * 7 + 6 - k, j * 7 + 6] = AUX.b[i * 7 + 6, j * 7 + 6 - k] = 1;
                        }
                        if (Game1.comp[aux.comp[i, j]].eng !=0)
                        {
                            for (int k = 2; k <= 4; k++)
                                for (int l = 2; l <= 4; l++)
                                {
                                    AUX.a[i * 7 + k, j * 7 + l] = 1004;
                                    AUX.b[i * 7 + k, j * 7 + l] = 1;
                                }
                        }
                        else if (Game1.comp[aux.comp[i, j]].pow != 0)
                        {
                            for (int k = 2; k <= 4; k++)
                                for (int l = 2; l <= 4; l++)
                                {
                                    AUX.a[i * 7 + k, j * 7 + l] = 1002;
                                    AUX.b[i * 7 + k, j * 7 + l] = 0;
                                }
                        }
                    }
            Queue<int> X = new Queue<int>();
            Queue<int> Y = new Queue<int>();
            int x, y;

            int[,] map = new int[37, 37];
            X.Enqueue(18);
            Y.Enqueue(18);
            map[18, 18] = 1;
            while (X.Count != 0)
            {
                x = X.Dequeue();
                y = Y.Dequeue();
                for (int d = 0; d < 4; d++)
                    if (Game1.ran.Next(0, 7) != 2)
                    {
                        int l, k;
                        k = x + Game1.d1[d];
                        l = y + Game1.d2[d];
                        if (k >= 0 && k < 37)
                            if (l >= 0 && l < 37)
                                if (map[k, l] == 0)
                                {
                                    X.Enqueue(k);
                                    Y.Enqueue(l);
                                    map[k, l] = map[x, y] + 1;
                                }
                    }
                    else
                    {
                        if (Game1.d1[d] == 0 && Game1.d2[d] == 1)
                            for (int k = 0; k <= 6; k++)
                                AUX.b[x * 7 + k, y * 7 + 6] = 1;

                        else if (Game1.d1[d] == 1 && Game1.d2[d] == 0)
                            for (int k = 0; k <= 6; k++)
                                AUX.b[x * 7 + 6, y * 7 + k] = 1;

                        else if (Game1.d1[d] == 0 && Game1.d2[d] == -1)
                            for (int k = 0; k <= 6; k++)
                                AUX.b[x * 7 + k, y * 7] = 1;

                        else if (Game1.d1[d] == -1 && Game1.d2[d] == 0)
                            for (int k = 0; k <= 6; k++)
                                AUX.b[x * 7, y * 7 + k] = 1;
                    }
            }


            for (int i = 1; i < 299; i++)
                for (int j = 1; j < 299; j++)
                    if (AUX.a[i, j] == 0)
                        if (AUX.a[i + 1, j] != 0 || AUX.a[i, j + 1] != 0 || AUX.a[i - 1, j] != 0 || AUX.a[i, j - 1] != 0)
                        {
                            X.Enqueue(i);
                            Y.Enqueue(j);
                        }

            while (X.Count != 0)
            {
                x = X.Dequeue();
                y = Y.Dequeue();
                AUX.a[x, y] = 1001;
                AUX.b[x, y] = 1;
            }

            map = new int[300, 300];
            X.Enqueue(18 * 7 + 3);
            Y.Enqueue(18 * 7 + 3);
            map[18 * 7 + 3, 18 * 7 + 3] = 1;
            int nr_locuri_libere = 0;
            while (X.Count != 0)
            {
                x = X.Dequeue();
                y = Y.Dequeue();
                for (int d = 0; d < 4; d++)
                {
                    int l, k;
                    k = x + Game1.d1[d];
                    l = y + Game1.d2[d];
                    if (k > 0 && k < 300)
                        if (l > 0 && l < 300)
                            if (map[k, l] == 0 || map[k, l] > map[x, y] + 1)
                                if (AUX.b[x, y] == 0 && AUX.a[x, y] != 0)
                                {
                                    X.Enqueue(k);
                                    Y.Enqueue(l);
                                    map[k, l] = map[x, y] + 1;
                                    nr_locuri_libere++;
                                }
                }
            }
            AUX.nr_creaturi = Game1.ran.Next(nr_locuri_libere / 4, nr_locuri_libere / 2);
            nr_locuri_libere = 0;
            AUX.creaturi = new Creatura[AUX.nr_creaturi];

            for (int i = 0; i < 300; i++)
                for (int j = 0; j < 300; j++)
                    if (map[i, j] == 0 && AUX.a[i, j] != 0)
                        AUX.b[i, j] = 1;
                    else if (AUX.a[i, j] != 0 && map[i, j] > 25 && AUX.b[i, j] == 0)
                        if (Game1.ran.Next(0, 10) == 3 && nr_locuri_libere < AUX.nr_creaturi)
                        {
                            Creatura being = new Creatura();
                            being.poz = new Vector2(i * 20, j * 20);
                            being.X = i;
                            being.Y = j;
                            being.viata = 100;
                            being.inteligenta = Game1.ran.Next(0, 100) % 4 + 1;
                            being.rot = new float[1];
                            if(Game1.ran.Next(0,10000000) == 2)
                            {
                                being.inteligenta = 0;
                                being.nume = "I have no ideea what I'm doing here, but well...";
                            }
                            being.pow = 50;
                            AUX.creaturi[nr_locuri_libere++] = being;
                        }
            AUX.nr_creaturi = nr_locuri_libere;

            Game1.PL_P.poz = new Vector2((18 * 7 + 3) * 20, (18 * 7 + 3) * 20);
            Game1.PL_P.viata = Game1.PL_P.max_viata;
            Game1.PLA_S = AUX;
        }
    }
}
