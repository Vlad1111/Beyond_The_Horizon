using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Reluare_priectre
{
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
}
