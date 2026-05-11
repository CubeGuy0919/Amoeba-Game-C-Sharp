namespace Amoeba_Game
{
    internal class Program
    {
        static string[] menuOptions = {
            "1.) Új Game!",
            "2.) Game Betöltése",
            "3.) Game Mentése",
            "4.) Tábla méret megadása",
            "5.) X lépés",
            "6.) O lépés",
            "7.) Statisztika",
        };
        static void Main(string[] args)
        {
            Console.Title = "Amoeba Game v1.0";
            Console.WindowHeight = 50;
            Console.WindowWidth = 150;
            bool running = true;
            int selectedIndex = 0;
            while (running)
            {
                DrawMenuEntry("Amoeba Game", selectedIndex);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex = (selectedIndex == 0) ? menuOptions.Length - 1 : selectedIndex - 1;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex = (selectedIndex == menuOptions.Length - 1) ? 0 : selectedIndex + 1;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    running = ExecuteSelection(selectedIndex);
                }
            }

            running = true;
            selectedIndex = 0;
            while (running)
            {
                DrawMenuEntry("Amoeba Game", selectedIndex);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex = (selectedIndex == 0) ? menuOptions.Length - 1 : selectedIndex - 1;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex = (selectedIndex == menuOptions.Length - 1) ? 0 : selectedIndex + 1;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    running = ExecuteSelection(selectedIndex);
                }
            }
            static void DrawMenuEntry(string title, int selectedIndex)
            {
                //Console.Clear();

                // Header
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔" + new string('═', 50) + "╗");
                Console.WriteLine("║" + title.PadLeft(25 + title.Length / 2).PadRight(50) + "║");
                Console.WriteLine("╚" + new string('═', 50) + "╝");
                Console.ResetColor();

                // Menu Items
                for (int i = 0; i < 2; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($"\t> {menuOptions[i]} <");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"\t{menuOptions[i]}");
                    }
                }

                Console.WriteLine(new string('-', 52));
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Navigáció: [Nyilak] | Kiválasztás: [Enter]");
                Console.ResetColor();
            }
            static void DrawMenuRunning(string title, int selectedIndex)
            {
                //Console.Clear();

                // Header
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("╔" + new string('═', 50) + "╗");
                Console.WriteLine("║" + title.PadLeft(25 + title.Length / 2).PadRight(50) + "║");
                Console.WriteLine("╚" + new string('═', 50) + "╝");
                Console.ResetColor();

                // Menu Items
                for (int i = 2; i < menuOptions.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($"\t> {menuOptions[i]} <");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"\t{menuOptions[i]}");
                    }
                }

                Console.WriteLine(new string('-', 52));
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("Navigáció: [Nyilak] | Kiválasztás: [Enter]");
                Console.ResetColor();
            }
            static bool ExecuteSelection(int selectedIndex)
            {
                Console.Clear();
                switch (selectedIndex)
                {
                    case 0:
                        CreateNewGame();
                        Console.ReadKey();
                        return true;
                    case 1: 
                        ReloadGame();
                        Console.ReadKey();
                        return true;
                    case 2:
                        SaveGame();
                        Console.ReadKey();
                        return true;
                    case 3:
                        SpecifyTableSize();
                        Console.ReadKey();
                        return true;
                    case 4: 
                        MoveWithX();
                        Console.ReadKey();
                        return true;
                    case 5: 
                        MoveWithO();
                        Console.ReadKey();
                        return true;
                    case 6: 
                        ShowStatistics();
                        return true;
                    default:
                        Console.WriteLine("Nincs ilyen funkció...");
                        Console.ReadKey();
                        return true;
                }
            }
        }
    }
}
