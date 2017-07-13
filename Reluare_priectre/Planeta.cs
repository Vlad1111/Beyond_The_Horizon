using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Reluare_priectre
{
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
}
