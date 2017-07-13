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
    }
}
