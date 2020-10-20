using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndertaleBattleSystemPrototype.Classes
{
    class Player
    {
        public int x, y, size;

        public Player()
        {
        }

        public Player(int _x, int _y, int _size)
        {
            x = _x;
            y = _y;
            size = _size;
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
