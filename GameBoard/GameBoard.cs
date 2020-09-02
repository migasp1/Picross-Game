using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

namespace Picross
{
    public class GameBoard
    {
        public readonly List<List<int>> HorizontalSpecs;
        public readonly List<List<int>> VerticalSpecs;
        public bool[,] BoardMatrix { get; set; }

        public GameBoard(List<List<int>> horizontalSpecs, List<List<int>> verticalSpecs)
        {
            HorizontalSpecs = horizontalSpecs;
            VerticalSpecs = verticalSpecs;
            CreateGameBoard(horizontalSpecs, verticalSpecs);
        }

        public void CreateGameBoard(List<List<int>> horizontalSpecs, List<List<int>> verticalSpecs)
        {
            if (horizontalSpecs.Count() != verticalSpecs.Count())
                throw new ArgumentException("Game board: Invalid Arguments");
            BoardMatrix = new bool[horizontalSpecs.Count, verticalSpecs.Count];
            Console.WriteLine("GameBoard successfully created!");
        }

        public Tuple<int, int> GetDimensions(GameBoard gameBoard)
        {
            Tuple<int, int> dimensions = new Tuple<int, int>(gameBoard.BoardMatrix.GetLength(0), gameBoard.BoardMatrix.GetLength(1));
            Console.WriteLine("(" + dimensions.Item1 + ", " + dimensions.Item2 + ")");
            return dimensions;
        }

        public List<List<List<int>>> GetSpecifications(GameBoard gameBoard)
        {

            var horizontals = AuxGetSpecifications(gameBoard.HorizontalSpecs);
            var verticals = AuxGetSpecifications(gameBoard.VerticalSpecs);
            var listOfTuples = new List<List<List<int>>>
            {
                horizontals,
                verticals
            };
            Console.WriteLine(horizontals + " " + verticals);
            return listOfTuples;
        }

        private static List<List<int>> AuxGetSpecifications(List<List<int>> specification)
        {

            Console.Write("(");
            foreach (List<int> ints in specification)
            {
                Console.Write("(");
                foreach (int i in ints)
                    Console.Write($"{i},");
                Console.Write(ints.Equals(specification.Last()) ? ")" : "), ");
            }
            Console.Write(")");
            return specification;
        }

        public int CellValue(GameBoard gameBoard, Coordinate coordinate)
        {
            if (coordinate.row > gameBoard.BoardMatrix.GetLength(0) || coordinate.column > gameBoard.BoardMatrix.GetLength(1))
                throw new ArgumentException("Coordinate: invalid");
            bool valueAtPosition = gameBoard.BoardMatrix[coordinate.row - 1, coordinate.column - 1];
            return valueAtPosition == true ? 2 : valueAtPosition == false ? 1 : 0;
        }

        public void FillCell(GameBoard gameBoard, Coordinate coordinate, int value)
        {
            if (value < 0 || value > 2)
                throw new ArgumentException("Integer Value: Invalid -> Accpetable values are 0, 1 or 2");
            if (coordinate.row > gameBoard.BoardMatrix.GetLength(0) || coordinate.column > gameBoard.BoardMatrix.GetLength(1))
                throw new ArgumentException("Coordinate: invalid");
            bool valueAtPosition = value == 2 ? true : value == 1 ? false : default;
            gameBoard.BoardMatrix[coordinate.row - 1, coordinate.column - 1] = valueAtPosition;
        }

        public static bool IsGameBoard(object gameBoard)
        {
            return gameBoard != null && gameBoard is GameBoard;
        }

        public static bool IsCorrectlyCompleted(GameBoard gb)
        {
            //if (spec.Count == 0)
            //    return list.All(val => val == false);
            List<List<int>> horizontal = gb.HorizontalSpecs;
            List<List<int>> vertical = gb.VerticalSpecs;
            //int value = spec.First();
            //(((2), (3), (2), (2,2), (2))
            //F T T F F
            //F F T T T
            //F F F T T
            //T T F T T 
            //T T F F F 
            var isInSpecH = false;
            var isInSpecV = false;
            int firstH = 0;
            int firstV = 0;
            for (int i = 0; i < gb.BoardMatrix.GetLength(0); i++)
            {
                int indexH = 0;
                int indexV = 0;
                firstH = horizontal[i].First();
                firstV = vertical[i].First();
                for (int j = 0; j < gb.BoardMatrix.GetLength(1); j++)
                {
                    if (gb.BoardMatrix[j, i])
                    {
                        isInSpecV = true;
                        firstV--;
                        if (j == gb.BoardMatrix.GetLength(1) - 1)
                            isInSpecV = false;
                    }
                    else if (isInSpecV && firstV != 0)
                        return false;
                    else if (isInSpecV && firstV == 0)
                    {
                        isInSpecV = false;
                        if (vertical[i].Count > ++indexV)
                        {
                            firstV = vertical[i][indexV];
                        }
                    }
                    if (gb.BoardMatrix[i, j])
                    {
                        isInSpecH = true;
                        firstH--;
                        if (j == gb.BoardMatrix.GetLength(1) - 1)
                            isInSpecH = false;
                    }
                    else if (isInSpecH && firstH != 0)
                        return false;
                    else if (isInSpecH && firstH == 0)
                    {
                        isInSpecH = false;
                        if (horizontal[i].Count > ++indexH)
                        {
                            firstH = horizontal[i][indexH];
                        }
                    }
                }
            }
            return firstH == 0 && firstV == 0; ;
        }


        public static bool EqualBoardGames(GameBoard gb1, GameBoard gb2)
        {
            if (!gb1.HorizontalSpecs.SelectMany(List => List).SequenceEqual(gb2.HorizontalSpecs.SelectMany(List => List)) ||
                !gb1.VerticalSpecs.SelectMany(List => List).SequenceEqual(gb2.VerticalSpecs.SelectMany(List => List)))
                return false;

            for (int i = 0; i < gb1.BoardMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < gb1.BoardMatrix.GetLength(1); j++)
                {
                    if (gb1.BoardMatrix[i, j] != gb2.BoardMatrix[i, j])
                        return false;
                }
            }
            return true;
        }

        public static void PrintGameBoard(bool[,] board, List<List<int>> horizontalSpec, List<List<int>> verticalSpec)
        {
            for (int i = verticalSpec.Max(spec => spec.Count); i > 0; i--)
            {
                foreach (var value in verticalSpec)
                    Console.Write(value.Count >= i ? $"{value[^i]} " : "  ");
                Console.WriteLine();
            }

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                    Console.Write(board[i, j] ? "X " : ". ");

                for (int j = 0; j < horizontalSpec[i].Count; j++)
                    Console.Write($"{horizontalSpec[i][j]} ");
                Console.WriteLine();
            }
        }
    }
}
