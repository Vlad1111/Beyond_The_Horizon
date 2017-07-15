using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Reluare_priectre
{
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
        public int max_viata;
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
}
