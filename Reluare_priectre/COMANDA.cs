using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Reluare_priectre
{
    public class COMANDA
    {
        static public void cmd(string a1, string a2, float v1, float v2)
        {
            Random ran = new Random();
            if (a1 == "set_menu")
            {
                Game1.ZOOM = 1;
                Game1.PL.rot = 0f;
                Game1.MENU = (int)v1;

                if(Game1.MENU == 4)
                    if (Game1.WINDOW_REZ.X < 700)
                        Game1.ZOOM = Game1.WINDOW_REZ.X / 700;

                if (Game1.MENU == 3)
                    Game1.MOUSE_T = Game1.game.Content.Load<Texture2D>("MOUSE1");
                else
                    Game1.MOUSE_T = Game1.game.Content.Load<Texture2D>("MOUSE2");
                if (Game1.MENU == 5)
                    Game1.PL_P.viata = Game1.PL_P.max_viata;
                Game1.TIME = 0;
                Game1.COMP_A = 0;
                if (Game1.MENU == 1)
                {
                    Game1.NR_PRO = 104;
                    for (int i = 1; i <= Game1.NR_PRO; i++)
                    {
                        if (i % 20 != 1 && i <= Game1.NR_PRO - Game1.NR_PRO % 20)
                        {
                            Game1.LAS[i].poz = Game1.LAS[(int)(i / 20) * 20 + 1].poz + new Vector2(10 - ran.Next(20), 10 - ran.Next(20));
                            Game1.LAS[i].t = (i - 1) % 20 + ran.Next(0, ((i - 1) % 20) / 2);
                            Game1.LAS[i].fx = 0f;
                            Game1.LAS[i].fy = 0f;
                        }
                        else
                        {
                            int rrr = ran.Next(0, 100);
                            if (rrr % 4 == 0)
                                Game1.LAS[i].poz = new Vector2(-20, ran.Next(5, (int)Game1.WINDOW_REZ.X - 5));
                            else if (rrr % 4 == 1)
                                Game1.LAS[i].poz = new Vector2(ran.Next(5, (int)Game1.WINDOW_REZ.Y - 5), (int)Game1.WINDOW_REZ.X + 20);
                            else if (rrr % 4 == 2)
                                Game1.LAS[i].poz = new Vector2((int)Game1.WINDOW_REZ.Y + 20, ran.Next(5, (int)Game1.WINDOW_REZ.X - 5));
                            else if (rrr % 4 == 3)
                                Game1.LAS[i].poz = new Vector2(ran.Next(5, (int)Game1.WINDOW_REZ.Y - 5), -20);

                            float ung = ran.Next(0, 360) / 3.14159265f;
                            Game1.LAS[i].fx = 7 * (float)Math.Sin(ung);
                            Game1.LAS[i].fy = 7 * (float)Math.Cos(ung);

                        }
                    }
                }
                else
                    Game1.NR_PRO = 0;
                Game1.BUTON_A_1 = true;
                Game1.BUTON_A_2 = true;
                Game1.BUTON_A_3 = true;
                if (Game1.MENU < 7)
                    Game1.MENIU_VECTOR = new Vector2(Game1.MENIU_TEX[1, Game1.MENU].Width / 2, Game1.MENIU_TEX[1, Game1.MENU].Height / 2);

                if (v1 >= 20)
                {
                    Game1.JOC_MATRICE = new int[50, 100];
                    Game1.JOC_SIR = new int[1000];
                    Game1.JOC_X = new float[1000];
                    Game1.JOC_Y = new float[1000];
                    Game1.JOC_VECTOR = new Vector2[1000];
                    Game1.NR_JOC = 1;

                    if (Game1.MENU == 20)
                    {
                        Game1.JOC_VECTOR[0].X = Game1.WINDOW_REZ.Y / 2;
                        Game1.JOC_VECTOR[0].Y = Game1.WINDOW_REZ.X;
                        Game1.JOC_VECTOR[0].Y -= Game1.WINDOW_REZ.X / 12;
                        for (int i = 1; i < 50; i++)
                            for (int j = 1; j < 100; j++)
                                Game1.JOC_MATRICE[i, j] = 1;

                        Game1.JOC_VECTOR[1] = Game1.JOC_VECTOR[0];
                        Game1.JOC_VECTOR[1].Y -= 100;
                        Game1.JOC_SIR[1] = 6;

                        Game1.JOC_Y[1] = 5;
                        Game1.JOC_X[1] = 0;
                    }
                    else if (Game1.MENU == 21)
                    {
                        Game1.JOC_VECTOR[0].X = Game1.WINDOW_REZ.Y / 2;
                        Game1.JOC_VECTOR[0].Y = Game1.WINDOW_REZ.X;
                        Game1.JOC_VECTOR[0].Y -= Game1.WINDOW_REZ.X / 12;

                        for (int aux = 0; aux < 14; aux++)
                        {
                            int xx, yy;
                            xx = ran.Next(1, 50);
                            yy = ran.Next(1, 100);

                            if (aux < 10)
                                for (int jj = yy - 2; jj <= yy + 2; jj++)
                                {
                                    if (jj > 0 && jj < 100)
                                        Game1.JOC_MATRICE[xx, jj] = 3;
                                }
                            else
                            {
                                while (xx > 25 || Game1.JOC_MATRICE[xx, yy] != 0)
                                {
                                    xx = ran.Next(1, 25);
                                    yy = ran.Next(1, 100);
                                }
                                Game1.JOC_MATRICE[xx, yy] = -aux % 2 - 1;
                            }
                        }

                        Game1.NR_JOC = 0;
                    }
                    else if (Game1.MENU == 22)
                    {
                        Game1.JOC_MATRICE = new int[51, 51];
                        for (int i = 0; i <= 50; i++)
                            Game1.JOC_MATRICE[0, i] = Game1.JOC_MATRICE[50, i] = Game1.JOC_MATRICE[i, 0] = Game1.JOC_MATRICE[i, 50] = 1;
                        for (int i = 1; i <= 21; i++)
                        {
                            for (int j = i * 2; j < 25; j++)
                                if (ran.Next(0, 3) != 2)
                                {
                                    Game1.JOC_MATRICE[i * 2, j] = Game1.JOC_MATRICE[50 - i * 2, j] = 1;
                                    Game1.JOC_MATRICE[i * 2, 50 - j] = Game1.JOC_MATRICE[50 - i * 2, 50 - j] = 1;

                                    Game1.JOC_MATRICE[j, i * 2] = Game1.JOC_MATRICE[j, 50 - i * 2] = 1;
                                    Game1.JOC_MATRICE[50 - j, i * 2] = Game1.JOC_MATRICE[50 - j, 50 - i * 2] = 1;
                                }
                        }
                        for (int i = 7; i < 18; i += 2)
                            for (int j = i; j <= 25; j++)
                            {
                                Game1.JOC_MATRICE[i, j] = Game1.JOC_MATRICE[50 - i, j] = 2;
                                Game1.JOC_MATRICE[i, 50 - j] = Game1.JOC_MATRICE[50 - i, 50 - j] = 2;

                                Game1.JOC_MATRICE[j, i] = Game1.JOC_MATRICE[j, 50 - i] = 2;
                                Game1.JOC_MATRICE[50 - j, i] = Game1.JOC_MATRICE[50 - j, 50 - i] = 2;

                            }

                        for (int i = 3; i <= 19; i += 2)
                        {
                            int x, y;
                            do
                            {
                                x = ran.Next(0, 51);
                                y = ran.Next(0, 51);
                            } while (Game1.JOC_MATRICE[x, y] == 1);
                            Game1.JOC_VECTOR[i] = new Vector2(y * 20, x * 20);
                            Game1.JOC_SIR[i] = ran.Next(1, 5);
                        }
                        Game1.NR_JOC = 0;

                        Game1.JOC_VECTOR[0] = new Vector2(500, 500);
                    }
                    else if (Game1.MENU == 23)
                    {
                        Game1.JOC_VECTOR[0].X = Game1.WINDOW_REZ.Y / 2;
                        Game1.JOC_VECTOR[0].Y = Game1.WINDOW_REZ.X / 2;

                        Game1.JOC_MATRICE = new int[51, 51];
                        for (int i = 0; i <= 50; i++)
                            Game1.JOC_MATRICE[0, i] = Game1.JOC_MATRICE[50, i] = Game1.JOC_MATRICE[i, 0] = Game1.JOC_MATRICE[i, 50] = 1;

                        for (int nr = 0; nr < 100; nr++)
                        {
                            int x, y;
                            do
                            {
                                x = ran.Next(1, 50);
                                y = ran.Next(1, 50);
                            } while (Game1.JOC_MATRICE[x, y] != 0 || (x > 20 && x < 30 && y > 20 && y < 30));
                            if ((x + y) % 3 == 0)
                                Game1.JOC_MATRICE[x, y] = 1;
                            do
                            {
                                x = ran.Next(1, 50);
                                y = ran.Next(1, 50);
                            } while (Game1.JOC_MATRICE[x, y] != 0 || (x > 20 && x < 30 && y > 20 && y < 30));
                            Game1.JOC_MATRICE[x, y] = 2;
                            do
                            {
                                x = ran.Next(1, 50);
                                y = ran.Next(1, 50);
                            } while (Game1.JOC_MATRICE[x, y] != 0 || (x > 20 && x < 30 && y > 20 && y < 30));
                            Game1.JOC_MATRICE[x, y] = 3;
                        }

                        Game1.NR_JOC = 0;
                    }
                }
                else
                {
                    Game1.JOC_MATRICE = new int[1, 1];
                    Game1.JOC_SIR = new int[1];
                    Game1.JOC_X = new float[1];
                    Game1.JOC_Y = new float[1];
                    Game1.JOC_VECTOR = new Vector2[1];
                    Game1.NR_JOC = 1;
                }
            }
            else if (a1 == "add")
            {
                if (a2 == "item")
                {
                    Game1.inventar[(int)v1] += (int)v2;
                   // ADD_CHAT_LINE("ADDED  ITEM " + (int)v1 + "; quantity  " + (int)v2);
                }
                else if (a2 == "planet")
                {
                    if (v1 < 1000)
                    {
                        Game1.L_PLA[Game1.nr_PLA_S].ID = (int)v1;
                        Game1.nr_PLA_S++;
                      ///  ADD_CHAT_LINE("ADDED  PLANET  with  ID  " + (int)v1);
                    }
                }
            }
            else if (a1 == "subtract")
            {
                if (a2 == "item")
                {
                    Game1.inventar[(int)v1] -= (int)v2;
                   /// ADD_CHAT_LINE("SUBTRACTED  ITEM  " + (int)v1 + ";  quantity  " + (int)v2);
                }
                else if (a2 == "planet")
                {
                    int ok = -1;
                    for (int i = 1; i <= Game1.nr_PLA_S; i++)
                        if (Game1.L_PLA[i].ID == (int)v1)
                            ok = i;
                    if (ok != -1)
                    {
                        for (int i = ok; i < Game1.nr_PLA_S; i++)
                            Game1.L_PLA[i] = Game1.L_PLA[i + 1];
                        Game1.nr_PLA_S--;
                       /// ADD_CHAT_LINE("SUBTRACTED  PLANET  with  ID  " + (int)v1);
                    }
                }
            }
            else if(a1 == "play")
            {
                if(a2 == "B_music_")
                {
                   // SoundEffect Backgound_music = Game1.game.Content.Load<SoundEffect>("Sounds/" + a2 + (int)v1);
                    MediaPlayer.Play(Song.FromUri("Sounds/" + a2 + (int)v1, new Uri("Sounds/" + a2 + (int)v1)));
                    
                }
                else
                {
                    SoundEffect sound = Game1.game.Content.Load<SoundEffect>("Sounds/" + a2 + (int)v1);
                    sound.Play(v2, 0f, 0f);
                }
            }
        }
    }
}
