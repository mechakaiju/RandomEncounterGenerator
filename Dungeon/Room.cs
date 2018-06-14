using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Dungeon
{
    class Room
    {
        public decimal X1 { get; set; }
        public decimal Y1 { get; set; }
        public decimal X2 { get; set; }
        public decimal Y2 { get; set; }

        public decimal Width { get; set; }
        public decimal Height { get; set; }

        public Point  Center{ get; set; }

        public Room(decimal x, decimal y, int w, int h)
        {
            X1 = x;
            X2 = x + h;
            Y1 = y;
            Y2 = y + w;
            //this.x = x * Main.TILE_WIDTH;
            //this.y = y * Main.TILE_HEIGHT;
            Width = w;
            Height = h;
            Center = new Point(Convert.ToInt32(Math.Floor((X1 + X2) / 2)), (Convert.ToInt32(Math.Floor((Y1 + Y2) / 2))));
        }
        public bool intersects(Room room)
        {
            return (X1 <= room.X2 && X2 >= room.X1 && Y1 <= room.Y2 && room.Y2 >= room.Y1);
        }
}
}
