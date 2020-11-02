using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndertaleBattleSystemPrototype.Classes
{
    class Projectiles
    {
        public int x, y, width, height, speed;

        public void Projectile(int _x, int _y, int _width, int _height, int _speed)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            speed = _speed;
        }
    }
}
