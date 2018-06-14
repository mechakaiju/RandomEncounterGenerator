using System;
using System.Collections;

namespace Dungeon
{
    public class MapGenerator
    {
        public int mapRows = 20;
        public int mapColumns = 40;

        public char[,] map;

        public string boxCharacters;
        private string[] boxCharacterUpFriends;
        private string[] boxCharacterDownFriends;
        private string[] boxCharacterLeftFriends;
        private string[] boxCharacterRightFriends;

        // Use this for initialization
        void Awake()
        {
            InitializeBoxCharacters();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void DisplayMap()
        {
            string output = "";
            for (int r = 0; r < mapRows; r++)
            {
                for (int c = 0; c < mapColumns; c++)
                {
                    output += map[r, c];
                }
                output += "\n";
            }
            Console.Write(output);
        }

        public void InitializeMap()
        {
            InitializeBoxCharacters();
            Random rnd = new Random();
            map = new char[mapRows, mapColumns];

            // Put 'X's in top and bottom rows.
            for (int c = 0; c < mapColumns; c++)
            {
                map[0, c] = 'X';
                map[mapRows - 1, c] = 'X';
            }

            // Put 'X's in the left and right columns.
            for (int r = 0; r < mapRows; r++)
            {
                map[r, 0] = 'X';
                map[r, mapColumns - 1] = 'X';
            }

            // Set 'O' for the other map spaces (which means 'free').
            for (int r = 1; r < mapRows - 1; r++)
            {
                for (int c = 1; c < mapColumns - 1; c++)
                {
                    map[r, c] = 'O';
                }
            }

            int mapSeed = System.DateTime.Now.Millisecond;
            rnd.Next(0, mapSeed);

            //Console.Write("Current seed = " + mapSeed);

            CreateRoom();

            for (int r = 1; r < mapRows - 1; r++)
            {
                for (int c = 1; c < mapColumns - 1; c++)
                {

                    if (map[r, c] == '@' || map[r, c] == '˄' || map[r, c] == '˂' || map[r, c] == '˃' || map[r, c] == '˅' ||
                        map[r, c] == '╔' || map[r, c] == '╧' || map[r, c] == '╤' || map[r, c] == '╗' || map[r, c] == '╟' || map[r, c] == '╢' || map[r, c] == '╚' || map[r, c] == '╝')
                    {
                        continue;
                    }

                    string validCharacters = GetValidBoxCharacters(r, c);
                    map[r, c] = validCharacters[rnd.Next(0, validCharacters.Length)];
                }
            }
        }

        private void CreateRoom()
        {
            Random rnd = new Random();
            int startRow = rnd.Next(2, mapRows - 7);
            int startColumn = rnd.Next(2, mapColumns - 7);
            // ╔ ═ ╗ ║ ╚ ╝
            map[startRow, startColumn] = '╔';
            map[startRow, startColumn + 1] = '╤';
            map[startRow, startColumn + 2] = '╤';
            map[startRow, startColumn + 3] = '╤';
            map[startRow, startColumn + 4] = '╗';

            map[startRow + 1, startColumn] = '╟';
            map[startRow + 1, startColumn + 1] = '@';
            map[startRow + 1, startColumn + 2] = '@';
            map[startRow + 1, startColumn + 3] = '@';
            map[startRow + 1, startColumn + 4] = '╢';

            map[startRow + 2, startColumn] = '˂';
            map[startRow + 2, startColumn + 1] = '@';
            map[startRow + 2, startColumn + 2] = '@';
            map[startRow + 2, startColumn + 3] = '@';
            map[startRow + 2, startColumn + 4] = '˃';

            map[startRow + 3, startColumn] = '╟';
            map[startRow + 3, startColumn + 1] = '@';
            map[startRow + 3, startColumn + 2] = '@';
            map[startRow + 3, startColumn + 3] = '@';
            map[startRow + 3, startColumn + 4] = '╢';

            map[startRow + 4, startColumn] = '╚';
            map[startRow + 4, startColumn + 1] = '╧';
            map[startRow + 4, startColumn + 2] = '╧';
            map[startRow + 4, startColumn + 3] = '╧';
            map[startRow + 4, startColumn + 4] = '╝';

        }


        private string GetValidBoxCharacters(int row, int column)
        {
            string validCharacters = "";

            for (int i = 0; i < boxCharacters.Length; i++)
            {
                char ch = boxCharacters[i];

                if (
                    boxCharacterLeftFriends[i].Contains(map[row, column - 1].ToString()) &&
                    boxCharacterRightFriends[i].Contains(map[row, column + 1].ToString()) &&
                    boxCharacterUpFriends[i].Contains(map[row - 1, column].ToString()) &&
                    boxCharacterDownFriends[i].Contains(map[row + 1, column].ToString()))
                {
                    validCharacters += ch.ToString();
                }
            }

            if (validCharacters.Length == 0)
            {
                validCharacters = "O";
            }

            return validCharacters;
        }

        public bool[,] TraverseMap()
        {
            bool[,] visitedCells = new bool[mapRows, mapColumns];
            int currentRow = 1;
            int currentColumn = 1;

            // This will start the recursive loop, updating the visitedCells array.
            TraverseCells(visitedCells, currentRow, currentColumn);

            return visitedCells;
        }


        private void TraverseCells(bool[,] visitedCells, int row, int column)
        {
            if (visitedCells[row, column])
            {
                return;
            }

            visitedCells[row, column] = true;

            switch (map[row, column])
            {
                case '┌':
                case '╔':
                    TraverseCells(visitedCells, row, column + 1);
                    TraverseCells(visitedCells, row + 1, column);
                    break;
                case '┐':
                case '╗':
                    TraverseCells(visitedCells, row + 1, column);
                    TraverseCells(visitedCells, row, column - 1);
                    break;
                case '─':
                case '˂':
                case '˃':
                    TraverseCells(visitedCells, row, column - 1);
                    TraverseCells(visitedCells, row, column + 1);
                    break;
                case '│':
                case '˄':
                case '˅':
                    TraverseCells(visitedCells, row - 1, column);
                    TraverseCells(visitedCells, row + 1, column);
                    break;
                case '└':
                case '╚':
                    TraverseCells(visitedCells, row, column + 1);
                    TraverseCells(visitedCells, row - 1, column);
                    break;
                case '┘':
                case '╝':
                    TraverseCells(visitedCells, row - 1, column);
                    TraverseCells(visitedCells, row, column - 1);
                    break;
                case '├':
                case '╟':
                    TraverseCells(visitedCells, row - 1, column);
                    TraverseCells(visitedCells, row + 1, column);
                    TraverseCells(visitedCells, row, column + 1);
                    break;
                case '┤':
                case '╢':
                    TraverseCells(visitedCells, row - 1, column);
                    TraverseCells(visitedCells, row + 1, column);
                    TraverseCells(visitedCells, row, column - 1);
                    break;
                case '┬':
                case '╤':
                    TraverseCells(visitedCells, row, column - 1);
                    TraverseCells(visitedCells, row, column + 1);
                    TraverseCells(visitedCells, row + 1, column);
                    break;
                case '┴':
                case '╧':
                    TraverseCells(visitedCells, row, column - 1);
                    TraverseCells(visitedCells, row, column + 1);
                    TraverseCells(visitedCells, row - 1, column);
                    break;
                case '┼':
                case '@':
                    TraverseCells(visitedCells, row, column - 1);
                    TraverseCells(visitedCells, row, column + 1);
                    TraverseCells(visitedCells, row - 1, column);
                    TraverseCells(visitedCells, row + 1, column);
                    break;
                case 'O':
                    return; // This is one of those pesky dead-ends!
                default:
                    //Console.Write("No idea how we got here (" + row + "," + column + ") '" + map[row, column]);
                    return;
            }
        }

        private void InitializeBoxCharacters()
        {
            boxCharacters = "─│┌┐└┘├┤┬┴┼";
            boxCharacterUpFriends = new string[boxCharacters.Length];
            boxCharacterDownFriends = new string[boxCharacters.Length];
            boxCharacterLeftFriends = new string[boxCharacters.Length];
            boxCharacterRightFriends = new string[boxCharacters.Length];

            // ˂˃˅˄
            // ╟╢╤╧╔╗╚╝
            boxCharacterLeftFriends[0] = "O─┌└├┬┴┼"; //    ─
            boxCharacterLeftFriends[1] = "O│┐┘┤X╟╢╤╧╔╗╚╝"; //     │
            boxCharacterLeftFriends[2] = "O│┐┘┤X╟╢╤╧╔╗╚╝"; //     ┌
            boxCharacterLeftFriends[3] = "O─┌└├┬┴┼"; //    ┐
            boxCharacterLeftFriends[4] = "O│┐┘┤X╟╢╤╧╔╗╚╝"; //     └
            boxCharacterLeftFriends[5] = "O─┌└├┬┴┼"; //    ┘
            boxCharacterLeftFriends[6] = "O│┐┘┤X╟╢╤╧╔╗╚╝"; //      ├
            boxCharacterLeftFriends[7] = "O─┌└├┬┴┼˃"; //   ┤
            boxCharacterLeftFriends[8] = "O─┌└├┬┴┼˃"; //    ┬
            boxCharacterLeftFriends[9] = "O─┌└├┬┴┼˃"; //    ┴
            boxCharacterLeftFriends[10] = "O─┌└├┬┴┼˃"; //   ┼

            boxCharacterRightFriends[0] = "O─┐┘┤┬┴┼"; //    ─
            boxCharacterRightFriends[1] = "O│┌└├X╟╢╤╧╔╗╚╝"; //     │
            boxCharacterRightFriends[2] = "O─┐┘┤┬┴┼"; //   ┌
            boxCharacterRightFriends[3] = "O│┌└├X╟╢╤╧╔╗╚╝"; //      ┐
            boxCharacterRightFriends[4] = "O─┐┘┤┬┴┼"; //   └
            boxCharacterRightFriends[5] = "O│┌└├X╟╢╤╧╔╗╚╝"; //      ┘
            boxCharacterRightFriends[6] = "O─┐┘┤┬┴┼˂"; //   ├
            boxCharacterRightFriends[7] = "O│┌└├X╟╢╤╧╔╗╚╝"; //      ┤
            boxCharacterRightFriends[8] = "O─┐┘┤┬┴┼˂"; //    ┬
            boxCharacterRightFriends[9] = "O─┐┘┤┬┴┼˂"; //    ┴
            boxCharacterRightFriends[10] = "O─┐┘┤┬┴┼˂"; //   ┼

            boxCharacterUpFriends[0] = "O─└┘┴X╟╢╤╧╔╗╚╝"; //       ─
            boxCharacterUpFriends[1] = "O│┌┐├┤┬┼"; //      │
            boxCharacterUpFriends[2] = "O─└┘┴X╟╢╤╧╔╗╚╝"; //       ┌
            boxCharacterUpFriends[3] = "O─└┘┴X╟╢╤╧╔╗╚╝"; //       ┐
            boxCharacterUpFriends[4] = "O│┌┐├┤┬┼"; //     └
            boxCharacterUpFriends[5] = "O│┌┐├┤┬┼"; //     ┘
            boxCharacterUpFriends[6] = "O│┌┐├┤┬┼˅"; //      ├
            boxCharacterUpFriends[7] = "O│┌┐├┤┬┼˅"; //      ┤
            boxCharacterUpFriends[8] = "O─└┘┴X╟╢╤╧╔╗╚╝"; //        ┬
            boxCharacterUpFriends[9] = "O│┌┐├┤┬┼˅"; //     ┴
            boxCharacterUpFriends[10] = "O│┌┐├┤┬┼˅"; //     ┼

            boxCharacterDownFriends[0] = "O─┌┐┬X╟╢╤╧╔╗╚╝"; //       ─
            boxCharacterDownFriends[1] = "O│└┘├┤┴┼"; //      │
            boxCharacterDownFriends[2] = "O│└┘├┤┴┼"; //     ┌
            boxCharacterDownFriends[3] = "O│└┘├┤┴┼"; //     ┐
            boxCharacterDownFriends[4] = "O─┌┐┬X╟╢╤╧╔╗╚╝"; //        └
            boxCharacterDownFriends[5] = "O─┌┐┬X╟╢╤╧╔╗╚╝"; //        ┘
            boxCharacterDownFriends[6] = "O│└┘├┤┴┼˄"; //      ├
            boxCharacterDownFriends[7] = "O│└┘├┤┴┼˄"; //      ┤
            boxCharacterDownFriends[8] = "O│└┘├┤┴┼˄"; //     ┬
            boxCharacterDownFriends[9] = "O─┌┐┬X╟╢╤╧╔╗╚╝"; //        ┴
            boxCharacterDownFriends[10] = "O│└┘├┤┴┼˄"; //     ┼
        }

    }
}
