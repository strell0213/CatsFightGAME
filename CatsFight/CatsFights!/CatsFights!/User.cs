using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsFights_
{
    internal class User
    {
        public int id { get; set; }
        private string nickname;
        public string Nickname { get { return nickname; } set { nickname = value; } }
        public int level { get; set; }
        public int hp { get; set; }
        public int coin { get; set; }
        public int slaughter { get; set; }

        public User() { }

        public User(string nickname, int level, int hp, int coin, int slaughter)
        {
            this.nickname = nickname;
            this.level = level;
            this.hp = hp;
            this.coin = coin;
            this.slaughter = slaughter;
        }
     }
}
