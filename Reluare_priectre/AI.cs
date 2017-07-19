using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

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
    }
}
