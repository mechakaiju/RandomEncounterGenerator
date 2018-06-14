using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
//https://dirkkok.wordpress.com/2007/11/21/generating-random-dungeons-part-1/

namespace Dungeon
{
    public class Map
    {
        private readonly bool[,] cells;
        private static Cell[,] xrcmap;
        public bool MustChangeDirection(int changeDirectionModifier)
        {
            // changeDirectionModifier of 100 will always change direction
            // value of 0 will never change direction
            Random rnd = new Random();
            return ((directionsPicked.Count > 0) || (changeDirectionModifier > rnd.Next(0, 99);
        }
        public Map(int width, int height)
        {
            xrcmap = new Cell[width, height];
            cells = new bool[width, height];
        }

        public void MarkCellsUnvisited()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    cells[x, y] = false;
        }
        public bool this[int x, int y]
        {
            set { cells[x, y] = true; }
            get { return cells[x, y]; }
        }

        public int Width
        {
            get { return cells.GetUpperBound(0) + 1; }
        }
        public Cell[,] XRCMap
        {
            get { return xrcmap; }
        }
        public int Height
        {
            get { return cells.GetUpperBound(1) + 1; }
        }
        public Map Generate(int width, int height, int changeDirectionModifier)
        {
            Map map = new Map(width, height);
            map.MarkCellsUnvisited();
            Point currentLocation = map.PickRandomCellAndMarkItVisited();
            DirectionType previousDirection = DirectionType.North;

            while (!map.AllCellsVisited)
            {
                DirectionPicker directionPicker = new DirectionPicker(previousDirection, changeDirectionModifier);
                DirectionType direction = directionPicker.GetNextDirection();

                while (!map.HasAdjacentCellInDirection(currentLocation, direction) || map.AdjacentCellInDirectionIsVisited(currentLocation, direction))
                {
                    if (directionPicker.HasNextDirection)
                        direction = directionPicker.GetNextDirection();
                    else
                    {
                        currentLocation = map.GetRandomVisitedCell(currentLocation); // Get a new previously visited location
                        directionPicker = new DirectionPicker(previousDirection, changeDirectionModifier); // Reset the direction picker
                        direction = directionPicker.GetNextDirection(); // Get a new direction
                    }
                }

                currentLocation = map.CreateCorridor(currentLocation, direction);
                map.FlagCellAsVisited(currentLocation);
                previousDirection = direction;
            }

            return map;
        }
        public Point PickRandomCellAndMarkItVisited()
        {
            Random rnd = new Random();
            Point randomLocation = new Point(rnd.Next(Width - 1), rnd.Next(Height - 1));
            this[randomLocation.X,randomLocation.Y] = true;
            return randomLocation;
        }
        public bool HasAdjacentCellInDirection(Point location, DirectionType direction)
        {
            // Check that the location falls within the bounds of the map
            if ((location.X < 0) || (location.X >= Width) || (location.Y < 0) || (location.Y >= Height)) return false;

            // Check if there is an adjacent cell in the direction
            switch (direction)
            {
                case DirectionType.North:
                    return location.Y > 0;
                case DirectionType.South:
                    return location.Y < (Height - 1);
                case DirectionType.West:
                    return location.X > 0;
                case DirectionType.East:
                    return location.X < (Width - 1);
                default:
                    return false;
            }
        }
        public bool AdjacentCellInDirectionIsVisited(Point location, DirectionType direction)
        {
            if (!HasAdjacentCellInDirection(location, direction)) throw new InvalidOperationException("No adjacent cell exists for the location and direction provided.");

            switch (direction)
            {
                case DirectionType.North:
                    return this[location.X, location.Y - 1];
                case DirectionType.West:
                    return this[location.X - 1, location.Y];
                case DirectionType.South:
                    return this[location.X, location.Y + 1];
                case DirectionType.East:
                    return this[location.X + 1, location.Y];
                default:
                    throw new InvalidOperationException();
            }
        }
        private readonly List<Point> visitedCells = new List<Point>();

        public void FlagCellAsVisited(Point location)
        {
            //bool pos = cells[location.X, location.Y];

            if (LocationIsOutsideBounds(location)) throw new ArgumentException("Location is outside of Map bounds”, “location");
            //if (cells[location.X, location.Y]) throw new ArgumentException("Location is already visited", "location");

            this[location.X, location.Y] = true;
            visitedCells.Add(location);
        }

        private bool LocationIsOutsideBounds(Point location)
        {
            return ((location.X < 0) || (location.X >= Width) || (location.Y < 0) || (location.Y >= Height));
        }
        public Point GetRandomVisitedCell(Point location)
        {
            Random rnd = new Random();
            if (visitedCells.Count == 0) throw new InvalidOperationException("There are no visited cells to return.");

            int index = rnd.Next(visitedCells.Count - 1);

            // Loop while the current cell is the visited cell
            while (visitedCells[index] == location)
                index = rnd.Next(visitedCells.Count - 1);

            return visitedCells[index];
        }
        public Point CreateCorridor(Point location, DirectionType direction)
        {
            Point target = GetTargetLocation(location, direction);

            if (!HasAdjacentCellInDirection(location, direction)) throw new InvalidOperationException("No adjacent cell exists for the location and direction provided.") ;


            bool b = this[location.X, location.Y];
            Cell cell = new Cell(location.X, location.Y, b, SideType.NorthSide, SideType.SouthSide, SideType.EastSide, SideType.WestSide);
            xrcmap[location.X, location.Y] = cell;
            xrcmap[location.X, location.Y - 1] = cell;
            xrcmap[location.X, location.Y + 1] = cell;
            switch (direction)
            {
                case DirectionType.North:
                    //cells[location.X, location.Y];
                    //location.NorthSide = SideType.Empty;
                    //this[location.X, location.Y - 1].SouthSide = SideType.Empty;
                    xrcmap[location.X, location.Y].NorthSide = SideType.Empty;
                    xrcmap[location.X, location.Y - 1].SouthSide = SideType.Empty;
                    break;
                case DirectionType.South:

                    //this[location].SouthSide = SideType.Empty;
                    //this[location.X, location.Y + 1].NorthSide = SideType.Empty;
                    xrcmap[location.X, location.Y].SouthSide = SideType.Empty;
                    xrcmap[location.X, location.Y + 1].NorthSide = SideType.Empty;
                    break;
                case DirectionType.West:
                    //this[location].WestSide = SideType.Empty;
                    //this[location.X - 1, location.Y].EastSide = SideType.Empty;
                    xrcmap[location.X, location.Y].WestSide = SideType.Empty;
                    xrcmap[location.X, location.Y - 1].EastSide = SideType.Empty;
                    break;
                case DirectionType.East:
                    //this[location].EastSide = SideType.Empty;
                    //this[location.X + 1, location.Y].WestSide = SideType.Empty;
                    xrcmap[location.X, location.Y].EastSide = SideType.Empty;
                    xrcmap[location.X, location.Y + 1].WestSide = SideType.Empty;
                    break;
            }
            return target;
        }
        private Point GetTargetLocation(Point location, DirectionType direction)
        {
            if (!HasAdjacentCellInDirection(location, direction)) throw new InvalidOperationException("No adjacent cell exists for the location and direction provided.") ;

            switch (direction)
            {
                case DirectionType.North:
                    return new Point(location.X, location.Y - 1);
                case DirectionType.West:
                    return new Point(location.X - 1, location.Y);
                case DirectionType.South:
                    return new Point(location.X, location.Y + 1);
                case DirectionType.East:
                    return new Point(location.X + 1, location.Y);
                default:
                    throw new InvalidOperationException();
            }
        }
        public bool AllCellsVisited
        {
            get { return visitedCells.Count == (Width * Height); }
        }
        public DirectionType GetNextDirection()
        {
            if (!HasNextDirection) throw new InvalidOperationException("No directions available");

            DirectionType directionPicked;

            do
            {
                directionPicked = MustChangeDirection ? PickDifferentDirection() : previousDirection;
            } while (directionsPicked.Contains(directionPicked));

            directionsPicked.Add(directionPicked);

            return directionPicked;
        }

        private DirectionType PickDifferentDirection()
        {
            DirectionType directionPicked;
            do
            {
                directionPicked = (DirectionType)Random.Instance.Next(3);
            } while ((directionPicked == previousDirection) && (directionsPicked.Count < 3));

            return directionPicked;
        }
    }
}
