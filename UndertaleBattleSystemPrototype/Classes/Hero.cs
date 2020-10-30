using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndertaleBattleSystemPrototype
{
    class Hero
    {

        public int x, y, size;

        public Hero(int _x, int _y, int _size)
        {
            x = _x;
            y = _y;
            size = _size;
        }

        public void MoveUpDown(int speed)
        {
            y += speed;
        }

        public void MoveRightLeft(int speed)
        {
            x += speed;
        }



        public void Move(int speed, string direction)
        {
            if (direction == "right")
            {
                x += speed;
            }
            if (direction == "left")
            {
                x -= speed;
            }
            if (direction == "up")
            {
                y -= speed;
            }
            if (direction == "down")
            {
                y += speed;
            }
        }

        public void Stop(string direction, int speed)
        {
            switch (direction)
            {
                case "up":
                    y += speed;
                    break;
                case "left":
                    x += speed;
                    break;
                case "down":
                    y -= speed;
                    break;
                case "right":
                    x -= speed;
                    break;
            }

        }
    }
}
