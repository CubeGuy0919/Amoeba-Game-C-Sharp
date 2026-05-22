using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;

namespace Amoeba_Game
{
    internal class Program
    {
        static GameEngine engine = new GameEngine();
        static bool isGameRunning = false;

        static string[] startMenu = { "1.) Új Game!", "2.) Game Betöltése", "3.) Kilépés" };
        static string[] gameMenu = { "1.) Mentés", "2.) X lépés", "3.) O lépés", "4.) Statisztika", "5.) Főmenü" };

        static void Main(string[] args)
        {
            Console.Title = "Amoeba Game v1.0";
            Console.WindowHeight = 50;
            Console.WindowWidth = 150;
            int selectedIndex = 0;
            bool isProgramRunning = true;

            while (isProgramRunning)
            {
                // Choose The Selected MenuOptions List
                string[] currentOptions = isGameRunning ? gameMenu : startMenu;
                DrawInterface(currentOptions, selectedIndex);

                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow) selectedIndex = (selectedIndex == 0) ? currentOptions.Length - 1 : selectedIndex - 1;
                else if (key.Key == ConsoleKey.DownArrow) selectedIndex = (selectedIndex == currentOptions.Length - 1) ? 0 : selectedIndex + 1;
                else if (key.Key == ConsoleKey.Enter)
                {
                    if (!isGameRunning)
                        isProgramRunning = HandleGameStarting(selectedIndex);
                    else HandleGameRunning(selectedIndex);
                    selectedIndex = 0;
                }
            }
        }

