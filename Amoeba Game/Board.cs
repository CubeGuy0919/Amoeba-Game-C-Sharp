using System;

namespace Amoeba_Game
{
    public class Board
    {
        public char[,] Matrix { get; private set; }
        public int Size { get; private set; }

        public Board(int size)
        {
            Size = size;
            Matrix = new char[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++) Matrix[i, j] = '.';
        }

        public void Draw()
        {
            Console.Write("\t" + new string(' ', 3));
            // Horizontal (ROW) Label
            for (int i = 1; i <= Size; i++)
            {
                Console.Write(i.ToString().PadRight(2));
            }
            Console.WriteLine("\n\t" + new string('-', Size * 2 + 3));

            // Vertical (COLUMN) Label
            for (int rowIndex = 1; rowIndex <= Size; rowIndex++)
            {
                Console.Write("\t" + rowIndex.ToString().PadRight(2) + "|");
                rowIndex--;
                for (int columnIndex = 0; columnIndex < Size; columnIndex++)
                {
                    if (Matrix[rowIndex, columnIndex] == 'X')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (Matrix[rowIndex, columnIndex] == 'O')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.Write(Matrix[rowIndex, columnIndex] + " ");
                    Console.ResetColor();
                }
                rowIndex++;
                Console.WriteLine("\n");
            }
        }

        public bool PlaceSign(int rowIndex, int columnIndex, char placeSign)
        {
            if (rowIndex >= 0 && rowIndex < Size && columnIndex >= 0 && columnIndex < Size && Matrix[rowIndex, columnIndex] == '.')
            {
                Matrix[rowIndex, columnIndex] = placeSign;
                return true;
            }
            return false;
        }
    }
}
