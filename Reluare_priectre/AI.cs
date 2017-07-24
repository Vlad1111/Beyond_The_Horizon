using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Reluare_priectre
{
    class AI
    {
        static public Creatura FIINTA(Creatura aux)
        {
            aux.X = (int)(aux.poz.X + 16 - Math.Abs(Math.Sin(aux.rot[4])) * 8) / 20 + 1;
            aux.Y = (int)(aux.poz.Y + 10) / 20;
            if (aux.X > 0 && aux.X < 300)
                if (aux.Y > 0 && aux.Y < 300)
                {
                    if (aux.X > 3)
                        if (Game1.PLA_S.b[aux.X - 2, aux.Y] != 0)
                        {
                            aux.poz.X += aux.fx;
                            aux.fx = 0;
                            aux.poz.X += (int)(aux.poz.X + 16 - Math.Abs(Math.Sin(aux.rot[4])) * 8) % 20 / 2;
                        }
                    if (Game1.PLA_S.b[aux.X, aux.Y] != 0)
                    {
                        //aux.forta = (float)aux.pow / 3;
                        aux.fx = 0;
                        aux.poz.X -= (int)(aux.poz.X + 16 - Math.Abs(Math.Sin(aux.rot[4])) * 8) % 20 / 2;
                    }
                    else aux.fx -= (float)Game1.PLA_S.forta / 30;
                    if (aux.fx <= -20)
                        aux.fx = -19;

                    aux.poz.X -= aux.fx;

                    int x, y;
                    x = aux.xx;
                    y = aux.yy;

                    if (aux.agresiune == 1)
                    {
                        float xx, yy;
                        xx = aux.X - Game1.PL_P.X;
                        yy = aux.Y - Game1.PL_P.Y;
                        if (xx * xx + yy * yy <= aux.r * aux.r)
                        {
                            x = Game1.PL_P.X;
                            y = Game1.PL_P.Y;
                            if (xx * xx + yy * yy <= 2)
                                Game1.PL_P.viata -= aux.pow;
                            else if (aux.inteligenta >= 5)
                            {
                                if (Game1.TIME % 4 == 0)
                                {
                                    float L, Lx, Ly;
                                    Lx = Game1.PL_P.poz.X - aux.poz.Y;
                                    Ly = Game1.PL_P.poz.Y - aux.poz.X;
                                    L = (float)Math.Sqrt(Lx * Lx + Ly * Ly);

                                    Game1.LAS[Game1.NR_PRO].poz.X = aux.poz.Y;
                                    Game1.LAS[Game1.NR_PRO].poz.Y = aux.poz.X;
                                    Game1.LAS[Game1.NR_PRO].fx -= -Lx / L * 2;
                                    Game1.LAS[Game1.NR_PRO].fy = Ly / L * 2;
                                    Game1.LAS[Game1.NR_PRO].poz.X += Game1.LAS[Game1.NR_PRO].fx;
                                    Game1.LAS[Game1.NR_PRO].poz.Y += Game1.LAS[Game1.NR_PRO].fy - 8;
                                    Game1.LAS[Game1.NR_PRO].pow = aux.pow;
                                    Game1.LAS[Game1.NR_PRO].tip_p = 7;

                                    Game1.NR_PRO++;
                                    COMANDA.cmd("play", "Laser_", 1, 1);
                                }
                            }
                        }

                        if (aux.Y < y)
                        {
                            aux.mers++;
                            if (aux.mers >= 20)
                                aux.mers = -19;
                            if (Game1.PLA_S.b[aux.X - 1, aux.Y + 1] == 0 && Game1.PLA_S.b[aux.X - 2, aux.Y + 1] == 0)
                            {
                                aux.poz.Y += 3;
                                aux.fata = SpriteEffects.FlipHorizontally;
                                aux.orientare = -1;
                            }
                            else if (Game1.PLA_S.b[aux.X, aux.Y] != 0 && Game1.PLA_S.b[aux.X - 3, aux.Y] == 0)
                            {
                                aux.fx = (float)aux.pow * 0.7f;
                                aux.poz.X -= aux.fx;
                            }

                        }
                        else if (aux.Y > y)
                        {
                            aux.mers++;
                            if (aux.mers >= 20)
                                aux.mers = -19;
                            if (aux.X > 3 && aux.Y > 2)
                                if (Game1.PLA_S.b[aux.X - 1, aux.Y - 1] == 0 && Game1.PLA_S.b[aux.X - 2, aux.Y - 1] == 0)
                                {
                                    aux.poz.Y -= 3;
                                    aux.fata = SpriteEffects.None;
                                    aux.orientare = 1;
                                }
                                else if (Game1.PLA_S.b[aux.X, aux.Y] != 0 && Game1.PLA_S.b[aux.X - 3, aux.Y] == 0)
                                {
                                    aux.fx = (float)aux.pow * 0.7f;
                                    aux.poz.X -= aux.fx;
                                }

                        }
                        else aux.mers = 10;
                    }
                }

            return aux;
        }

        static public Nava NPC(Nava aux)
        {
            if (aux.eng < aux.eng_m)
                aux.eng++;
            float rot;
            float dif_rot;
            if (aux.auto_pilot == -1)
            {
                rot = -MATH.ung(Game1.PL.poz, aux.poz);
                rot = MATH.prim_cadran(rot);
                float fractie = 0.001f * (float)aux.pow / aux.nr_c;
                dif_rot = MATH.prim_cadran(rot - aux.rot);
                if (dif_rot > 3.1415926f)
                    dif_rot -= 2f * 3.1415926f;
                if (fractie > 1)
                    fractie = 1;


                if (MATH.dis_prt(Game1.PL.poz, aux.poz) > 500 * 500)
                {
                    aux.rot += (dif_rot + 3.1415926f / 2f) * fractie;

                    aux.F += 0.005f;
                    if (aux.F > 1)
                        aux.F = 1;
                }
                else
                {
                    aux.rot += 0.01f * dif_rot * fractie * MATH.semn((float)(10 - Game1.ran.Next(0, 20)));
                    aux.F -= 0.005f;
                    if (aux.F < 0)
                        aux.F = 0;
                }
            }
            else if (Game1.NPC[aux.auto_pilot] != null)
            {
                rot = -MATH.ung(Game1.NPC[aux.auto_pilot].poz, aux.poz);
                rot = MATH.prim_cadran(rot);
                dif_rot = MATH.prim_cadran(rot - aux.rot);
                if (dif_rot > 3.1415926f)
                    dif_rot -= 2f * 3.1415926f;

                float fractie = 0.001f * (float)aux.pow / aux.nr_c;
                if (fractie > 1)
                    fractie = 1;
                if (MATH.dis_prt(Game1.NPC[aux.auto_pilot].poz, aux.poz) > 500 * 500)
                {
                    aux.rot += (dif_rot + 3.1415926f / 2f) * fractie;
                    aux.F += 0.005f;
                    if (aux.F > 1)
                        aux.F = 1;
                }
                else
                {
                    aux.rot += 0.01f * dif_rot * fractie * MATH.semn((float)(10 - Game1.ran.Next(0, 20)));
                    aux.F -= 0.005f;
                    if (aux.F < 0.5f)
                        aux.F = 0.5f;
                }

                if (aux.eng > 0)
                    if (Game1.TIME % 5 == 0)
                    {
                        rot = -MATH.ung(Game1.PL.poz, aux.poz);
                        rot = MATH.prim_cadran(rot);
                        dif_rot = MATH.prim_cadran(rot - aux.rot);
                        if (dif_rot > 3.1415926f)
                            dif_rot -= 2f * 3.1415926f;
                        if (MATH.prim_cadran(Math.Abs(dif_rot + 3.1415926f / 2f)) < 3.1415926f / 10f)
                        {
                            Vector2 poz = Game1.PL_P_E;
                            for (int i = 0; i < 37; i++)
                                for (int j = 0; j < 37; j++)
                                    if (Game1.comp[aux.comp[i, j]].proi != 0)
                                    {
                                        poz = Game1.PL_P_E + aux.poz - Game1.PL_P_E;
                                        poz.X += ((float)((i - 18) * Math.Cos(aux.rot) - (j - 18) * Math.Sin(aux.rot))) * 20;
                                        poz.Y += ((float)((j - 18) * Math.Cos(aux.rot) + (i - 18) * Math.Sin(aux.rot))) * 20;

                                        Game1.LAS[Game1.NR_PRO].poz = poz;
                                        Game1.LAS[Game1.NR_PRO].tip_p = 9;
                                        Game1.LAS[Game1.NR_PRO].fx = (float)Math.Cos(aux.rot) * 40;
                                        Game1.LAS[Game1.NR_PRO].fy = (float)Math.Sin(aux.rot) * 40;
                                        Game1.LAS[Game1.NR_PRO].pow = Game1.comp[aux.comp[i, j]].proi;
                                        Game1.LAS[Game1.NR_PRO].t = 100;
                                        Game1.NR_PRO++;

                                        aux.eng--;
                                    }
                        }
                    }
            }
            aux.poz.X += aux.F * aux.pow * (float)Math.Cos(aux.rot) / aux.nr_c;
            aux.poz.Y += aux.F * aux.pow * (float)Math.Sin(aux.rot) / aux.nr_c;
            return aux;
        }
    }
}
