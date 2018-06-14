using System;
using System.Collections.Generic;
using System.Text;

namespace Dungeon
{
    public class Cell
    {
        private bool visited;

        public bool Visited
        {
            get { return visited; }
            set { visited = value; }
        }
        //NorthSide, SouthSide, EastSide and WestSide
        public SideType NorthSide { get; set; }
        public SideType SouthSide { get; set; }
        public SideType EastSide { get; set; }
        public SideType WestSide { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Cell (int x, int y, bool visited, SideType northside = SideType.NorthSide, SideType southside = SideType.SouthSide, SideType eastside = SideType.EastSide, SideType westside = SideType.WestSide)
        {
            X = x;
            Y = y;
            Visited = visited;
            NorthSide = northside;
            SouthSide = southside;
            WestSide = westside;
            EastSide = eastside;
        }
    }
}
