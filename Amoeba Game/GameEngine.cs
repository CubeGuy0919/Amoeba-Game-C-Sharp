using System;

namespace Amoeba_Game
{
    public class GameEngine
    {
        public Board GameBoard { get; set; }
        public int TotalMoves { get; private set; }
        //public string[] WinRates { get; private set; }
        private string savePath = "amoebaGame_datas_save.txt";

        public void CreateNewGame(int size)
        {
            GameBoard = new Board(size);
            TotalMoves = 0;
        }

        public bool CheckWin(int rowIndex, int columnIndex, char placeSign)
        {
            int[] rowDirections = { 0, 1, 1, 1 };
            int[] columnDirections = { 1, 0, 1, -1 };

            // 4 Directions
            int newRowCoordinate, newColumnCoordinate;
            for (int i = 0; i < 4; i++)
            {
                int count = 1;
                for (int step = 1; step < 5; step++)
                {
                    newRowCoordinate = rowIndex + rowDirections[i] * step; 
                    newColumnCoordinate = columnIndex + columnDirections[i] * step;
                    if (newRowCoordinate >= 0 && newRowCoordinate < GameBoard.Size && newColumnCoordinate >= 0 && newColumnCoordinate < GameBoard.Size && GameBoard.Matrix[newRowCoordinate, newColumnCoordinate] == placeSign)
                    {
                        count++; 
                    } 
                    else break;
                }
                for (int step = 1; step < 5; step++)
                {
                    newRowCoordinate = rowIndex - rowDirections[i] * step; 
                    newColumnCoordinate = columnIndex - columnDirections[i] * step;
                    if (newRowCoordinate >= 0 && newRowCoordinate < GameBoard.Size && newColumnCoordinate >= 0 && newColumnCoordinate < GameBoard.Size && GameBoard.Matrix[newRowCoordinate, newColumnCoordinate] == placeSign)
                    {
                        count++;
                    }
                    else break;
                }
                if (count >= 5) return true;
            }
            return false;
        }

        public void IncrementMoves() => TotalMoves++;

        public void SaveGameToFile()
        {
            if (GameBoard == null) return;
            List<string> saveFileDatas = new List<string> 
            { 
                GameBoard.Size.ToString(), 
                TotalMoves.ToString() 
            };
            for (int i = 0; i < GameBoard.Size; i++)
            {
                Console.WriteLine(i);
                string row = "";
                for (int j = 0; j < GameBoard.Size; j++)
                {
                    row += GameBoard.Matrix[i, j];
                    Console.WriteLine(row);
                }
                saveFileDatas.Add(row);
            }
            File.WriteAllLines(savePath, saveFileDatas);
        }

        public bool LoadGame()
        {
            if (!File.Exists(savePath)) return false;
            string[] loadedGameLines = File.ReadAllLines(savePath);
            int tableSize = int.Parse(loadedGameLines[0]);
            TotalMoves = int.Parse(loadedGameLines[1]);
            GameBoard = new Board(tableSize);
            for (int rowIndex = 0; rowIndex < tableSize; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < tableSize; columnIndex++)
                {
                    GameBoard.PlaceSign(rowIndex, columnIndex, loadedGameLines[rowIndex + 2][columnIndex]);
                }
            }
            return true;
        }
    }
}
