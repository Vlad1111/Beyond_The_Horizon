using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;
using System.IO;
using System.Text;
using System.IO.Compression;

namespace Reluare_priectre
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public class Proiectil
        {
            public Vector2 poz;
            public int tip_p;
            public float fx, fy;
            public int pow;
            public int t;
        }
        public class Componenta
        {
            public Texture2D T;
            public int pow;
            public int v;
            public int eng;
            public int nr;
            public int proi;
        }
        public class Nava
        {
            public Vector2 poz;
            public int[,] comp;
            public int[,] viata;
            public float rot;
            public int pow;
            public int eng;
            public int eng_m;
            public int nr_c;
        }
        public class Creatura
        {
            public int[] parti;
            public float[] rot;
            public int[] tip_parti;
            public Vector2 poz;
            public int pow;
            public int r;
            public SpriteEffects fata;
            public int orientare;
            public int viata;
            public float mers;
            public float fx, fy;
            public int inteligenta;
            public int agresiune;
            public string nume;

            public int X, Y; ///pozitia picioarelor pe planeta
            public int xx, yy; ///pozitia "casei" 

            public Creatura()
            {
                this.rot = new float[10];
            }
        }

        public class Planeta
        {
            public int[,] a, b, apa;  /// a-element, b-"viata" 
            public int[] ord_elm;
            public Creatura[] creaturi;
            public int inaltime;
            public int ar, ge, be;     ///Culoarea cerului pe timp de zii
            public Vector3 V_SKY;
            public Vector3 SKY;
            public float timp;
            public int forta;
            public int temperatura_z, temperatura_n;
            public int MOON;
            public int nr_creaturi;

            public Planeta()
            {
                this.a = new int[300, 300];
                this.b = new int[300, 300];
                this.apa = new int[300, 300];
            }
        }
        public class LocPlaneta
        {
            public int[] ord_elm;
            public int MOON;
            public int ID;
            public Vector2 poz;
            public string NUME;
            public int SALVAT;
            public int ar, ge, be;     ///Culoarea cerului pe timp de zii   
            public int temperatura_z;
            public int forta;
            public int R; //raza fata de soare
            public int p_orb; // planeta de care orbiteaza
            public float ung; // unghiul la care se afla planeta fata de soare/planeta
        }

        #region VARIABILE
        string saveDir;

        public Texture2D[] BACK_IMG = new Texture2D[40];
        public Texture2D MOUSE_T;
        public Texture2D[,] MENIU_TEX;
        public int L_BACK;
        public int N_BACK = 6;
        public int MENU = 3;
        public int MENU_AUX;
        public int[] d1 = { 0, 1, 0, -1, 1, 1, -1, -1 }, d2 = { -1, 0, 1, 0, 1, -1, 1, -1 };//SA NU SCHIMB DIRECTIILE!
        public int[] OBTIUNI = { 0, 768, 1366, 0, 1 };
        public int TIME = 0;
        public Vector2 PL_P_E;
        public Vector2 MOUSE_P;
        public Vector2 WINDOW_REZ;
        public Vector2 MENIU_VECTOR;
        public bool BUTON_A_1 = false;
        public bool BUTON_A_2 = false;
        public bool BUTON_A_3 = false;
        public string[] CHAT = new string[10];
        public Random ran = new Random();

        private SpriteFont[] font = new SpriteFont[10];
        private int nr_FOTNTS;

        public Nava PL = new Nava();


        public Componenta[] comp;
        public int NR_comp = 16;
        public Texture2D[] items;
        public int NR_subs = 5;
        public int NR_elem = 22;
        public int NR_item = 9;
        public int[] inventar;
        public int COMP_A = 0;


        public int ZOOM_VAL = Mouse.GetState().ScrollWheelValue;
        public float ZOOM = 1;


        public Proiectil[] LAS = new Proiectil[100000];
        public Texture2D[] LAS_T = new Texture2D[10];
        public int NR_PRO = 0;
        public bool LAS_A = false;

        public Texture2D[,] PAR_C;
        public int NR_PARTI = 4;
        public Creatura PL_P = new Creatura();

        public Planeta PLA_S = new Planeta();
        public LocPlaneta[] L_PLA = new LocPlaneta[1000];
        public Texture2D[] MOONS = new Texture2D[4];
        public int nr_PLA_S = 1;
        public int PLA_A = 1;
        public Texture2D[,] PLA_T;
        public Texture2D[] PLA_TEX = new Texture2D[10];

        public int[,] JOC_MATRICE;
        public int[] JOC_SIR;
        public float[] JOC_X, JOC_Y;
        public Vector2[] JOC_VECTOR;
        public int NR_JOC;
        #endregion

        public void ADD_CHAT_LINE(string s)
        {
            for (int j = 9; j > 0; j--)
                CHAT[j] = CHAT[j - 1];
            CHAT[0] = s;
        }

        public void COMANDA(string a1, string a2, float v1, float v2)
        {
            if (a1 == "set_menu")
            {
                ZOOM = 1;
                PL.rot = 0f;
                MENU = (int)v1;
                if(MENU == 3)
                    MOUSE_T = Content.Load<Texture2D>("MOUSE1");
                else
                    MOUSE_T = Content.Load<Texture2D>("MOUSE2");
                COMP_A = 0;
                if (MENU == 1)
                {
                    NR_PRO = 104;
                    for (int i = 1; i <= NR_PRO; i++)
                    {
                        if (i % 20 != 1 && i <= NR_PRO - NR_PRO % 20)
                        {
                            LAS[i].poz = LAS[(int)(i / 20) * 20 + 1].poz + new Vector2(10 - ran.Next(20), 10 - ran.Next(20));
                            LAS[i].t = (i - 1) % 20 + ran.Next(0, ((i - 1) % 20) / 2);
                            LAS[i].fx = 0f;
                            LAS[i].fy = 0f;
                        }
                        else
                        {
                            int rrr = ran.Next(0, 100);
                            if (rrr % 4 == 0)
                                LAS[i].poz = new Vector2(-20, ran.Next(5, (int)WINDOW_REZ.X - 5));
                            else if (rrr % 4 == 1)
                                LAS[i].poz = new Vector2(ran.Next(5, (int)WINDOW_REZ.Y - 5), (int)WINDOW_REZ.X + 20);
                            else if (rrr % 4 == 2)
                                LAS[i].poz = new Vector2((int)WINDOW_REZ.Y + 20, ran.Next(5, (int)WINDOW_REZ.X - 5));
                            else if (rrr % 4 == 3)
                                LAS[i].poz = new Vector2(ran.Next(5, (int)WINDOW_REZ.Y - 5), -20);

                            float ung = ran.Next(0, 360) / 3.14159265f;
                            LAS[i].fx = 7 * (float)Math.Sin(ung);
                            LAS[i].fy = 7 * (float)Math.Cos(ung);

                        }
                    }
                }
                else
                    NR_PRO = 0;
                BUTON_A_1 = true;
                BUTON_A_2 = true;
                BUTON_A_3 = true;
                if (MENU < 7)
                    MENIU_VECTOR = new Vector2(MENIU_TEX[1, MENU].Width / 2, MENIU_TEX[1, MENU].Height / 2);

                if (v1 >= 20)
                {
                    JOC_MATRICE = new int[50, 100];
                    JOC_SIR = new int[1000];
                    JOC_X = new float[1000];
                    JOC_Y = new float[1000];
                    JOC_VECTOR = new Vector2[1000];
                    NR_JOC = 1;

                    if (MENU == 20)
                    {
                        JOC_VECTOR[0].X = WINDOW_REZ.Y / 2;
                        JOC_VECTOR[0].Y = WINDOW_REZ.X;
                        JOC_VECTOR[0].Y -= WINDOW_REZ.X / 12;
                        for (int i = 1; i < 50; i++)
                            for (int j = 1; j < 100; j++)
                                JOC_MATRICE[i, j] = 1;

                        JOC_VECTOR[1] = JOC_VECTOR[0];
                        JOC_VECTOR[1].Y -= 100;
                        JOC_SIR[1] = 6;

                        JOC_Y[1] = 5;
                        JOC_X[1] = 0;
                    }
                    else if (MENU == 21)
                    {
                        JOC_VECTOR[0].X = WINDOW_REZ.Y / 2;
                        JOC_VECTOR[0].Y = WINDOW_REZ.X;
                        JOC_VECTOR[0].Y -= WINDOW_REZ.X / 12;

                        for (int aux = 0; aux < 14; aux++)
                        {
                            int xx, yy;
                            xx = ran.Next(1, 50);
                            yy = ran.Next(1, 100);

                            if (aux < 10)
                                for (int jj = yy - 2; jj <= yy + 2; jj++)
                                {
                                    if (jj > 0 && jj < 100)
                                        JOC_MATRICE[xx, jj] = 3;
                                }
                            else
                            {
                                while (xx > 25 || JOC_MATRICE[xx, yy] != 0)
                                {
                                    xx = ran.Next(1, 25);
                                    yy = ran.Next(1, 100);
                                }
                                JOC_MATRICE[xx, yy] = -aux % 2 - 1;
                            }
                        }

                        NR_JOC = 0;
                    }
                    else if (MENU == 22)
                    {
                        JOC_MATRICE = new int[51, 51];
                        for (int i = 0; i <= 50; i++)
                            JOC_MATRICE[0, i] = JOC_MATRICE[50, i] = JOC_MATRICE[i, 0] = JOC_MATRICE[i, 50] = 1;
                        for (int i = 1; i <= 21; i++)
                        {
                            for (int j = i * 2; j < 25; j++)
                                if (ran.Next(0, 3) != 2)
                                {
                                    JOC_MATRICE[i * 2, j] = JOC_MATRICE[50 - i * 2, j] = 1;
                                    JOC_MATRICE[i * 2, 50 - j] = JOC_MATRICE[50 - i * 2, 50 - j] = 1;

                                    JOC_MATRICE[j, i * 2] = JOC_MATRICE[j, 50 - i * 2] = 1;
                                    JOC_MATRICE[50 - j, i * 2] = JOC_MATRICE[50 - j, 50 - i * 2] = 1;

                                }
                        }
                        for (int i = 7; i < 18; i += 2)
                            for (int j = i; j <= 25; j++)
                            {
                                JOC_MATRICE[i, j] = JOC_MATRICE[50 - i, j] = 2;
                                JOC_MATRICE[i, 50 - j] = JOC_MATRICE[50 - i, 50 - j] = 2;

                                JOC_MATRICE[j, i] = JOC_MATRICE[j, 50 - i] = 2;
                                JOC_MATRICE[50 - j, i] = JOC_MATRICE[50 - j, 50 - i] = 2;

                            }

                        for (int i = 3; i <= 19; i += 2)
                        {
                            int x, y;
                            do
                            {
                                x = ran.Next(0, 51);
                                y = ran.Next(0, 51);
                            } while (JOC_MATRICE[x, y] == 1);
                            JOC_VECTOR[i] = new Vector2(y * 20, x * 20);
                            JOC_SIR[i] = ran.Next(1, 5);
                        }
                        NR_JOC = 0;

                        JOC_VECTOR[0] = new Vector2(500, 500);
                    }
                    else if (MENU == 23)
                    {
                        JOC_VECTOR[0].X = WINDOW_REZ.Y / 2;
                        JOC_VECTOR[0].Y = WINDOW_REZ.X / 2;

                        JOC_MATRICE = new int[51, 51];
                        for (int i = 0; i <= 50; i++)
                            JOC_MATRICE[0, i] = JOC_MATRICE[50, i] = JOC_MATRICE[i, 0] = JOC_MATRICE[i, 50] = 1;

                        for (int nr = 0; nr < 100; nr++)
                        {
                            int x, y;
                            do
                            {
                                x = ran.Next(1, 50);
                                y = ran.Next(1, 50);
                            } while (JOC_MATRICE[x, y] != 0 || (x > 20 && x < 30 && y > 20 && y < 30));
                            if ((x + y) % 3 == 0)
                                JOC_MATRICE[x, y] = 1;
                            do
                            {
                                x = ran.Next(1, 50);
                                y = ran.Next(1, 50);
                            } while (JOC_MATRICE[x, y] != 0 || (x > 20 && x < 30 && y > 20 && y < 30));
                            JOC_MATRICE[x, y] = 2;
                            do
                            {
                                x = ran.Next(1, 50);
                                y = ran.Next(1, 50);
                            } while (JOC_MATRICE[x, y] != 0 || (x > 20 && x < 30 && y > 20 && y < 30));
                            JOC_MATRICE[x, y] = 3;
                        }

                        NR_JOC = 0;
                    }
                }
                else
                {
                    JOC_MATRICE = new int[1, 1];
                    JOC_SIR = new int[1];
                    JOC_X = new float[1];
                    JOC_Y = new float[1];
                    JOC_VECTOR = new Vector2[1];
                    NR_JOC = 1;
                }
            }
            else if (a1 == "add")
            {
                if (a2 == "item")
                {
                    inventar[(int)v1] += (int)v2;
                    ADD_CHAT_LINE("ADDED  ITEM " + (int)v1 + "; quantity  " + (int)v2);
                }
                else if (a2 == "planet")
                {
                    if (v1 < 1000)
                    {
                        L_PLA[nr_PLA_S].ID = (int)v1;
                        nr_PLA_S++;
                        ADD_CHAT_LINE("ADDED  PLANET  with  ID  " + (int)v1);
                    }
                }
            }
            else if (a1 == "subtract")
            {
                if (a2 == "item")
                {
                    inventar[(int)v1] -= (int)v2;
                    ADD_CHAT_LINE("SUBTRACTED  ITEM  " + (int)v1 + ";  quantity  " + (int)v2);
                }
                else if (a2 == "planet")
                {
                    int ok = -1;
                    for (int i = 1; i <= nr_PLA_S; i++)
                        if (L_PLA[i].ID == (int)v1)
                            ok = i;
                    if (ok != -1)
                    {
                        for (int i = ok; i < nr_PLA_S; i++)
                            L_PLA[i] = L_PLA[i + 1];
                        nr_PLA_S--;
                        ADD_CHAT_LINE("SUBTRACTED  PLANET  with  ID  " + (int)v1);
                    }
                }
            }
        }

        public void SAVE()
        {
            string[] planete_id = new string[nr_PLA_S+1];
            planete_id[0] = "1001";
            for (int i = 1; i < nr_PLA_S; i++)
            {
                MaxiFun.IO.Save<LocPlaneta>(L_PLA[i], saveDir, "LocPlaneta" + L_PLA[i].ID);
                planete_id[i] = L_PLA[i].ID + "";
            }
            planete_id[nr_PLA_S] = "";
            MaxiFun.IO.Save<string[]>(planete_id, saveDir, "PLANETE_ID");
            
            MaxiFun.IO.Save<Nava>(PL, saveDir, "NAVA_PLAYER");
            if (PLA_A != 0)
                MaxiFun.IO.Save<Planeta>(PLA_S, saveDir, "PLANETA" + PLA_A);
            MaxiFun.IO.Save<int[]>(inventar, saveDir, "INVENTAR");
        }

        public int ret_v(int x)
        {
            if (x <= 0)
                return 0;
            if (x <= NR_comp)
                return comp[x].v;
            else if (x - NR_comp <= NR_elem * NR_subs)
            {
                if ((x - NR_comp - 1) / NR_subs + 1 == 1)
                    return 100;
                else if ((x - NR_comp - 1) / NR_subs + 1 == 2)
                    return 200;
                else if ((x - NR_comp - 1) / NR_subs + 1 == 3)
                    return 50;
                else if ((x - NR_comp - 1) / NR_subs + 1 == 4)
                    return 5;
                else if ((x - NR_comp - 1) / NR_subs + 1 == 5 ||
                    (x - NR_comp - 1) / NR_subs == 6 || 
                    (x - NR_comp - 1) / NR_subs == 7 ||
                    (x - NR_comp - 1) / NR_subs == 12)
                    return 0;
                else if ((x - NR_comp - 1) / NR_subs + 1 == 8)
                    return 200;
                else if ((x - NR_comp - 1) / NR_subs + 1 == 9)
                    return 100;
                else if ((x - NR_comp - 1) / NR_subs + 1 == 10 ||
                    (x - NR_comp - 1) / NR_subs + 1 == 15 ||
                    (x - NR_comp - 1) / NR_subs + 1 == 16)
                    return 400;
                else if ((x - NR_comp - 1) / NR_subs + 1 == 11)
                    return 50;
                else if ((x - NR_comp - 1) / NR_subs + 1 == 13)
                    return 400;
                else if ((x - NR_comp - 1) / NR_subs + 1 == 14)
                    return 100;
                else if ((x - NR_comp - 1) / NR_subs + 1 == 17 ||
                    (x - NR_comp - 1) / NR_subs + 1 == 18 ||
                    (x - NR_comp - 1) / NR_subs + 1 == 19 ||
                    (x - NR_comp - 1) / NR_subs + 1 == 20)
                    return 150;
            }
            return 0;
        }

        public void ELIMINARE_LAS(int k)
        {
            for (int i = k; i < NR_PRO; i++)
                LAS[i] = LAS[i + 1];
            LAS[NR_PRO - 1] = new Proiectil();
            LAS[NR_PRO] = new Proiectil();
            NR_PRO--;
        }

        public void ELIMINARE_APA(int x, int y)
        {
            ///if (PLA_S.apa[x, y] / 1000 == 7)
            ///    return;
            for (int d = 0; d < 4; d++)
            {
                int i, j;
                i = x + d1[d];
                j = y + d2[d];
                if (i > 0 && i < 300 && j > 0 && j < 300)
                    if (PLA_S.apa[x, y] / 10 - PLA_S.apa[i, j] / 10 == -1)
                        ELIMINARE_APA(i, j);
            }
            if (PLA_S.apa[x, y] / 1000 != 12 && PLA_S.apa[x, y] / 10 != 600)
                PLA_S.apa[x, y] = 0;

        }

        public void CURENT(int x, int y)
        {
            int[,] aux = new int[300, 300];
            for (int i = 0; i < 300; i++)
                for (int j = 0; j < 300; j++)
                    aux[i, j] = 0;
            Queue<int> X = new Queue<int>();
            Queue<int> Y = new Queue<int>();
            Queue<int> T_c = new Queue<int>();
            X.Enqueue(x);
            Y.Enqueue(y);
            aux[x, y] = 0;
            int val;
            if (PLA_S.a[x, y] / 100 == 16)
                T_c.Enqueue(-PLA_S.a[x, y] % 100 - 1);
            else T_c.Enqueue(PLA_S.apa[x, y] % 100);
            while (X.Count != 0)
            {
                int xx, yy;
                xx = X.Dequeue();
                yy = Y.Dequeue();
                val = T_c.Dequeue();

                for (int d = 0; d < 4; d++)
                    if (val >= 0 || -val - 2 == d || val == -1)
                    {
                        int dx, dy;
                        dx = xx + d1[d];
                        dy = yy + d2[d];
                        if (dx > 0 && dx < 300)
                            if (dy > 0 && dy < 300)
                                if (aux[dx, dy] == 0)
                                {
                                    if (PLA_S.apa[dx, dy] % 100 == val || (val < 0 && PLA_S.apa[dx, dy] / 1000 == 12))
                                    {
                                        aux[dx, dy] = 1;
                                        X.Enqueue(dx);
                                        Y.Enqueue(dy);
                                        T_c.Enqueue(PLA_S.apa[dx, dy] % 100);
                                    }
                                    if (PLA_S.a[dx, dy] / 100 == 15 && (PLA_S.a[dx, dy] % 100 == val || val < 0))
                                    {
                                        aux[dx, dy] = 1;
                                        X.Enqueue(dx);
                                        Y.Enqueue(dy);
                                        T_c.Enqueue(PLA_S.a[dx, dy] % 100);
                                    }
                                    if (PLA_S.a[dx, dy] / 100 == 16)
                                    {
                                        aux[dx, dy] = 1;
                                        X.Enqueue(dx);
                                        Y.Enqueue(dy);
                                        T_c.Enqueue(-PLA_S.a[dx, dy] % 100 - 1);
                                    }
                                    if (PLA_S.a[dx, dy] / 100 == 13 && (PLA_S.a[dx, dy] % 100 == val % 100 || val < 0))
                                    {
                                        if (PLA_S.b[dx, dy] == 0)
                                            PLA_S.b[dx, dy] = 400;
                                        else PLA_S.b[dx, dy] = 0;

                                        aux[dx, dy] = 1;
                                        X.Enqueue(dx);
                                        Y.Enqueue(dy);
                                        T_c.Enqueue(PLA_S.a[dx, dy] % 100);
                                    }
                                }
                    }
            }
        }

        public int VERIFICARE(Nava AUX)
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
                    xx = d1[d] + x;
                    yy = d2[d] + y;

                    if (xx >= 0 && xx < 50 && yy >= 0 && yy < 50)
                        if (AUX.comp[xx, yy] != 0)
                            if (mat[xx, yy] != 1)
                            {
                                co[sf++] = new Vector2(xx, yy);
                                mat[xx, yy] = 1;
                            }
                }
                inc++;
            }

            if (MENU == 3)
                for (int i = 0; i < 50; i++)
                    for (int j = 0; j < 50; j++)
                        if (mat[i, j] == 0)
                        {
                            AUX.comp[i, j] = 0;
                            AUX.viata[i, j] = 0;
                        }
            return sf;
        }

        public LocPlaneta CREARE_LOC_PLANETA(int k, int kk, int TIP)
        {
            LocPlaneta AUX = new LocPlaneta();
            AUX.SALVAT = 0;
            int ok = 1;
            int aux = 0;
            AUX.ord_elm = new int[NR_subs + 3];           ///ORDINEA ELEMENTELOR
            for (int i = 1; i < NR_subs; i++)
            {
                ok = 0;
                while (ok == 0)
                {
                    ok = 1;
                    AUX.ord_elm[i] = ran.Next(1, NR_subs);
                    for (int j = 0; j < i; j++)
                        if (AUX.ord_elm[j] == AUX.ord_elm[i])
                            ok = 0;
                }
            }
            AUX.ord_elm[0] = 0;
            AUX.temperatura_z = ran.Next(-300, 300);
            AUX.p_orb = k;


            if (TIP == 1)
            {
                if (k == kk)
                {
                    do
                    {
                        ok = 1;
                        AUX.poz = PL.poz;
                        AUX.poz.X += 800000 - ran.Next(200000, 1600000);
                        AUX.poz.Y += 800000 - ran.Next(200000, 1600000);

                        for (int i = 1; i <= nr_PLA_S; i++)
                        {
                            float xx, yy, r;
                            xx = L_PLA[i].poz.X - AUX.poz.X;
                            yy = L_PLA[i].poz.Y - AUX.poz.Y;
                            r = (float)Math.Sqrt(xx * xx + yy * yy);

                            if (r <= 100000)
                                ok = 0;
                        }
                        aux++;
                    }
                    while (ok == 0 && aux < 50000);

                    AUX.ord_elm[NR_subs + 1] = 8;

                    /*AUX.poz = new Vector2(60000 * kk, 60000 * kk);  */
                }
                else
                {
                    aux = ran.Next(0, 15);
                    if (aux != 10)
                    {
                        AUX.ar = ran.Next(150, 255);
                        AUX.be = ran.Next(150, 255);
                        AUX.ge = ran.Next(150, 255);

                        aux = ran.Next(0, 4);
                        AUX.MOON = aux;

                        if (Math.Abs(AUX.temperatura_z) < 200)
                            AUX.ord_elm[NR_subs + 1] = AUX.ord_elm[1];
                        else if (AUX.temperatura_z < 0)
                            AUX.ord_elm[NR_subs + 1] = NR_subs;
                        else AUX.ord_elm[NR_subs + 1] = NR_subs + 1;

                        AUX.forta = ran.Next(5, 25);
                    }
                    else
                    {
                        AUX.ar = ran.Next(10, 115);
                        AUX.be = ran.Next(10, 115);
                        AUX.ge = ran.Next(10, 115);
                        AUX.MOON = 0;
                        AUX.ord_elm[NR_subs + 1] = 0;
                    }

                    AUX.poz = L_PLA[k].poz;
                    AUX.R = ran.Next(20000, 100000);
                    AUX.ung = (float)ran.Next(0, 360) / 3.14159f;
                    AUX.poz.X += (float)Math.Sin(AUX.ung) * AUX.R;
                    AUX.poz.Y += (float)Math.Cos(AUX.ung) * AUX.R;

                }
            }
            else if (TIP == 0)
            {
                AUX.ord_elm[NR_subs + 1] = 7;
                AUX.R = ran.Next(3500, 5000);
                AUX.poz = L_PLA[k].poz;
                AUX.ung = (float)ran.Next(0, 360) / 3.14159f;
                AUX.poz.X += (float)Math.Sin(AUX.ung) * AUX.R;
                AUX.poz.Y += (float)Math.Cos(AUX.ung) * AUX.R;
            }
            else if (TIP == 2)
            {
                if (k != kk)
                {
                    AUX.poz = L_PLA[k].poz;

                    AUX.ord_elm[NR_subs + 1] = 9;
                    AUX.R = ran.Next(20000, 100000);
                    AUX.poz = L_PLA[k].poz;
                    AUX.ung = (float)ran.Next(0, 360) / 3.14159f;
                    AUX.poz.X += (float)Math.Sin(AUX.ung) * AUX.R;
                    AUX.poz.Y += (float)Math.Cos(AUX.ung) * AUX.R;
                }
                else
                {
                    AUX.poz = new Vector2(0, 0);
                    AUX.ord_elm[NR_subs + 1] = 8;
                }
            }

            return AUX;
        }

        public Planeta CREARE_PLANETA()
        {
            Planeta AUX = new Planeta();
            int aux;
            int auy;

            AUX.a = new int[300, 300];
            AUX.b = new int[300, 300];


            AUX.ar = L_PLA[PLA_A].ar;
            AUX.be = L_PLA[PLA_A].ge;
            AUX.ge = L_PLA[PLA_A].be;
            AUX.V_SKY = new Vector3((float)AUX.ar / 1000, (float)AUX.ge / 1000, (float)AUX.be / 1000);
            AUX.SKY = new Vector3(AUX.ar, AUX.ge, AUX.be);
            AUX.temperatura_z = L_PLA[PLA_A].temperatura_z;
            AUX.temperatura_n = AUX.temperatura_z - ran.Next(10, 200);

            AUX.MOON = L_PLA[PLA_A].MOON;

            AUX.forta = L_PLA[PLA_A].forta;

            AUX.ord_elm = L_PLA[PLA_A].ord_elm;


            AUX.inaltime = ran.Next(1, 10);

            aux = 150;
            //aux = ran.Next(100, 250);

            for (int i = 0; i < 300; i++)
            {                                                                       /// CREAREA TERENULUI
                auy = 1;
                for (int j = aux; j < 300; j++)
                {
                    int auz = ran.Next(0, NR_subs * NR_subs * NR_subs * NR_subs);              /// elementul la puterea a 4
                    AUX.a[j, i] = auy * 100 + AUX.ord_elm[4 - (int)Math.Sqrt((int)Math.Sqrt(auz))];
                    AUX.b[j, i] = auy * 100;

                    if (j >= 180)
                        if (auy < 2)
                            if (ran.Next(0, AUX.inaltime) == 0)
                                auy++;
                }

                int semn = 1 - ran.Next(0, 100) % 3;
                if (ran.Next(0, 100) % 3 != 0)
                    aux -= semn * (int)Math.Sqrt(ran.Next(1, AUX.inaltime * AUX.inaltime));

                if (aux <= 0)
                    aux = 1;
                if (aux >= 300)
                    aux = 295;
            }

            for (int i = 0; i < 30; i++)                   /// PESTERI
            {
                auy = ran.Next(0, 290);
                aux = ran.Next(100, 290);

                for (int j = 0; j <= 500; j++)
                {
                    AUX.b[aux, auy] = 0;
                    int semn = 1 - ran.Next(0, 100) % 3;
                    aux += semn;
                    semn = 1 - ran.Next(0, 100) % 3;
                    auy += semn;

                    if (aux <= 0 || aux >= 300 || auy <= 0 || auy >= 300)
                        break;
                }
            }



            AUX.apa = new int[300, 300];             /// adaugarea apei
            int y;
            int nr_apa = ran.Next(10, 10 + (300 - Math.Abs(AUX.temperatura_z)));

            for (int i = 0; i < nr_apa; i++)
            {
                int x = 10;
                y = ran.Next(1, 300);
                for (int j = x; j < 299; j++)
                {
                    x = j;
                    if (AUX.b[j, y] == 0 && AUX.b[j + 1, y] != 0 && ran.Next(0, 10 - AUX.inaltime) == 1)
                        break;
                }
                int tip_apa = ran.Next(0, Math.Abs(5 - AUX.inaltime));
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
                    for (; AUX.a[i, j] / 100 == 1 && ran.Next(0, nr_c) != 0 && i < 298; i++)
                        AUX.a[i, j] = 900 + AUX.a[i, j] % 100;

                }
            }
            else if (AUX.temperatura_z > -50)
            {
                nr_c = ran.Next(5, Math.Abs(AUX.temperatura_z) + 5);
                int x;
                for (int i = 0; i < nr_c; i++)
                {
                    do
                    {
                        x = ran.Next(10, 290);
                        y = ran.Next(10, 290);
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
                        for (; (AUX.a[i, j] / 100 == 1 || AUX.a[i, j] / 100 == 8) && ran.Next(0, nr_c * 2) != 0 && i < 298; i++)
                        {
                            AUX.a[i, j] = 1100 + AUX.a[i, j] % 100;
                            if (AUX.b[i, j] != 0)
                                AUX.b[i, j] = 50;
                        }
                        i++;
                        for (; i < 298 && (AUX.a[i, j] / 100 == 2 || AUX.a[i, j] / 100 == 8) && ran.Next(0, nr_c * 2) != 0; i++)
                        {
                            AUX.a[i, j] = 800 + AUX.a[i, j] % 100;
                            if (AUX.b[i, j] != 0)
                                AUX.b[i, j] = 200;
                        }
                    }
                }
            }


            nr_c = 30 - ran.Next(0, 30 - (Math.Abs(AUX.temperatura_z) / 10));                     ///CREAREA COPACILOR
            y = 0;
            for (int nrn = 0; nrn < nr_c; nrn++)
            {
                int x = 0;
                do
                {
                    x = 0;
                    y += ran.Next(3, 6 * (30 - nr_c) + 3);
                    if (y >= 300)
                        break;
                    while (x < 299 && AUX.a[x + 1, y] == 0)
                        x++;
                } while (AUX.a[x + 1, y] / 100 != 1 && AUX.a[x + 1, y] / 100 != 11 && AUX.a[x + 1, y] / 100 != 9);

                if (y >= 300)
                    break;
                int tip, inal, lat;
                tip = (int)Math.Sqrt(ran.Next(0, 25));
                inal = ran.Next(2, AUX.inaltime * 3);
                lat = ran.Next(1, inal + 1);
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
                            if (ran.Next(0, inal / 3) % 10 == 2)
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

                                            if (ran.Next(0, 10) == 2)
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

                                            if (ran.Next(0, 10) == 2)
                                                break;
                                        }
                                        else break;
                        }
                    }
                    for (int i = 0; i < ran.Next(1, 5); i++)
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
                if (ran.Next(1, AUX.inaltime) == 1)
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

            aux = ran.Next(1, 299);                       /// POZITIA JUCATORULI
            for (int i = 0; i < 298; i++)
            {
                PL_P.poz.X = aux * 20;
                PL_P.poz.Y = i * 20;
                if (AUX.b[i + 2, aux] != 0)
                    break;
            }

            PL_P.Y = (int)(PL_P.poz.X + 10) / 20;
            PL_P.X = (int)(PL_P.poz.Y + 30) / 20;
            for (int j = 1; j < 300; j++)
                AUX.b[299, j] = 500;

            for (int j = 1; j < 300; j++)                          /// ADAUGAREA LAVA
                for (int i = 299; i > 0; i--)
                {
                    if (AUX.a[i, j] / 100 == 1)
                        break;
                    AUX.apa[i, j] = 7000 + ran.Next(0, 5) + (299 - i) * 10;
                    if (i < 290 && (ran.Next(0, (Math.Abs(AUX.temperatura_z)) / 10 + 1) == 0 || AUX.temperatura_z <= 0))
                        break;
                }



            AUX.creaturi = new Creatura[10];         //creare creaturi
            AUX.nr_creaturi = ran.Next(0, (300 - Math.Abs(AUX.temperatura_z)) / 20);
            for (int i = 0; i < AUX.nr_creaturi; i++)
            {
                Creatura cc = new Creatura();
                cc.pow = ran.Next(2, 100);
                cc.viata = 1000;
                int x = 0;
                int iii;
                for (iii = 0; iii <= 1000; iii++)
                {
                    y = ran.Next(20, 280);
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
                nr_c = ran.Next(1, 5);
                cc.parti[0] = ran.Next(1, NR_PARTI + 1);
                cc.rot[0] = 0;
                if (nr_c >= 2)
                {
                    cc.parti[2] = ran.Next(1, NR_PARTI + 1);
                    cc.rot[2] = 0;
                }
                if (nr_c >= 3)
                {
                    cc.parti[1] = cc.parti[2];
                    cc.rot[1] = (float)(ran.Next(0, 45)) * 0.01745327777777777777777777777778f;
                }
                if (nr_c >= 4)
                {
                    cc.parti[3] = cc.parti[0];
                    cc.rot[3] = cc.rot[1];
                }

                cc.parti[4] = ran.Next(1, NR_PARTI + 1);
                cc.rot[4] = (float)(90 - ran.Next(0, 140)) * 0.01745327777777777777777777777778f;

                nr_c = ran.Next(0, 5);
                if (nr_c >= 1)
                {
                    cc.parti[5] = ran.Next(1, NR_PARTI + 1);
                    cc.rot[5] = 0;
                }
                if (nr_c >= 2)
                {
                    cc.parti[7] = ran.Next(1, NR_PARTI + 1);
                    cc.rot[7] = 0;
                }
                if (nr_c >= 3)
                {
                    cc.parti[6] = cc.parti[7];
                    cc.rot[6] = (float)(ran.Next(0, 45)) * 0.01745327777777777777777777777778f;
                }
                if (nr_c >= 4)
                {
                    cc.parti[8] = cc.parti[5];
                    cc.rot[8] = cc.rot[1];
                }
                cc.parti[9] = ran.Next(1, NR_PARTI + 1);
                cc.rot[9] = 0;

                cc.mers = 10;

                cc.pow = ran.Next(5, 21);
                cc.inteligenta = ran.Next(1, 6);
                cc.r = 20;
                cc.agresiune = -1;
                if (cc.inteligenta == 2)
                    cc.agresiune = 1;
                else if (cc.inteligenta == 4)
                {
                    cc.r = 7;
                    if (AUX.a[cc.X + 1, cc.Y] < 100)
                        AUX.a[cc.X + 1, cc.Y] = 100;
                    if (AUX.b[cc.X + 1, cc.Y] == 0)
                        AUX.b[cc.X + 1, cc.Y] = 100;
                    for (int ii = 0; ii < 5; ii++)
                        for (int j = -2; j <= 2; j++)
                        {
                            AUX.a[cc.X - ii, cc.Y + j] = AUX.a[cc.X + 1, cc.Y];
                            AUX.b[cc.X - ii, cc.Y + j] = 0;
                            AUX.apa[cc.X - ii, cc.Y + j] = 0;
                        }
                    for (int ii = 0; ii < 5; ii++)
                    {
                        AUX.a[cc.X - ii, cc.Y + 3] = 300 + AUX.a[cc.X + 1, cc.Y] % 100;
                        AUX.b[cc.X - ii, cc.Y + 3] = 5;
                        AUX.apa[cc.X - ii, cc.Y + 3] = 0;
                        AUX.a[cc.X - ii, cc.Y - 3] = 300 + AUX.a[cc.X + 1, cc.Y] % 100;
                        AUX.b[cc.X - ii, cc.Y - 3] = 5;
                        AUX.apa[cc.X - ii, cc.Y - 3] = 0;
                        AUX.a[cc.X - 5, cc.Y - 2 + ii] = 300 + AUX.a[cc.X + 1, cc.Y] % 100;
                        AUX.b[cc.X - 5, cc.Y - 2 + ii] = 5;
                        AUX.apa[cc.X - 5, cc.Y - 2 + ii] = 0;
                    }
                    AUX.b[cc.X, cc.Y - 3] = 0;
                    AUX.b[cc.X - 1, cc.Y - 3] = 0;
                    AUX.b[cc.X, cc.Y + 3] = 0;
                    AUX.b[cc.X - 1, cc.Y + 3] = 0;
                    for (int ii = 4; ii <= 7; ii++)
                    {
                        AUX.b[cc.X, cc.Y + ii] = 0;
                        AUX.b[cc.X, cc.Y - ii] = 0;
                        AUX.b[cc.X - 1, cc.Y + ii] = 0;
                        AUX.b[cc.X - 1, cc.Y - ii] = 0;
                        AUX.apa[cc.X, cc.Y + ii] = 0;
                        AUX.apa[cc.X, cc.Y - ii] = 0;
                        AUX.apa[cc.X - 1, cc.Y + ii] = 0;
                        AUX.apa[cc.X - 1, cc.Y - ii] = 0;
                    }
                    for (int ii = -7; ii <= 7; ii++)
                        for (int j = 1; j <= 3; j++)
                        {
                            AUX.apa[cc.X + j, cc.Y + ii] = 0;
                            AUX.b[cc.X + j, cc.Y + ii] = AUX.b[cc.X + 1, cc.Y];
                            AUX.a[cc.X + j, cc.Y + ii] = (int)(AUX.a[cc.X + 1, cc.Y] / 100) * 100 + AUX.a[cc.X + j, cc.Y + ii] % 100;
                        }
                }
                else
                {
                    cc.r = 10;
                    aux = 1000 + AUX.ord_elm[1];
                    auy = ran.Next(0, 2) - 1;
                    if (auy == 0)
                        auy = 1;
                    for (int ii = 0; ii <= 9; ii++)
                        for (int jj = 10; jj >= -10; jj--)
                        {
                            AUX.b[cc.X - ii, cc.Y + jj * auy] = 0;
                            AUX.apa[cc.X - ii, cc.Y + jj * auy] = 0;
                        }
                    for (int ii = 0; ii <= 5; ii++)
                        for (int jj = -6; jj <= 6; jj++)
                        {
                            AUX.a[cc.X - ii, cc.Y + jj * auy - 4 * auy] = aux;
                            if ((ii >= 2 && (jj == -6 || jj == 6)) || ii == 5)
                                AUX.b[cc.X - ii, cc.Y + jj * auy - 4 * auy] = 300;
                            else AUX.b[cc.X - ii, cc.Y + jj * auy - 4 * auy] = 0;
                            AUX.apa[cc.X - ii, cc.Y + jj * auy - 4 * auy] = 0;
                        }
                    for (int jj = -10; jj <= 9; jj++)
                    {
                        AUX.a[cc.X + 1, cc.Y + jj * auy] = aux;
                        AUX.b[cc.X + 1, cc.Y + jj * auy] = 300;
                        AUX.apa[cc.X + 1, cc.Y + jj * auy] = 0;
                    }

                    AUX.a[cc.X, cc.Y + 8 * auy] = aux;
                    AUX.b[cc.X, cc.Y + 8 * auy] = 300;
                    AUX.a[cc.X, cc.Y + 4 * auy] = aux;
                    AUX.b[cc.X, cc.Y + 4 * auy] = 300;
                    AUX.a[cc.X, cc.Y + 6 * auy] = aux;
                    AUX.b[cc.X, cc.Y + 6 * auy] = 300;
                    AUX.a[cc.X - 1, cc.Y + 6 * auy] = aux;
                    AUX.b[cc.X - 1, cc.Y + 6 * auy] = 0;
                    AUX.apa[cc.X - 2, cc.Y + 6 * auy] = 6000;

                    for (int ii = 6; ii <= 10; ii++)
                        for (int jj = -13 + (ii - 5) * 2; jj <= 2 + (int)((10 - ii) / 4) * 2; jj++)
                        {
                            AUX.a[cc.X - ii + 1, cc.Y + jj * auy] = aux;
                            if (jj <= -13 + (ii - 5) * 2 + 1 || ii == 6 || ii == 10)
                                AUX.b[cc.X - ii + 1, cc.Y + jj * auy] = 300;
                            else
                                AUX.b[cc.X - ii + 1, cc.Y + jj * auy] = 0;
                            AUX.apa[cc.X - ii + 1, cc.Y + jj * auy] = 0;
                        }
                    AUX.a[cc.X - 6, cc.Y + 4 * auy] = aux;
                    AUX.b[cc.X - 6, cc.Y + 4 * auy] = 300;
                    AUX.b[cc.X - 8, cc.Y + 2 * auy] = 300;
                    for (int ii = 1; ii <= 6; ii++)
                        for (int jj = -5; jj <= 3; jj++)
                        {
                            AUX.a[cc.X + ii, cc.Y + jj * auy] = aux;
                            if (jj == -5 || jj == 3 || ii == 1 || ii == 6)
                                AUX.b[cc.X + ii + 1, cc.Y + jj * auy] = 300;
                            else
                                AUX.b[cc.X + ii, cc.Y + jj * auy] = 0;
                            AUX.apa[cc.X + ii, cc.Y + jj * auy] = 0;
                        }
                    for (int jj = -6; jj <= 0; jj++)
                        for (int ii = 2; ii <= -jj; ii++)
                        {
                            AUX.b[cc.X + 5 - ii + 2, cc.Y + jj * auy] = 300;
                            AUX.a[cc.X + 5 - ii + 2, cc.Y + jj * auy] = aux;
                        }
                    for (int jj = -5; jj <= -3; jj++)
                        AUX.b[cc.X + 1, cc.Y + jj * auy] = 0;

                }

                AUX.creaturi[i] = cc;
            }

            return AUX;
        }
        public Planeta CREARE_STATIE()
        {
            Planeta AUX = new Planeta();

            AUX.a = new int[300, 300];
            AUX.b = new int[300, 300];
            AUX.apa = new int[300, 300];

            AUX.ar = L_PLA[PLA_A].ar;
            AUX.be = L_PLA[PLA_A].ge;
            AUX.ge = L_PLA[PLA_A].be;

            AUX.V_SKY = new Vector3((float)AUX.ar / 1000, (float)AUX.ge / 1000, (float)AUX.be / 1000);
            AUX.SKY = Vector3.Zero;
            AUX.timp = 0;
            AUX.forta = 7;
            AUX.temperatura_n = AUX.temperatura_z = 30;
            AUX.MOON = 0;

            PL_P.poz = new Vector2(3000, 3000);
            PL_P.X = 150;
            PL_P.Y = 150;


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

                                    if (mod_loc[tip_loc[nr]][i + 9][j + 9] == 3 && AUX.nr_creaturi < 150 && (AUX.nr_creaturi == 0 || ran.Next(0, 5) == 3))
                                    {
                                        #region ADAUGARE_CREATURI
                                        Creatura cc = new Creatura();
                                        cc.pow = ran.Next(2, 100);
                                        cc.viata = 1000;
                                        int x, y;
                                        x = (int)loc[nr].X + i;
                                        y = (int)loc[nr].Y + j;
                                        cc.r = 15;

                                        cc.poz = new Vector2(x * 20, y * 20);
                                        cc.X = cc.xx = x + 2;
                                        cc.Y = cc.yy = y;

                                        cc.pow = ran.Next(5, 21);
                                        cc.r = 20;
                                        cc.agresiune = -1;

                                        cc.rot = new float[11];
                                        cc.parti = new int[11];

                                        if (ran.Next(0, 5) != 2)
                                        {
                                            cc.inteligenta = ran.Next(1, 6);
                                            for (int k = 0; k <= 10; k++)
                                                cc.rot[k] = -99f;

                                            int nr_c = ran.Next(1, 5);
                                            cc.parti[0] = ran.Next(1, NR_PARTI + 1);
                                            cc.rot[0] = 0;
                                            if (nr_c >= 2)
                                            {
                                                cc.parti[2] = ran.Next(1, NR_PARTI + 1);
                                                cc.rot[2] = 0;
                                            }
                                            if (nr_c >= 3)
                                            {
                                                cc.parti[1] = cc.parti[2];
                                                cc.rot[1] = (float)(ran.Next(0, 45)) * 0.01745327777777777777777777777778f;
                                            }
                                            if (nr_c >= 4)
                                            {
                                                cc.parti[3] = cc.parti[0];
                                                cc.rot[3] = cc.rot[1];
                                            }

                                            cc.parti[4] = ran.Next(1, NR_PARTI + 1);
                                            cc.rot[4] = (float)(90 - ran.Next(0, 140)) * 0.01745327777777777777777777777778f;

                                            nr_c = ran.Next(0, 5);
                                            if (nr_c >= 1)
                                            {
                                                cc.parti[5] = ran.Next(1, NR_PARTI + 1);
                                                cc.rot[5] = 0;
                                            }
                                            if (nr_c >= 2)
                                            {
                                                cc.parti[7] = ran.Next(1, NR_PARTI + 1);
                                                cc.rot[7] = 0;
                                            }
                                            if (nr_c >= 3)
                                            {
                                                cc.parti[6] = cc.parti[7];
                                                cc.rot[6] = (float)(ran.Next(0, 45)) * 0.01745327777777777777777777777778f;
                                            }
                                            if (nr_c >= 4)
                                            {
                                                cc.parti[8] = cc.parti[5];
                                                cc.rot[8] = cc.rot[1];
                                            }
                                            cc.parti[9] = ran.Next(1, NR_PARTI + 1);
                                            cc.rot[9] = 0;

                                            cc.mers = 10;


                                            if (mod_loc[tip_loc[nr]][i + 9][j + 9 - 1] != 1 && ran.Next(0, 3) != 0)
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
                                        else cc.inteligenta = -ran.Next(2, 6);

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
                                next_loc = ran.Next(2, NR_LOC);
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
                                next_loc = ran.Next(2, NR_LOC);
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
                                next_loc = ran.Next(2, NR_LOC);
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
                                next_loc = ran.Next(2, NR_LOC);
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
        public Planeta CREARE_ASTERORID()
        {
            Planeta AUX = new Planeta();

            int r1, r2;
            r1 = r2 = 0;
            int j = 1;
            for (j = -100; j <= 100; j++)
            {
                r1 = 101 + ran.Next(-1, 2);
                r2 = 101 + ran.Next(-1, 2);
                for (int i = -(int)Math.Sqrt(r1 * r1 - j * j); i <= (int)Math.Sqrt(r2 * r2 - j * j); i++)
                {
                    AUX.a[150 + i, 150 + j] = 200 + ran.Next(0, NR_subs);
                    AUX.b[150 + i, 150 + j] = 200;
                }
            }
            for (j = -20; j <= 20; j++)
            {
                r1 = 21 + ran.Next(-1, 2);
                r2 = 21 + ran.Next(-1, 2);
                for (int i = -(int)Math.Sqrt(r1 * r1 - j * j); i <= (int)Math.Sqrt(r2 * r2 - j * j); i++)
                {
                    AUX.apa[150 + i, 150 + j] = 7000 + ran.Next(0, NR_subs);
                    // AUX.b[150 + i, 150 + j] = 200;
                }
            }

            AUX.forta = 10;

            j = 2;
            while (AUX.a[150, j + 3] == 0)
                j++;

            PL_P.poz = new Vector2(150 * 20, j * 20);
            PL_P.Y = (int)(PL_P.poz.X + 10) / 20;
            PL_P.X = (int)(PL_P.poz.Y + 30) / 20;

            AUX.creaturi = new Creatura[2];
            AUX.creaturi[0] = new Creatura();
            AUX.creaturi[0].inteligenta = -1;
            AUX.creaturi[0].poz = PL_P.poz;

            return AUX;
        }
        public Planeta CREARE_PLANETA_AUX()
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

            PL_P.poz = new Vector2(100, 100);
            AUX.creaturi[0].poz = new Vector2(200, 200);
            AUX.creaturi[0].X = 10;
            AUX.creaturi[0].Y = 10;
            AUX.creaturi[0].rot[4] = 0f;

            return AUX;
        }
        public Planeta CREARE_PLANETA_FROM_IMG(Texture2D aux)
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
            for(int k=1;k <= aux.Height - 301; k++)
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
            PL_P.X = PLAN.creaturi[0].Y;
            PL_P.Y = PLAN.creaturi[0].X;
            PL_P.poz = new Vector2(PL_P.X * 20, PL_P.Y * 20);
            return PLAN;
        }

        public void SELECTEAZA_PLANETA()
        {
            if (L_PLA[PLA_A].SALVAT == 0)
            {
                if (L_PLA[PLA_A].ID < 1000 && L_PLA[PLA_A].ID > 0)
                {
                    PLA_S = CREARE_PLANETA_FROM_IMG(Content.Load<Texture2D>("PLANETA_" + (L_PLA[PLA_A].ID - 1)));
                    L_PLA[PLA_A].SALVAT = 1;
                }
                else
                {
                    if (L_PLA[PLA_A].ord_elm[NR_subs + 1] == 7)
                    {
                        PLA_S = CREARE_ASTERORID();
                    }
                    else
                    {
                        if (L_PLA[PLA_A].ord_elm[NR_subs + 1] == 0)
                            PLA_S = CREARE_STATIE();
                        else PLA_S = CREARE_PLANETA();
                        PLA_S.creaturi[PLA_S.nr_creaturi] = new Creatura();
                        PLA_S.creaturi[PLA_S.nr_creaturi].viata = 100000000;
                        PLA_S.creaturi[PLA_S.nr_creaturi].inteligenta = -1;
                        PLA_S.creaturi[PLA_S.nr_creaturi].poz.X = PL_P.poz.Y;
                        PLA_S.creaturi[PLA_S.nr_creaturi].poz.Y = PL_P.poz.X;
                        PLA_S.creaturi[PLA_S.nr_creaturi].agresiune = 0;
                        PLA_S.creaturi[PLA_S.nr_creaturi].rot = new float[11];
                        PLA_S.nr_creaturi++;
                    }
                    L_PLA[PLA_A].SALVAT = 1;
                    MaxiFun.IO.Save<Planeta>(PLA_S, saveDir, "PLANETA" + L_PLA[PLA_A].ID);
                }
            }
            else
            {
                PLA_S = MaxiFun.IO.Load<Planeta>(saveDir, "PLANETA" + L_PLA[PLA_A].ID);
                // PLA_S = Newtonsoft.Json.JsonConvert.DeserializeObject<Planeta>(Unzip(File.ReadAllBytes("test.json")));

                int aux_sel = -1;
                for (int i = 0; i < PLA_S.nr_creaturi; i++)
                    if (PLA_S.creaturi[i].inteligenta == -1)
                        if (aux_sel == -1 || ran.Next(0, PLA_S.nr_creaturi + 1) == 1)
                            aux_sel = i;

                if (aux_sel != -1)
                {
                    PL_P.poz.X = PLA_S.creaturi[aux_sel].poz.Y;
                    PL_P.poz.Y = PLA_S.creaturi[aux_sel].poz.X;
                }
                else
                {
                    PL_P.poz = new Vector2(100, 100);
                }
            }
        }

        public void ELIMINARE_BLOCK(int i, int j)
        {
            int r;
            for (int ass = 0; ass < PLA_S.nr_creaturi; ass++)
            {
                int x, y;
                x = PLA_S.creaturi[ass].xx;
                y = PLA_S.creaturi[ass].yy;
                x -= i;
                y -= j;
                r = PLA_S.creaturi[ass].r;

                if (x * x + y * y <= r * r)
                    if (ran.Next(0, x * x + y * y) == 0)
                        PLA_S.creaturi[ass].agresiune = 1;
            }
        }
        public Creatura AI_FIINTA(Creatura aux)
        {
            aux.X = (int)(aux.poz.X + 16 - Math.Abs(Math.Sin(aux.rot[4])) * 8) / 20 + 1;
            aux.Y = (int)(aux.poz.Y + 10) / 20;
            if (aux.X > 0 && aux.X < 300)
                if (aux.Y > 0 && aux.Y < 300)
                {
                    if (aux.X > 3)
                        if (PLA_S.b[aux.X - 2, aux.Y] != 0)
                        {
                            aux.poz.X += aux.fx;
                            aux.fx = 0;
                            aux.poz.X += (int)(aux.poz.X + 16 - Math.Abs(Math.Sin(aux.rot[4])) * 8) % 20 / 2;
                        }
                    if (PLA_S.b[aux.X, aux.Y] != 0)
                    {
                        //aux.forta = (float)aux.pow / 3;
                        aux.fx = 0;
                        aux.poz.X -= (int)(aux.poz.X + 16 - Math.Abs(Math.Sin(aux.rot[4])) * 8) % 20 / 2;
                    }
                    else aux.fx -= (float)PLA_S.forta / 30;
                    if (aux.fx <= -20)
                        aux.fx = -19;

                    aux.poz.X -= aux.fx;

                    int x, y;
                    x = aux.xx;
                    y = aux.yy;

                    if (aux.agresiune == 1)
                    {
                        float xx, yy;
                        xx = aux.X - PL_P.X;
                        yy = aux.Y - PL_P.Y;
                        if (xx * xx + yy * yy <= aux.r * aux.r)
                        {
                            x = PL_P.X;
                            y = PL_P.Y;
                            if (xx * xx + yy * yy <= 2)
                            {
                                PL_P.viata -= aux.pow;
                                if (PL_P.viata < 0)
                                {
                                    PL_P.fx = 0;
                                    MaxiFun.IO.Save<Planeta>(PLA_S, saveDir, "PLANETA" + PLA_A);
                                    COMANDA("set_menu", "", 3, 0);
                                }
                            }
                            else if (aux.inteligenta >= 5)
                            {
                                if (TIME % 4 == 0)
                                {
                                    float L, Lx, Ly;
                                    Lx = PL_P.poz.X - aux.poz.Y;
                                    Ly = PL_P.poz.Y - aux.poz.X;
                                    L = (float)Math.Sqrt(Lx * Lx + Ly * Ly);

                                    LAS[NR_PRO].poz.X = aux.poz.Y;
                                    LAS[NR_PRO].poz.Y = aux.poz.X;
                                    LAS[NR_PRO].fx -= -Lx / L * 2;
                                    LAS[NR_PRO].fy = Ly / L * 2;
                                    LAS[NR_PRO].poz.X += LAS[NR_PRO].fx;
                                    LAS[NR_PRO].poz.Y += LAS[NR_PRO].fy - 8;
                                    LAS[NR_PRO].pow = aux.pow;
                                    LAS[NR_PRO].tip_p = 7;

                                    NR_PRO++;
                                }
                            }
                        }

                        if (aux.Y < y)
                        {
                            aux.mers++;
                            if (aux.mers >= 20)
                                aux.mers = -19;
                            if (PLA_S.b[aux.X - 1, aux.Y + 1] == 0 && PLA_S.b[aux.X - 2, aux.Y + 1] == 0)
                            {
                                aux.poz.Y += 3;
                                aux.fata = SpriteEffects.FlipHorizontally;
                                aux.orientare = -1;
                            }
                            else if (PLA_S.b[aux.X, aux.Y] != 0 && PLA_S.b[aux.X - 3, aux.Y] == 0)
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
                                if (PLA_S.b[aux.X - 1, aux.Y - 1] == 0 && PLA_S.b[aux.X - 2, aux.Y - 1] == 0)
                                {
                                    aux.poz.Y -= 3;
                                    aux.fata = SpriteEffects.None;
                                    aux.orientare = 1;
                                }
                                else if (PLA_S.b[aux.X, aux.Y] != 0 && PLA_S.b[aux.X - 3, aux.Y] == 0)
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

        public int CRAFTING(int a1, int a2)
        {
            if (a1 != 0 && a2 != 0)
            {
                if (a2 >= 0)
                {
                    if (inventar[COMP_A] > 0)
                    {
                        inventar[COMP_A]--;
                        inventar[NR_comp + NR_subs * NR_elem + a2 + 1]++;
                    }
                    else return 0;
                }
                else if (a2 == -1)
                {
                    int s1 = NR_comp + NR_subs * NR_elem + 1;
                    if (a1 == 2)    //10 fer + 5 carbon
                    {
                        if (inventar[s1 + 1] < 10 || inventar[s1 + 3] < 5)
                            return 0;
                        inventar[s1 + 1] -= 10;
                        inventar[s1 + 3] -= 5;
                        inventar[COMP_A]++;
                    }
                    else if (a1 == 3) //10 fer + 10 carbon + 20 uraniu + 5 sulfur
                    {
                        if (inventar[s1 + 1] < 10 || inventar[s1 + 3] < 10 || inventar[s1 + 4] < 20 || inventar[s1 + 2] < 5)
                            return 0;
                        inventar[s1 + 1] -= 10;
                        inventar[s1 + 3] -= 10;
                        inventar[s1 + 4] -= 20;
                        inventar[s1 + 2] -= 5;
                        inventar[COMP_A]++;
                    }
                    else if (a1 == 4) // 10 fer + 10 uraniu + 10 sulfir
                    {
                        if (inventar[s1 + 1] < 10 || inventar[s1 + 2] < 10 || inventar[s1 + 4] < 10)
                            return 0;
                        inventar[s1 + 1] -= 10;
                        inventar[s1 + 2] -= 10;
                        inventar[s1 + 4] -= 10;
                        inventar[COMP_A]++;
                    }
                    else if (a1 == 5)    // 10 fer + 10 carbon + 2 uraniu
                    {
                        if (inventar[s1 + 1] < 10 || inventar[s1 + 3] < 10 || inventar[s1 + 4] < 2)
                            return 0;
                        inventar[s1 + 1] -= 10;
                        inventar[s1 + 3] -= 10;
                        inventar[s1 + 4] -= 2;
                        inventar[COMP_A]++;
                    }
                    else if (a1 == 6) //30 fer + 10 carbon
                    {
                        if (inventar[s1 + 1] < 30 || inventar[s1 + 3] < 10)
                            return 0;
                        inventar[s1 + 1] -= 30;
                        inventar[s1 + 3] -= 10;
                        inventar[COMP_A]++;
                    }
                    else if (a1 == 7) //1 Placaj1 + 5 fer + 10 carbon
                    {
                        if (inventar[2] < 1 || inventar[s1 + 1] < 5 || inventar[s1 + 3] < 10)
                            return 0;
                        inventar[2] -= 1;
                        inventar[s1 + 1] -= 5;
                        inventar[s1 + 3] -= 10;
                        inventar[COMP_A]++;
                    }
                    else if (a1 == 8) //3 Propulsoare1 + 10 fer + 10 carbon
                    {
                        if (inventar[3] < 3 || inventar[s1 + 1] < 10 || inventar[s1 + 3] < 10)
                            return 0;
                        inventar[3] -= 3;
                        inventar[s1 + 1] -= 10;
                        inventar[s1 + 3] -= 10;
                        inventar[COMP_A]++;
                    }
                    else if (a1 == 9) // 2 Generatoare1 + 5 fer + 10 carbon + 10 uraniu
                    {
                        if (inventar[4] < 2 || inventar[s1 + 1] < 5 || inventar[s1 + 3] < 10 || inventar[s1 + 4] < 10)
                            return 0;
                        inventar[4] -= 2;
                        inventar[s1 + 1] -= 5;
                        inventar[s1 + 3] -= 10;
                        inventar[s1 + 4] -= 10;
                        inventar[COMP_A]++;
                    }
                    else if (a1 == 10) // 1 Tureta1 + 10 fer + 5 carbon + 5 uraniu
                    {
                        if (inventar[5] < 1 || inventar[s1 + 1] < 10 || inventar[s1 + 3] < 5 || inventar[s1 + 4] < 5)
                            return 0;
                        inventar[5] -= 1;
                        inventar[s1 + 1] -= 10;
                        inventar[s1 + 3] -= 5;
                        inventar[s1 + 4] -= 5;
                        inventar[s1 + 2] -= 5;
                        inventar[COMP_A]++;
                    }
                    else if (a1 == 11) // 4 Placaj2 + 10 fer + 10 sulfur + 5 carbon
                    {
                        if (inventar[7] < 4 || inventar[s1 + 1] < 10 || inventar[s1 + 3] < 5 || inventar[s1 + 2] < 10)
                            return 0;
                        inventar[7] -= 4;
                        inventar[s1 + 1] -= 10;
                        inventar[s1 + 3] -= 5;
                        inventar[s1 + 2] -= 10;
                        inventar[COMP_A]++;
                    }
                    else if (a1 == 12) //4 Generatoare2 + 5 fer + 10 uraniu
                    {
                        if (inventar[9] < 4 || inventar[s1 + 1] < 5 || inventar[s1 + 4] < 10)
                            return 0;
                        inventar[9] -= 4;
                        inventar[s1 + 1] -= 5;
                        inventar[s1 + 4] -= 10;
                        inventar[COMP_A]++;
                    }
                    else if (a1 == 13) // 2 Tureta2 + 5 uraniu + 5 carbon + 5 fer
                    {
                        if (inventar[10] < 2 || inventar[s1 + 1] < 5 || inventar[s1 + 3] < 5 || inventar[s1 + 4] < 5)
                            return 0;
                        inventar[10] -= 2;
                        inventar[s1 + 1] -= 5;
                        inventar[s1 + 4] -= 5;
                        inventar[s1 + 3] -= 5;
                        inventar[COMP_A]++;
                    }
                    else if (a1 == 14) // 4 Propulsoare2 + 10 uraniu + 10 fer + 10 sulf
                    {
                        if (inventar[8] < 4 || inventar[s1 + 1] < 10 || inventar[s1 + 4] < 10 || inventar[s1 + 2] < 10)
                            return 0;
                        inventar[8] -= 4;
                        inventar[s1 + 1] -= 10;
                        inventar[s1 + 4] -= 20;
                        inventar[s1 + 2] -= 10;
                        inventar[COMP_A]++;
                    }
                    else if (a1 == 15) //
                    {
                        if (inventar[11] < 3 || inventar[s1 + 1] < 20 || inventar[s1 + 3] < 10)
                            return 0;
                        inventar[11] -= 3;
                        inventar[s1 + 1] -= 20;
                        inventar[s1 + 3] -= 10;
                        inventar[COMP_A]++;
                    }
                    else if (a1 == 16) //
                    {
                        if (inventar[11] < 3 || inventar[s1 + 1] < 5 || inventar[s1 + 3] < 20 || inventar[s1 + 4] < 20 || inventar[s1 + 2] < 10)
                            return 0;
                        inventar[11] -= 3;
                        inventar[s1 + 1] -= 5;
                        inventar[s1 + 3] -= 20;
                        inventar[s1 + 4] -= 20;
                        inventar[s1 + 2] -= 10;
                        inventar[COMP_A]++;
                    }
                }
            }
            return 1;
        }


        protected override void Initialize()
        {
            saveDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/B_T_H";
            WINDOW_REZ.Y = GraphicsDevice.DisplayMode.Width;
            WINDOW_REZ.X = GraphicsDevice.DisplayMode.Height;
            PL_P_E = new Vector2(WINDOW_REZ.Y / 2, WINDOW_REZ.X / 2);
            graphics.PreferredBackBufferHeight = (int)WINDOW_REZ.X;
            graphics.PreferredBackBufferWidth = (int)WINDOW_REZ.Y;

            OBTIUNI[1] = (int)WINDOW_REZ.X;
            OBTIUNI[2] = (int)WINDOW_REZ.Y;
            graphics.ApplyChanges();

            nr_FOTNTS = 6;
            for(int i=1;i<=nr_FOTNTS;i++)
                font[i] = Content.Load<SpriteFont>("FONT_"+i);
            for (int i = 0; i < 10; i++)
                CHAT[i] = "";


            comp = new Componenta[NR_comp + 1];
            LAS = new Proiectil[100000];
            for (int i = 0; i < 100000; i++)
                LAS[i] = new Proiectil();
            for (int i = 0; i <= NR_comp; i++)
                comp[i] = new Componenta();
            for (int i = 0; i < 1000; i++)
                L_PLA[i] = new LocPlaneta();

            items = new Texture2D[NR_item + 1];
            inventar = new int[NR_comp + NR_elem * NR_subs + NR_item + 5];
            inventar[2] = 5;
            inventar[3] = 2;
            inventar[4] = 2;
            inventar[5] = 2;
            inventar[6] = 1;
            /*for (int i = 0; i <= NR_comp + NR_elem * NR_subs + NR_item; i++)
                inventar[i] = 1;*/

            for (int i = 0; i < NR_item; i++)
                items[i] = Content.Load<Texture2D>("I" + i);
            MENIU_TEX = new Texture2D[2, 10];
            for (int i = 1; i <= 6; i++)
                for (int j = 1; j <= 2; j++)
                    MENIU_TEX[j - 1, i] = Content.Load<Texture2D>("MENIU" + i + j);


            MOUSE_T = Content.Load<Texture2D>("MOUSE2");
            for (int i = 1; i <= N_BACK * N_BACK; i++)
                BACK_IMG[i] = Content.Load<Texture2D>("BACK" + i);
            L_BACK = BACK_IMG[1].Height;


            #region PRESETARE_COMPONENTE_NAVA
            for (int i = 0; i <= NR_comp; i++)
            {
                comp[i].T = Content.Load<Texture2D>("C" + i);
                comp[i].pow = 0;
                comp[i].eng = 0;
                comp[i].v = 100;
                comp[i].nr = 0;
                comp[i].proi = 0;
            }

            comp[2].v = 200;            //////// COMPONENTE ATRIBUTE
            comp[3].pow = 70;
            comp[4].eng = 100;
            comp[5].proi = 100;
            comp[6].v = 300;
            comp[7].v = 250;
            comp[8].pow = 90;
            comp[8].v = 150;
            comp[9].eng = 150;
            comp[10].proi = 200;
            comp[10].proi = 130;
            comp[10].v = 130;
            comp[11].v = 350;
            comp[12].eng = 300;
            comp[13].proi = 300;
            comp[13].v = 160;
            comp[14].pow = 150;
            comp[14].v = 250;
            comp[15].v = 500;
            comp[16].proi = 400;
            comp[16].v = 180;
            #endregion

            #region PRESETARE_PLAYER
            PL.poz = new Vector2(0, 0);
            PL.comp = new int[50, 50];
            PL.viata = new int[50, 50];
            PL.comp[18, 18] = 1;
            PL.viata[18, 18] = 100;
            PL.rot = 0;
            PL.pow = PL.eng = PL.eng = 0;
            PL.eng_m = 10;
            PL.nr_c = 1;

            PL_P.parti = new int[10];
            PL_P.parti[0] = 1;
            PL_P.rot = new float[7];
            PL_P.mers = 0;
            PL_P.tip_parti = new int[10];
            PL_P.pow = 10;
            PL_P.r = 35;
            #endregion

            for (int i = 1; i <= 8; i++)
                LAS_T[i] = Content.Load<Texture2D>("P" + i);

            PLA_T = new Texture2D[NR_elem + 1, NR_subs];
            for (int i = 1; i <= NR_elem; i++)
                for (int j = 0; j < NR_subs; j++)
                    PLA_T[i, j] = Content.Load<Texture2D>("S" + i + j);



            PAR_C = new Texture2D[NR_PARTI + 10, 7];
            for (int i = 0; i <= NR_PARTI; i++)
                for (int j = 1; j < 6; j++)
                    PAR_C[i, j] = Content.Load<Texture2D>("BODY" + i + j);
            for (int i = NR_PARTI + 1; i <= NR_PARTI + 9; i++)
                PAR_C[i, 0] = Content.Load<Texture2D>("BODY" + i + "0");


            for (int i = 0; i < NR_subs + 5; i++)
                PLA_TEX[i] = Content.Load<Texture2D>("PLANETA" + i);

            L_PLA[0].ID = 1000;
            int aux = 1;
            int tip_PLA = 1;
            int tip_AST = 0;
            int nr_pln_s = 3;
            nr_PLA_S = 500;
            for (int i = 1; i < nr_PLA_S; i++)
            {
                if (i <= nr_pln_s)
                {
                    L_PLA[i] = CREARE_LOC_PLANETA(aux, i, 2);
                    L_PLA[i].ID = i;
                    L_PLA[i].SALVAT = 0;
                }
                else
                {
                    if (tip_PLA != 0 || aux == i)
                        L_PLA[i] = CREARE_LOC_PLANETA(aux, i, 1);
                    else L_PLA[i] = CREARE_LOC_PLANETA(tip_AST, i, 0);

                    if (L_PLA[i].MOON == 1 && L_PLA[i].p_orb != 0)
                    {
                        tip_AST = i;
                        tip_PLA = 0;
                    }
                    if ((int)Math.Sqrt(ran.Next(0, 100025)) <= 24 || i - tip_AST >= 15)
                        tip_AST = 0;
                    if (tip_AST == 0)
                        tip_PLA = i;

                    int last_id = L_PLA[i - 1].ID;
                    if (last_id < 0)
                        last_id *= -1;
                    if (last_id < 1000)
                        last_id = 1000;
                    if (tip_AST != 0)
                        L_PLA[i].ID = last_id + 1;
                    else
                        L_PLA[i].ID = -last_id - i;
                    if (ran.Next(0, 40) == 2)
                    {
                        aux = i + 1;
                        tip_AST = 0;
                    }
                }
            }
            for (int i = 1; i <= 3; i++)
                MOONS[i] = Content.Load<Texture2D>("MOON" + i);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            string filePath = saveDir + "/INVENTAR.dat";
            if (File.Exists(filePath))
            {
                string[] PLANETE_SALVATE = MaxiFun.IO.Load<string[]>(saveDir, "PLANETE_ID");

                for (nr_PLA_S = 1; PLANETE_SALVATE[nr_PLA_S] != ""; nr_PLA_S++)
                {
                    L_PLA[nr_PLA_S] = MaxiFun.IO.Load<LocPlaneta>(saveDir, "LocPlaneta" + PLANETE_SALVATE[nr_PLA_S]);
                }
                PL = MaxiFun.IO.Load<Nava>(saveDir, "NAVA_PLAYER");
                inventar = MaxiFun.IO.Load<int[]>(saveDir, "INVENTAR");
            }
            PLA_S = CREARE_PLANETA_FROM_IMG(Content.Load<Texture2D>("PLANETA_2"));
            //OBTIUNI[3] = 1;
            COMANDA("set_menu", "", 5, 0);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                SAVE();
                this.Exit();
            }

            MOUSE_P.X = Mouse.GetState().X;
            MOUSE_P.Y = Mouse.GetState().Y;


            if (MENU == 1)   // FIRST MENIU
            {
                #region MENIU_1

                for (int i = 1; i <= NR_PRO; i++)
                {
                    LAS[i].poz += new Vector2(LAS[i].fx, LAS[i].fy);
                    LAS[i].pow++;

                    if (i % 20 != 1 && i <= NR_PRO - NR_PRO % 20)
                    {
                        if (ran.Next(0, 3) == 1)
                        {
                            LAS[i].fx += (float)(1 - ran.Next(0, 3)) / 3;
                            LAS[i].fy += (float)(1 - ran.Next(0, 3)) / 3;
                        }
                        LAS[i].t--;
                        if (LAS[i].t <= 0)
                        {
                            LAS[i].poz = LAS[(int)(i / 20) * 20 + 1].poz + new Vector2(10 - ran.Next(20), 10 - ran.Next(20));
                            LAS[i].t = (i - 1) % 20 + ran.Next(0, ((i - 1) % 20) / 2);
                            LAS[i].fx = 0f;
                            LAS[i].fy = 0f;
                        }
                    }
                    else
                    {
                        if (LAS[i].poz.X <= -80 || LAS[i].poz.X >= WINDOW_REZ.Y + 80
                            || LAS[i].poz.Y <= -80 || LAS[i].poz.Y >= WINDOW_REZ.X + 80)
                        {
                            int rrr = ran.Next(0, 100);
                            if (rrr % 4 == 0)
                                LAS[i].poz = new Vector2(-70, ran.Next(5, (int)WINDOW_REZ.X - 5));
                            else if (rrr % 4 == 1)
                                LAS[i].poz = new Vector2(ran.Next(5, (int)WINDOW_REZ.Y - 5), (int)WINDOW_REZ.X + 70);
                            else if (rrr % 4 == 2)
                                LAS[i].poz = new Vector2((int)WINDOW_REZ.Y + 70, ran.Next(5, (int)WINDOW_REZ.X - 5));
                            else if (rrr % 4 == 3)
                                LAS[i].poz = new Vector2(ran.Next(5, (int)WINDOW_REZ.Y - 5), -70);

                            float ung = ran.Next(0, 360) / 3.14159265f;
                            LAS[i].fx = 7 * (float)Math.Sin(ung);
                            LAS[i].fy = 7 * (float)Math.Cos(ung);
                        }
                    }
                }

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    int x, y;
                    x = (int)PL_P_E.X - Mouse.GetState().X + 150;
                    y = (int)PL_P_E.Y - Mouse.GetState().Y + 150;

                    if (x >= 0 && x <= 300)
                    {
                        if (y >= 0 && y <= 100)
                        {
                            SAVE();
                            this.Exit();
                        }
                        else if (y > 100 && y <= 200)
                            COMANDA("set_menu", "", 2, 0);
                        else if (y > 200 && y <= 300)
                            COMANDA("set_menu", "", 3, 0);
                    }
                }

                #endregion
            }
            else if (MENU == 2)  // OBTIUNI
            {
                #region MENIU_2

                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    if (BUTON_A_3 == false)
                        COMANDA("set_menu", "", 1, 0);
                    BUTON_A_3 = true;
                }
                else BUTON_A_3 = false;
                Keyboard.GetState();

                var keyboardState = Keyboard.GetState();
                var keys = keyboardState.GetPressedKeys();
                if (keys.Length > 0)
                {
                    if (BUTON_A_1 == false)
                    {
                        var keyValue = keys[0].ToString()[keys[0].ToString().Length - 1];
                        ADD_CHAT_LINE(CHAT[0] + keyValue + "");
                        BUTON_A_1 = true;
                    }
                }
                else BUTON_A_1 = false;


                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (BUTON_A_2 == false)
                    {
                        int x, y;
                        x = ((int)PL_P_E.X - Mouse.GetState().X + 250);
                        y = ((int)PL_P_E.Y - Mouse.GetState().Y + 250);

                        if (x >= 475 && x <= 500)
                        {
                            if (y >= 375 && y <= 400)
                            {
                                if (OBTIUNI[0] == 0)
                                {
                                    OBTIUNI[0] = 1;
                                    graphics.IsFullScreen = true;
                                    graphics.ApplyChanges();
                                }
                                else
                                {
                                    graphics.IsFullScreen = false;
                                    graphics.ApplyChanges();
                                    OBTIUNI[0] = 0;
                                }
                            }
                        }
                        else if (x >= 0 && x <= 25)
                        {
                            if (y >= 375 && y <= 400)
                            {
                                OBTIUNI[3] = (OBTIUNI[3] + 1) % 2;
                            }
                        }
                        else if (y >= 125 && y <= 150)
                        {
                            if (x >= 275 && x <= 475)
                            {
                                OBTIUNI[2] = (475 - x) * 2000 / 200;
                                if (OBTIUNI[2] < 700)
                                    OBTIUNI[2] = 700;
                                WINDOW_REZ = new Vector2(OBTIUNI[1], OBTIUNI[2]);
                                PL_P_E = new Vector2(WINDOW_REZ.Y / 2, WINDOW_REZ.X / 2);
                                graphics.PreferredBackBufferHeight = (int)WINDOW_REZ.X;
                                graphics.PreferredBackBufferWidth = (int)WINDOW_REZ.Y;
                                graphics.ApplyChanges();
                            }
                            else if (x >= 25 && x <= 225)
                            {
                                OBTIUNI[1] = (225 - x) * 1500 / 200;
                                if (OBTIUNI[1] < 700)
                                    OBTIUNI[1] = 700;
                                WINDOW_REZ = new Vector2(OBTIUNI[1], OBTIUNI[2]);
                                PL_P_E = new Vector2(WINDOW_REZ.Y / 2, WINDOW_REZ.X / 2);
                                graphics.PreferredBackBufferHeight = (int)WINDOW_REZ.X;
                                graphics.PreferredBackBufferWidth = (int)WINDOW_REZ.Y;
                                graphics.ApplyChanges();
                            }
                        }
                    }
                    BUTON_A_2 = true;
                }
                else BUTON_A_2 = false;

                #endregion
            }
            else if (MENU == 3) // MENIUL DE JOC
            {
                #region MENIU_3    

                PL.rot = (float)Math.Atan((PL_P_E.Y - MOUSE_P.Y) / (PL_P_E.X - MOUSE_P.X));
                if (PL_P_E.X >= MOUSE_P.X)
                    PL.rot += 3.1415f;

                if (Keyboard.GetState().IsKeyDown(Keys.W))  /// MISCAREA
                {
                    PL.poz.X += PL.pow * (float)Math.Cos(PL.rot) / PL.nr_c;
                    PL.poz.Y += PL.pow * (float)Math.Sin(PL.rot) / PL.nr_c;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    PL.poz.X -= PL.pow * (float)Math.Cos(PL.rot) / PL.nr_c / 1.5f;
                    PL.poz.Y -= PL.pow * (float)Math.Sin(PL.rot) / PL.nr_c / 1.5f;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    PL.poz.X -= PL.pow * (float)Math.Sin(PL.rot) / PL.nr_c / 1.5f;
                    PL.poz.Y += PL.pow * (float)Math.Cos(PL.rot) / PL.nr_c / 1.5f;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    PL.poz.X += PL.pow * (float)Math.Sin(PL.rot) / PL.nr_c / 1.5f;
                    PL.poz.Y -= PL.pow * (float)Math.Cos(PL.rot) / PL.nr_c / 1.5f;
                }

                if (ZOOM_VAL < Mouse.GetState().ScrollWheelValue)
                {
                    ZOOM += ZOOM / 10;
                    ZOOM_VAL = Mouse.GetState().ScrollWheelValue;
                }
                else if (ZOOM_VAL > Mouse.GetState().ScrollWheelValue)
                {
                    ZOOM -= ZOOM / 10;
                    ZOOM_VAL = Mouse.GetState().ScrollWheelValue;
                }



                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    if (BUTON_A_1 == false)
                    {
                        ZOOM = 1;
                        PL.rot = 0;
                        MENU = 4;
                        BUTON_A_1 = true;
                        COMP_A = 2;
                        MENIU_VECTOR = new Vector2(MENIU_TEX[1, MENU].Width / 2, MENIU_TEX[1, MENU].Height / 2);
                    }
                }
                else BUTON_A_1 = false;


                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (TIME % 5 == 0)
                        LAS_A = true;
                    else LAS_A = false;
                }
                else LAS_A = false;

                TIME++;
                if (TIME == 30)
                {
                    TIME = 0;
                    PL.eng += PL.eng_m / 100;
                    if (PL.eng > PL.eng_m)
                        PL.eng = PL.eng_m;
                }

                // if(TIME%20==0)
                for (int i = 1; i <= nr_PLA_S; i++)
                    if (L_PLA[i].p_orb != 0)
                    {
                        L_PLA[i].poz = L_PLA[L_PLA[i].p_orb].poz;
                        L_PLA[i].ung += (float)Math.Sqrt(L_PLA[i].R) / 200 * 0.0001f;
                        L_PLA[i].poz.X += (float)Math.Sin(L_PLA[i].ung) * L_PLA[i].R;
                        L_PLA[i].poz.Y += (float)Math.Cos(L_PLA[i].ung) * L_PLA[i].R;
                        float xx, yy, r;
                        xx = PL.poz.X - L_PLA[i].poz.X;
                        yy = PL.poz.Y - L_PLA[i].poz.Y;
                        r = (float)Math.Sqrt(xx * xx + yy * yy);
                        if (r < 400 && Keyboard.GetState().IsKeyDown(Keys.E))
                        {
                            if (BUTON_A_2 == false)
                            {
                                BUTON_A_2 = true;
                                if (L_PLA[i].p_orb != 0)
                                {
                                    PLA_A = i;
                                    SELECTEAZA_PLANETA();
                                    if (L_PLA[PLA_A].ord_elm[NR_subs + 1] != 7)
                                        COMANDA("set_menu", "", 5, 0);
                                    else
                                        COMANDA("set_menu", "", 8, 0);
                                    break;
                                }
                            }
                        }
                        else BUTON_A_2 = false;
                    }


                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    if (BUTON_A_3 == false)
                        COMANDA("set_menu", "", 1, 0);
                    BUTON_A_3 = true;
                }
                else BUTON_A_3 = false;
                #endregion
            }
            else if (MENU == 4)  // MENIUL DE CONSTRUCTIE A NAVEI
            {
                #region MENIU_4   

                int xx, yy;
                xx = (int)(PL_P_E.X - MOUSE_P.X);
                yy = (int)(PL_P_E.Y - MOUSE_P.Y);
                xx -= xx % 10;
                yy -= yy % 10;

                MOUSE_P.X = PL_P_E.X - xx;
                MOUSE_P.Y = PL_P_E.Y - yy;

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    if (BUTON_A_1 == false)
                    {
                        if (VERIFICARE(PL) == PL.nr_c)
                        {
                            ZOOM = 1;
                            PL.rot = 0;
                            MENU = 3;
                            BUTON_A_1 = true;
                            COMP_A = 0;
                            MENIU_VECTOR = new Vector2(MENIU_TEX[1, MENU].Width / 2, MENIU_TEX[1, MENU].Height / 2);
                        }
                    }
                }
                else BUTON_A_1 = false;
                
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)  /// PUNERE/SCOATERE COMPONENTE
                {
                    int AUX_ = (int)((MOUSE_P.X) / 20) * (int)(WINDOW_REZ.X / 20) + (int)((MOUSE_P.Y - 40) / 20) - 2;/// ALEGEREA COMPONENTELEOR

                    int AUY = 0;
                    for (int nr = 0, i = 0; nr <= NR_comp; nr++)
                    {
                        AUY = nr;
                        if (inventar[nr] != 0)
                            i++;
                        if (i == AUX_)
                            break;
                        if (nr == NR_comp)
                            AUY = 0;
                    }

                    if (AUY != 0)
                        COMP_A = AUY;
                    /*if (AUX_ <= NR_comp - 1 && AUX_ >= 1)
                        COMP_A = AUX_ + 1;   */
                    else
                    {
                        int i, j;
                        i = (int)(PL_P_E.X - MOUSE_P.X);
                        j = (int)(PL_P_E.Y - MOUSE_P.Y);

                        i /= 20;
                        j /= 20;

                        i += 18;
                        j += 18;
                        if (i >= 0 && i <= 36 && j >= 0 && j <= 36)
                            if (PL.comp[36 - i, 36 - j] == 0)
                                if (inventar[COMP_A] > 0)
                                {
                                    PL.eng_m += comp[COMP_A].eng;
                                    PL.pow += comp[COMP_A].pow;

                                    PL.comp[36 - i, 36 - j] = COMP_A;
                                    PL.nr_c++;
                                    PL.viata[36 - i, 36 - j] = comp[COMP_A].v;
                                    inventar[COMP_A]--;
                                }
                    }

                }
                else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                {
                    int i, j;
                    i = (int)(PL_P_E.X - MOUSE_P.X);
                    j = (int)(PL_P_E.Y - MOUSE_P.Y);

                    i /= 20;
                    j /= 20;

                    i += 18;
                    j += 18;
                    if (i >= 0 && i <= 36 && j >= 0 && j <= 36 && (i != 18 || j != 18))
                        if (PL.comp[36 - i, 36 - j] != 0)
                        {
                            PL.eng_m -= comp[PL.comp[36 - i, 36 - j]].eng;
                            PL.pow -= comp[PL.comp[36 - i, 36 - j]].pow;

                            inventar[PL.comp[36 - i, 36 - j]]++;
                            PL.comp[36 - i, 36 - j] = 0;
                            PL.viata[36 - i, 36 - j] = 0;
                            PL.nr_c--;
                        }
                }
                #endregion
            }
            else if (MENU == 5)  // PE PLANETA
            {
                #region MENIU_5
                if (Keyboard.GetState().IsKeyDown(Keys.W))  /// MISCAREA 
                    if (PLA_S.b[PL_P.X, PL_P.Y] != 0)
                        PL_P.fx = 6;
                int x, y;

                PL_P.poz.Y -= PL_P.fx;
                PL_P.X = (int)(PL_P.poz.Y + 30) / 20;
                if (PLA_S.b[PL_P.X - 2, PL_P.Y] != 0)
                {
                    PL_P.poz.Y += PL_P.fx;
                    PL_P.fx = 0;
                    PL_P.poz.Y += (PL_P.poz.Y + 30) % 20 / 2;
                }
                if (PLA_S.b[PL_P.X, PL_P.Y] != 0)
                {
                    PL_P.fx = 0;
                    PL_P.poz.Y -= (PL_P.poz.Y + 30) % 20 / 2;
                }
                else PL_P.fx -= (float)PLA_S.forta / 30;
                if (PL_P.fx <= -20)
                    PL_P.fx = -19;



                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    PL_P.poz.X -= 3;
                    PL_P.mers++;
                    if (PL_P.mers >= 20)
                        PL_P.mers = -19;
                    PL_P.fata = SpriteEffects.None;

                    PL_P.Y = (int)(PL_P.poz.X + 5) / 20;
                    if (PL_P.Y <= 0)
                    {
                        MENU = 3;
                        TIME = 0;
                        base.Update(gameTime);
                        return;
                    }
                    if (PLA_S.b[PL_P.X - 1, PL_P.Y] != 0 || PLA_S.b[PL_P.X - 2, PL_P.Y] != 0)
                        PL_P.poz.X += 3;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    PL_P.poz.X += 3;
                    PL_P.mers--;
                    if (PL_P.mers <= -20)
                        PL_P.mers = 19;
                    PL_P.fata = SpriteEffects.FlipHorizontally;

                    PL_P.Y = (int)(PL_P.poz.X + 15) / 20;
                    if (PL_P.Y >= 300)
                    {
                        MENU = 3;
                        TIME = 0;
                        base.Update(gameTime);
                        return;
                    }
                    if (PLA_S.b[PL_P.X - 1, PL_P.Y] != 0 || PLA_S.b[PL_P.X - 2, PL_P.Y] != 0)
                        PL_P.poz.X -= 3;
                }
                else
                {
                    PL_P.mers += (10 - PL_P.mers) / 10;
                    if (PL_P.viata < 1000)
                        PL_P.viata += 1;
                }


                if (ZOOM_VAL < Mouse.GetState().ScrollWheelValue)
                {
                    ZOOM += ZOOM / 10;
                    ZOOM_VAL = Mouse.GetState().ScrollWheelValue;
                }
                else if (ZOOM_VAL > Mouse.GetState().ScrollWheelValue)
                {
                    ZOOM -= ZOOM / 10;
                    ZOOM_VAL = Mouse.GetState().ScrollWheelValue;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    if (BUTON_A_1 == false)
                    {
                        BUTON_A_1 = true;
                        COMANDA("set_menu", "", 6, 0);
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.F))
                {
                    if (BUTON_A_1 == false)
                    {
                        BUTON_A_1 = true;
                        COMANDA("set_menu", "", 9, 0);
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.M))
                {
                    if (BUTON_A_1 == false)
                    {
                        BUTON_A_1 = true;
                        MENU_AUX = -1;
                        COMANDA("set_menu", "", 7, 0);
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Add))
                {
                    if (BUTON_A_1 == false)
                    {
                        PL_P.r += 20;
                        BUTON_A_1 = true;
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Subtract))
                {
                    if (BUTON_A_1 == false)
                    {
                        PL_P.r -= 20;
                        BUTON_A_1 = true;
                    }
                }
                else BUTON_A_1 = false;


                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (PL_P.parti[0] == 1)
                    {
                        //if (TIME % 5 == 0 && LAS_A == false)
                        LAS_A = true;
                        //else LAS_A = false;
                    }
                    else if (PL_P.parti[0] == 2)
                    {
                        if (inventar[PL_P.parti[1]] >= 0)
                        {
                            y = (int)((PL_P_E.X - MOUSE_P.X) / ZOOM);
                            x = (int)((PL_P_E.Y - MOUSE_P.Y) / ZOOM);

                            if (Math.Sqrt(x * x + y * y) <= 80 && Math.Sqrt(x * x + y * y) >= 30)
                            {
                                y = (int)(PL_P.poz.X + y + 10) / 20;
                                x = (int)(PL_P.poz.Y - x + 10) / 20;

                                if (x > 0 && x < 300 && y > 0 && y < 300)
                                    if (PLA_S.a[x, y] == 0)
                                    {
                                        if (PL_P.parti[1] <= NR_comp)
                                            PLA_S.a[x, y] = PL_P.parti[1];
                                        else
                                        {
                                            int aux = PL_P.parti[1] - NR_comp - 1;
                                            PLA_S.a[x, y] = (int)(aux / NR_subs + 1) * 100 + aux % NR_subs;
                                        }
                                        PLA_S.b[x, y] = ret_v(PL_P.parti[1]);
                                        ELIMINARE_APA(x, y);
                                        if (inventar[PL_P.parti[1]] > 0)
                                            inventar[PL_P.parti[1]]--;
                                        else PL_P.parti[1] = 0;
                                    }
                            }
                        }
                    }
                    else if (PL_P.parti[0] == 3)
                    {
                        if (TIME % 4 == 0)
                        {
                            float L, Lx, Ly;
                            Lx = MOUSE_P.X - PL_P_E.X;
                            Ly = MOUSE_P.Y - PL_P_E.Y + 8 * ZOOM;
                            L = (float)Math.Sqrt(Lx * Lx + Ly * Ly);

                            LAS[NR_PRO].poz = PL_P.poz;
                            LAS[NR_PRO].fx = -Lx / L * 2;
                            LAS[NR_PRO].fy = Ly / L * 2;
                            LAS[NR_PRO].poz.X += LAS[NR_PRO].fx;
                            LAS[NR_PRO].poz.Y += LAS[NR_PRO].fy - 8;
                            LAS[NR_PRO].pow = PL_P.pow;
                            LAS[NR_PRO].tip_p = 5;

                            NR_PRO++;
                        }
                    }
                }
                else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                {
                    if (PL_P.parti[0] == 2)
                    {
                        y = (int)((PL_P_E.X - MOUSE_P.X) / ZOOM);
                        x = (int)((PL_P_E.Y - MOUSE_P.Y) / ZOOM);

                        if (Math.Sqrt(x * x + y * y) <= 80)
                        {
                            y = (int)(PL_P.poz.X + y + 10) / 20;
                            x = (int)(PL_P.poz.Y - x + 10) / 20;

                            if (x > 0 && x < 300 && y > 0 && y < 300)
                                if (PLA_S.b[x, y] == 0)
                                {
                                    PLA_S.b[x, y] = 0;
                                    PLA_S.a[x, y] = 0;
                                }
                        }
                    }
                }
                else LAS_A = false;


                for (int i = 0; i < PLA_S.nr_creaturi; i++)
                    PLA_S.creaturi[i] = AI_FIINTA(PLA_S.creaturi[i]);

                if (Keyboard.GetState().IsKeyDown(Keys.E))
                {
                    if (BUTON_A_2 == false)
                    {
                        BUTON_A_2 = true;
                        int okkk = 0;
                        if (PLA_S.a[PL_P.X, PL_P.Y] == 1600)
                        {
                            CURENT(PL_P.X, PL_P.Y);
                            okkk = 1;
                        }
                        else if (PL_P.fata == SpriteEffects.None)
                        {
                            if (PLA_S.a[PL_P.X, PL_P.Y - 1] == 1600)
                            {
                                CURENT(PL_P.X, PL_P.Y - 1);
                                okkk = 1;
                            }
                            else if (PLA_S.a[PL_P.X - 1, PL_P.Y - 1] == 1600)
                            {
                                CURENT(PL_P.X - 1, PL_P.Y - 1);
                                okkk = 1;
                            }
                        }
                        else
                        {
                            if (PLA_S.a[PL_P.X, PL_P.Y + 1] == 1600)
                            {
                                CURENT(PL_P.X, PL_P.Y + 1);
                                okkk = 1;
                            }
                            else if (PLA_S.a[PL_P.X - 1, PL_P.Y + 1] == 1600)
                            {
                                CURENT(PL_P.X - 1, PL_P.Y + 1);
                                okkk = 1;
                            }
                        }
                        if (okkk == 0)
                            for (int i = 0; i < PLA_S.nr_creaturi; i++)
                            {
                                float xxx, yyy;
                                xxx = PL_P.X - PLA_S.creaturi[i].X;
                                yyy = PL_P.Y - PLA_S.creaturi[i].Y;

                                if (xxx * xxx + yyy * yyy <= 3)
                                {
                                    if (PLA_S.creaturi[i].inteligenta == -1)
                                    {
                                        MaxiFun.IO.Save<Planeta>(PLA_S, saveDir, "PLANETA" + L_PLA[PLA_A].ID);
                                        COMANDA("set_menu", "", 3, 0);
                                        BUTON_A_2 = false;
                                        break;
                                    }
                                    else if (PLA_S.creaturi[i].inteligenta == -2)
                                        COMANDA("set_menu", "", 20, 0);
                                    else if (PLA_S.creaturi[i].inteligenta == -3)
                                        COMANDA("set_menu", "", 21, 0);
                                    else if (PLA_S.creaturi[i].inteligenta == -4)
                                        COMANDA("set_menu", "", 22, 0);
                                    else if (PLA_S.creaturi[i].inteligenta == -5)
                                        COMANDA("set_menu", "", 23, 0);
                                    else if (PLA_S.creaturi[i].inteligenta == -6)
                                        CURENT(PLA_S.creaturi[i].X, PLA_S.creaturi[i].Y);
                                    else if (PLA_S.creaturi[i].inteligenta == -8)
                                    {
                                        PL_P.X = PLA_S.creaturi[PLA_S.creaturi[i].parti[9]].Y;
                                        PL_P.Y = PLA_S.creaturi[PLA_S.creaturi[i].parti[9]].X - 3;
                                        PL_P.poz = new Vector2(PL_P.X * 20, PL_P.Y * 20);
                                    }
                                }
                            }
                    }
                }
                else BUTON_A_2 = false;

                #region FIZICA_APA      
                x = (int)PL_P.poz.X / 20;
                y = (int)PL_P.poz.Y / 20;

                int MM = (int)((WINDOW_REZ.X / 40) / ZOOM) + 2;
                int NN = (int)((WINDOW_REZ.Y / 40) / ZOOM) + 2;
                for (int ii = (int)(-MM * 1); ii <= (int)(MM * 1); ii++)
                    if (y + ii > 0 && y + ii < 300)
                    {
                        int i = y + ii;
                        for (int jj = -NN; jj <= NN; jj++)
                            if (x + jj > 0 && x + jj < 300)
                            {
                                int j = x + jj;
                                if (PLA_S.apa[i, j] / 1000 == 6)
                                {
                                    if (i > 1 && i < 299 && j > 1 && j < 299)
                                    {
                                        if (PLA_S.b[i + 1, j] == 0)
                                        {
                                            PLA_S.apa[i + 1, j]++;
                                            if (PLA_S.apa[i + 1, j] >= 10)
                                            {
                                                PLA_S.apa[i + 1, j] = PLA_S.apa[i, j] + 10;
                                                //if ((PLA_S.apa[i + 1, j] % 1000) / 10 == 0)
                                                //    PLA_S.apa[i + 1, j] += 10;
                                            }
                                        }
                                        else
                                        {
                                            if (PLA_S.b[i, j + 1] == 0)
                                            {
                                                if (PLA_S.apa[i, j + 1] < 10)
                                                {
                                                    PLA_S.apa[i, j + 1]++;
                                                    if (PLA_S.apa[i, j + 1] >= 10)
                                                    {
                                                        PLA_S.apa[i, j + 1] = PLA_S.apa[i, j] + 10;
                                                        if ((PLA_S.apa[i, j + 1] % 1000) / 10 > 30)
                                                            PLA_S.apa[i, j + 1] = 0;
                                                    }
                                                }
                                            }
                                            if (PLA_S.b[i, j - 1] == 0)
                                            {
                                                if (PLA_S.apa[i, j - 1] < 10)
                                                {
                                                    PLA_S.apa[i, j - 1]++;
                                                    if (PLA_S.apa[i, j - 1] >= 10)
                                                    {
                                                        PLA_S.apa[i, j - 1] = PLA_S.apa[i, j] + 10;
                                                        if ((PLA_S.apa[i, j - 1] % 1000) / 10 > 30)
                                                            PLA_S.apa[i, j - 1] = 0;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                    }
                #endregion


                TIME = TIME % 120 + 1;

                PLA_S.SKY.X = Math.Abs(1000 - PLA_S.timp) * PLA_S.V_SKY.X;
                PLA_S.SKY.Y = Math.Abs(1000 - PLA_S.timp) * PLA_S.V_SKY.Y;
                PLA_S.SKY.Z = Math.Abs(1000 - PLA_S.timp) * PLA_S.V_SKY.Z;

                PLA_S.timp += 0.1f;
                if (PLA_S.timp >= 2001)
                    PLA_S.timp = 0;


                PL_P.Y = (int)(PL_P.poz.X + 10) / 20;
                if (PL_P.X <= 0 || PL_P.X >= 300 || PL_P.Y <= 0 || PL_P.Y >= 300)
                {
                    MENU = 3;
                    TIME = 0;
                    base.Update(gameTime);
                    return;
                }
                #endregion
            }
            else if (MENU == 6)  // MENIU DE INVANTAR
            {
                #region MENIU_6
                if (Keyboard.GetState().IsKeyDown(Keys.Q))
                {
                    if (BUTON_A_1 == false)
                    {
                        ZOOM = 1.5f;
                        PL.rot = 0;
                        MENU = 5;
                        BUTON_A_1 = true;
                        COMP_A = 0;
                        MENIU_VECTOR = new Vector2(MENIU_TEX[1, MENU].Width / 2, MENIU_TEX[1, MENU].Height / 2);
                    }
                }
                else if (Mouse.GetState().LeftButton == ButtonState.Pressed)  /// PUNERE/SCOATERE COMPONENTE
                {
                    int AUX_ = (int)((MOUSE_P.X) / 20) * (int)((WINDOW_REZ.X - 100) / 20) + (int)((MOUSE_P.Y - 100) / 20) + 1;/// ALEGEREA COMPONENTELEOR
                    int AUY = 0;
                    for (int nr = 0, i = 0; nr < NR_comp + NR_elem * NR_subs + NR_item + 5; nr++)
                    {
                        AUY = nr - 1;
                        if (i == AUX_)
                            break;
                        if (inventar[nr] != 0)
                            i++;
                        if (nr == NR_comp + NR_elem * NR_subs + NR_item + 4)
                            AUY = 0;
                    }

                    if (AUY > 0)
                        COMP_A = AUY;
                    else
                    {
                        if (BUTON_A_1 == false)
                        {
                            BUTON_A_1 = true;
                            int x, y;
                            x = (int)(MOUSE_P.X - PL_P_E.X) + 250;
                            y = (int)(MOUSE_P.Y - PL_P_E.Y) + 250;

                            if (x >= 93 && x <= 135)
                            {
                                if (y >= 250 && y <= 292)
                                {
                                    PL_P.parti[0] = PL_P.parti[0] % 3 + 1;
                                }
                                else if (y >= 303 && y <= 345)
                                {
                                    if (PL_P.parti[1] == 0)
                                    {
                                        PL_P.parti[1] = COMP_A;
                                        inventar[COMP_A]--;
                                        COMP_A = 0;
                                    }
                                    else
                                    {
                                        BUTON_A_1 = true;
                                        COMP_A = PL_P.parti[1];
                                        inventar[COMP_A]++;
                                        PL_P.parti[1] = 0;
                                    }
                                }
                                else BUTON_A_1 = false;
                            }
                        }
                    }
                }
                else BUTON_A_1 = false;
                #endregion
            }
            else if (MENU == 7)  // HARTA
            {
                #region MENIU_7

                if (Keyboard.GetState().IsKeyDown(Keys.M))
                {
                    if (BUTON_A_1 == false)
                    {
                        BUTON_A_1 = true;
                        COMANDA("set_menu", "", 5, 0);
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    if (BUTON_A_1 == false)
                    {
                        BUTON_A_1 = true;
                        MENU_AUX--;
                        if (MENU_AUX <= -2 || MENU_AUX >= PLA_S.nr_creaturi)
                            MENU_AUX = PLA_S.nr_creaturi - 1;
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    if (BUTON_A_1 == false)
                    {
                        BUTON_A_1 = true;
                        MENU_AUX++;
                        if (MENU_AUX <= -2 || MENU_AUX >= PLA_S.nr_creaturi)
                            MENU_AUX = -1;
                    }
                }
                else BUTON_A_1 = false;

                if (ZOOM_VAL < Mouse.GetState().ScrollWheelValue)
                {
                    ZOOM += ZOOM / 10;
                    ZOOM_VAL = Mouse.GetState().ScrollWheelValue;
                }
                else if (ZOOM_VAL > Mouse.GetState().ScrollWheelValue)
                {
                    ZOOM -= ZOOM / 10;
                    ZOOM_VAL = Mouse.GetState().ScrollWheelValue;
                }

                #endregion
            }
            else if (MENU == 8)  // PE ASTEROID
            {
                #region MENIU_8   
                if (ZOOM_VAL < Mouse.GetState().ScrollWheelValue)
                {
                    ZOOM += ZOOM / 10;
                    ZOOM_VAL = Mouse.GetState().ScrollWheelValue;
                }
                else if (ZOOM_VAL > Mouse.GetState().ScrollWheelValue)
                {
                    ZOOM -= ZOOM / 10;
                    ZOOM_VAL = Mouse.GetState().ScrollWheelValue;
                }

                float FX, FY;
                FX = FY = 0;

                float x, y, r;
                x = 150 * 20 - PL_P.poz.X;
                y = 150 * 20 - PL_P.poz.Y;
                r = (float)Math.Sqrt(x * x + y * y);
                if (r == 0)
                    r = 1;

                PL_P.X = (int)(PL_P.poz.Y + 10) / 20;
                PL_P.Y = (int)(PL_P.poz.X + 10) / 20;

                for (int a = 1; a <= 4; a++)
                {
                    int xxx, yyy;
                    xxx = (int)(PL_P.poz.Y + 10 + (a % 2) * (4 - 8 * (int)((a - 1) / 2))) / 20;
                    yyy = (int)(PL_P.poz.X + 10 + ((a + 1) % 2) * (4 - 8 * (int)((a - 1) / 2))) / 20;

                    if (xxx > 0 && xxx < 300)
                        if (yyy > 0 && yyy < 300)
                            if (PLA_S.b[xxx, yyy] != 0)
                            {
                                PL_P.poz.X -= PL_P.fx;
                                PL_P.poz.Y -= PL_P.fy;

                                if (a % 2 == 1)
                                {
                                    PL_P.X = (int)(PL_P.poz.Y) / 20;
                                    PL_P.fx -= PL_P.fx / 3;
                                    PL_P.fy = 0f;
                                }
                                else
                                {
                                    PL_P.X = (int)(PL_P.poz.Y) / 20;
                                    PL_P.fx = 0f;
                                    PL_P.fy -= PL_P.fy / 3;
                                }
                            }
                }


                // if (PLA_S.b[PL_P.X, PL_P.Y] != 0)
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    FX = 0.3f;
                    FY = 0;

                    LAS[NR_PRO].poz = PL_P.poz;
                    LAS[NR_PRO].pow = 270 + 5 - ran.Next(10);
                    LAS[NR_PRO].fx = -3f;
                    LAS[NR_PRO].fy = 0f;
                    LAS[NR_PRO].t = 2 + ran.Next(2);
                    NR_PRO++;

                    PL_P.fata = SpriteEffects.FlipHorizontally;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    FX = -0.3f;
                    FY = 0;

                    LAS[NR_PRO].poz = PL_P.poz;
                    LAS[NR_PRO].pow = 90 + 5 - ran.Next(10);
                    LAS[NR_PRO].fx = 3f;
                    LAS[NR_PRO].fy = 0f;
                    LAS[NR_PRO].t = 2 + ran.Next(2);
                    NR_PRO++;

                    PL_P.fata = SpriteEffects.None;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    FY = -0.3f;
                    FX = 0;

                    LAS[NR_PRO].poz = PL_P.poz;
                    LAS[NR_PRO].pow = 0 + 5 - ran.Next(10);
                    LAS[NR_PRO].fx = 0f;
                    LAS[NR_PRO].fy = 3f;
                    LAS[NR_PRO].t = 2 + ran.Next(2);
                    NR_PRO++;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    FY = 0.3f;
                    FX = 0;

                    LAS[NR_PRO].poz = PL_P.poz;
                    LAS[NR_PRO].pow = 180 + 5 - ran.Next(10);
                    LAS[NR_PRO].fx = 0f;
                    LAS[NR_PRO].fy = -3f;
                    LAS[NR_PRO].t = 2 + ran.Next(2);
                    NR_PRO++;
                }



                PL_P.fx += FX;
                PL_P.fy += FY;
                PL_P.fx += ((float)PLA_S.forta / 100) * x / r;
                PL_P.fy += ((float)PLA_S.forta / 100) * y / r;
                PL_P.poz.X += PL_P.fx;
                PL_P.poz.Y += PL_P.fy;


                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    LAS_A = true;
                else LAS_A = false;

                PLA_S.timp += 0.0005f;


                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    PLA_S.creaturi[0].poz = PL_P.poz;
                    if (BUTON_A_3 == false)
                    {
                        MaxiFun.IO.Save<Planeta>(PLA_S, saveDir, "PLANETA" + L_PLA[PLA_A].ID);
                        COMANDA("set_menu", "", 3, 0);
                    }
                    BUTON_A_3 = true;
                }
                else BUTON_A_3 = false;
                #endregion
            }
            else if (MENU == 9)  // MENIU DE CRAFTARE
            {
                #region MENIU_9
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (BUTON_A_1 == false)
                    {
                        int AUX_ = (int)((MOUSE_P.X) / 20) * (int)((WINDOW_REZ.X - 100) / 20) + 1 + (int)((MOUSE_P.Y - 100) / 20);

                        if (AUX_ > 0 && AUX_ <= NR_comp + NR_elem * NR_subs + NR_item)
                            COMP_A = AUX_;// + (int)(MOUSE_P.X / 20);
                        else COMP_A = 0;
                    }
                    BUTON_A_1 = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    if (BUTON_A_1 == false)
                    {
                        /* int nrn1 = 0, nrn2 = 0;
                         if (COMP_A <= NR_comp)
                         {
                             nrn1 = COMP_A;
                             nrn2 = -1;
                         }
                         else if (COMP_A <= NR_comp + NR_elem * NR_subs)
                         {
                             nrn1 = (COMP_A - NR_comp - 1) / NR_subs + 1;
                             nrn2 = (COMP_A - NR_comp - 1) % NR_subs;
                         }
                         else if (items[COMP_A - (NR_comp + NR_elem * NR_subs) - 1] != null)
                         {
                             nrn1 = COMP_A - (NR_comp + NR_elem * NR_subs) - 1;
                             nrn2 = -2;
                         }
                         BUTON_A_3 = false;
                         if (CRAFTING(nrn1, nrn2) == 0)
                             BUTON_A_3 = true;*/

                        inventar[COMP_A]++;
                    }
                    BUTON_A_1 = true;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.F))
                {
                    if (BUTON_A_1 == false)
                    {
                        COMANDA("set_menu", "", 5, 0);
                    }
                    BUTON_A_1 = true;
                }
                else BUTON_A_1 = false;

                #endregion
            }
            else if (MENU == 20) // JOC1
            {
                #region MENIU_20

                JOC_VECTOR[0].X = MOUSE_P.X;


                for (int i = 1; i <= NR_JOC; i++)
                {
                    JOC_VECTOR[i].X += JOC_X[i];
                    JOC_VECTOR[i].Y += JOC_Y[i];

                    if (JOC_VECTOR[i].Y <= WINDOW_REZ.X / 2)
                    {
                        for (int d = 0; d < 4; d++)
                        {
                            float xx, yy;
                            int xxx, yyy;
                            xx = WINDOW_REZ.Y;
                            yy = WINDOW_REZ.X / 2;
                            xx /= 100;
                            yy /= 50;

                            xxx = (int)((JOC_VECTOR[i].X + d1[d] * 2) / xx) + 1;
                            yyy = (int)((JOC_VECTOR[i].Y + d2[d] * 2) / yy);

                            if (xxx > 0 && xxx < 100)
                                if (yyy > 0 && yyy < 50)
                                    if (JOC_MATRICE[yyy, xxx] == 1)
                                    {
                                        JOC_MATRICE[yyy, xxx] = 0;
                                        if (d2[d] != 0)
                                            JOC_X[i] *= -1;
                                        if (d1[d] != 0)
                                            JOC_Y[i] *= -1;

                                        if (ran.Next(0, 3) == 2)
                                        {
                                            NR_JOC++;
                                            if (NR_JOC >= 999)
                                            {
                                                for (int j = 1; j < NR_JOC; j++)
                                                {
                                                    JOC_X[j] = JOC_X[j + 1];
                                                    JOC_Y[j] = JOC_Y[j + 1];
                                                    JOC_VECTOR[j] = JOC_VECTOR[j + 1];
                                                    JOC_SIR[j] = JOC_SIR[j + 1];
                                                }
                                                NR_JOC--;
                                            }
                                            JOC_VECTOR[NR_JOC] = JOC_VECTOR[i];
                                            JOC_SIR[NR_JOC] = ran.Next(1, 8);

                                            JOC_Y[NR_JOC] = 5;
                                            JOC_X[NR_JOC] = 0;
                                        }
                                        break;
                                    }
                        }
                    }

                    if (JOC_VECTOR[i].X < 15 || JOC_VECTOR[i].X > WINDOW_REZ.Y - 15)
                        JOC_X[i] *= -1;
                    if (JOC_VECTOR[i].Y <= 0)
                        JOC_Y[i] *= -1;
                    else if (JOC_VECTOR[i].Y >= JOC_VECTOR[0].Y && JOC_VECTOR[i].Y <= JOC_VECTOR[0].Y + 15)
                    {
                        if (JOC_VECTOR[i].X <= JOC_VECTOR[0].X + 100 && JOC_VECTOR[i].X >= JOC_VECTOR[0].X - 100)
                        {
                            int aux = (int)(JOC_VECTOR[i].X - JOC_VECTOR[0].X);

                            if (aux > 70)
                                aux = 70;

                            JOC_Y[i] = -(100 - Math.Abs(aux)) / 20f;
                            JOC_X[i] = aux / 20f;
                        }
                    }
                    else if (JOC_VECTOR[i].Y > JOC_VECTOR[0].Y + 15)
                    {
                        for (int j = i; j <= NR_JOC; j++)
                        {
                            JOC_X[j] = JOC_X[j + 1];
                            JOC_Y[j] = JOC_Y[j + 1];
                            JOC_VECTOR[j] = JOC_VECTOR[j + 1];
                            JOC_SIR[j] = JOC_SIR[j + 1];
                        }
                        NR_JOC--;
                    }
                }

                #endregion
            }
            else if (MENU == 21)    // JOC2
            {
                #region MENIU_21

                JOC_VECTOR[0].X += (MOUSE_P.X - JOC_VECTOR[0].X) / 10;

                if (Mouse.GetState().LeftButton == ButtonState.Pressed && Mouse.GetState().RightButton == ButtonState.Pressed)
                    JOC_X[0] = 0;
                else JOC_X[0] = 1;

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (BUTON_A_1 == false)
                    {
                        BUTON_A_1 = true;
                        NR_JOC++;
                        JOC_VECTOR[NR_JOC] = JOC_VECTOR[0];
                        JOC_VECTOR[NR_JOC].X -= 10;
                        JOC_SIR[NR_JOC] = 2;
                        JOC_Y[NR_JOC] = -10f;
                    }
                }
                else if (Mouse.GetState().RightButton == ButtonState.Pressed)
                {
                    if (BUTON_A_1 == false)
                    {
                        BUTON_A_1 = true;
                        NR_JOC++;
                        JOC_VECTOR[NR_JOC] = JOC_VECTOR[0];
                        JOC_VECTOR[NR_JOC].X += 10;
                        JOC_SIR[NR_JOC] = 2;
                        JOC_Y[NR_JOC] = -10f;
                    }
                }
                else BUTON_A_1 = false;

                float xx, yy;
                xx = WINDOW_REZ.Y;
                yy = WINDOW_REZ.X;
                xx /= 100;
                yy /= 50;
                TIME = TIME % 60 + 1;
                for (int i = 1; i < 50; i++)
                    for (int j = 1; j < 100; j++)
                        if (JOC_MATRICE[i, j] < 0)
                        {
                            if ((TIME + i + j) % 30 == 0)
                            {
                                NR_JOC++;
                                JOC_VECTOR[NR_JOC] = new Vector2(j * xx, i * yy);
                                JOC_VECTOR[NR_JOC].X -= xx + 3;
                                JOC_SIR[NR_JOC] = 6;
                                JOC_Y[NR_JOC] = 10f;
                            }
                            if (TIME % 20 == 0)
                                JOC_MATRICE[i, j] = -(2 + JOC_MATRICE[i, j] + 1);
                            if (TIME % 40 == 0)
                            {
                                int x = 1 - ran.Next(0, 3);
                                if (j + x < 100)
                                {
                                    if (JOC_MATRICE[i, j + x] == 0)
                                    {
                                        JOC_MATRICE[i, j + x] = JOC_MATRICE[i, j];
                                        JOC_MATRICE[i, j] = 0;
                                    }
                                }
                                else if (j - x > 0)
                                {
                                    if (JOC_MATRICE[i, j - x] == 0)
                                    {
                                        JOC_MATRICE[i, j - x] = JOC_MATRICE[i, j];
                                        JOC_MATRICE[i, j] = 0;
                                    }
                                }
                            }
                        }
                #endregion
            }
            else if (MENU == 22)   // JOC3
            {
                #region MENIU_22

                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    JOC_VECTOR[1] = new Vector2(-3, 0);
                    JOC_VECTOR[0].X -= (JOC_VECTOR[0].X + 10) % 20 - 10;
                    JOC_VECTOR[0].Y += 10 - (JOC_VECTOR[0].Y + 10) % 20;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    JOC_VECTOR[1] = new Vector2(3, 0);
                    JOC_VECTOR[0].X -= (JOC_VECTOR[0].X + 10) % 20 - 10;
                    JOC_VECTOR[0].Y += 10 - (JOC_VECTOR[0].Y + 10) % 20;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    JOC_VECTOR[1] = new Vector2(0, 3);
                    JOC_VECTOR[0].X -= (JOC_VECTOR[0].X + 10) % 20 - 10;
                    JOC_VECTOR[0].Y += 10 - (JOC_VECTOR[0].Y + 10) % 20;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    JOC_VECTOR[1] = new Vector2(0, -3);
                    JOC_VECTOR[0].X -= (JOC_VECTOR[0].X + 10) % 20 - 10;
                    JOC_VECTOR[0].Y += 10 - (JOC_VECTOR[0].Y + 10) % 20;
                }

                JOC_VECTOR[0].X += JOC_VECTOR[1].X;
                JOC_VECTOR[0].Y += JOC_VECTOR[1].Y;

                if (JOC_MATRICE[(int)(JOC_VECTOR[0].Y + 10 + 10 * JOC_VECTOR[1].Y / 3) / 20, (int)(JOC_VECTOR[0].X + 10 + 10 * JOC_VECTOR[1].X / 3) / 20] == 1)
                {
                    JOC_VECTOR[0].X -= JOC_VECTOR[1].X;
                    JOC_VECTOR[0].Y -= JOC_VECTOR[1].Y;
                    JOC_VECTOR[0].X -= (JOC_VECTOR[0].X + 10) % 20 - 10;
                    JOC_VECTOR[0].Y += 10 - (JOC_VECTOR[0].Y + 10) % 20;
                    JOC_VECTOR[1] = Vector2.Zero;
                }
                else if (JOC_MATRICE[(int)(JOC_VECTOR[0].Y + 10) / 20, (int)(JOC_VECTOR[0].X + 10) / 20] == 2)
                {
                    JOC_MATRICE[(int)(JOC_VECTOR[0].Y + 10) / 20, (int)(JOC_VECTOR[0].X + 10) / 20] = 0;
                    JOC_SIR[0]++;
                }



                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (JOC_SIR[0] != 0 && BUTON_A_1 == false)
                    {
                        BUTON_A_1 = true;
                        JOC_SIR[0]--;
                        NR_JOC++;
                        JOC_SIR[19 + NR_JOC * 2] = -1;
                        JOC_VECTOR[19 + NR_JOC * 2] = JOC_VECTOR[0];

                        float xx, yy;
                        xx = WINDOW_REZ.Y;
                        yy = WINDOW_REZ.X;
                        xx /= 50;
                        yy /= 50;
                        float L, Lx, Ly, Rot;
                        Lx = MOUSE_P.X - (JOC_VECTOR[0].X / 20 * xx);
                        Ly = MOUSE_P.Y - (JOC_VECTOR[0].Y / 20 * yy);
                        L = (float)Math.Sqrt(Lx * Lx + Ly * Ly);

                        Rot = (float)Math.Atan(Ly / Lx) - 1.570795f;
                        if (JOC_VECTOR[0].X / 20 * xx >= MOUSE_P.X)
                            Rot += 3.1415f;
                        Rot += 3.14159f / 2;

                        JOC_VECTOR[20 + NR_JOC * 2] = new Vector2(10 * (float)Math.Cos(Rot), 10 * (float)Math.Sin(Rot));
                    }
                }
                else BUTON_A_1 = false;


                for (int i = 3; i <= 19 + NR_JOC * 2; i += 2)
                    if (JOC_SIR[i] != 0)
                    {
                        JOC_VECTOR[i].X += JOC_VECTOR[i + 1].X;
                        JOC_VECTOR[i].Y += JOC_VECTOR[i + 1].Y;

                        float x, y;

                        if (JOC_SIR[i] > 0)
                        {

                            if (JOC_MATRICE[(int)(JOC_VECTOR[i].Y + 10 + 10 * JOC_VECTOR[i + 1].Y / 3) / 20, (int)(JOC_VECTOR[i].X + 10 + 10 * JOC_VECTOR[i + 1].X / 3) / 20] == 1 || ran.Next(0, i * 3) == 2)
                            {
                                JOC_VECTOR[i].X -= JOC_VECTOR[i + 1].X;
                                JOC_VECTOR[i].Y -= JOC_VECTOR[i + 1].Y;

                                JOC_VECTOR[i].X -= (JOC_VECTOR[i].X + 10) % 20 - 10;
                                JOC_VECTOR[i].Y += 10 - (JOC_VECTOR[i].Y + 10) % 20;

                                int aux = ran.Next(0, 100);
                                if (aux % 4 == 0)
                                    JOC_VECTOR[i + 1] = new Vector2(-3, 0);
                                else if (aux % 4 == 1)
                                    JOC_VECTOR[i + 1] = new Vector2(3, 0);
                                else if (aux % 4 == 2)
                                    JOC_VECTOR[i + 1] = new Vector2(0, -3);
                                else if (aux % 4 == 3)
                                    JOC_VECTOR[i + 1] = new Vector2(0, 3);
                            }
                            x = JOC_VECTOR[i].X - JOC_VECTOR[0].X;
                            y = JOC_VECTOR[i].Y - JOC_VECTOR[0].Y;

                            if (x * x + y * y <= 300)
                            {
                                COMANDA("set_menu", "", 5, 0);
                                break;
                            }

                        }
                        else
                        {
                            int ok = 0;
                            for (int j = 3; j <= 19; j += 2)
                            {
                                x = JOC_VECTOR[i].X - JOC_VECTOR[j].X;
                                y = JOC_VECTOR[i].Y - JOC_VECTOR[j].Y;

                                if (x * x + y * y <= 300)
                                {
                                    ok = 1;
                                    JOC_SIR[j] = 0;
                                }
                            }
                            if (JOC_MATRICE[(int)(JOC_VECTOR[i].Y + 10) / 20, (int)(JOC_VECTOR[i].X + 10) / 20] == 1 || ok == 1)
                            {
                                for (int j = i; j <= 19 + NR_JOC * 2; j += 2)
                                {
                                    JOC_SIR[j] = JOC_SIR[j + 2];
                                    JOC_VECTOR[j] = JOC_VECTOR[j + 2];
                                    JOC_VECTOR[j + 1] = JOC_VECTOR[j + 3];
                                }
                                NR_JOC--;
                            }
                        }
                    }
                TIME = TIME % 120 + 1;
                #endregion
            }
            else if (MENU == 23)   // JOC4
            {
                #region MANIU_23

                for (int i = NR_JOC; i > 0; i--)
                    JOC_VECTOR[i] = JOC_VECTOR[i - 1];

                float xx, yy, zz;
                xx = MOUSE_P.X - JOC_VECTOR[0].X;
                yy = MOUSE_P.Y - JOC_VECTOR[0].Y;
                zz = (float)Math.Sqrt(xx * xx + yy * yy);

                if (zz < 3 && ran.Next(0, 100) == 2)
                    NR_JOC--;

                JOC_VECTOR[0].X += xx / zz * 5;
                JOC_VECTOR[0].Y += yy / zz * 5;

                xx = WINDOW_REZ.Y;
                yy = WINDOW_REZ.X;
                xx /= 50;
                yy /= 50;
                int x, y;
                x = (int)(JOC_VECTOR[0].X / xx + 0.5f);
                y = (int)(JOC_VECTOR[0].Y / yy + 0.5f);
                if (JOC_MATRICE[y, x] == 3)
                {
                    JOC_MATRICE[y, x] = 0;
                    if (NR_JOC < 999)
                        NR_JOC++;
                }
                else if (JOC_MATRICE[y, x] == 2)
                {
                    JOC_MATRICE[y, x] = 0;
                    if (NR_JOC < 999)
                        NR_JOC--;
                }
                else if (JOC_MATRICE[y, x] == 1)
                    COMANDA("set_menu", "", 23, 0);
                #endregion
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);



            spriteBatch.Begin();
            if (MENU == 1)
            {
                #region MENIU_1
                GraphicsDevice.Clear(new Color(5,5,15));
                for(int i=1;i<=NR_PRO;i++)
                {
                    if (i <= NR_PRO - NR_PRO % 20)
                        spriteBatch.Draw(LAS_T[3], LAS[i].poz, null, Color.White, LAS[i].pow / 3.14159265f, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);
                    else
                        spriteBatch.Draw(PLA_TEX[7], LAS[i].poz, null, Color.White, (float)LAS[i].pow / 50, new Vector2(PLA_TEX[7].Height / 2, PLA_TEX[7].Width / 2), 0.25f, SpriteEffects.None, 0f);
                }

                spriteBatch.Draw(MENIU_TEX[0, MENU], Vector2.Zero, Color.White);
                spriteBatch.Draw(MENIU_TEX[1, MENU], PL_P_E, null, Color.White, 0f, MENIU_VECTOR, ZOOM, SpriteEffects.None, 0f);
                #endregion
            }
            else if (MENU == 2)
            {
                #region MENIU_2
                GraphicsDevice.Clear(new Color(15, 15, 50));

                if (OBTIUNI[0] == 1)
                    spriteBatch.Draw(LAS_T[1], PL_P_E - MENIU_VECTOR + (new Vector2(2, 102)), null, Color.White, 0f, Vector2.Zero, ZOOM * 1.5f, SpriteEffects.None, 0f);
                if (OBTIUNI[3] == 1)
                    spriteBatch.Draw(LAS_T[1], PL_P_E - MENIU_VECTOR + (new Vector2(476, 102)), null, Color.White, 0f, Vector2.Zero, ZOOM * 1.5f, SpriteEffects.None, 0f);


                spriteBatch.Draw(LAS_T[1], PL_P_E - MENIU_VECTOR + (new Vector2(25, 350)), null, Color.White, 0f, Vector2.Zero, new Vector2(200 / (2000f / (OBTIUNI[2] / 15f)), ZOOM * 1.5f), SpriteEffects.None, 0f);
                spriteBatch.DrawString(font[3], OBTIUNI[2] + "", PL_P_E - MENIU_VECTOR + (new Vector2(25, 375)), Color.LightBlue, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);

                spriteBatch.Draw(LAS_T[1], PL_P_E - MENIU_VECTOR + (new Vector2(275, 350)), null, Color.White, 0f, Vector2.Zero, new Vector2(200 / (1500f / (OBTIUNI[1] / 15f)), ZOOM * 1.5f), SpriteEffects.None, 0f);
                spriteBatch.DrawString(font[3], OBTIUNI[1] + "", PL_P_E - MENIU_VECTOR + (new Vector2(275, 375)), Color.LightBlue, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);

                spriteBatch.Draw(MENIU_TEX[0, MENU], Vector2.Zero, Color.White);
                spriteBatch.Draw(MENIU_TEX[1, MENU], PL_P_E, null, Color.White, 0f, MENIU_VECTOR, ZOOM, SpriteEffects.None, 0f);

                #endregion
            }
            else if (MENU == 3 || MENU == 4)
            {
                #region MENIU_3_4

                #region FUNDAL 
                int xxx, yyy, x_, y_, a_, b_;
                xxx = (int)PL.poz.X;
                yyy = (int)PL.poz.Y;

                xxx = L_BACK * N_BACK + xxx % (L_BACK * N_BACK);
                yyy = L_BACK * N_BACK + yyy % (L_BACK * N_BACK);


                x_ = xxx - xxx % (L_BACK * N_BACK);
                y_ = yyy - yyy % (L_BACK * N_BACK);

                a_ = (xxx - x_) / L_BACK;
                b_ = (yyy - y_) / L_BACK;



                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                    {
                        int aa, bb;
                        aa = a_ + i;
                        bb = b_ + j;
                        if (aa < 0)
                            aa = N_BACK - 1;
                        if (bb < 0)
                            bb = N_BACK - 1;

                        if (aa >= N_BACK)
                            aa = 0;
                        if (bb >= N_BACK)
                            bb = 0;


                        spriteBatch.Draw(BACK_IMG[bb * N_BACK + aa + 1], new Vector2(PL_P_E.X - (xxx - x_) + a_ * L_BACK + i * L_BACK, PL_P_E.Y - (yyy - y_) + b_ * L_BACK + j * L_BACK), Color.White);

                    }
                #endregion

                spriteBatch.Draw(MENIU_TEX[0, MENU], Vector2.Zero, Color.White);
                spriteBatch.Draw(MENIU_TEX[1, MENU], PL_P_E, null, Color.White, 0f, MENIU_VECTOR, ZOOM, SpriteEffects.None, 0f);
                
                #region PLANETE
                for (int i = 1; i < nr_PLA_S; i++)
                {
                    Vector2 poz = PL_P_E;
                    float xx, yy, r;
                    xx = PL.poz.X - L_PLA[i].poz.X;
                    yy = PL.poz.Y - L_PLA[i].poz.Y;
                    r = (float)Math.Sqrt(xx * xx + yy * yy);
                    if (r < (WINDOW_REZ.Y * 5) / ZOOM)
                    {
                        /**if (r < 400 && Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.E))
                        {
                            if (BUTON_A_2 == false)
                            {
                                BUTON_A_2 = true;
                                if (L_PLA[i].p_orb != 0)
                                {
                                    PLA_A = i;
                                    SELECTEAZA_PLANETA();
                                    ZOOM = 1.5f;
                                    PL.rot = 0;
                                    MENU = 5;
                                    COMP_A = 0;
                                    MENIU_VECTOR = new Vector2(MENIU_TEX[1, MENU].Width / 2, MENIU_TEX[1, MENU].Height / 2);
                                    break;
                                }
                            }
                        }
                        else BUTON_A_2 = false; */
                        poz.X -= xx * ZOOM;
                        poz.Y -= yy * ZOOM;

                        float zoom = 1;
                        if (L_PLA[i].ord_elm[NR_subs + 1] == 8)
                            zoom = 7;
                        else if (L_PLA[i].ord_elm[NR_subs + 1] == 7)
                            zoom = 0.8f;
                        spriteBatch.Draw(PLA_TEX[L_PLA[i].ord_elm[NR_subs + 1]], poz, null, Color.White, 0f, new Vector2(500, 500), ZOOM * zoom, SpriteEffects.None, 1f);
                    }
                }
                #endregion

                #region PROIECTILE/LASERE
                for (int i = 0; i < NR_PRO - 1; i++)
                    if (LAS[i].tip_p != 0)
                    {
                        int xx, yy;
                        xx = (int)((-PL.poz.X + LAS[i].poz.X) * ZOOM + PL_P_E.X);
                        yy = (int)((-PL.poz.Y + LAS[i].poz.Y) * ZOOM + PL_P_E.Y);

                        spriteBatch.Draw(LAS_T[LAS[i].tip_p], new Vector2(xx, yy), null, Color.White, PL.rot, new Vector2(2, 2), ZOOM, SpriteEffects.None, 0f);
                        LAS[i].poz.X += LAS[i].fx;
                        LAS[i].poz.Y += LAS[i].fy;
                        LAS[i].t--;
                        if (LAS[i].t <= 0)
                            ELIMINARE_LAS(i);
                    }
                    else
                    {
                        NR_PRO = i;
                        ELIMINARE_LAS(i);
                    }
                #endregion

                #region AFISARE_NAVA
                for (int i = 18; i >= -18; i--)
                    for (int j = 18; j >= -18; j--)
                        if (PL.comp[i + 18, j + 18] != 0)
                        {
                            Vector2 poz = PL_P_E;
                            poz.X += ((float)(i * Math.Cos(PL.rot) - j * Math.Sin(PL.rot))) * ZOOM * 20;
                            poz.Y += ((float)(j * Math.Cos(PL.rot) + i * Math.Sin(PL.rot))) * ZOOM * 20;

                            spriteBatch.Draw(comp[PL.comp[i + 18, j + 18]].T, poz, null, Color.White, PL.rot, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);

                            if (LAS_A)
                                if (PL.eng > 0)
                                {
                                    if (comp[PL.comp[i + 18, j + 18]].proi != 0)
                                    {
                                        LAS[NR_PRO].poz = new Vector2((poz.X - PL_P_E.X) / ZOOM + PL.poz.X, (poz.Y - PL_P_E.Y) / ZOOM + PL.poz.Y);
                                        LAS[NR_PRO].tip_p = 1;
                                        LAS[NR_PRO].fx = (float)Math.Cos(PL.rot) * 40;
                                        LAS[NR_PRO].fy = (float)Math.Sin(PL.rot) * 40;
                                        LAS[NR_PRO].pow = comp[PL.comp[i + 18, j + 18]].proi;
                                        LAS[NR_PRO].t = 100;
                                        NR_PRO++;

                                        PL.eng--;
                                    }
                                }
                            if (comp[PL.comp[i + 18, j + 18]].pow != 0)
                                if (ran.Next(1, 5) == 2)
                                {
                                    LAS[NR_PRO].poz = new Vector2((poz.X - PL_P_E.X) / ZOOM + PL.poz.X + ran.Next(-15, 10), (poz.Y - PL_P_E.Y) / ZOOM + PL.poz.Y + ran.Next(-15, 10));
                                    LAS[NR_PRO].tip_p = 3;
                                    LAS[NR_PRO].fx = ran.Next(-2, 2);
                                    LAS[NR_PRO].fy = ran.Next(-2, 2);
                                    LAS[NR_PRO].pow = 1;
                                    LAS[NR_PRO].t = 10;
                                    NR_PRO++;
                                }
                        }

                #endregion

                #region INVENTAR COMPONENTE NAVA
                if (MENU == 4)
                {
                    for (int nr = 0, i = 0, j = 100; nr < NR_comp; nr++)
                        if (inventar[nr] > 0)
                        {
                            if (nr <= NR_comp)
                                spriteBatch.Draw(comp[nr].T, new Vector2(i, j), Color.White);
                            else if (nr <= NR_comp + NR_elem * NR_subs)
                                spriteBatch.Draw(PLA_T[(nr - NR_comp - 1) / NR_subs + 1, (nr - NR_comp - 1) % NR_subs], new Vector2(i, j), Color.White);
                            else
                                spriteBatch.Draw(items[nr - (NR_comp + NR_elem * NR_subs) - 1], new Vector2(i + 10, j + 10), null, Color.White, 0f, new Vector2(40, 40), 0.25f, SpriteEffects.None, 0f);
                            j += 20;
                            if (j >= WINDOW_REZ.X)
                            {
                                j = 100;
                                i += 20;
                            }
                        }
                }
                else for (float i = 0, j = 0, n = 1; n <= PL.eng; n++)
                    {
                        if (j + 50 >= WINDOW_REZ.X)
                        {
                            j = 0;
                            i += (float)(50 / (int)(PL.eng_m / (int)(WINDOW_REZ.X / 3) + 2));
                        }
                        j += 3;
                        spriteBatch.Draw(LAS_T[2], new Vector2(i, j + 50), null, Color.White);
                    }
                #endregion

                if (COMP_A < NR_comp && COMP_A > 1)
                    spriteBatch.Draw(comp[COMP_A].T, MOUSE_P, null, Color.White, PL.rot, new Vector2(-5, -5), 1f, SpriteEffects.None, 0f);

                #endregion
            }
            else if (MENU == 5)
            {
                #region MENIU_5
                spriteBatch.Draw(MENIU_TEX[0, MENU], Vector2.Zero, Color.White);
                spriteBatch.Draw(MENIU_TEX[1, MENU], PL_P_E, null, Color.White, 0f, MENIU_VECTOR, ZOOM, SpriteEffects.None, 0f);

                GraphicsDevice.Clear(new Color((int)PLA_S.SKY.X, (int)PLA_S.SKY.Y, (int)PLA_S.SKY.Z));
                if (PLA_S.MOON != 0)
                    spriteBatch.Draw(MOONS[PLA_S.MOON], new Vector2(PL_P_E.X * 0.5f, PL_P_E.Y * 1.5f), null, Color.White, (float)(PLA_S.timp) * 0.00314159f, new Vector2(1000, 1000), 1f, SpriteEffects.None, 0f);

                int x, y;
                if (OBTIUNI[3] == 0)
                {
                    #region AFISARE_PLANETA
                    x = (int)PL_P.poz.X / 20;
                    y = (int)PL_P.poz.Y / 20;

                    int MM = (int)((WINDOW_REZ.X / 40) / ZOOM) + 2;
                    int NN = (int)((WINDOW_REZ.Y / 40) / ZOOM) + 2;
                    for (int ii = (int)(-MM * 1); ii <= (int)(MM * 1); ii++)
                        if (y + ii > 0 && y + ii < 300)
                        {
                            int i = y + ii;
                            for (int jj = -NN; jj <= NN; jj++)
                                if (x + jj > 0 && x + jj < 300)
                                {
                                    int j = x + jj;
                                    if (PLA_S.a[i, j] != 0)
                                    {
                                        Vector2 poz;
                                        poz = PL_P_E;

                                        poz.X += (PL_P.poz.X - j * 20) * ZOOM;
                                        poz.Y -= (PL_P.poz.Y - i * 20) * ZOOM;
                                        if (PLA_S.b[i, j] == 0)
                                        {
                                            /*if (PLA_S.a[i, j] / 100 != 3 && PLA_S.a[i, j] / 100 != 4 && PLA_S.a[i, j] / 100 != 5 && PLA_S.a[i, j] / 100 != 10 && PLA_S.a[i, j] / 100 != 13)
                                                spriteBatch.Draw(PLA_T[PLA_S.a[i, j] / 100, 0], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);
                                            else */spriteBatch.Draw(PLA_T[PLA_S.a[i, j] / 100, PLA_S.a[i, j] % 100], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);

                                            if (PLA_S.a[i, j] / 100 != 5)
                                                spriteBatch.Draw(comp[0].T, poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);
                                        }
                                        else
                                        {
                                            spriteBatch.Draw(PLA_T[PLA_S.a[i, j] / 100, PLA_S.a[i, j] % 100], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);
                                        }
                                    }
                                    if (PLA_S.apa[i, j] / 1000 == 6)
                                    {
                                        Vector2 poz;
                                        poz = PL_P_E;

                                        poz.X += (PL_P.poz.X - j * 20) * ZOOM;
                                        poz.Y -= (PL_P.poz.Y - i * 20) * ZOOM;
                                        spriteBatch.Draw(PLA_T[PLA_S.apa[i, j] / 1000, PLA_S.apa[i, j] % 10], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);

                                    }
                                    else if (PLA_S.apa[i, j] / 1000 == 7 || PLA_S.apa[i, j] / 1000 == 12 || PLA_S.apa[i, j] / 1000 == 13)
                                    {
                                        Vector2 poz;
                                        poz = PL_P_E;

                                        poz.X += (PL_P.poz.X - j * 20) * ZOOM;
                                        poz.Y -= (PL_P.poz.Y - i * 20) * ZOOM;
                                        spriteBatch.Draw(PLA_T[PLA_S.apa[i, j] / 1000, PLA_S.apa[i, j] % 10], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);

                                    }
                                }
                        }
                    #endregion
                }
                else
                {
                    #region AFISARE_PLANETA_2
                    float xxx, yyy;
                    for (int U = 0; U < 360; U += 1)
                    {
                        xxx = PL_P.poz.Y;
                        yyy = PL_P.poz.X + 10;
                        int nrr = 0;
                        for (int nr = 1; nr < 36 / ZOOM; nr++)
                        {
                            x = (int)xxx / 20;
                            y = (int)yyy / 20;

                            if (x > 0 && x < 300 && y > 0 && y < 300)
                            {
                                Vector2 poz;
                                poz = PL_P_E;
                                if (PLA_S.a[x, y] != 0)
                                {

                                    poz.X += (PL_P.poz.X - y * 20) * ZOOM;
                                    poz.Y -= (PL_P.poz.Y - x * 20) * ZOOM;
                                    if (PLA_S.b[x, y] == 0)
                                    {
                                        /*if (PLA_S.a[x, y] / 100 != 3 && PLA_S.a[x, y] / 100 != 4 && PLA_S.a[x, y] / 100 != 5 && PLA_S.a[x, y] / 100 != 10 && PLA_S.a[x, y] / 100 != 13)
                                            spriteBatch.Draw(PLA_T[PLA_S.a[x, y] / 100, 0], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);
                                        else */spriteBatch.Draw(PLA_T[PLA_S.a[x, y] / 100, PLA_S.a[x, y] % 100], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);

                                        if (PLA_S.a[x, y] / 100 != 5)
                                            spriteBatch.Draw(comp[0].T, poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);
                                    }
                                    else
                                    {
                                        spriteBatch.Draw(PLA_T[PLA_S.a[x, y] / 100, PLA_S.a[x, y] % 100], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);
                                        nrr++;
                                        if (nrr >= ran.Next(2, 4))
                                            break;
                                    }

                                }
                                if (PLA_S.apa[x, y] / 1000 == 6)
                                {
                                    poz = PL_P_E;

                                    poz.X += (PL_P.poz.X - y * 20) * ZOOM;
                                    poz.Y -= (PL_P.poz.Y - x * 20) * ZOOM;
                                    spriteBatch.Draw(PLA_T[PLA_S.apa[x, y] / 1000, PLA_S.apa[x, y] % 10], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);

                                }
                                else if (PLA_S.apa[x, y] / 1000 == 7 || PLA_S.apa[x, y] / 1000 == 12 || PLA_S.apa[x, y] / 1000 == 13)
                                {
                                    poz = PL_P_E;

                                    poz.X += (PL_P.poz.X - y * 20) * ZOOM;
                                    poz.Y -= (PL_P.poz.Y - x * 20) * ZOOM;
                                    spriteBatch.Draw(PLA_T[PLA_S.apa[x, y] / 1000, PLA_S.apa[x, y] % 10], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);

                                }
                            }
                            else break;
                            xxx += (float)Math.Sin(3.14159 / 180 * U) * 18;
                            yyy += (float)Math.Cos(3.14159 / 180 * U) * 18;
                        }
                    }

                    #endregion
                }

                float L, Lx, Ly, Rot;
                Lx = MOUSE_P.X - PL_P_E.X;
                Ly = MOUSE_P.Y - PL_P_E.Y + 8 * ZOOM;
                L = (float)Math.Sqrt(Lx * Lx + Ly * Ly);

                Rot = (float)Math.Atan(Ly / Lx) - 1.570795f;
                if (PL_P_E.X > MOUSE_P.X)
                    Rot += 3.1415f;

                #region AFIARE_LASER
                Vector2 po;
                if (LAS_A)
                {
                    for (int i = 10; i <= PL_P.r; i += 1)
                    {
                        po = PL_P_E;
                        po.X += (i * Lx / L) * ZOOM;
                        po.Y += (i * Ly / L - 8) * ZOOM;

                        x = (int)((PL_P.poz.Y + i * Ly / L + 2) / 20);
                        y = (int)((PL_P.poz.X - i * Lx / L - 10) / 20) + 1;

                        if (x > 0 && x < 300 && y > 0 && y < 300)
                        {
                            if (PLA_S.b[x, y] != 0)
                            {
                                PLA_S.b[x, y] -= PL_P.pow;

                                if (PLA_S.b[x, y] <= 0)
                                {
                                    ELIMINARE_BLOCK(x, y);
                                    if (PLA_S.a[x, y] / 100 <= NR_elem && PLA_S.a[x, y] != 0)
                                        inventar[NR_comp + (PLA_S.a[x, y] / 100 - 1) * NR_subs + PLA_S.a[x, y] % 100 + 1]++;
                                    PLA_S.b[x, y] = 0;
                                    ELIMINARE_APA(x - 1, y);
                                    if (PLA_S.a[x, y] / 100 == 3 || PLA_S.a[x, y] / 100 == 4)
                                        PLA_S.a[x, y] = 0;
                                    if (PLA_S.a[x - 1, y] / 100 == 5)
                                    {
                                        PLA_S.b[x - 1, y] = 0;
                                        PLA_S.a[x - 1, y] = 0;
                                    }
                                }
                                break;
                            }
                            if (PLA_S.apa[x, y] / 1000 == 7)
                                ELIMINARE_APA(x, y);
                        }

                        spriteBatch.Draw(LAS_T[4], po, null, Color.White, Rot, new Vector2(2, 2), ZOOM * 0.35f, SpriteEffects.None, 0f);
                    }
                }
                #endregion

                #region PRIOECTILE
                for (int i = 0; i < NR_PRO; i++)
                {
                    int OKK = 1;
                    for (int nr = 0; nr < 5 && OKK == 1; nr++)
                    {
                        po = PL_P_E;
                        po.X += (PL_P.poz.X - LAS[i].poz.X) * ZOOM;
                        po.Y -= (PL_P.poz.Y - LAS[i].poz.Y) * ZOOM;
                        spriteBatch.Draw(LAS_T[LAS[i].tip_p], po, null, Color.White, 0f, new Vector2(1, 1), ZOOM, SpriteEffects.None, 1f);

                        y = (int)(LAS[i].poz.X + 10) / 20;
                        x = (int)(LAS[i].poz.Y + 10) / 20;

                        if (x <= 0 || x >= 300 || y <= 0 || y >= 300)
                        {
                            ELIMINARE_LAS(i);
                            OKK = 0;
                        }
                        else
                        {
                            if (PLA_S.b[x, y] > 0)
                            {
                                PLA_S.b[x, y] -= 5;
                                if (PLA_S.b[x, y] <= 0)
                                    PLA_S.b[x, y] = 0;
                                ELIMINARE_LAS(i);
                                OKK = 0;
                                i--;
                                if (i == NR_PRO - 1)
                                    break;
                            }
                            else
                            {
                                if (LAS[i].tip_p == 5)
                                {
                                    int OK = 0;
                                    for (int jj = 0; jj < PLA_S.nr_creaturi && OK == 0; jj++)
                                    {
                                        x = (int)(PLA_S.creaturi[jj].poz.Y - LAS[i].poz.X) - 10;
                                        y = (int)(PLA_S.creaturi[jj].poz.X - LAS[i].poz.Y) - 10;

                                        if (x >= -Math.Sin(Math.Abs(PLA_S.creaturi[jj].rot[4])) * 16 - 3 && x <= Math.Sin(Math.Abs(PLA_S.creaturi[jj].rot[4]) * 16 + 30))
                                            if (y >= -Math.Cos(Math.Abs(PLA_S.creaturi[jj].rot[4])) * 16 - 20 && y <= Math.Cos(Math.Abs(PLA_S.creaturi[jj].rot[4]) * 16 + 30))
                                            {
                                                OK = 1;
                                                if (PLA_S.creaturi[jj].inteligenta >= 0)
                                                {
                                                    PLA_S.creaturi[jj].viata -= LAS[i].pow;
                                                    if (PLA_S.creaturi[jj].agresiune == -1)
                                                        PLA_S.creaturi[jj].agresiune = 1;
                                                    PLA_S.creaturi[jj].fx = LAS[i].fy * 3;
                                                    if (PLA_S.creaturi[jj].viata <= 0)
                                                    {
                                                        for (int aui = jj; aui < PLA_S.nr_creaturi; aui++)
                                                            PLA_S.creaturi[aui] = PLA_S.creaturi[aui + 1];
                                                        PLA_S.nr_creaturi--;
                                                        jj--;
                                                    }
                                                }
                                            }
                                    }
                                    if (OK == 1)
                                    {
                                        ELIMINARE_LAS(i);
                                        OKK = 0;
                                        i--;
                                    }
                                    else
                                    {
                                        LAS[i].poz.X += LAS[i].fx;
                                        LAS[i].poz.Y += LAS[i].fy;
                                    }
                                }
                                else
                                {
                                    LAS[i].poz.X += LAS[i].fx;
                                    LAS[i].poz.Y += LAS[i].fy;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region AFISARE_CREATURI
                Vector2 oz;
                for (int i = 0; i < PLA_S.nr_creaturi; i++)
                    if (PLA_S.creaturi[i].viata >= 0)
                    {
                        Vector2 ox;
                        if (PLA_S.creaturi[i].inteligenta >= 0)
                        {
                            ox = PL_P_E;
                            Vector2 op, om, oc;
                            int aux = PLA_S.creaturi[i].orientare;

                            ox.X += (PL_P.poz.X - PLA_S.creaturi[i].poz.Y) * ZOOM;
                            ox.Y -= (PL_P.poz.Y - PLA_S.creaturi[i].poz.X) * ZOOM;
                            oc = ox;
                            oc.X += (float)Math.Sin(PLA_S.creaturi[i].rot[4]) * 8 * ZOOM * aux;
                            oc.Y -= (float)Math.Cos(PLA_S.creaturi[i].rot[4]) * 8 * ZOOM;
                            op = ox;
                            op.X -= (float)Math.Sin(PLA_S.creaturi[i].rot[4]) * 7 * ZOOM * aux;
                            op.Y += (float)Math.Cos(PLA_S.creaturi[i].rot[4]) * 8 * ZOOM;
                            om = ox;
                            om.X += (float)Math.Sin(PLA_S.creaturi[i].rot[4]) * 8 * ZOOM * aux;
                            om.Y -= (float)Math.Cos(PLA_S.creaturi[i].rot[4]) * 4 * ZOOM;

                            for (int j = 2; j < 4; j++)
                                if (PLA_S.creaturi[i].rot[j] >= -10)
                                    spriteBatch.Draw(PAR_C[PLA_S.creaturi[i].parti[j], 1], op, null, Color.White, PLA_S.creaturi[i].rot[j] * aux - (1f - Math.Abs(PLA_S.creaturi[i].mers) / 10), new Vector2(8, 4), ZOOM / 2, PLA_S.creaturi[i].fata, 0f);   //MAINI

                            for (int j = 5; j < 7; j++)
                                if (PLA_S.creaturi[i].rot[j] >= -10)
                                    spriteBatch.Draw(PAR_C[PLA_S.creaturi[i].parti[j], 3], om, null, Color.White, PLA_S.creaturi[i].rot[j] * aux + (1f - Math.Abs(PLA_S.creaturi[i].mers) / 10), new Vector2(8, 4), ZOOM / 2, PLA_S.creaturi[i].fata, 0f);   //PICIOARE


                            spriteBatch.Draw(PAR_C[PLA_S.creaturi[i].parti[4], 2], ox, null, Color.White, PLA_S.creaturi[i].rot[4] * aux, new Vector2(8, 16), ZOOM / 2, PLA_S.creaturi[i].fata, 0f);                                            //CORP
                            spriteBatch.Draw(PAR_C[PLA_S.creaturi[i].parti[9], 4], oc, null, Color.White, PLA_S.creaturi[i].rot[9] * aux, new Vector2(16, 16), ZOOM / 2, PLA_S.creaturi[i].fata, 0f);                                           //CAP  

                            for (int j = 0; j < 2; j++)
                                if (PLA_S.creaturi[i].rot[j] >= -10)
                                    spriteBatch.Draw(PAR_C[PLA_S.creaturi[i].parti[j], 1], op, null, Color.White, PLA_S.creaturi[i].rot[j] * aux + (1f - Math.Abs(PLA_S.creaturi[i].mers) / 10), new Vector2(8, 4), ZOOM / 2, PLA_S.creaturi[i].fata, 0f);    //MAINI


                            for (int j = 7; j < 9; j++)
                                if (PLA_S.creaturi[i].rot[j] >= -10)
                                    spriteBatch.Draw(PAR_C[PLA_S.creaturi[i].parti[j], 3], om, null, Color.White, PLA_S.creaturi[i].rot[j] * aux - (1f - Math.Abs(PLA_S.creaturi[i].mers) / 10), new Vector2(8, 4), ZOOM / 2, PLA_S.creaturi[i].fata, 0f);    //PICIOARE

                            /* oz = PL_P_E;
                             oz.Y -= (PL_P.poz.Y - PLA_S.creaturi[i].X * 20) * ZOOM;
                             oz.X += (PL_P.poz.X - PLA_S.creaturi[i].Y * 20) * ZOOM;
                             spriteBatch.Draw(comp[0].T, oz, null, Color.White, 0f, new Vector2(10, 10), 1f, PLA_S.creaturi[i].fata, 0f);

                             oz = PL_P_E;
                             oz.Y -= (PL_P.poz.Y - PL_P.X * 20) * ZOOM;
                             oz.X += (PL_P.poz.X - PL_P.Y * 20) * ZOOM;
                             spriteBatch.Draw(comp[0].T, oz, null, Color.White, 0f, new Vector2(10, 10), 1f, PLA_S.creaturi[i].fata, 0f);  */
                        }
                        else
                        {
                            ox = PL_P_E;
                            ox.X += (PL_P.poz.X - PLA_S.creaturi[i].poz.Y) * ZOOM;
                            ox.Y -= (PL_P.poz.Y - PLA_S.creaturi[i].poz.X) * ZOOM;


                            spriteBatch.Draw(PAR_C[NR_PARTI - PLA_S.creaturi[i].inteligenta, 0], ox, null, Color.White, 0f, new Vector2(40, 40), ZOOM / 1.5f, PLA_S.creaturi[i].fata, 0f);

                            /*oz = PL_P_E;
                            oz.Y -= (PL_P.poz.Y - PLA_S.creaturi[i].X * 20) * ZOOM;
                            oz.X += (PL_P.poz.X - PLA_S.creaturi[i].Y * 20) * ZOOM;
                            spriteBatch.Draw(comp[0].T, oz, null, Color.White, 0f, new Vector2(10, 10), 1f, PLA_S.creaturi[i].fata, 0f);  */
                        }
                        if (PLA_S.creaturi[i].nume != null)
                            if ((PL_P.poz.X - PLA_S.creaturi[i].poz.Y) * (PL_P.poz.X - PLA_S.creaturi[i].poz.Y)
                            + (PL_P.poz.Y - PLA_S.creaturi[i].poz.X) * (PL_P.poz.Y - PLA_S.creaturi[i].poz.X) < 5000)
                            {
                                ox = PL_P_E;
                                ox.X += (PL_P.poz.X - PLA_S.creaturi[i].poz.Y) * ZOOM;
                                ox.Y -= (PL_P.poz.Y - PLA_S.creaturi[i].poz.X) * ZOOM + (30 + 5 * (int)(PLA_S.creaturi[i].nume.Length / 30)) * ZOOM;
                                if (PLA_S.creaturi[i].nume.Length < 30)
                                    ox.X += -5 * PLA_S.creaturi[i].nume.Length * ZOOM / 3;
                                else
                                    ox.X += -150 * ZOOM / 3;

                                spriteBatch.DrawString(font[6], PLA_S.creaturi[i].nume, ox - new Vector2(ZOOM / 3, 0), Color.Blue, 0f, Vector2.Zero, 0.1f * ZOOM, SpriteEffects.None, 0f);
                                spriteBatch.DrawString(font[6], PLA_S.creaturi[i].nume, ox + new Vector2(ZOOM / 3, 0), Color.White, 0f, Vector2.Zero, 0.1f * ZOOM, SpriteEffects.None, 0f);
                                spriteBatch.DrawString(font[6], PLA_S.creaturi[i].nume, ox, Color.LightBlue, 0f, Vector2.Zero, 0.1f * ZOOM, SpriteEffects.None, 0f);

                            }
                    }
                #endregion

                oz = PL_P_E;
                oz.Y -= 8 * ZOOM;
                spriteBatch.Draw(PAR_C[0, 3], oz, null, Color.White, PL_P.rot[5] + (1f - Math.Abs(PL_P.mers) / 10), new Vector2(8, -2), ZOOM / 2, PL_P.fata, 1f);  //MANA STANGA
                oz.Y += 16 * ZOOM;
                spriteBatch.Draw(PAR_C[0, 1], oz, null, Color.White, PL_P.rot[1] - (1f - Math.Abs(PL_P.mers) / 10), new Vector2(8, 4), ZOOM / 2, PL_P.fata, 1f);  //PICIORUL STANG
                oz = PL_P_E;
                spriteBatch.Draw(PAR_C[0, 2], oz, null, Color.White, PL_P.rot[3], new Vector2(8, 16), ZOOM / 2, PL_P.fata, 1f);  //CORP
                oz.Y += 8 * ZOOM;
                spriteBatch.Draw(PAR_C[0, 1], oz, null, Color.White, PL_P.rot[2] + (1f - Math.Abs(PL_P.mers) / 10), new Vector2(8, 4), ZOOM / 2, PL_P.fata, 1f);  //PICIOR DREPT
                oz.Y -= 16 * ZOOM;
                spriteBatch.Draw(PAR_C[0, 4], oz, null, Color.White, PL_P.rot[6], new Vector2(16, 16), ZOOM / 2, PL_P.fata, 1f);  //CAP                            
                spriteBatch.Draw(PAR_C[0, 3], oz, null, Color.White, PL_P.rot[4] + Rot, new Vector2(8, 0), ZOOM / 2, PL_P.fata, 1f);  //MANA DREPT


                for (int nr = 1; nr <= PL_P.viata; nr += 3)
                    spriteBatch.Draw(LAS_T[6], new Vector2(nr / 3, 50), Color.White);
                #endregion
            }
            else if (MENU == 6)
            {
                #region MENIU_6
                spriteBatch.Draw(MENIU_TEX[0, MENU], Vector2.Zero, Color.White);
                spriteBatch.Draw(MENIU_TEX[1, MENU], PL_P_E, null, Color.White, 0f, MENIU_VECTOR, ZOOM, SpriteEffects.None, 0f);

                for (int nr = 0, i = 0, j = 100; nr < NR_comp + NR_elem * NR_subs + NR_item + 5; nr++)
                    if (inventar[nr] > 0)
                    {
                        if (nr <= NR_comp)
                            spriteBatch.Draw(comp[nr].T, new Vector2(i, j), Color.White);
                        else if (nr <= NR_comp + NR_elem * NR_subs)
                            spriteBatch.Draw(PLA_T[(nr - NR_comp - 1) / NR_subs + 1, (nr - NR_comp - 1) % NR_subs], new Vector2(i, j), Color.White);
                        else
                            spriteBatch.Draw(items[nr - (NR_comp + NR_elem * NR_subs) - 1], new Vector2(i + 10, j + 10), null, Color.White, 0f, new Vector2(40, 40), 0.25f, SpriteEffects.None, 0f);
                        j += 20;
                        if (j >= WINDOW_REZ.X)
                        {
                            j = 100;
                            i += 20;
                        }
                    }


                Vector2 poz = PL_P_E;
                poz.X -= 157;
                if (PL_P.parti[0] != 0)
                    spriteBatch.Draw(items[PL_P.parti[0] + 4], poz, null, Color.White, 0f, Vector2.Zero, 0.55f, SpriteEffects.None, 0f);

                poz.Y += 53;
                if (PL_P.parti[1] <= NR_comp)
                    spriteBatch.Draw(comp[PL_P.parti[1]].T, poz, Color.White);
                else if (PL_P.parti[1] <= NR_comp + NR_elem * NR_subs)
                    spriteBatch.Draw(PLA_T[(PL_P.parti[1] - NR_comp - 1) / NR_subs + 1, (PL_P.parti[1] - NR_comp - 1) % NR_subs], poz, null, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                else
                    spriteBatch.Draw(items[PL_P.parti[1] - (NR_comp + NR_elem * NR_subs) - 1], poz, null, Color.White, 0f, Vector2.Zero, 0.55f, SpriteEffects.None, 0f);




                if (COMP_A <= NR_comp)
                    spriteBatch.Draw(comp[COMP_A].T, MOUSE_P, Color.White);
                else if (COMP_A <= NR_comp + NR_elem * NR_subs)
                    spriteBatch.Draw(PLA_T[(COMP_A - NR_comp - 1) / NR_subs + 1, (COMP_A - NR_comp - 1) % NR_subs], MOUSE_P, Color.White);
                else
                    spriteBatch.Draw(items[COMP_A - (NR_comp + NR_elem * NR_subs) - 1], MOUSE_P, Color.White);
                #endregion
            }
            else if (MENU == 7)
            {
                #region MENIU_7        

                Vector2 poz, aux;

                int x, y;
                if (MENU_AUX == -1 || MENU_AUX >= PLA_S.nr_creaturi)
                {
                    aux.X = PL_P.poz.X;
                    aux.Y = PL_P.poz.Y;
                }
                else
                {
                    aux.Y = PLA_S.creaturi[MENU_AUX].poz.X;
                    aux.X = PLA_S.creaturi[MENU_AUX].poz.Y;
                }
                x = (int)aux.X / 20;
                y = (int)aux.Y / 20;

                int MM = (int)((WINDOW_REZ.X / 40) / ZOOM) + 2;
                int NN = (int)((WINDOW_REZ.Y / 40) / ZOOM) + 2;
                for (int ii = (int)(-MM * 1); ii <= (int)(MM * 1); ii++)
                    if (y + ii > 0 && y + ii < 300)
                    {
                        int i = y + ii;
                        for (int jj = -NN; jj <= NN; jj++)
                            if (x + jj > 0 && x + jj < 300)
                            {
                                int j = x + jj;
                                if (PLA_S.a[i, j] != 0 && (PLA_S.b[i, j] != 0 || PLA_S.a[i, j] / 100 == 5))
                                {
                                    int ok = 0;
                                    for (int d = 0; d < 8; d++)
                                        if (i + d1[d] > 0 && i + d1[d] < 300)
                                            if (j + d2[d] > 0 && j + d2[d] < 300)
                                                if (PLA_S.a[i + d1[d], j + d2[d]] == 0 || PLA_S.b[i + d1[d], j + d2[d]] == 0)
                                                    ok = 1;
                                    if (ok == 1)
                                    {
                                        poz = PL_P_E;

                                        poz.X += (aux.X - j * 20) * ZOOM;
                                        poz.Y -= (aux.Y - i * 20) * ZOOM;
                                        spriteBatch.Draw(PLA_T[PLA_S.a[i, j] / 100, PLA_S.a[i, j] % 100], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);
                                    }
                                }
                            }
                    }
                poz = PL_P_E;

                poz.Y -= (aux.Y - PL_P.poz.Y) * ZOOM;
                poz.X += (aux.X - PL_P.poz.X) * ZOOM;
                spriteBatch.Draw(PAR_C[0, 4], poz, null, Color.White, 0f, new Vector2(16, 8), ZOOM, SpriteEffects.None, 0f);

                for (int i = 0; i < PLA_S.nr_creaturi; i++)
                {
                    poz = PL_P_E;

                    poz.X += (aux.X - PLA_S.creaturi[i].poz.Y) * ZOOM;
                    poz.Y -= (aux.Y - PLA_S.creaturi[i].poz.X) * ZOOM;

                    if (PLA_S.creaturi[i].inteligenta >= 0)
                        spriteBatch.Draw(PAR_C[PLA_S.creaturi[i].parti[9], 4], poz, null, Color.White, 0f, new Vector2(16, 8), ZOOM, SpriteEffects.None, 0f);
                    else
                        spriteBatch.Draw(PAR_C[NR_PARTI - PLA_S.creaturi[i].inteligenta, 0], poz, null, Color.White, 0f, new Vector2(40, 40), ZOOM / 1.5f, SpriteEffects.None, 0f);

                }

                #endregion
            }
            else if (MENU == 8)
            {
                #region MENIU_8
                GraphicsDevice.Clear(new Color(5, 5, 15));

                int x, y;

                
                spriteBatch.Draw(PLA_TEX[L_PLA[L_PLA[PLA_A].p_orb].ord_elm[NR_subs + 1]], 
                    new Vector2(-200 + 900 * (float)Math.Sin(PLA_S.timp), -200 + 900 * (float)Math.Cos(PLA_S.timp)), 
                    null, Color.White, 0f, new Vector2(500, 500), 
                    (float)Math.Sqrt(WINDOW_REZ.X*WINDOW_REZ.X+WINDOW_REZ.Y*WINDOW_REZ.Y) / 2121.32f, SpriteEffects.None, 0f);


                if (OBTIUNI[3] == 0)
                {
                    #region AFISARE_PLANETA
                    x = (int)PL_P.poz.X / 20;
                    y = (int)PL_P.poz.Y / 20;

                    int MM = (int)((WINDOW_REZ.X / 40) / ZOOM) + 2;
                    int NN = (int)((WINDOW_REZ.Y / 40) / ZOOM) + 2;
                    for (int ii = (int)(-MM * 1); ii <= (int)(MM * 1); ii++)
                        if (y + ii > 0 && y + ii < 300)
                        {
                            int i = y + ii;
                            for (int jj = -NN; jj <= NN; jj++)
                                if (x + jj > 0 && x + jj < 300)
                                {
                                    int j = x + jj;
                                    if (PLA_S.a[i, j] != 0)
                                    {
                                        Vector2 poz;
                                        poz = PL_P_E;

                                        poz.X += (PL_P.poz.X - j * 20) * ZOOM;
                                        poz.Y -= (PL_P.poz.Y - i * 20) * ZOOM;
                                        if (PLA_S.b[i, j] == 0)
                                        {
                                            if (PLA_S.a[i, j] / 100 != 3 && PLA_S.a[i, j] / 100 != 4 && PLA_S.a[i, j] / 100 != 5 && PLA_S.a[i, j] / 100 != 10 && PLA_S.a[i, j] / 100 != 13)
                                                spriteBatch.Draw(PLA_T[PLA_S.a[i, j] / 100, 0], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);
                                            else spriteBatch.Draw(PLA_T[PLA_S.a[i, j] / 100, PLA_S.a[i, j] % 100], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);

                                            if (PLA_S.a[i, j] / 100 != 5)
                                                spriteBatch.Draw(comp[0].T, poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);
                                        }
                                        else
                                        {
                                            spriteBatch.Draw(PLA_T[PLA_S.a[i, j] / 100, PLA_S.a[i, j] % 100], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);
                                        }
                                    }
                                    if (PLA_S.apa[i, j] / 1000 == 6)
                                    {
                                        Vector2 poz;
                                        poz = PL_P_E;

                                        poz.X += (PL_P.poz.X - j * 20) * ZOOM;
                                        poz.Y -= (PL_P.poz.Y - i * 20) * ZOOM;
                                        spriteBatch.Draw(PLA_T[PLA_S.apa[i, j] / 1000, PLA_S.apa[i, j] % 10], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);

                                    }
                                    else if (PLA_S.apa[i, j] / 1000 == 7 || PLA_S.apa[i, j] / 1000 == 12 || PLA_S.apa[i, j] / 1000 == 13)
                                    {
                                        Vector2 poz;
                                        poz = PL_P_E;

                                        poz.X += (PL_P.poz.X - j * 20) * ZOOM;
                                        poz.Y -= (PL_P.poz.Y - i * 20) * ZOOM;
                                        spriteBatch.Draw(PLA_T[PLA_S.apa[i, j] / 1000, PLA_S.apa[i, j] % 10], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);

                                    }
                                }
                        }
                    #endregion
                }
                else
                {
                    #region AFISARE_PLANETA_2
                    float xxx, yyy;
                    for (int U = 0; U < 360; U += 1)
                    {
                        xxx = PL_P.poz.Y;
                        yyy = PL_P.poz.X + 10;
                        int nrr = 0;
                        for (int nr = 1; nr < 36 / ZOOM; nr++)
                        {
                            x = (int)xxx / 20;
                            y = (int)yyy / 20;

                            if (x > 0 && x < 300 && y > 0 && y < 300)
                            {
                                Vector2 poz;
                                poz = PL_P_E;
                                if (PLA_S.a[x, y] != 0)
                                {

                                    poz.X += (PL_P.poz.X - y * 20) * ZOOM;
                                    poz.Y -= (PL_P.poz.Y - x * 20) * ZOOM;
                                    if (PLA_S.b[x, y] == 0)
                                    {
                                        if (PLA_S.a[x, y] / 100 != 3 && PLA_S.a[x, y] / 100 != 4 && PLA_S.a[x, y] / 100 != 5 && PLA_S.a[x, y] / 100 != 10 && PLA_S.a[x, y] / 100 != 13)
                                            spriteBatch.Draw(PLA_T[PLA_S.a[x, y] / 100, 0], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);
                                        else spriteBatch.Draw(PLA_T[PLA_S.a[x, y] / 100, PLA_S.a[x, y] % 100], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);

                                        if (PLA_S.a[x, y] / 100 != 5)
                                            spriteBatch.Draw(comp[0].T, poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);
                                    }
                                    else
                                    {
                                        spriteBatch.Draw(PLA_T[PLA_S.a[x, y] / 100, PLA_S.a[x, y] % 100], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);
                                        nrr++;
                                        if (nrr >= ran.Next(2, 4))
                                            break;
                                    }

                                }
                                if (PLA_S.apa[x, y] / 1000 == 6)
                                {
                                    poz = PL_P_E;

                                    poz.X += (PL_P.poz.X - y * 20) * ZOOM;
                                    poz.Y -= (PL_P.poz.Y - x * 20) * ZOOM;
                                    spriteBatch.Draw(PLA_T[PLA_S.apa[x, y] / 1000, PLA_S.apa[x, y] % 10], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);

                                }
                                else if (PLA_S.apa[x, y] / 1000 == 7 || PLA_S.apa[x, y] / 1000 == 12 || PLA_S.apa[x, y] / 1000 == 13)
                                {
                                    poz = PL_P_E;

                                    poz.X += (PL_P.poz.X - y * 20) * ZOOM;
                                    poz.Y -= (PL_P.poz.Y - x * 20) * ZOOM;
                                    spriteBatch.Draw(PLA_T[PLA_S.apa[x, y] / 1000, PLA_S.apa[x, y] % 10], poz, null, Color.White, 0f, new Vector2(10, 10), ZOOM, SpriteEffects.None, 0f);

                                }
                            }
                            else break;
                            xxx += (float)Math.Sin(3.14159 / 180 * U) * 18;
                            yyy += (float)Math.Cos(3.14159 / 180 * U) * 18;
                        }
                    }

                    #endregion
                }

                for (int i = 0; i < NR_PRO; i++)
                {
                    Vector2 p = PL_P_E;
                    p.X += (PL_P.poz.X - LAS[i].poz.X) * ZOOM;
                    p.Y -= (PL_P.poz.Y - LAS[i].poz.Y) * ZOOM;

                    spriteBatch.Draw(LAS_T[8], p, null, Color.White, 3.14159265f * (float)LAS[i].pow / 180f, new Vector2(5, 0), new Vector2(ZOOM / 3, ZOOM), SpriteEffects.None, 0f);
                    LAS[i].poz += new Vector2(LAS[i].fx, LAS[i].fy);

                    LAS[i].fx += 1 - ran.Next(0, 3);
                    LAS[i].fy += 1 - ran.Next(0, 3);
                    LAS[i].t--;
                    if (LAS[i].t < 0)
                    {
                        ELIMINARE_LAS(i);
                        i--;
                    }
                }

                float L, Lx, Ly, Rot;
                Lx = MOUSE_P.X - PL_P_E.X;
                Ly = MOUSE_P.Y - PL_P_E.Y;
                L = (float)Math.Sqrt(Lx * Lx + Ly * Ly);

                Rot = (float)Math.Atan(Ly / Lx) - 1.570795f;
                if (PL_P_E.X > MOUSE_P.X)
                    Rot += 3.1415f;

                #region AFIARE_LASER
                Vector2 po;
                if (LAS_A)
                {
                    for (int i = 4; i <= 40; i += 1)
                    {
                        po = PL_P_E;
                        po.X += (i * Lx / L) * ZOOM;
                        po.Y += (i * Ly / L) * ZOOM;

                        x = (int)((PL_P.poz.Y + i * Ly / L + 10) / 20);
                        y = (int)((PL_P.poz.X - i * Lx / L - 10) / 20) + 1;

                        if (x > 0 && x < 300 && y > 0 && y < 300)
                        {
                            if (PLA_S.b[x, y] != 0)
                            {
                                PLA_S.b[x, y] -= PL_P.pow;

                                if (PLA_S.b[x, y] <= 0)
                                {
                                    ELIMINARE_BLOCK(x, y);
                                    if (PLA_S.a[x, y] / 100 <= NR_elem && PLA_S.a[x, y] != 0)
                                        inventar[NR_comp + (PLA_S.a[x, y] / 100 - 1) * NR_subs + PLA_S.a[x, y] % 100 + 1]++;
                                    PLA_S.b[x, y] = 0;
                                    ELIMINARE_APA(x - 1, y);
                                    if (PLA_S.a[x, y] / 100 == 3 || PLA_S.a[x, y] / 100 == 4)
                                        PLA_S.a[x, y] = 0;
                                    if (PLA_S.a[x - 1, y] / 100 == 5)
                                    {
                                        PLA_S.b[x - 1, y] = 0;
                                        PLA_S.a[x - 1, y] = 0;
                                    }
                                }
                                break;
                            }
                            if (PLA_S.apa[x, y] / 1000 == 7)
                                ELIMINARE_APA(x, y);
                        }

                        spriteBatch.Draw(LAS_T[4], po, null, Color.White, Rot, new Vector2(2, 2), ZOOM * 0.35f, SpriteEffects.None, 0f);
                    }
                }
                #endregion


                float fx, fy, ung;
                fx = 150 * 20 - PL_P.poz.X;
                fy = 150 * 20 - PL_P.poz.Y;
                ung = (float)Math.Atan2(fx, fy);
                spriteBatch.Draw(PAR_C[0, 4], PL_P_E, null, Color.White, ung, new Vector2(16, 8), ZOOM / 2, PL_P.fata, 0f);
                #endregion
            }
            else if (MENU == 9)
            {
                #region MENIU_9


                for (int nr = 1, i = 0, j = 100; nr < NR_comp + NR_elem * NR_subs + NR_item + 1; nr++)
                // if (inventar[nr] > 0)
                {
                    if (nr <= NR_comp)
                        spriteBatch.Draw(comp[nr].T, new Vector2(i, j), Color.White);
                    else if (nr <= NR_comp + NR_elem * NR_subs)
                        spriteBatch.Draw(PLA_T[(nr - NR_comp - 1) / NR_subs + 1, (nr - NR_comp - 1) % NR_subs], new Vector2(i, j), Color.White);
                    else
                        spriteBatch.Draw(items[nr - (NR_comp + NR_elem * NR_subs) - 1], new Vector2(i + 10, j + 10), null, Color.White, 0f, new Vector2(40, 40), 0.25f, SpriteEffects.None, 0f);
                    j += 20;
                    if (j > WINDOW_REZ.X - 20)
                    {
                        j = 100;
                        i += 20;
                    }
                }

                /*if(BUTON_A_3 == true)
                {
                    spriteBatch.Draw(LAS_T[4], PL_P_E + new Vector2(80,80), null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None, 0f);
                }   */

                spriteBatch.DrawString(font[2], inventar[COMP_A] + "", Vector2.Zero, Color.DarkBlue, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);


                if (COMP_A <= NR_comp)
                    spriteBatch.Draw(comp[COMP_A].T, PL_P_E, null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
                else if (COMP_A <= NR_comp + NR_elem * NR_subs)
                    spriteBatch.Draw(PLA_T[(COMP_A - NR_comp - 1) / NR_subs + 1, (COMP_A - NR_comp - 1) % NR_subs], PL_P_E, null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
                else if (COMP_A < NR_comp + NR_elem * NR_subs + NR_item + 1)
                    spriteBatch.Draw(items[COMP_A - (NR_comp + NR_elem * NR_subs) - 1], PL_P_E, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                #endregion
            }
            else if (MENU == 20)
            {
                #region MENIU_20
                GraphicsDevice.Clear(new Color(225 - NR_JOC / 5, 225 - NR_JOC / 5, 225 - NR_JOC / 5));
                for (int i = -10; i <= 10; i++)
                    spriteBatch.Draw(LAS_T[ran.Next(1, 8)], new Vector2(JOC_VECTOR[0].X + i * 10 - 5, JOC_VECTOR[0].Y), Color.White);

                float xx, yy;
                xx = WINDOW_REZ.Y;
                yy = WINDOW_REZ.X / 2;
                xx /= 100;
                yy /= 50;
                int ok = 0;
                for (int i = 1; i < 50; i++)
                    for (int j = 1; j < 100; j++)
                        if (JOC_MATRICE[i, j] == 1)
                        {
                            spriteBatch.Draw(LAS_T[3], new Vector2((j - 1) * xx, i * yy), Color.White);
                            ok = 1;
                        }

                for (int i = 1; i <= NR_JOC; i++)
                    spriteBatch.Draw(LAS_T[JOC_SIR[i]], JOC_VECTOR[i], Color.White);
                if (ok == 0 || NR_JOC <= 0)
                    COMANDA("set_menu", "", 5, 0);

                #endregion
            }
            else if (MENU == 21)
            {
                #region MENIU_21

                GraphicsDevice.Clear(new Color(60, 31, 146));

                #region AFISAREA_"PLACII DE JOC"
                float xx, yy;
                xx = WINDOW_REZ.Y;
                yy = WINDOW_REZ.X;
                xx /= 100;
                yy /= 50;
                int ok = 0;
                for (int i = 45; i > 0; i--)
                    for (int j = 99; j > 0; j--)
                        if (JOC_MATRICE[i, j] != 0)
                        {
                            if (JOC_MATRICE[i, j] > 0)
                                spriteBatch.Draw(PLA_T[10, 1], new Vector2((j - 1) * xx, i * yy), null, Color.White, 3.14159f / 2, new Vector2(10, 10), new Vector2(yy / 20, xx / 20), SpriteEffects.None, 0f);
                            else
                            {
                                spriteBatch.Draw(comp[13].T, new Vector2((j - 1) * xx, i * yy + 5), null, Color.White, 3.14159f / 2, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);

                                spriteBatch.Draw(comp[4].T, new Vector2((j - 1) * xx - 10, i * yy - 3), null, Color.White, -3.14159f / 2 - 0.25f, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);
                                spriteBatch.Draw(comp[4].T, new Vector2((j - 1) * xx + 10, i * yy - 3), null, Color.White, -3.14159f / 2 + 0.25f, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);

                                if (JOC_MATRICE[i, j] == -2)
                                {
                                    spriteBatch.Draw(comp[16].T, new Vector2((j - 1) * xx - 15, i * yy), null, Color.White, 3.14159f / 2 - 0.0f, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);
                                    spriteBatch.Draw(comp[16].T, new Vector2((j - 1) * xx + 15, i * yy), null, Color.White, 3.14159f / 2 + 0.0f, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);
                                }
                                else
                                {
                                    spriteBatch.Draw(comp[16].T, new Vector2((j - 1) * xx - 15, i * yy), null, Color.White, 3.14159f / 2 + 0.5f, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);
                                    spriteBatch.Draw(comp[16].T, new Vector2((j - 1) * xx + 15, i * yy), null, Color.White, 3.14159f / 2 - 0.5f, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);
                                }
                                spriteBatch.Draw(comp[3].T, new Vector2((j - 1) * xx - 10, i * yy), null, Color.White, -3.14159f / 2, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);
                                spriteBatch.Draw(comp[3].T, new Vector2((j - 1) * xx + 10, i * yy), null, Color.White, -3.14159f / 2, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);
                                ok = 1;
                            }
                        }
                #endregion

                if (JOC_X[0] == 0)
                {
                    for (int i = -50; i <= 50; i++)
                    {
                        spriteBatch.Draw(LAS_T[3], new Vector2(JOC_VECTOR[0].X + i - 10, JOC_VECTOR[0].Y + (float)Math.Sqrt(2500 - i * i) - 10), Color.White);
                        spriteBatch.Draw(LAS_T[3], new Vector2(JOC_VECTOR[0].X + i - 10, JOC_VECTOR[0].Y - (float)Math.Sqrt(2500 - i * i) - 10), Color.White);
                    }
                }

                #region AFSARE_PROIECTILE
                for (int i = 1; i <= NR_JOC; i++)
                {
                    spriteBatch.Draw(LAS_T[JOC_SIR[i]], JOC_VECTOR[i], Color.White);
                    JOC_VECTOR[i].Y += JOC_Y[i];
                    int ok_ = 1;

                    if (JOC_SIR[i] == 6)
                    {
                        float xxx, yyy;
                        xxx = JOC_VECTOR[i].X - JOC_VECTOR[0].X;
                        yyy = JOC_VECTOR[i].Y - JOC_VECTOR[0].Y;
                        if (JOC_X[0] == 0)
                        {
                            if (xxx * xxx + yyy * yyy <= 2500)
                                ok_ = 0;
                        }
                        else if (xxx * xxx + yyy * yyy <= 900)
                        {
                            ok_ = 0;
                            ok = 0;
                            break;
                        }
                    }
                    else
                    {
                        int xxx, yyy;
                        yyy = (int)(JOC_VECTOR[i].X / xx) + 1;
                        xxx = (int)(JOC_VECTOR[i].Y / yy);
                        if (xxx > 0 && xxx < 50)
                            if (yyy > 1 && yyy < 99)
                            {
                                if (JOC_MATRICE[xxx, yyy + 1] > 0)
                                {
                                    ok_ = 0;
                                    JOC_MATRICE[xxx, yyy + 1]--;
                                }
                                else if (JOC_MATRICE[xxx, yyy] < 0)
                                {
                                    ok_ = 0;
                                    JOC_MATRICE[xxx, yyy] = 0;
                                }
                                else if (JOC_MATRICE[xxx, yyy - 1] < 0)
                                {
                                    ok_ = 0;
                                    JOC_MATRICE[xxx, yyy - 1] = 0;
                                }
                                else if (JOC_MATRICE[xxx, yyy + 1] < 0)
                                {
                                    ok_ = 0;
                                    JOC_MATRICE[xxx, yyy + 1] = 0;
                                }
                            }
                    }

                    if (JOC_VECTOR[i].Y <= 0 || JOC_VECTOR[i].Y >= WINDOW_REZ.X || ok_ == 0)
                    {
                        for (int j = i; j <= NR_JOC; j++)
                        {
                            JOC_VECTOR[j] = JOC_VECTOR[j + 1];
                            JOC_Y[j] = JOC_Y[j + 1];
                            JOC_SIR[j] = JOC_SIR[j + 1];
                        }
                        NR_JOC--;
                        i--;
                    }
                }
                #endregion

                #region AFISARE_NAVA_PLAYER
                spriteBatch.Draw(comp[5].T, new Vector2(JOC_VECTOR[0].X - 10, JOC_VECTOR[0].Y - 7), null, Color.White, -3.14159f / 2, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(comp[5].T, new Vector2(JOC_VECTOR[0].X + 10, JOC_VECTOR[0].Y - 7), null, Color.White, -3.14159f / 2, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);

                spriteBatch.Draw(comp[10].T, new Vector2(JOC_VECTOR[0].X - 15, JOC_VECTOR[0].Y + 10), null, Color.White, 3.14159f - 0.5f, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(comp[10].T, new Vector2(JOC_VECTOR[0].X + 15, JOC_VECTOR[0].Y + 10), null, Color.White, 0f + 0.5f, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);

                spriteBatch.Draw(comp[11].T, new Vector2(JOC_VECTOR[0].X - 17, JOC_VECTOR[0].Y + 5), null, Color.White, 3.14159f - 1f, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(comp[11].T, new Vector2(JOC_VECTOR[0].X + 17, JOC_VECTOR[0].Y + 5), null, Color.White, 0f + 1f, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);

                spriteBatch.Draw(comp[8].T, new Vector2(JOC_VECTOR[0].X - 10, JOC_VECTOR[0].Y + 15), null, Color.White, -3.14159f / 2, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(comp[8].T, new Vector2(JOC_VECTOR[0].X + 10, JOC_VECTOR[0].Y + 15), null, Color.White, -3.14159f / 2, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);

                spriteBatch.Draw(comp[1].T, new Vector2(JOC_VECTOR[0].X, JOC_VECTOR[0].Y), null, Color.White, -3.14159f / 2, new Vector2(10, 10), 1f, SpriteEffects.None, 0f);
                #endregion

                if (ok == 0)
                    COMANDA("set_menu", "", 5, 0);

                #endregion
            }
            else if (MENU == 22)
            {
                #region MENIU_22    
                GraphicsDevice.Clear(Color.DarkGray);

                float xx, yy;
                xx = WINDOW_REZ.Y;
                yy = WINDOW_REZ.X;
                xx /= 50;
                yy /= 50;
                int ok = 0;
                for (int i = 1; i <= JOC_SIR[0]; i += 3)
                {
                    Vector2 poz = PL_P_E;
                    poz.X += i / 3;
                    spriteBatch.Draw(LAS_T[6], poz, null, Color.White, 0f, new Vector2(5, 5), Math.Min(xx / 20, yy / 20), SpriteEffects.None, 0f);

                    poz = PL_P_E;
                    poz.X -= i / 3;
                    spriteBatch.Draw(LAS_T[6], poz, null, Color.White, 0f, new Vector2(5, 5), Math.Min(xx / 20, yy / 20), SpriteEffects.FlipHorizontally, 0f);
                }
                for (int i = 50; i >= 0; i--)
                    for (int j = 50; j >= 0; j--)
                        if (JOC_MATRICE[i, j] != 0)
                        {
                            if (JOC_MATRICE[i, j] == 1)
                                spriteBatch.Draw(PLA_T[10, 1], new Vector2(j * xx, i * yy), null, Color.White, 0f, new Vector2(10, 10), new Vector2(xx / 20, yy / 20), SpriteEffects.None, 0f);
                            else if (JOC_MATRICE[i, j] == 2)
                            {
                                spriteBatch.Draw(PLA_T[10, 4], new Vector2(j * xx, i * yy), null, Color.White, 0f, new Vector2(10, 10), new Vector2(xx / 20, yy / 20), SpriteEffects.None, 0f);
                                ok = 1;
                            }
                        }


                float L, Lx, Ly, Rot;
                Lx = MOUSE_P.X - (JOC_VECTOR[0].X / 20 * xx);
                Ly = MOUSE_P.Y - (JOC_VECTOR[0].Y / 20 * yy);
                L = (float)Math.Sqrt(Lx * Lx + Ly * Ly);

                Rot = (float)Math.Atan(Ly / Lx) - 1.570795f;
                if (JOC_VECTOR[0].X / 20 * xx >= MOUSE_P.X)
                    Rot += 3.1415f;
                Rot += 3.14159f / 2;

                spriteBatch.Draw(PAR_C[0, 5], new Vector2(JOC_VECTOR[0].X / 20 * xx, JOC_VECTOR[0].Y / 20 * yy), null, Color.White, Rot, new Vector2(20, 20), new Vector2(Math.Min(xx / 40, yy / 40), Math.Min(xx / 40, yy / 40)), SpriteEffects.None, 0f);

                for (int i = 3; i <= 19 + NR_JOC * 2; i += 2)
                    if (JOC_SIR[i] > 0)
                    {
                        ok = 1;
                        Lx = -(JOC_VECTOR[0].X / 20 * xx) + (JOC_VECTOR[i].X / 20 * xx);
                        Ly = -(JOC_VECTOR[0].Y / 20 * yy) + (JOC_VECTOR[i].Y / 20 * yy);
                        L = (float)Math.Sqrt(Lx * Lx + Ly * Ly);

                        Rot = (float)Math.Atan(Ly / Lx) - 1.570795f;
                        if (JOC_VECTOR[i].X / 20 * xx < (JOC_VECTOR[0].X / 20 * xx))
                            Rot += 3.1415f;
                        Rot -= 3.14159f / 2;

                        spriteBatch.Draw(PAR_C[JOC_SIR[i], 5], new Vector2(JOC_VECTOR[i].X / 20 * xx, JOC_VECTOR[i].Y / 20 * yy), null, Color.White, Rot, new Vector2(20, 20), new Vector2(Math.Min(xx / 30, yy / 30), Math.Min(xx / 30, yy / 30)), SpriteEffects.None, 0f);
                    }
                    else if (JOC_SIR[i] < 0)
                        spriteBatch.Draw(LAS_T[1], new Vector2(JOC_VECTOR[i].X / 20 * xx, JOC_VECTOR[i].Y / 20 * yy), null, Color.White, Rot, new Vector2(10, 10), new Vector2(Math.Min(xx / 40, yy / 40), Math.Min(xx / 40, yy / 40)), SpriteEffects.None, 0f);

                if (ok == 0)
                    COMANDA("set_menu", "", 5, 0);

                #endregion
            }
            else if (MENU == 23)
            {
                #region MENIU_23   
                GraphicsDevice.Clear(Color.DarkViolet);
                float xx, yy, zz;
                xx = WINDOW_REZ.Y;
                yy = WINDOW_REZ.X;
                xx /= 50;
                yy /= 50;
                zz = Math.Min(xx, yy);
                int ok = 0;
                for (int i = 50; i >= 0; i--)
                    for (int j = 50; j >= 0; j--)
                        if (JOC_MATRICE[i, j] != 0)
                        {
                            if (JOC_MATRICE[i, j] == 1)
                                spriteBatch.Draw(PLA_T[10, 1], new Vector2(j * xx, i * yy), null, Color.White, 0f, new Vector2(10, 10), new Vector2(xx / 20, yy / 20), SpriteEffects.None, 0f);
                            else if (JOC_MATRICE[i, j] == 2)
                                spriteBatch.Draw(PLA_T[10, 4], new Vector2(j * xx, i * yy), null, Color.White, 0f, new Vector2(10, 10), new Vector2(xx / 20, yy / 20), SpriteEffects.None, 0f);
                            else if (JOC_MATRICE[i, j] == 3)
                            {
                                spriteBatch.Draw(PLA_T[10, 0], new Vector2(j * xx, i * yy), null, Color.White, 0f, new Vector2(10, 10), new Vector2(xx / 20, yy / 20), SpriteEffects.None, 0f);
                                ok = 1;
                            }
                        }

                for (int i = 0; i <= NR_JOC; i++)
                    spriteBatch.Draw(LAS_T[6], JOC_VECTOR[i], null, Color.White, 0f, new Vector2(7, 7), zz / (15 + i), SpriteEffects.None, 0f);

                if (ok == 0 || NR_JOC < 0)
                    COMANDA("set_menu", "", 5, 0);
                #endregion
            }

            for (int i = 0; i < 10; i++)
            {
                spriteBatch.DrawString(font[5], CHAT[i], new Vector2(17, 100 + 20 * i), Color.White, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font[5], CHAT[i], new Vector2(13, 100 + 20 * i), Color.Blue, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font[5], CHAT[i], new Vector2(15, 100 + 20 * i), Color.LightBlue, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(MOUSE_T, MOUSE_P, null, Color.White, PL.rot, new Vector2(40, 40), 1f, SpriteEffects.None, 0f);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

/* TOATE COMPONENTELE "RETETA"

 2. Placaj 1 - 10 fer + 5 carbon
 3. Propulsoare 1 - 10 fer + 10 carbon + 20 uraniu + 5 sulfur
 4. Generatoare 1 - 10 fer + 10 uraniu + 10 sulfir
 5. Tureta 1 - 10 fer + 10 carbon + 2 uraniu
6. Depozit 1 - 30 fer + 10 carbon
7. Placaj 2 - 1 Placaj1 + 5 fer + 10 carbon
 8. Propulsoare 2 - 3 Propulsoare1 + 10 fer + 10 carbon
 9. Generatoare 2 - 2 Generatoare1 + 5 fer + 10 carbon + 10 uraniu
 10.Tureta 2 - 1 Tureta1 + 10 fer + 5 carbon + 5 uraniu
 11.Placaj 3 - 4 Placaj2 + 10 fer + 10 sulfur + 5 carbon
 12.Generatoare 3 - 4 Generatoare2 + 5 fer + 10 uraniu
 13.Tureta 3 - 2 Tureta2 + 5 uraniu + 5 carbon + 5 fer
 14.Propulsoare 3 - 4 Propulsoare2 + 10 uraniu + 10 fer + 10 sulf

*/
/*     TOATE MENIURILE  -INTELES
 * menu 1  - Meniu principal
 * menu 2  - Meniu de optiuni
 * menu 3  - In spatiu cu nava printre planete
 * menu 4  - Crearea navei personale
 * menu 5  - Pe planeta aleasa
 * menu 6  - Meniul de inventar
 * menu 7  - HARTA
 * menu 8  - Pe asteroidul ales 
 * menu 9  - Meniu de craftare
 * menu 20 - J0c de "ping-pong" - bun pentru "hack"  
 * menu 21 - Joc copie Space Invaders
 * menu 22 - Joc copie Pac-Man     
 * menu 23 - Joc copie Snake
 * menu 24 - Joc copie Tetris
 * menu 25 - Joc copie Mario
 * menu 26 - Joc copie Donkey Kong
 * menu 27 - Joc copie Q*bert
*/
