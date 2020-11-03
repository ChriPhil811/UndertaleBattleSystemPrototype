using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace UndertaleBattleSystemPrototype.Classes
{
    class Projectile
    {
        public int x, y, width, height;
        public Image image;

        public Projectile()
        {
        }

        public Projectile(int _x, int _y, int _width, int _height, Image _image)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            image = _image;
        }

        public void HornAttack(int speed)
        {
            y -= speed;
        }
    }
}
