using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reluare_priectre
{
    public class CREARE
    {
        static public Planeta PLANETA()
        {
            Planeta AUX = new Planeta();
            int aux;
            int auy;

            AUX.a = new int[300, 300];
            AUX.b = new int[300, 300];


            AUX.ar = Game1.L_PLA[Game1.PLA_A].ar;
            AUX.be = Game1.L_PLA[Game1.PLA_A].ge;
            AUX.ge = Game1.L_PLA[Game1.PLA_A].be;
            AUX.V_SKY = new Vector3((float)AUX.ar / 1000, (float)AUX.ge / 1000, (float)AUX.be / 1000);
            AUX.SKY = new Vector3(AUX.ar, AUX.ge, AUX.be);
            AUX.temperatura_z = Game1.L_PLA[Game1.PLA_A].temperatura_z;
            AUX.temperatura_n = AUX.temperatura_z - Game1.ran.Next(10, 200);

            AUX.MOON = Game1.L_PLA[Game1.PLA_A].MOON;

            AUX.forta = Game1.L_PLA[Game1.PLA_A].forta;

            AUX.ord_elm = Game1.L_PLA[Game1.PLA_A].ord_elm;


            AUX.inaltime = Game1.ran.Next(1, 10);

            aux = 150;
            //aux = Game1.ran.Next(100, 250);

            for (int i = 0; i < 300; i++)
            {                                                                       /// CREAREA TERENULUI
                auy = 1;
                for (int j = aux; j < 300; j++)
                {
                    int auz = Game1.ran.Next(0, Game1.NR_subs * Game1.NR_subs * Game1.NR_subs * Game1.NR_subs);              /// elementul la puterea a 4
                    AUX.a[j, i] = auy * 100 + AUX.ord_elm[4 - (int)Math.Sqrt((int)Math.Sqrt(auz))];
                    AUX.b[j, i] = auy * 100;

                    if (j >= 180)
                        if (auy < 2)
                            if (Game1.ran.Next(0, AUX.inaltime) == 0)
                                auy++;
                }

                int semn = 1 - Game1.ran.Next(0, 100) % 3;
                if (Game1.ran.Next(0, 100) % 3 != 0)
                    aux -= semn * (int)Math.Sqrt(Game1.ran.Next(1, AUX.inaltime * AUX.inaltime));

                if (aux <= 0)
                    aux = 1;
                if (aux >= 300)
                    aux = 295;
            }

            for (int i = 0; i < 30; i++)                   /// PESTERI
            {
                auy = Game1.ran.Next(0, 290);
                aux = Game1.ran.Next(100, 290);

                for (int j = 0; j <= 500; j++)
                {
                    AUX.b[aux, auy] = 0;
                    int semn = 1 - Game1.ran.Next(0, 100) % 3;
                    aux += semn;
                    semn = 1 - Game1.ran.Next(0, 100) % 3;
                    auy += semn;

                    if (aux <= 0 || aux >= 300 || auy <= 0 || auy >= 300)
                        break;
                }
            }



            AUX.apa = new int[300, 300];             /// adaugarea apei
            int y;
            int nr_apa = Game1.ran.Next(10, 10 + (300 - Math.Abs(AUX.temperatura_z)));

            for (int i = 0; i < nr_apa; i++)
            {
                int x = 10;
                y = Game1.ran.Next(1, 300);
                for (int j = x; j < 299; j++)
                {
                    x = j;
                    if (AUX.b[j, y] == 0 && AUX.b[j + 1, y] != 0 && Game1.ran.Next(0, 10 - AUX.inaltime) == 1)
                        break;
                }
                int tip_apa = Game1.ran.Next(0, Math.Abs(5 - AUX.inaltime));
                if (tip_apa <= 1)
                {
                    if (AUX.temperatura_z > 3)
                        AUX.apa[x, y] = 6000 + AUX.ord_elm[1];
                    else
                    {
                        AUX.a[x, y] = 800 + AUX.ord_elm[1];
                        AUX.b[x, y] = 200;
                    }
                }
                else
                {
                    int k, l;
                    k = l = y;
                    while (k < 300 && AUX.b[x, k] == 0)
                        k++;
                    while (AUX.b[x, l] == 0 && l > 0)
                        l--;
                    if (k <= 290 && l >= 10)
                        for (int j = l + 1; j < k; j++)
                            for (int jj = x; AUX.b[jj, j] == 0 && jj < 290; jj++)
                            {

                                if (AUX.temperatura_z > 3)
                                    AUX.apa[jj, j] = 6000 + AUX.ord_elm[1];
                                else
                                {
                                    AUX.a[jj, j] = 800 + AUX.ord_elm[1];
                                    AUX.b[jj, j] = 200;
                                }
                            }
                    else if (AUX.temperatura_z > 3)
                        AUX.apa[x, y] = 6000 + AUX.ord_elm[1];
                    else
                    {
                        AUX.a[x, y] = 800 + AUX.ord_elm[1];
                        AUX.b[x, y] = 200;
                    }
                }
            }

            int nr_c;

            if (AUX.temperatura_z > 200)                                    ///AUGARE NISIP
            {
                nr_c = AUX.temperatura_z - AUX.temperatura_n;
                nr_c = 203 - nr_c;

                for (int j = 1; j < 300; j++)
                {
                    int i = 0;
                    while (AUX.a[i, j] / 100 != 1 && i < 299)
                        i++;
                    for (; AUX.a[i, j] / 100 == 1 && Game1.ran.Next(0, nr_c) != 0 && i < 298; i++)
                        AUX.a[i, j] = 900 + AUX.a[i, j] % 100;

                }
            }
            else if (AUX.temperatura_z > -50)
            {
                nr_c = Game1.ran.Next(5, Math.Abs(AUX.temperatura_z) + 5);
                int x;
                for (int i = 0; i < nr_c; i++)
                {
                    do
                    {
                        x = Game1.ran.Next(10, 290);
                        y = Game1.ran.Next(10, 290);
                    } while (AUX.a[x, y] / 100 != 1 && AUX.a[x, y] / 100 != 2);

                    AUX.a[x, y] = 900 + AUX.a[x, y] % 100;
                }
            }
            else if (AUX.temperatura_z < -200)                                    ///AUGARE GHEATA
            {
                nr_c = AUX.temperatura_z - AUX.temperatura_n;
                nr_c = 203 - nr_c;

                for (int j = 1; j < 300; j++)
                {
                    int i = 0;
                    while (AUX.a[i, j] / 100 != 1 && i < 299)
                        i++;
                    if (i != 300)
                    {
                        for (; (AUX.a[i, j] / 100 == 1 || AUX.a[i, j] / 100 == 8) && Game1.ran.Next(0, nr_c * 2) != 0 && i < 298; i++)
                        {
                            AUX.a[i, j] = 1100 + AUX.a[i, j] % 100;
                            if (AUX.b[i, j] != 0)
                                AUX.b[i, j] = 50;
                        }
                        i++;
                        for (; i < 298 && (AUX.a[i, j] / 100 == 2 || AUX.a[i, j] / 100 == 8) && Game1.ran.Next(0, nr_c * 2) != 0; i++)
                        {
                            AUX.a[i, j] = 800 + AUX.a[i, j] % 100;
                            if (AUX.b[i, j] != 0)
                                AUX.b[i, j] = 200;
                        }
                    }
                }
            }


            nr_c = 30 - Game1.ran.Next(0, 30 - (Math.Abs(AUX.temperatura_z) / 10));                     ///CREAREA COPACILOR
            y = 0;
            for (int nrn = 0; nrn < nr_c; nrn++)
            {
                int x = 0;
                do
                {
                    x = 0;
                    y += Game1.ran.Next(3, 6 * (30 - nr_c) + 3);
                    if (y >= 300)
                        break;
                    while (x < 299 && AUX.a[x + 1, y] == 0)
                        x++;
                } while (AUX.a[x + 1, y] / 100 != 1 && AUX.a[x + 1, y] / 100 != 11 && AUX.a[x + 1, y] / 100 != 9);

                if (y >= 300)
                    break;
                int tip, inal, lat;
                tip = (int)Math.Sqrt(Game1.ran.Next(0, 25));
                inal = Game1.ran.Next(2, AUX.inaltime * 3);
                lat = Game1.ran.Next(1, inal + 1);
                if (AUX.apa[x, y] == 0 && AUX.a[x, y] / 100 != 8)
                {
                    if (AUX.b[x + 1, y] == 0 && tip >= 2)
                        tip -= 2;

                    for (int i = 1; i <= inal; i++)
                    {
                        AUX.a[x, y] = 3 * 100 + AUX.ord_elm[tip % 2];
                        if (tip >= 2)
                            AUX.b[x, y] = 50;
                        else
                            AUX.b[x, y] = 0;
                        x--;
                        if (x <= 0)
                            break;

                        if (i >= inal / 2)
                        {
                            if (Game1.ran.Next(0, inal / 3) % 10 == 2)
                            {
                                for (int j = 1; j <= lat; j++)
                                    if (y - j > 0 && y + j < 300)
                                        if (AUX.a[x, y + j] == 0 && AUX.a[x, y - j] == 0)
                                        {
                                            AUX.a[x, y + j] = 3 * 100 + AUX.ord_elm[tip % 2];
                                            if (tip >= 2)
                                                AUX.b[x, y + j] = 50;

                                            AUX.a[x, y - j] = 3 * 100 + AUX.ord_elm[tip % 2];
                                            if (tip >= 2)
                                                AUX.b[x, y - j] = 50;

                                            if (Game1.ran.Next(0, 10) == 2)
                                                break;
                                        }
                                        else break;
                            }
                            else for (int j = 1; j <= lat; j++)
                                    if (y - j > 0 && y + j < 300)
                                        if (AUX.a[x, y + j] == 0 && AUX.a[x, y - j] == 0)
                                        {
                                            AUX.a[x, y + j] = 4 * 100 + AUX.ord_elm[tip % 2];
                                            if (tip >= 2)
                                                AUX.b[x, y + j] = 5;

                                            AUX.a[x, y - j] = 4 * 100 + AUX.ord_elm[tip % 2];
                                            if (tip >= 2)
                                                AUX.b[x, y - j] = 5;

                                            if (Game1.ran.Next(0, 10) == 2)
                                                break;
                                        }
                                        else break;
                        }
                    }
                    for (int i = 0; i < Game1.ran.Next(1, 5); i++)
                        if (x - i > 0)
                        {
                            AUX.a[x - i, y] = 4 * 100 + AUX.ord_elm[tip % 2];
                            if (tip >= 2)
                                AUX.b[x - i, y] = 5;
                        }
                        else break;
                }
            }

            for (int j = 1; j < 300; j++)
                if (Game1.ran.Next(1, AUX.inaltime) == 1)
                {
                    int i = 1;
                    while (AUX.a[i, j] / 100 != 1 && i < 299)
                        i++;
                    if (i != 300)
                        if (AUX.a[i - 1, j] == 0 && AUX.b[i, j] != 0)
                        {
                            AUX.a[i - 1, j] = 500 + AUX.a[i, j] % 100;
                            AUX.b[i - 1, j] = 0;
                            AUX.a[i, j] = 1400 + AUX.a[i, j] % 100;
                        }
                }

            aux = Game1.ran.Next(1, 299);                       /// POZITIA JUCATORULI
            for (int i = 0; i < 298; i++)
            {
                Game1.PL_P.poz.X = aux * 20;
                Game1.PL_P.poz.Y = i * 20;
                if (AUX.b[i + 2, aux] != 0)
                    break;
            }

            Game1.PL_P.Y = (int)(Game1.PL_P.poz.X + 10) / 20;
            Game1.PL_P.X = (int)(Game1.PL_P.poz.Y + 30) / 20;
            for (int j = 1; j < 300; j++)
                AUX.b[299, j] = 500;

            for (int j = 1; j < 300; j++)                          /// ADAUGAREA LAVA
                for (int i = 299; i > 0; i--)
                {
                    if (AUX.a[i, j] / 100 == 1)
                        break;
                    AUX.apa[i, j] = 7000 + Game1.ran.Next(0, 5) + (299 - i) * 10;
                    if (i < 290 && (Game1.ran.Next(0, (Math.Abs(AUX.temperatura_z)) / 10 + 1) == 0 || AUX.temperatura_z <= 0))
                        break;
                }



            AUX.creaturi = new Creatura[10];         //creare creaturi
            AUX.nr_creaturi = Game1.ran.Next(0, (300 - Math.Abs(AUX.temperatura_z)) / 20);
            for (int i = 0; i < AUX.nr_creaturi; i++)
            {
                Creatura cc = new Creatura();
                cc.pow = Game1.ran.Next(2, 100);
                cc.viata = 1000;
                int x = 0;
                int iii;
                for (iii = 0; iii <= 1000; iii++)
                {
                    y = Game1.ran.Next(20, 280);
                    x = 0;
                    while (x < 290 && AUX.b[x + 2, y] == 0)
                        x++;
                    int OK = 0;
                    for (int j = 0; j < i; j++)
                    {
                        int xxx, yyy;
                        xxx = Math.Abs(AUX.creaturi[j].X - x);
                        yyy = Math.Abs(AUX.creaturi[j].Y - y);

                        if (xxx * xxx + yyy * yyy < (AUX.creaturi[j].r + 100) * (AUX.creaturi[j].r + 100))
                            OK = 1;
                    }
                    if (OK == 0)
                        break;
                }
                if (iii >= 1000)
                {
                    AUX.nr_creaturi = i;
                    break;
                }
                cc.poz = new Vector2(x * 20, y * 20);
                cc.X = cc.xx = x + 2;
                cc.Y = cc.yy = y;
                cc.rot = new float[11];
                cc.parti = new int[11];
                for (int j = 0; j <= 10; j++)
                    cc.rot[j] = -99f;
                nr_c = Game1.ran.Next(1, 5);
                cc.parti[0] = Game1.ran.Next(1, Game1.NR_PARTI + 1);
                cc.rot[0] = 0;
                if (nr_c >= 2)
                {
                    cc.parti[2] = Game1.ran.Next(1, Game1.NR_PARTI + 1);
                    cc.rot[2] = 0;
                }
                if (nr_c >= 3)
                {
                    cc.parti[1] = cc.parti[2];
                    cc.rot[1] = (float)(Game1.ran.Next(0, 45)) * 0.0174532778f;
                }
                if (nr_c >= 4)
                {
                    cc.parti[3] = cc.parti[0];
                    cc.rot[3] = cc.rot[1];
                }

                cc.parti[4] = Game1.ran.Next(1, Game1.NR_PARTI + 1);
                cc.rot[4] = (float)(90 - Game1.ran.Next(0, 140)) * 0.0174532778f;

                nr_c = Game1.ran.Next(0, 5);
                if (nr_c >= 1)
                {
                    cc.parti[5] = Game1.ran.Next(1, Game1.NR_PARTI + 1);
                    cc.rot[5] = 0;
                }
                if (nr_c >= 2)
                {
                    cc.parti[7] = Game1.ran.Next(1, Game1.NR_PARTI + 1);
                    cc.rot[7] = 0;
                }
                if (nr_c >= 3)
                {
                    cc.parti[6] = cc.parti[7];
                    cc.rot[6] = (float)(Game1.ran.Next(0, 45)) * 0.0174532778f;
                }
                if (nr_c >= 4)
                {
                    cc.parti[8] = cc.parti[5];
                    cc.rot[8] = cc.rot[1];
                }
                cc.parti[9] = Game1.ran.Next(1, Game1.NR_PARTI + 1);
                cc.rot[9] = 0;

                cc.mers = 10;

                cc.pow = Game1.ran.Next(5, 21);
                cc.inteligenta = Game1.ran.Next(1, 6);
                cc.r = 20;
                cc.agresiune = -1;
                if (cc.inteligenta == 2)
                    cc.agresiune = 1;
                else if (cc.inteligenta == 4)
                {
                    cc.r = 7;

                    int[][] casa = new int[8][];

                    casa[0] = new int[] { 0, 0, 0, 0, 0, 0, 4, 4, 4, 4, 4, 4, 4, 4, 4, 0, 0, 0, 0, 0, 0 };
                    casa[1] = new int[] { 0, 0, 0, 0, 0, 4, 4, 2, 2, 2, 4, 2, 2, 2, 4, 4, 0, 0, 0, 0, 0 };
                    casa[2] = new int[] { 0, 0, 0, 0, 0, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 0, 0, 0, 0, 0 };
                    casa[3] = new int[] { 0, 0, 0, 0, 0, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 0, 0, 0, 0, 0 };
                    casa[4] = new int[] { 1, 5, 5, 5, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 5, 5, 5, 1 };
                    casa[5] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1 };
                    casa[6] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                    casa[7] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

                    int cub = AUX.a[cc.X, cc.Y];
                    if(cub/100==14)
                        cub = 100 + cub % 100;

                    for (int k = 0; k <= 7; k++)
                        for (int l = 0; l < 21; l++)
                            if (casa[k][l] == 0)
                            {
                                AUX.a[cc.X + k - 6, cc.Y + l - 10] = 0;
                                AUX.b[cc.X + k - 6, cc.Y + l - 10] = 0;
                                AUX.apa[cc.X + k - 6, cc.Y + l - 10] = 0;
                            }
                            else if (casa[k][l] == 1)
                            {
                                AUX.a[cc.X + k - 6, cc.Y + l - 10] = cub;
                                AUX.b[cc.X + k - 6, cc.Y + l - 10] = VALOARE_CUB.val((Game1.NR_subs * (cub / 100 - 1) + Game1.NR_comp + 1));
                                AUX.apa[cc.X + k - 6, cc.Y + l - 10] = 0;
                            }
                            else if (casa[k][l] == 2)
                            {
                                AUX.a[cc.X + k - 6, cc.Y + l - 10] = cub;
                                AUX.b[cc.X + k - 6, cc.Y + l - 10] = 0;
                                AUX.apa[cc.X + k - 6, cc.Y + l - 10] = 0;
                            }
                            else if (casa[k][l] == 3)
                            {
                                AUX.a[cc.X + k - 6, cc.Y + l - 10] = 300 + cub % 100;
                                AUX.b[cc.X + k - 6, cc.Y + l - 10] = 0;
                                AUX.apa[cc.X + k - 6, cc.Y + l - 10] = 0;
                            }
                            else if (casa[k][l] == 4)
                            {
                                AUX.a[cc.X + k - 6, cc.Y + l - 10] = 300 + cub % 100;
                                AUX.b[cc.X + k - 6, cc.Y + l - 10] = VALOARE_CUB.val(Game1.NR_subs * 2 + Game1.NR_comp + 1);
                                AUX.apa[cc.X + k - 6, cc.Y + l - 10] = 0;
                            }
                            else if (casa[k][l] == 5)
                            {
                                AUX.apa[cc.X + k - 6, cc.Y + l - 10] = 6000 + cub % 100;
                                AUX.a[cc.X + k - 6, cc.Y + l - 10] = 0;
                                AUX.b[cc.X + k - 6, cc.Y + l - 10] = 0;
                            }
                }
                else
                {
                    cc.r = 10;
                    aux = 1000 + AUX.ord_elm[1];
                    auy = Game1.ran.Next(0, 2) - 1;
                    if (auy == 0)
                        auy = 1;

                    int[][] casa = new int[14][];
                    casa[0] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    casa[1] = new int[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    casa[2] = new int[] { 1, 2, 0, 0, 2, 2, 2, 2, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    casa[3] = new int[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0 };
                    casa[4] = new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 };
                    casa[5] = new int[] { 1, 2, 0, 0, 2, 2, 2, 2, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    casa[6] = new int[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    casa[7] = new int[] { 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 3, 0, 0, 0, 0 };
                    casa[8] = new int[] { 1, 2, 0, 0, 2, 2, 2, 2, 0, 0, 2, 2, 0, 0, 0, 0, 1, 0, 0, 0, 0 };
                    casa[9] = new int[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 1, 0, 1, 0, 1, 0, 0 };
                    casa[10] = new int[] { 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                    casa[11] = new int[] { 4, 4, 4, 4, 1, 1, 2, 2, 2, 2, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 };
                    casa[12] = new int[] { 4, 4, 4, 4, 1, 1, 1, 2, 2, 2, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 };
                    casa[13] = new int[] { 4, 4, 4, 4, 1, 1, 1, 1, 1, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 };

                    for (int k = 0; k <= 13; k++)
                        for (int l = 0; l < 21; l++)
                            if (casa[k][l] == 0)
                            {
                                AUX.b[cc.X + k - 8, cc.Y + (l - 10) * auy] = 0;
                                AUX.apa[cc.X + k - 8, cc.Y + (l - 10) * auy] = 0;
                            }
                            else if (casa[k][l] == 1)
                            {
                                AUX.a[cc.X + k - 8, cc.Y + (l - 10) * auy] = 1000 + AUX.ord_elm[0];
                                AUX.b[cc.X + k - 8, cc.Y + (l - 10) * auy] = VALOARE_CUB.val(Game1.NR_subs * 9 + Game1.NR_comp + 1);
                                AUX.apa[cc.X + k - 8, cc.Y + (l - 10) * auy] = 0;
                            }
                            else if (casa[k][l] == 2)
                            {
                                AUX.a[cc.X + k - 8, cc.Y + (l - 10) * auy] = 1000 + AUX.ord_elm[0];
                                AUX.b[cc.X + k - 8, cc.Y + (l - 10) * auy] = 0;
                                AUX.apa[cc.X + k - 8, cc.Y + (l - 10) * auy] = 0;
                            }
                            else if (casa[k][l] == 3)
                            {
                                AUX.apa[cc.X + k - 8, cc.Y + (l - 10) * auy] = 6000 + AUX.ord_elm[0];
                                AUX.a[cc.X + k - 8, cc.Y + (l - 10) * auy] = 0;
                                AUX.b[cc.X + k - 8, cc.Y + (l - 10) * auy] = 0;
                            }
                }

                AUX.creaturi[i] = cc;
            }

            return AUX;
        }
        static public Planeta STATIE()
        {
            Planeta AUX = new Planeta();

            AUX.a = new int[300, 300];
            AUX.b = new int[300, 300];
            AUX.apa = new int[300, 300];

            AUX.ar = Game1.L_PLA[Game1.PLA_A].ar;
            AUX.be = Game1.L_PLA[Game1.PLA_A].ge;
            AUX.ge = Game1.L_PLA[Game1.PLA_A].be;

            AUX.V_SKY = new Vector3((float)AUX.ar / 1000, (float)AUX.ge / 1000, (float)AUX.be / 1000);
            AUX.SKY = Vector3.Zero;
            AUX.timp = 0;
            AUX.forta = 7;
            AUX.temperatura_n = AUX.temperatura_z = 30;
            AUX.MOON = 0;

            Game1.PL_P.poz = new Vector2(3000, 3000);
            Game1.PL_P.X = 150;
            Game1.PL_P.Y = 150;


            int[][][] mod_loc = new int[20][][];
            #region MOD2
            mod_loc[2] = new int[20][];
            mod_loc[2][0] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[2][1] = new int[] { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 };
            mod_loc[2][2] = new int[] { 0, 0, 1, 2, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 2, 1, 0, 0 };
            mod_loc[2][3] = new int[] { 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0 };
            mod_loc[2][4] = new int[] { 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 1, 0, 0 };
            mod_loc[2][5] = new int[] { 0, 0, 1, 2, 2, 2, 2, 2, 2, 3, 2, 2, 2, 2, 2, 2, 1, 0, 0 };
            mod_loc[2][6] = new int[] { 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0 };
            mod_loc[2][7] = new int[] { 1, 1, 1, 2, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 2, 1, 1, 1 };
            mod_loc[2][8] = new int[] { 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[2][9] = new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
            mod_loc[2][10] = new int[] { 2, 2, 2, 1, 1, 1, 3, 2, 2, 2, 2, 2, 3, 1, 1, 1, 2, 2, 2 };
            mod_loc[2][11] = new int[] { 2, 2, 1, 1, 2, 2, 2, 2, 0, 0, 0, 2, 2, 2, 2, 1, 1, 2, 2 };
            mod_loc[2][12] = new int[] { 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1 };
            mod_loc[2][13] = new int[] { 1, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 1 };
            mod_loc[2][14] = new int[] { 1, 2, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 2, 2, 1 };
            mod_loc[2][15] = new int[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[2][16] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            mod_loc[2][17] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[2][18] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            #endregion
            #region MOD3
            mod_loc[3] = new int[20][];
            mod_loc[3][0] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[3][1] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[3][2] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[3][3] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[3][4] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[3][5] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[3][6] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[3][7] = new int[] { 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0 };
            mod_loc[3][8] = new int[] { 1, 1, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 1, 1 };
            mod_loc[3][9] = new int[] { 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2 };
            mod_loc[3][10] = new int[] { 2, 3, 0, 0, 0, 2, 2, 2, 0, 0, 0, 2, 2, 2, 0, 0, 0, 3, 2 };
            mod_loc[3][11] = new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
            mod_loc[3][12] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            mod_loc[3][13] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[3][14] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[3][15] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[3][16] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[3][17] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[3][18] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            #endregion
            #region MOD4
            mod_loc[4] = new int[20][];
            mod_loc[4][0] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[4][1] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[4][2] = new int[] { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[4][3] = new int[] { 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[4][4] = new int[] { 0, 0, 0, 1, 3, 0, 0, 2, 0, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[4][5] = new int[] { 0, 0, 0, 1, 2, 0, 0, 2, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[4][6] = new int[] { 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[4][7] = new int[] { 1, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 };
            mod_loc[4][8] = new int[] { 1, 2, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1 };
            mod_loc[4][9] = new int[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
            mod_loc[4][10] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 3, 2, 2, 2, 2, 2, 2, 3, 2, 2, 2 };
            mod_loc[4][11] = new int[] { 1, 2, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
            mod_loc[4][12] = new int[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1 };
            mod_loc[4][13] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 0, 0, 0, 0 };
            mod_loc[4][14] = new int[] { 1, 2, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0 };
            mod_loc[4][15] = new int[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0 };
            mod_loc[4][16] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[4][17] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[4][18] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            #endregion
            #region MOD5
            mod_loc[5] = new int[20][];
            mod_loc[5][0] = new int[] { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            mod_loc[5][1] = new int[] { 0, 1, 2, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[5][2] = new int[] { 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[5][3] = new int[] { 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 3, 2, 2, 2, 2, 1, 1, 1, 1 };
            mod_loc[5][4] = new int[] { 0, 1, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 2, 2, 2, 2, 1 };
            mod_loc[5][5] = new int[] { 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[5][6] = new int[] { 0, 1, 2, 3, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 3, 2, 1 };
            mod_loc[5][7] = new int[] { 0, 1, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 1 };
            mod_loc[5][8] = new int[] { 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[5][9] = new int[] { 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 1 };
            mod_loc[5][10] = new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 2, 1 };
            mod_loc[5][11] = new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[5][12] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1 };
            mod_loc[5][13] = new int[] { 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[5][14] = new int[] { 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[5][15] = new int[] { 0, 0, 0, 0, 0, 1, 2, 3, 2, 1, 1, 1, 1, 1, 1, 2, 3, 2, 1 };
            mod_loc[5][16] = new int[] { 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 0, 0, 0, 0, 2, 2, 2, 2, 1 };
            mod_loc[5][17] = new int[] { 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[5][18] = new int[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            #endregion
            #region MOD6
            mod_loc[6] = new int[20][];
            mod_loc[6][0] = new int[] { 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 0, 0, 0 };
            mod_loc[6][1] = new int[] { 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0, 0 };
            mod_loc[6][2] = new int[] { 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0, 0 };
            mod_loc[6][3] = new int[] { 0, 0, 0, 1, 3, 2, 2, 1, 1, 1, 1, 1, 2, 2, 3, 1, 0, 0, 0 };
            mod_loc[6][4] = new int[] { 0, 0, 0, 1, 0, 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 1, 0, 0, 0 };
            mod_loc[6][5] = new int[] { 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0, 0 };
            mod_loc[6][6] = new int[] { 0, 0, 0, 1, 1, 1, 1, 2, 2, 3, 2, 2, 1, 1, 1, 1, 0, 0, 0 };
            mod_loc[6][7] = new int[] { 0, 0, 0, 1, 2, 2, 2, 2, 0, 0, 0, 2, 2, 2, 2, 1, 0, 0, 0 };
            mod_loc[6][8] = new int[] { 1, 1, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 1, 1 };
            mod_loc[6][9] = new int[] { 2, 1, 1, 1, 2, 2, 1, 1, 1, 1, 1, 1, 1, 2, 2, 1, 1, 1, 2 };
            mod_loc[6][10] = new int[] { 2, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 2 };
            mod_loc[6][11] = new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
            mod_loc[6][12] = new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 2, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1 };
            mod_loc[6][13] = new int[] { 0, 0, 0, 1, 2, 2, 2, 2, 0, 0, 0, 2, 2, 2, 2, 1, 0, 0, 0 };
            mod_loc[6][14] = new int[] { 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0, 0 };
            mod_loc[6][15] = new int[] { 0, 0, 0, 1, 2, 2, 3, 1, 1, 1, 1, 1, 3, 2, 2, 1, 0, 0, 0 };
            mod_loc[6][16] = new int[] { 0, 0, 0, 1, 1, 2, 2, 2, 0, 0, 0, 2, 2, 2, 1, 1, 0, 0, 0 };
            mod_loc[6][17] = new int[] { 0, 0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0 };
            mod_loc[6][18] = new int[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 };
            #endregion
            #region MOD7
            mod_loc[7] = new int[20][];
            mod_loc[7][0] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[7][1] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[7][2] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[7][3] = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 };
            mod_loc[7][4] = new int[] { 0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0 };
            mod_loc[7][5] = new int[] { 0, 0, 0, 0, 1, 1, 2, 0, 0, 0, 0, 0, 2, 1, 1, 0, 0, 0, 0 };
            mod_loc[7][6] = new int[] { 0, 0, 0, 0, 1, 2, 3, 0, 0, 0, 0, 0, 3, 2, 1, 0, 0, 0, 0 };
            mod_loc[7][7] = new int[] { 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1 };
            mod_loc[7][8] = new int[] { 1, 2, 2, 2, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 2, 2, 2, 1 };
            mod_loc[7][9] = new int[] { 2, 2, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 2, 2 };
            mod_loc[7][10] = new int[] { 2, 0, 0, 0, 0, 2, 3, 2, 2, 2, 2, 3, 2, 2, 0, 0, 0, 0, 2 };
            mod_loc[7][11] = new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
            mod_loc[7][12] = new int[] { 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1 };
            mod_loc[7][13] = new int[] { 1, 2, 2, 2, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 2, 2, 2, 1 };
            mod_loc[7][14] = new int[] { 1, 2, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 2, 1 };
            mod_loc[7][15] = new int[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[7][16] = new int[] { 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1 };
            mod_loc[7][17] = new int[] { 0, 0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0, 0, 0, 0 };
            mod_loc[7][18] = new int[] { 0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0 };
            #endregion
            #region MOD8
            mod_loc[8] = new int[20][];
            mod_loc[8][0] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1 };
            mod_loc[8][1] = new int[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[8][2] = new int[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[8][3] = new int[] { 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1 };
            mod_loc[8][4] = new int[] { 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[8][5] = new int[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[8][6] = new int[] { 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1 };
            mod_loc[8][7] = new int[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[8][8] = new int[] { 1, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 1 };
            mod_loc[8][9] = new int[] { 2, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 2 };
            mod_loc[8][10] = new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
            mod_loc[8][11] = new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
            mod_loc[8][12] = new int[] { 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1 };
            mod_loc[8][13] = new int[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[8][14] = new int[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[8][15] = new int[] { 1, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 1 };
            mod_loc[8][16] = new int[] { 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1 };
            mod_loc[8][17] = new int[] { 0, 0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0, 0, 0, 0 };
            mod_loc[8][18] = new int[] { 0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0 };
            #endregion
            #region MOD9
            mod_loc[9] = new int[20][];
            mod_loc[9][0] = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][1] = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][2] = new int[] { 0, 0, 0, 0, 0, 1, 1, 2, 0, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][3] = new int[] { 0, 0, 0, 0, 1, 1, 2, 2, 0, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][4] = new int[] { 0, 0, 0, 1, 1, 2, 0, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][5] = new int[] { 0, 0, 1, 1, 2, 2, 0, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][6] = new int[] { 0, 1, 1, 2, 0, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][7] = new int[] { 1, 1, 2, 2, 0, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][8] = new int[] { 1, 2, 0, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][9] = new int[] { 2, 2, 0, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][10] = new int[] { 2, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][11] = new int[] { 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][12] = new int[] { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][13] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][14] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][15] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][16] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][17] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[9][18] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            #endregion 
            #region MOD10
            mod_loc[10] = new int[20][];
            mod_loc[10][0] = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0 };
            mod_loc[10][1] = new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0 };
            mod_loc[10][2] = new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 2, 2, 0, 2, 1, 1, 0, 0, 0, 0, 0 };
            mod_loc[10][3] = new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 2, 2, 0, 2, 2, 1, 1, 0, 0, 0, 0 };
            mod_loc[10][4] = new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 0, 2, 1, 1, 0, 0, 0 };
            mod_loc[10][5] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 0, 2, 2, 1, 1, 0, 0 };
            mod_loc[10][6] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 0, 2, 1, 1, 0 };
            mod_loc[10][7] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 0, 2, 2, 1, 1 };
            mod_loc[10][8] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 0, 2, 1 };
            mod_loc[10][9] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 0, 2, 2 };
            mod_loc[10][10] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 2 };
            mod_loc[10][11] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 2 };
            mod_loc[10][12] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 };
            mod_loc[10][13] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[10][14] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[10][15] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[10][16] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[10][17] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[10][18] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            #endregion
            #region MOD11
            mod_loc[11] = new int[20][];
            mod_loc[11][0] = new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0 };
            mod_loc[11][1] = new int[] { 0, 0, 0, 0, 0, 1, 1, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[11][2] = new int[] { 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[11][3] = new int[] { 0, 0, 0, 0, 0, 1, 2, 2, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0 };
            mod_loc[11][4] = new int[] { 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0 };
            mod_loc[11][5] = new int[] { 0, 0, 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0, 0, 0, 0 };
            mod_loc[11][6] = new int[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 0, 0, 0 };
            mod_loc[11][7] = new int[] { 0, 0, 0, 0, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0, 0, 0 };
            mod_loc[11][8] = new int[] { 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0, 0 };
            mod_loc[11][9] = new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0 };
            mod_loc[11][10] = new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0, 0 };
            mod_loc[11][11] = new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0, 0 };
            mod_loc[11][12] = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 0, 0 };
            mod_loc[11][13] = new int[] { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 };
            mod_loc[11][14] = new int[] { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0 };
            mod_loc[11][15] = new int[] { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 };
            mod_loc[11][16] = new int[] { 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 };
            mod_loc[11][17] = new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[11][18] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            #endregion
            #region MOD12
            mod_loc[12] = new int[20][];
            mod_loc[12][0] = new int[] { 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 1, 1, 0, 0, 0, 0, 0, 0 };
            mod_loc[12][1] = new int[] { 0, 0, 1, 2, 0, 0, 2, 2, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[12][2] = new int[] { 0, 0, 1, 2, 0, 0, 2, 2, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[12][3] = new int[] { 0, 0, 1, 2, 2, 2, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[12][4] = new int[] { 0, 0, 1, 2, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[12][5] = new int[] { 0, 0, 1, 2, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[12][6] = new int[] { 0, 0, 1, 1, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[12][7] = new int[] { 0, 0, 1, 2, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[12][8] = new int[] { 0, 0, 1, 2, 0, 0, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 };
            mod_loc[12][9] = new int[] { 0, 0, 1, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2 };
            mod_loc[12][10] = new int[] { 0, 0, 1, 2, 2, 2, 2, 2, 0, 0, 0, 2, 0, 0, 0, 2, 2, 2, 2 };
            mod_loc[12][11] = new int[] { 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
            mod_loc[12][12] = new int[] { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            mod_loc[12][13] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[12][14] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[12][15] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[12][16] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[12][17] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[12][18] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            #endregion
            #region MODEL_0
            /*
            mod_loc[\] = new int[20][];
            mod_loc[\][0] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][1] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][2] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][3] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][4] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][5] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][6] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][7] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][8] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][9] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][10] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][11] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][12] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][13] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][14] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][15] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][16] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][17] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            mod_loc[\][18] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
              */
            #endregion
            int NR_LOC = 13;

            Vector2[] loc = new Vector2[1000];
            int[] tip_loc = new int[1000];
            int nr_loc = 1;
            loc[1] = new Vector2(150, 150);
            tip_loc[1] = 2;
            AUX.creaturi = new Creatura[150];

            for (int nr = 1; nr <= nr_loc; nr++)
            {
                for (int i = -9; i <= +9; i++)
                    for (int j = -9; j <= +9; j++)
                        if ((int)loc[nr].X + i < 300 && (int)loc[nr].X + i > 0)
                            if ((int)loc[nr].Y + j < 300 && (int)loc[nr].Y + j > 0)
                                if (mod_loc[tip_loc[nr]][i + 9][j + 9] != 0)
                                {
                                    AUX.a[(int)loc[nr].X + i, (int)loc[nr].Y + j] = 1000;
                                    if (mod_loc[tip_loc[nr]][i + 9][j + 9] == 1)
                                        AUX.b[(int)loc[nr].X + i, (int)loc[nr].Y + j] = 1000000000;

                                    if (mod_loc[tip_loc[nr]][i + 9][j + 9] == 3 && AUX.nr_creaturi < 150 && (AUX.nr_creaturi == 0 || Game1.ran.Next(0, 5) == 3))
                                    {
                                        #region ADAUGARE_CREATURI
                                        Creatura cc = new Creatura();
                                        cc.pow = Game1.ran.Next(2, 100);
                                        cc.viata = 1000;
                                        int x, y;
                                        x = (int)loc[nr].X + i;
                                        y = (int)loc[nr].Y + j;
                                        cc.r = 15;

                                        cc.poz = new Vector2(x * 20, y * 20);
                                        cc.X = cc.xx = x + 2;
                                        cc.Y = cc.yy = y;

                                        cc.pow = Game1.ran.Next(5, 21);
                                        cc.r = 20;
                                        cc.agresiune = -1;

                                        cc.rot = new float[11];
                                        cc.parti = new int[11];

                                        if (Game1.ran.Next(0, 5) != 2)
                                        {
                                            cc.inteligenta = Game1.ran.Next(1, 6);
                                            for (int k = 0; k <= 10; k++)
                                                cc.rot[k] = -99f;

                                            int nr_c = Game1.ran.Next(1, 5);
                                            cc.parti[0] = Game1.ran.Next(1, Game1.NR_PARTI + 1);
                                            cc.rot[0] = 0;
                                            if (nr_c >= 2)
                                            {
                                                cc.parti[2] = Game1.ran.Next(1, Game1.NR_PARTI + 1);
                                                cc.rot[2] = 0;
                                            }
                                            if (nr_c >= 3)
                                            {
                                                cc.parti[1] = cc.parti[2];
                                                cc.rot[1] = (float)(Game1.ran.Next(0, 45)) * 0.01745327777777777777777777777778f;
                                            }
                                            if (nr_c >= 4)
                                            {
                                                cc.parti[3] = cc.parti[0];
                                                cc.rot[3] = cc.rot[1];
                                            }

                                            cc.parti[4] = Game1.ran.Next(1, Game1.NR_PARTI + 1);
                                            cc.rot[4] = (float)(90 - Game1.ran.Next(0, 140)) * 0.01745327777777777777777777777778f;

                                            nr_c = Game1.ran.Next(0, 5);
                                            if (nr_c >= 1)
                                            {
                                                cc.parti[5] = Game1.ran.Next(1, Game1.NR_PARTI + 1);
                                                cc.rot[5] = 0;
                                            }
                                            if (nr_c >= 2)
                                            {
                                                cc.parti[7] = Game1.ran.Next(1, Game1.NR_PARTI + 1);
                                                cc.rot[7] = 0;
                                            }
                                            if (nr_c >= 3)
                                            {
                                                cc.parti[6] = cc.parti[7];
                                                cc.rot[6] = (float)(Game1.ran.Next(0, 45)) * 0.01745327777777777777777777777778f;
                                            }
                                            if (nr_c >= 4)
                                            {
                                                cc.parti[8] = cc.parti[5];
                                                cc.rot[8] = cc.rot[1];
                                            }
                                            cc.parti[9] = Game1.ran.Next(1, Game1.NR_PARTI + 1);
                                            cc.rot[9] = 0;

                                            cc.mers = 10;


                                            if (mod_loc[tip_loc[nr]][i + 9][j + 9 - 1] != 1 && Game1.ran.Next(0, 3) != 0)
                                            {
                                                cc.fata = SpriteEffects.None;
                                                cc.orientare = 1;
                                            }
                                            else
                                            {
                                                cc.fata = SpriteEffects.FlipHorizontally;
                                                cc.orientare = -1;
                                            }
                                        }
                                        else cc.inteligenta = -Game1.ran.Next(2, 6);

                                        AUX.creaturi[AUX.nr_creaturi++] = cc;
                                        #endregion
                                    }
                                }
                int next_loc;
                #region URM_LOC_2  
                if (tip_loc[nr] == 2 || tip_loc[nr] == 3 || tip_loc[nr] == 6 || tip_loc[nr] == 7 || tip_loc[nr] == 8 || tip_loc[nr] == 9 || tip_loc[nr] == 10 || tip_loc[nr] == 11)
                {
                    if (loc[nr].Y - 19 > 30 && tip_loc[nr] != 10 && tip_loc[nr] != 12)
                    {
                        int ok = 1;
                        for (int xx = 1; xx <= nr_loc; xx++)
                            if (loc[xx].X == loc[nr].X && loc[xx].Y == loc[nr].Y - 19)
                                ok = 0;
                        if (ok == 1)
                        {
                            nr_loc++;
                            do
                                next_loc = Game1.ran.Next(2, NR_LOC);
                            while (next_loc == 5 || next_loc == 9 || next_loc == 11);

                            tip_loc[nr_loc] = next_loc;
                            loc[nr_loc] = new Vector2(loc[nr].X, loc[nr].Y - 19);
                        }
                    }
                    if (loc[nr].Y + 19 < 270 && tip_loc[nr] != 9 && tip_loc[nr] != 11)
                    {
                        int ok = 1;
                        for (int xx = 1; xx <= nr_loc; xx++)
                            if (loc[xx].X == loc[nr].X && loc[xx].Y == loc[nr].Y + 19)
                                ok = 0;
                        if (ok == 1)
                        {
                            nr_loc++;
                            do
                                next_loc = Game1.ran.Next(2, NR_LOC);
                            while (next_loc == 4 || next_loc == 10 || next_loc == 12);

                            tip_loc[nr_loc] = next_loc;
                            loc[nr_loc] = new Vector2(loc[nr].X, loc[nr].Y + 19);
                        }
                    }
                }

                if (tip_loc[nr] == 6 || tip_loc[nr] == 8 || tip_loc[nr] == 9 || tip_loc[nr] == 10 || tip_loc[nr] == 11 || tip_loc[nr] == 12)
                    if (loc[nr].X - 19 > 30)
                    {
                        int ok = 1;
                        for (int xx = 1; xx <= nr_loc; xx++)
                            if (loc[xx].X == loc[nr].X - 19 && loc[xx].Y == loc[nr].Y)
                                ok = 0;
                        if (ok == 1)
                        {
                            do
                                next_loc = Game1.ran.Next(2, NR_LOC);
                            while (next_loc != 7);
                            nr_loc++;
                            tip_loc[nr_loc] = next_loc;
                            loc[nr_loc] = new Vector2(loc[nr].X - 19, loc[nr].Y);
                        }
                    }
                if (tip_loc[nr] == 7 || tip_loc[nr] == 8)
                    if (loc[nr].X + 19 < 270)
                    {
                        int ok = 1;
                        for (int xx = 1; xx <= nr_loc; xx++)
                            if (loc[xx].X == loc[nr].X + 19 && loc[xx].Y == loc[nr].Y)
                                ok = 0;
                        if (ok == 1)
                        {
                            do
                                next_loc = Game1.ran.Next(2, NR_LOC);
                            while (next_loc != 6 && next_loc != 9 && next_loc != 10 && next_loc != 11 && next_loc != 12);
                            nr_loc++;
                            tip_loc[nr_loc] = next_loc;
                            loc[nr_loc] = new Vector2(loc[nr].X + 19, loc[nr].Y);
                        }
                    }
                #endregion
            }

            for (int nr = 1; nr <= nr_loc; nr++)
            {
                int xx, yy;
                xx = (int)loc[nr].X;
                yy = (int)loc[nr].Y;

                if (AUX.a[xx, yy - 10] == 0)
                    AUX.b[xx, yy - 9] = 1000000000;
                if (AUX.a[xx + 2, yy - 10] == 0)
                    AUX.b[xx + 2, yy - 9] = 1000000000;
                if (AUX.a[xx + 1, yy - 10] == 0)
                    AUX.b[xx + 1, yy - 9] = 1000000000;

                if (AUX.a[xx, yy + 10] == 0)
                    AUX.b[xx, yy + 9] = 1000000000;
                if (AUX.a[xx + 2, yy + 10] == 0)
                    AUX.b[xx + 2, yy + 9] = 1000000000;
                if (AUX.a[xx + 1, yy + 10] == 0)
                    AUX.b[xx + 1, yy + 9] = 1000000000;

                if (AUX.a[xx + 10, yy] == 0)
                    AUX.b[xx + 9, yy] = 1000000000;
                if (AUX.a[xx + 10, yy + 1] == 0)
                    AUX.b[xx + 9, yy + 1] = 1000000000;
                if (AUX.a[xx + 10, yy - 1] == 0)
                    AUX.b[xx + 9, yy - 1] = 1000000000;
                if (AUX.a[xx + 10, yy + 2] == 0)
                    AUX.b[xx + 9, yy + 2] = 1000000000;
                if (AUX.a[xx + 10, yy - 2] == 0)
                    AUX.b[xx + 9, yy - 2] = 1000000000;

                if (AUX.a[xx - 10, yy] == 0)
                    AUX.b[xx - 9, yy] = 1000000000;
                if (AUX.a[xx - 10, yy + 1] == 0)
                    AUX.b[xx - 9, yy + 1] = 1000000000;
                if (AUX.a[xx - 10, yy - 1] == 0)
                    AUX.b[xx - 9, yy - 1] = 1000000000;
            }

            return AUX;
        }
        static public Planeta ASTERORID()
        {
            Planeta AUX = new Planeta();

            int r1, r2;
            r1 = r2 = 0;
            int j = 1;
            for (j = -100; j <= 100; j++)
            {
                r1 = 101 + Game1.ran.Next(-1, 2);
                r2 = 101 + Game1.ran.Next(-1, 2);
                for (int i = -(int)Math.Sqrt(r1 * r1 - j * j); i <= (int)Math.Sqrt(r2 * r2 - j * j); i++)
                {
                    AUX.a[150 + i, 150 + j] = 200 + Game1.ran.Next(0, Game1.NR_subs);
                    AUX.b[150 + i, 150 + j] = 200;
                }
            }
            for (j = -20; j <= 20; j++)
            {
                r1 = 21 + Game1.ran.Next(-1, 2);
                r2 = 21 + Game1.ran.Next(-1, 2);
                for (int i = -(int)Math.Sqrt(r1 * r1 - j * j); i <= (int)Math.Sqrt(r2 * r2 - j * j); i++)
                {
                    AUX.apa[150 + i, 150 + j] = 7000 + Game1.ran.Next(0, Game1.NR_subs);
                    // AUX.b[150 + i, 150 + j] = 200;
                }
            }

            AUX.forta = 10;

            j = 2;
            while (AUX.a[150, j + 3] == 0)
                j++;

            Game1.PL_P.poz = new Vector2(150 * 20, j * 20);
            Game1.PL_P.Y = (int)(Game1.PL_P.poz.X + 10) / 20;
            Game1.PL_P.X = (int)(Game1.PL_P.poz.Y + 30) / 20;

            AUX.creaturi = new Creatura[2];
            AUX.creaturi[0] = new Creatura();
            AUX.creaturi[0].inteligenta = -1;
            AUX.creaturi[0].poz = Game1.PL_P.poz;

            return AUX;
        }
        static public Planeta PLANETA_AUX()
        {
            Planeta AUX = new Planeta();

            AUX.forta = 10;
            AUX.creaturi = new Creatura[100];
            for (int i = 0; i < 100; i++)
                AUX.creaturi[i] = new Creatura();
            AUX.nr_creaturi = 1;
            for (int i = 150; i < 300; i++)
                for (int j = 1; j < 300; j++)
                {
                    AUX.a[i, j] = 100;
                    AUX.b[i, j] = 1000000;
                }
            for (int i = 1; i < 300; i++)
                AUX.apa[150, i] = 12003;

            AUX.creaturi[0].inteligenta = -6;

            for (int i = 15; i < 30; i++)
            {
                AUX.a[149, i] = 1300;
                AUX.b[149, i] = 1000000;
                AUX.a[148, i] = 1301;
                AUX.b[148, i] = 0;
                AUX.a[147, i] = 1302;
                AUX.b[147, i] = 0;
            }
            for (int i = 2; i < 30; i++)
            {
                AUX.a[145, i] = 1303;
                AUX.b[145, i] = 0;
            }
            for (int i = 140; i <= 150; i++)
                AUX.apa[i, 6] = 12003;

            Game1.PL_P.poz = new Vector2(100, 100);
            AUX.creaturi[0].poz = new Vector2(200, 200);
            AUX.creaturi[0].X = 10;
            AUX.creaturi[0].Y = 10;
            AUX.creaturi[0].rot[4] = 0f;

            return AUX;
        }
        static public Planeta PLANETA_FROM_IMG(Texture2D aux)
        {
            Planeta PLAN = new Planeta();
            int nr = 0;
            Color[] pixels = new Color[aux.Height * aux.Width];
            aux.GetData<Color>(pixels);
            PLAN.ar = pixels[0].R;
            PLAN.ge = pixels[0].G;
            PLAN.be = pixels[0].B;
            PLAN.V_SKY = new Vector3((float)PLAN.ar / 1000, (float)PLAN.ge / 1000, (float)PLAN.be / 1000);

            for (int i = 1; i < 300; i++)
                for (int j = 1; j < 300; j++)
                {
                    Color auxx = pixels[i * 300 + j];
                    PLAN.a[i, j] = auxx.R * 100 + (auxx.B % 100) / 10;
                    PLAN.apa[i, j] = auxx.G * 1000 + auxx.B % 10;
                    if (auxx.B >= 100)
                        PLAN.b[i, j] = 1;
                    else PLAN.b[i, j] = 0;
                }
            PLAN.creaturi = new Creatura[aux.Height - 301];
            PLAN.nr_creaturi = 0;
            for (int k = 1; k <= aux.Height - 301; k++)
            {
                int i = 300 + k;
                Creatura cre = new Creatura();
                cre.X = cre.Y = 0;

                Color C_p = pixels[i * 300 + 0];
                cre.X = C_p.R * 100 + C_p.G * 10 + C_p.B;

                C_p = pixels[i * 300 + 1];
                cre.Y = C_p.R * 100 + C_p.G * 10 + C_p.B;
                cre.poz = new Vector2(cre.X * 20, cre.Y * 20);

                C_p = pixels[i * 300 + 2];
                cre.inteligenta = C_p.G;
                if (C_p.R % 10 == 1)
                    cre.inteligenta *= -1;
                cre.parti = new int[10];
                for (int j = 0; j <= 9; j++)
                    cre.parti[j] = C_p.B;
                cre.rot = new float[10];
                cre.orientare = 1 - C_p.R / 10;
                if (cre.orientare == -1)
                    cre.fata = SpriteEffects.FlipHorizontally;
                cre.rot[0] = -100;
                cre.rot[1] = -0.9f * cre.orientare;
                cre.rot[2] = -100;
                cre.rot[3] = 0.9f * cre.orientare;
                cre.rot[4] = 0f; /// Corp
                cre.rot[5] = -0.9f * cre.orientare;
                cre.rot[6] = -100;
                cre.rot[7] = 0.9f * cre.orientare;
                cre.rot[8] = -100;
                cre.rot[9] = 0f; /// Cap

                string nume = "";
                for (int j = 0; ; j++)
                {
                    C_p = pixels[i * 300 + 3 + j];
                    char character = (char)C_p.R;
                    if (character != 0)
                        nume += character + "";
                    character = (char)C_p.G;
                    if (character != 0)
                        nume += character + "";
                    character = (char)C_p.B;
                    if (character != 0)
                        nume += character + "";
                    if (C_p.R == 0 || C_p.G == 0 || C_p.B == 0 || j == 290)
                        break;
                }
                cre.nume = nume;

                PLAN.creaturi[PLAN.nr_creaturi++] = cre;
            }
            PLAN.forta = 10;
            Game1.PL_P.X = PLAN.creaturi[0].Y;
            Game1.PL_P.Y = PLAN.creaturi[0].X;
            Game1.PL_P.poz = new Vector2(Game1.PL_P.X * 20, Game1.PL_P.Y * 20);
            return PLAN;
        }

        static public LocPlaneta LOC_PLANETA(int k, int kk, int TIP)
        {
            LocPlaneta AUX = new LocPlaneta();
            AUX.SALVAT = 0;
            int ok = 1;
            int aux = 0;
            AUX.ord_elm = new int[Game1.NR_subs + 3];           ///ORDINEA ELEMENTELOR
            for (int i = 1; i < Game1.NR_subs; i++)
            {
                ok = 0;
                while (ok == 0)
                {
                    ok = 1;
                    AUX.ord_elm[i] = Game1.ran.Next(1, Game1.NR_subs);
                    for (int j = 0; j < i; j++)
                        if (AUX.ord_elm[j] == AUX.ord_elm[i])
                            ok = 0;
                }
            }
            AUX.ord_elm[0] = 0;
            AUX.temperatura_z = Game1.ran.Next(-300, 300);
            AUX.p_orb = k;


            if (TIP == 1)
            {
                if (k == kk)
                {
                    do
                    {
                        ok = 1;
                        AUX.poz = Game1.PL.poz;
                        AUX.poz.X += 800000 - Game1.ran.Next(200000, 1600000);
                        AUX.poz.Y += 800000 - Game1.ran.Next(200000, 1600000);

                        for (int i = 1; i <= Game1.nr_PLA_S; i++)
                        {
                            float xx, yy, r;
                            xx = Game1.L_PLA[i].poz.X - AUX.poz.X;
                            yy = Game1.L_PLA[i].poz.Y - AUX.poz.Y;
                            r = (float)Math.Sqrt(xx * xx + yy * yy);

                            if (r <= 100000)
                                ok = 0;
                        }
                        aux++;
                    }
                    while (ok == 0 && aux < 50000);

                    AUX.ord_elm[Game1.NR_subs + 1] = 8;

                    /*AUX.poz = new Vector2(60000 * kk, 60000 * kk);  */
                }
                else
                {
                    aux = Game1.ran.Next(0, 15);
                    if (aux != 10)
                    {
                        AUX.ar = Game1.ran.Next(150, 255);
                        AUX.be = Game1.ran.Next(150, 255);
                        AUX.ge = Game1.ran.Next(150, 255);

                        aux = Game1.ran.Next(0, 4);
                        AUX.MOON = aux;

                        if (Math.Abs(AUX.temperatura_z) < 200)
                            AUX.ord_elm[Game1.NR_subs + 1] = AUX.ord_elm[1];
                        else if (AUX.temperatura_z < 0)
                            AUX.ord_elm[Game1.NR_subs + 1] = Game1.NR_subs;
                        else AUX.ord_elm[Game1.NR_subs + 1] = Game1.NR_subs + 1;

                        AUX.forta = Game1.ran.Next(5, 25);
                    }
                    else
                    {
                        AUX.ar = Game1.ran.Next(10, 115);
                        AUX.be = Game1.ran.Next(10, 115);
                        AUX.ge = Game1.ran.Next(10, 115);
                        AUX.MOON = 0;
                        AUX.ord_elm[Game1.NR_subs + 1] = 0;
                    }

                    AUX.poz = Game1.L_PLA[k].poz;
                    AUX.R = Game1.ran.Next(20000, 100000);
                    AUX.ung = (float)Game1.ran.Next(0, 360) / 3.14159f;
                    AUX.poz.X += (float)Math.Sin(AUX.ung) * AUX.R;
                    AUX.poz.Y += (float)Math.Cos(AUX.ung) * AUX.R;

                }
            }
            else if (TIP == 0)
            {
                AUX.ord_elm[Game1.NR_subs + 1] = 7;
                AUX.R = Game1.ran.Next(3500, 5000);
                AUX.poz = Game1.L_PLA[k].poz;
                AUX.ung = (float)Game1.ran.Next(0, 360) / 3.14159f;
                AUX.poz.X += (float)Math.Sin(AUX.ung) * AUX.R;
                AUX.poz.Y += (float)Math.Cos(AUX.ung) * AUX.R;
            }
            else if (TIP == 2)
            {
                if (k != kk)
                {
                    AUX.poz = Game1.L_PLA[k].poz;

                    AUX.ord_elm[Game1.NR_subs + 1] = 9;
                    AUX.R = Game1.ran.Next(20000, 100000);
                    AUX.poz = Game1.L_PLA[k].poz;
                    AUX.ung = (float)Game1.ran.Next(0, 360) / 3.14159f;
                    AUX.poz.X += (float)Math.Sin(AUX.ung) * AUX.R;
                    AUX.poz.Y += (float)Math.Cos(AUX.ung) * AUX.R;
                }
                else
                {
                    AUX.poz = new Vector2(0, 0);
                    AUX.ord_elm[Game1.NR_subs + 1] = 8;
                }
            }

            return AUX;
        }

        static public void SISTEM()
        {
            Game1.L_PLA[0].ID = 1000;
            int aux = 1;
            int tip_PLA = 1;
            int tip_AST = 0;
            int nr_pln_s = 3;
            Game1.nr_PLA_S = 500;
            for (int i = 1; i < Game1.nr_PLA_S; i++)
            {
                if (i <= nr_pln_s)
                {
                    Game1.L_PLA[i] = LOC_PLANETA(aux, i, 2);
                    Game1.L_PLA[i].ID = i;
                    Game1.L_PLA[i].SALVAT = 0;
                }
                else
                {
                    if (tip_PLA != 0 || aux == i)
                        Game1.L_PLA[i] = LOC_PLANETA(aux, i, 1);
                    else Game1.L_PLA[i] = LOC_PLANETA(tip_AST, i, 0);

                    if (Game1.L_PLA[i].MOON == 1 && Game1.L_PLA[i].p_orb != 0)
                    {
                        tip_AST = i;
                        tip_PLA = 0;
                    }
                    if ((int)Math.Sqrt(Game1.ran.Next(0, 100025)) <= 24 || i - tip_AST >= 15)
                        tip_AST = 0;
                    if (tip_AST == 0)
                        tip_PLA = i;

                    int last_id = Game1.L_PLA[i - 1].ID;
                    if (last_id < 0)
                        last_id *= -1;
                    if (last_id < 1000)
                        last_id = 1000;
                    if (tip_AST != 0)
                        Game1.L_PLA[i].ID = last_id + 1;
                    else
                        Game1.L_PLA[i].ID = -last_id - i;
                    if (Game1.ran.Next(0, 40) == 2)
                    {
                        aux = i + 1;
                        tip_AST = 0;
                    }
                }
            }
        }
    }
}
