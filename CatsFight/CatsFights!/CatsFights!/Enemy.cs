using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsFights_
{
    internal class Enemy
    {
        public int id { get; set; }
        private string name;
        public string Name { get { return name; } set { name = value; } }
         public int slaughterE { get; set; }
        public int hpE { get; set; }
        public int levelE { get; set; }
        public Enemy() { }

        public Enemy(string name, int slaughterE, int hpE, int levelE)
        {
            this.name = name;
            this.slaughterE = slaughterE;
            this.hpE = hpE;
            this.levelE = levelE;
        }
    }
}
