using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace UndertaleBattleSystemPrototype.Classes
{
    class Player
    {
        public int x, y, size, hp, atk, def;
        public string name;

        public Player()
        {
        }

        public Player(int _x, int _y, int _size, int _hp, int _atk, int _def, string _name)
        {
            x = _x;
            y = _y;
            size = _size;
            hp = _hp;
            atk = _atk;
            def = _def;
            name = _name;
        }

        public void MoveUpDown(int speed)
        {
            y += speed;
        }

        public void MoveLeftRight(int speed)
        {
            x += speed;
        }
    }
}
