using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class Coords
    {
        private int x;
        private int y;

        public int X { get { return x; } }

        public int Y { get { return y; } }

        public Coords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
           
            Coords other = obj as Coords;
            return x == other.x && y == other.y;
        }

        public void applyDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    x--;
                    break;
                case Direction.Right:
                    x++;
                    break;
                case Direction.Up:
                    y--;
                    break;
                case Direction.Down:
                    y++;
                    break;
                default:
                    throw new ArgumentException("Invalid direction");
            }
        }

    }


}
