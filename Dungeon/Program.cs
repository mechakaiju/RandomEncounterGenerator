using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Dungeon
{
    //https://gamedevelopment.tutsplus.com/tutorials/create-a-procedurally-generated-dungeon-cave-system--gamedev-10099

    class Program
    {
        private static int X = 0;
        private static int Y = 0;
        private static List<Room> rooms = new List<Room>();
        private static string[,] array2Da;
        private Random rnd = new Random();


        private static int Seed = System.Environment.TickCount;
        private static int NRows = 199; //must be an odd number
        private static int NCols = 199; //must be an odd number
        private static string DungeonLayout = "None"; //Box, Cross, None
        private static int RoomMin = 10; //room minum size
        private static int RoomMax = 40; //room maximum size
        private static string RoomLayout = "Packed"; //Packed, Scattered
        private static string CorridorLayout = "Straight"; //Straight, Labyrinth, Bent
        private static int RemoveDeadends = 100; //percentage
        private static int AddStairs = 0;  //number of stairs
        private static int TileResolution = 12; //pixels
        private static MapGenerator mapGenerator;

        static void Main()
        {
            Console.WriteLine("Hello World!");

            X = 100; //height
            Y = 100; //width
            //cleanDungeon();
            //placeRooms();
            //drawDungeon();
            
            dungeon dun = GenerateDungeon();
            //PrintMap(dun);

            //GenerateDungeon2();


            Map map = new Map(10, 10);
            map.MarkCellsUnvisited();
            map.PickRandomCellAndMarkItVisited();
            map.Generate();


        }
        private static void GenerateDungeon2()
        {
            mapGenerator = new MapGenerator();
            
            int minimumMazePercentage = 1;
            bool[,] visitedCells; 

            int visitedCellCount = 0;

            int minimumMazeCells = Convert.ToInt32((mapGenerator.mapRows - 2) * (mapGenerator.mapColumns - 2) * minimumMazePercentage);

            while (visitedCellCount < minimumMazeCells)
            {
                //Console.Write("Current dungeon size = " + visitedCellCount + " which is less than the required " + minimumMazeCells + ". Retrying");
                mapGenerator.InitializeMap();
                visitedCells = mapGenerator.TraverseMap();
                visitedCellCount = GetVisitedCellsCount(visitedCells);
                //Console.Write("visited cell count = " + visitedCellCount);
            }
            mapGenerator.DisplayMap();
        }
        private static int GetVisitedCellsCount(bool[,] visitedCells)
        {
            int visitedCellsCount = 0;

            for (int r = 1; r < mapGenerator.mapRows - 1; r++)
            {
                for (int c = 1; c < mapGenerator.mapColumns - 1; c++)
                {
                    if (visitedCells[r, c])
                    {
                        visitedCellsCount++;
                    }
                }
            }

            return visitedCellsCount;
        }
        private static string[,] cleanupEdges(string[,] array2Da)
        {
            //accross the top and bottom
            for (int x = 0; x <NCols; x++)
            {
                //across top
                array2Da[0, x] = "0";
                //across the bottom
                array2Da[NRows - 1, x] = "0";
            }
            for (int x = 0; x < NRows; x++)
            {
                //across top
                array2Da[0, x] = "0";
                //down left side
                array2Da[x, 0] = "0";
                //down right side
                array2Da[x, NRows - 1] = "0";
                //across the bottom
                array2Da[NRows - 1, x] = "0";
            }


            return array2Da;
        }
        public static void PrintMap(dungeon d)
        {
            //dungeon.ARCH
            //dungeon.BLOCKED
            //dungeon.BLOCK_CORR
            //dungeon.BLOCK_DOOR
            //dungeon.BLOCK_ROOM
            //dungeon.CORRIDOR
            //dungeon.DOOR
            //dungeon.DOORSPACE
            //dungeon.ENTRANCE
            //dungeon.LOCKED
            //dungeon.OPENSPACE
            //dungeon.PERIMETER
            //dungeon.ROOM
            //dungeon.SECRET
            //dungeon.STAIRS
            //dungeon.TRAPPED

            string map = string.Empty;
            for (int r = 0; r <= d.n_rows; r++)
            {
                if (map.Length > 0)
                    map += "\n";
                for (int c = 0; c <= d.n_cols; c++)
                {
                    if ((d.cell[r][c] & dungeon.PERIMETER) != dungeon.NOTHING)
                        map += "0";
                    else if ((d.cell[r][c] & dungeon.DOOR) != dungeon.NOTHING)
                        map += "-";// map += "-";
                    else if ((d.cell[r][c] & dungeon.ROOM) != dungeon.NOTHING)
                        map += " ";// map += "#";
                    else if ((d.cell[r][c] & dungeon.CORRIDOR) != dungeon.NOTHING)
                        map += " ";// map += "#";
                    else
                        map += "0";
                }
            }
            Console.Write(map);
        }
        public static dungeon GenerateDungeon()
        {
            DungeonOptions opts = new DungeonOptions()
            {
                Seed = Seed,
                NRows = NRows,
                NCols = NCols,
                DungeonLayout = DungeonLayout,
                RoomMin = RoomMin,
                RoomMax = RoomMax,
                RoomLayout = RoomLayout,
                CorridorLayout = CorridorLayout,
                RemoveDeadends = RemoveDeadends,
                AddStairs = AddStairs
            };
            return new dungeon(opts);
        }
        private static int[] bubbleSort(int[] a)
        {
            int t;
            for (int j = 0; j <= a.Length - 2; j++)
            {
                for (int i = 0; i <= a.Length - 2; i++)
                {
                    if (a[i] > a[i + 1])
                    {
                        t = a[i + 1];
                        a[i + 1] = a[i];
                        a[i] = t;
                    }
                }
            }
            return a;
        }
        private static void hCorridor(int x1, int x2, int y)
        {
            for (int x = Convert.ToInt32(x1); x <= Convert.ToInt32(x2) - 1; x++)
            {
                array2Da[x, y] = ".";
            }
        }
        private static void vCorridor(int y1, int y2, int x)
        {
            for (int y = Convert.ToInt32(y1); y <= Convert.ToInt32(y2) - 1; y++)
            {
                array2Da[x, y] = ".";
            }
        }
        private static void cleanDungeon()
        {
            array2Da = new string[X, Y];
            for (int x = 0; x < X - 1; x++)
            {
                for (int y = 0; y < Y - 1; y++)
                {
                    array2Da[x, y] = "0";
                }
            }

        }
        private static void drawDungeon()
        {
            int r = 0;
            foreach(Room room in rooms)
            {
                array2Da[room.Center.X, room.Center.Y] = "c";
                array2Da[Convert.ToInt32(room.X1), Convert.ToInt32(room.Y1)] = "1";
                array2Da[Convert.ToInt32(room.X2), Convert.ToInt32(room.Y2)] = "2";
                for (int x = Convert.ToInt32(room.X1); x <= Convert.ToInt32(room.X2); x++)
                {
                    for (int y = Convert.ToInt32(room.Y1); y <= Convert.ToInt32(room.Y2); y++)
                    {
                        array2Da[x,y] = ".";
                    }
                }
                r++;
            }
            for (int x = 0; x < X; x++)
            {
                for (int y = 0; y < Y; y++)
                {
                    Console.Write(array2Da[x,y]);
                }
                Console.WriteLine();
            }
        }
        private static void placeRooms()
        {
            
            Point prevCenter;
            Point newCenter;
            int maxRoomSize = 10;
            int minRoomSize = 5;
            Random rnd = new Random();
            for (int i = 0; i < 100; i++)
            {
                var w = minRoomSize + rnd.Next(maxRoomSize - minRoomSize + 1);
                var h = minRoomSize + rnd.Next(maxRoomSize - minRoomSize + 1);
                var x = rnd.Next(Y - w - 1) + 1;
                var y = rnd.Next(X - h - 1) + 1;
                Room newRoom = new Room(x, y, w, h);

                var failed = false;
                foreach (Room otherRoom in rooms)
                {

                    if (newRoom.intersects(otherRoom))
                    {
                        failed = true;
                        break;
                    }
                }
                if (!failed && (newRoom.Y2 < Y && newRoom.X2 < X))
                {
                    newCenter = newRoom.Center;
                    if (prevCenter.IsEmpty == false)
                    {
                        if (rnd.Next(2) == 1)
                        {
                            int[] a = { prevCenter.X, newCenter.X };
                            a = bubbleSort(a);
                            hCorridor(a[0], a[1], prevCenter.Y);

                            int[] b = { prevCenter.Y, newCenter.Y };
                            b = bubbleSort(b);
                            vCorridor(b[0], b[1], newCenter.X);
                        }
                        else
                        {
                            int[] c = { prevCenter.Y, newCenter.Y };
                            c = bubbleSort(c);
                            vCorridor(c[0], c[1], prevCenter.X);

                            int[] d = { prevCenter.X, newCenter.X };
                            d = bubbleSort(d);
                            hCorridor(d[0], d[1], newCenter.Y);
                        }
                    }                    // local function to carve out new room
                    rooms.Add(newRoom);
                    prevCenter = newRoom.Center;
                }
            }
        }
    }
}
