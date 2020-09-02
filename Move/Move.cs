using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Picross
{

    class Move
    {
        public Coordinate coordinate;
        public int value;

        public Move(Coordinate coordinate, int value)
        {
            if (!Coordinate.IsValidCoordinate(coordinate))
                throw new ArgumentException("Invalid coordinate");

            if (value < 0 || value > 2)
                throw new ArgumentException("Integer Value: Invalid -> Accpetable values are 0, 1 or 2");
            this.coordinate = coordinate;
            this.value = value;
        }

        public static Coordinate CoordinateFromMove(Move move)
        {
            return move.coordinate;
        }

        public static int ValueFromMove(Move move)
        {
            return move.value;
        }

        public static bool IsValidMove(object obj)
        {
            return obj is Move;
        }

        public override bool Equals(object obj)
        {
            if (obj is null || !this.GetType().Equals(obj.GetType())) return false;
            var move = (Move)obj;
            return this.coordinate.row == move.coordinate.row && this.coordinate.column == move.coordinate.column && this.value == move.value;
        }

        public static void AskForMove(GameBoard gb)
        {
            var dimensions = gb.BoardMatrix.GetLength(0);
            Console.WriteLine("Enter a move: ");
            Console.WriteLine("Enter a coordinate between (1 : 1) and " + "(" + dimensions + " : " + dimensions + ")");
            try
            {
                string coordinate = Console.ReadLine();
                coordinate = coordinate.Replace(" ", "");
                int row = int.Parse(Regex.Match(coordinate, @"\d+").Value);
                int column = int.Parse(Regex.Match(coordinate, @"[:]\d+").Value.Replace(":", ""));
                Console.WriteLine("Enter a value (0, 1 or 2)");
                string value = Console.ReadLine();
                gb.FillCell(gb, new Coordinate(row, column), int.Parse(value));
            }
            catch
            {
                Console.WriteLine("Invalid move/input");
            }
        }

        public override string ToString()
        {
            return coordinate.ToString() + " --> " + value;
        }
    }
}
