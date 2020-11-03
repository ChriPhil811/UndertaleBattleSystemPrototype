using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace UndertaleBattleSystemPrototype
{
   
    class Object
    {
        public int x, y, height, width;
        public Image sprite;
        
        
        public Object(int _x, int _y, int _height, int _width, Image _sprite)
        {
            x = _x;
            y = _y;
            height = _height;
            width = _width;
            sprite = _sprite;
        }
        public void MoveLeftRight(int speed)
        {
            x += speed;
        }
    }
}