        static void DrawInterface(string[] options, int selected)
        {
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔" + new string('═', 40) + "╗");
            Console.WriteLine("║" + (isGameRunning ? " JÁTÉK MÓD " : " FŐMENÜ ").PadLeft(25).PadRight(40) + "║");
            Console.WriteLine("╚" + new string('═', 40) + "╝");
            Console.ResetColor();

            if (isGameRunning) engine.GameBoard.Draw();

            for (int i = 0; i < options.Length; i++)
            {
                if (i == selected)
                {
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"\t>  {options[i]}  <");
                    Console.ResetColor();
                }
                else Console.WriteLine($"\t{options[i]}");
            }
        }

        static bool HandleGameStarting(int selectedIndex)
        {
            // Create New Game
            if (selectedIndex == 0)
            {
                Console.Write(" Tábla méret megadása (3-25): ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                if (int.TryParse(Console.ReadLine(), out int tableSize) && tableSize >= 3 && tableSize <= 25)
                {
                    engine.CreateNewGame(tableSize);
                    isGameRunning = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" A tábla mérete 3-25-ig adható meg!");
                    Console.ResetColor();
                    Console.ReadKey();
                    DisplaySpinner("Visszalépés a menübe!");
                }
            }
            // Load Game
            else if (selectedIndex == 1)
            {
                string[] previousFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt");
                if (previousFiles.Length > 0)
                {
                    string inputChoice = "";
                    do
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" Szeretné az előző mentést betölteni? (I/N): ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        inputChoice = Console.ReadLine().Trim().ToUpper();
                        Console.ResetColor();
                    }
                    while (inputChoice != "I" && inputChoice != "N");

                    if (inputChoice == "I")
                    {
                        // Sort files by last write time 
                        string latestSave = previousFiles
                            .Select(f => new FileInfo(f))
                            .OrderByDescending(f => f.LastWriteTime)
                            .First()
                            .FullName;
                        //Console.WriteLine(latestSave);
                        if (engine.LoadGame(latestSave))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($" Sikeresen betöltve a legutóbbi játék: {Path.GetFileNameWithoutExtension(latestSave)}");
                            Console.ResetColor();
                            Console.ReadKey();
                            isGameRunning = true;
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(" Elérhető mentések:");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        for (int i = 0; i < previousFiles.Length; i++)
                        {
                            Console.WriteLine($"\t{i + 1}. {Path.GetFileNameWithoutExtension(previousFiles[i])}");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(" Írja be a betöltendő fájl sorszámát: ");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        string inFileNameOrdinalNum = Console.ReadLine();
                        string filePath = "";
                        if (int.TryParse(inFileNameOrdinalNum, out int ordinalNum) && ordinalNum > 0 && ordinalNum <= previousFiles.Length)
                        {
                            filePath = previousFiles[ordinalNum - 1];

                            //Console.WriteLine(filePath);
                            if (filePath != "" && engine.LoadGame(filePath))
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(" Sikeres betöltés!");
                                Console.ResetColor();
                                Console.ReadKey();
                                isGameRunning = true;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(" Sikertelen betöltés! Érvénytelen sorszám!");
                            Console.ResetColor();
                            Console.ReadKey();
                        }
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Nem található mentett játék!");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }
            else return false;
            return true;
        }

        private static void DisplaySpinner(string message)
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write($"{message} ");
            for (int i = 0; i < 5; i++)
            {
                Console.Write(i + " ");
                Thread.Sleep(100);
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
            }
        }

        static void HandleGameRunning(int selectedIndex)
        {
            switch (selectedIndex)
            {
                case 0:
                    Console.Write(" Adja meg a mentés nevét (pl. slot1): ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    string saveName = Console.ReadLine();
                    engine.SaveGameToFile(saveName);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" Játék sikeresen mentve!");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;
                case 1:
                    MovePlayer('X');
                    Console.ReadKey();
                    break;
                case 2:
                    MovePlayer('O');
                    Console.ReadKey();
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("╔" + new string('═', 40) + "╗");
                    Console.WriteLine("║" + ("STATISZTIKÁK").PadLeft(25).PadRight(40) + "║");
                    Console.WriteLine("╚" + new string('═', 40) + "╝");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\tX győzelmek: {engine.XWins}");
                    Console.WriteLine($"\tO győzelmek: {engine.OWins}");
                    Console.WriteLine($"\tDöntetlenek: {engine.Draws}");
                    Console.WriteLine($"\tAktuális tábla: {engine.GameBoard.Size}x{engine.GameBoard.Size}");
                    Console.WriteLine($"\tAktuális lépésszám: {engine.TotalMoves}");
                    Console.ResetColor();
                    // engine.GameBoard.DisplayMatrix();
                    Console.ReadKey();
                    break;
                case 4:
                    DisplaySpinner("Visszalépés a főmenübe!");
                    isGameRunning = false;
                    Console.ReadKey();
                    break;
                default:
                    Console.WriteLine(" Nincs ilyen funkció...");
                    Console.ReadKey();
                    isGameRunning = false;
                    break;
            }
        }

        static void MovePlayer(char placeSign)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(" Pl.: 1 2");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("S O".PadLeft(9));
            Console.ResetColor();
            Console.Write($" {placeSign} jel elhelyezése (sor oszlop) koordinátára: ");
            string inputCoordinates = "";
            try
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                inputCoordinates = Console.ReadLine();
                Console.ResetColor();
                string[] tableCoordinates = inputCoordinates.Split(' ');
                int rowCoordinate = int.Parse(tableCoordinates[0]) - 1;
                int columnCoordinate = int.Parse(tableCoordinates[1]) - 1;
                if (engine.GameBoard.PlaceSign(rowCoordinate, columnCoordinate, placeSign))
                {
                    engine.IncrementMoves();
                    if (engine.CheckWin(rowCoordinate, columnCoordinate, placeSign))
                    {
                        DrawInterface(gameMenu, 0);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"\n\tNYERTÉL: ");
                        Console.ForegroundColor = placeSign == 'X' ? ConsoleColor.Red : ConsoleColor.Yellow;
                        Console.Write(placeSign + "!");
                        engine.RecordWin(placeSign);
                        isGameRunning = false;
                        Console.ReadKey();
                    }
                    else if (engine.CheckDraw())
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("\n DÖNTETLEN! A tábla megtelt.");
                        Console.ResetColor();
                        engine.RecordWin('-');
                        isGameRunning = false;
                        Console.ReadKey();
                    }
                }
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Nem megfelelő formátumú koordináta megadás!");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($" --> {inputCoordinates} <--");
                Console.ResetColor();
                DisplaySpinner("Visszalépés a menübe!");
            }
        }
    }
}