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
        static public Game1 game;

        public Game1()
        {
            game = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        #region VARIABILE
        static public string saveDir;

        static public Texture2D[] BACK_IMG = new Texture2D[40];
        static public Texture2D MOUSE_T;
        static public Texture2D[,] MENIU_TEX;
        static public int L_BACK;
        static public int N_BACK = 6;
        static public int MENU = 3;
        static public int MENU_AUX;
        static public int[] d1 = { 0, 1, 0, -1, 1, 1, -1, -1 }, d2 = { -1, 0, 1, 0, 1, -1, 1, -1 };//SA NU SCHIMB DIRECTIILE!
        static public int[] OBTIUNI = { 0, 768, 1366, 0, 1 };
        static public int TIME = 0;
        static public Vector2 PL_P_E;
        static public Vector2 MOUSE_P;
        static public Vector2 WINDOW_REZ;
        static public Vector2 MENIU_VECTOR;
        static public bool BUTON_A_1 = false;
        static public bool BUTON_A_2 = false;
        static public bool BUTON_A_3 = false;
        static public string[] CHAT = new string[10];
        static public Random ran = new Random();

        static public SpriteFont[] font = new SpriteFont[10];
        static public int nr_FOTNTS;

        static public Nava PL = new Nava();


        static public Componenta[] comp;
        static public int NR_comp = 16;
        static public Texture2D[] items;
        static public int NR_subs = 5;
        static public int NR_elem = 22;
        static public int NR_item = 9;
        static public int[] inventar;
        static public int COMP_A = 0;


        static public int ZOOM_VAL = Mouse.GetState().ScrollWheelValue;
        static public float ZOOM = 1;


        static public Proiectil[] LAS = new Proiectil[100000];
        static public Texture2D[] LAS_T = new Texture2D[10];
        static public int NR_PRO = 0;
        static public bool LAS_A = false;

        static public Texture2D[,] PAR_C;
        static public int NR_PARTI = 4;
        static public Creatura PL_P = new Creatura();

        static public Planeta PLA_S = new Planeta();
        static public LocPlaneta[] L_PLA = new LocPlaneta[1000];
        static public Texture2D[] MOONS = new Texture2D[4];
        static public int nr_PLA_S = 1;
        static public int PLA_A = 1;
        static public Texture2D[,] PLA_T;
        static public Texture2D[] PLA_TEX = new Texture2D[10];

        static public int[,] JOC_MATRICE;
        static public int[] JOC_SIR;
        static public float[] JOC_X, JOC_Y;
        static public Vector2[] JOC_VECTOR;
        static public int NR_JOC;
        #endregion

        public void ADD_CHAT_LINE(string s)
        {
            for (int j = 9; j > 0; j--)
                CHAT[j] = CHAT[j - 1];
            CHAT[0] = s;
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
            else CREARE.SISTEM();
            PLA_S = CREARE.PLANETA_FROM_IMG(Content.Load<Texture2D>("PLANETA_2"));
            //OBTIUNI[3] = 1;
            COMANDA.cmd("set_menu", "", 5, 0);
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
                            COMANDA.cmd("set_menu", "", 2, 0);
                        else if (y > 200 && y <= 300)
                            COMANDA.cmd("set_menu", "", 3, 0);
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
                        COMANDA.cmd("set_menu", "", 1, 0);
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
                                if (L_PLA[i].p_orb != i)
                                {
                                    PLA_A = i;
                                    SELECTARE.PLANETA();
                                    if (L_PLA[PLA_A].ord_elm[NR_subs + 1] != 7)
                                        COMANDA.cmd("set_menu", "", 5, 0);
                                    else
                                        COMANDA.cmd("set_menu", "", 8, 0);
                                    break;
                                }
                            }
                        }
                        else BUTON_A_2 = false;
                    }


                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    if (BUTON_A_3 == false)
                        COMANDA.cmd("set_menu", "", 1, 0);
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
                        COMANDA.cmd("set_menu", "", 6, 0);
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.F))
                {
                    if (BUTON_A_1 == false)
                    {
                        BUTON_A_1 = true;
                        COMANDA.cmd("set_menu", "", 9, 0);
                    }
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.M))
                {
                    if (BUTON_A_1 == false)
                    {
                        BUTON_A_1 = true;
                        MENU_AUX = -1;
                        COMANDA.cmd("set_menu", "", 7, 0);
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
                    PLA_S.creaturi[i] = AI.FIINTA(PLA_S.creaturi[i]);

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
                                        COMANDA.cmd("set_menu", "", 3, 0);
                                        BUTON_A_2 = false;
                                        break;
                                    }
                                    else if (PLA_S.creaturi[i].inteligenta == -2)
                                        COMANDA.cmd("set_menu", "", 20, 0);
                                    else if (PLA_S.creaturi[i].inteligenta == -3)
                                        COMANDA.cmd("set_menu", "", 21, 0);
                                    else if (PLA_S.creaturi[i].inteligenta == -4)
                                        COMANDA.cmd("set_menu", "", 22, 0);
                                    else if (PLA_S.creaturi[i].inteligenta == -5)
                                        COMANDA.cmd("set_menu", "", 23, 0);
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
                        COMANDA.cmd("set_menu", "", 5, 0);
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
                        COMANDA.cmd("set_menu", "", 3, 0);
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
                        COMANDA.cmd("set_menu", "", 5, 0);
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
                                COMANDA.cmd("set_menu", "", 5, 0);
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
                    COMANDA.cmd("set_menu", "", 23, 0);
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
                                                        for (int aui = jj; aui < PLA_S.nr_creaturi - 1; aui++)
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
                    COMANDA.cmd("set_menu", "", 5, 0);

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
                    COMANDA.cmd("set_menu", "", 5, 0);

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
                    COMANDA.cmd("set_menu", "", 5, 0);

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
                    COMANDA.cmd("set_menu", "", 5, 0);
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
