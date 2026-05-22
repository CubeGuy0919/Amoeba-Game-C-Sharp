using System;

namespace Amoeba_Game
{
    public class GameEngine
    {
        public Board GameBoard { get; set; }
        public int TotalMoves { get; private set; }
        //public string[] WinRates { get; private set; }
        //private string savePath = "amoebaGame_datas_save.txt";

        private string winnerStatsPath = "amoeba_stats.sav";
        public int XWins { get; private set; }
        public int OWins { get; private set; }
        public int Draws { get; private set; }

        public GameEngine()
        {
            LoadStats();
        }

        private void LoadStats()
        {
            if (File.Exists(winnerStatsPath))
            {
                string[] lines = File.ReadAllLines(winnerStatsPath);
                if (lines.Length >= 2)
                {
                    XWins = int.Parse(lines[0]);
                    OWins = int.Parse(lines[1]);
                    Draws = int.Parse(lines[2]);
                }
            }
        }

        public void RecordWin(char winner)
        {
            if (winner == 'X') XWins++;
            else if (winner == 'O') OWins++;
            else Draws++;

                File.WriteAllLines(winnerStatsPath, new string[]
                {
                XWins.ToString(),
                OWins.ToString(),
                Draws.ToString()
                });
        }

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
                if (count >= 3) return true;
            }
            return false;
        }

        public void IncrementMoves() => TotalMoves++;

        //public void SaveGameToFile()
        //{
        //    if (GameBoard == null) return;
        //    List<string> saveFileDatas = new List<string> 
        //    { 
        //        GameBoard.Size.ToString(), 
        //        TotalMoves.ToString() 
        //    };
        //    for (int i = 0; i < GameBoard.Size; i++)
        //    {
        //        Console.WriteLine(i);
        //        string row = "";
        //        for (int j = 0; j < GameBoard.Size; j++)
        //        {
        //            row += GameBoard.Matrix[i, j];
        //            Console.WriteLine(row);
        //        }
        //        saveFileDatas.Add(row);
        //    }
        //    File.WriteAllLines(savePath, saveFileDatas);
        //}

        public void SaveGameToFile(string fileName)
        {
            if (GameBoard == null) return;
            List<string> saveFileDatas = new List<string> 
            { 
                GameBoard.Size.ToString(), 
                TotalMoves.ToString() 
            };
            for (int i = 0; i < GameBoard.Size; i++)
            {
                string row = "";
                for (int j = 0; j < GameBoard.Size; j++) row += GameBoard.Matrix[i, j];
                saveFileDatas.Add(row);
            }
            File.WriteAllLines(fileName + ".txt", saveFileDatas);
        }

        public bool LoadGame(string fileName)
        {
            string filePath = fileName;
            if (!File.Exists(filePath)) return false;

            string[] loadedGameLines = File.ReadAllLines(filePath);
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
        public bool CheckDraw()
        {
            return TotalMoves >= GameBoard.Size * GameBoard.Size;
        }

    }
}
