using System;
using System.Collections.Generic;
using System.Text;

namespace Picross
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Coordinate
    {
        public readonly int row;
        public readonly int column;

        public Coordinate(int row, int column)
        {
            this.row = row < 1 ? throw new ArgumentException("Row: Invalid argument") : row;
            this.column = column < 1 ? throw new ArgumentException("Column: Invalid Argument") : column;
        }

        public static bool IsValidCoordinate(object obj)
        {
            return obj is Coordinate coordinate && coordinate.row >= 1 && coordinate.column >= 1;
        }


        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Coordinate coordinate = obj as Coordinate;
                return this.row == coordinate.row && this.column == coordinate.column;
            }
        }

        public override string ToString()
        {
            return "(" + this.row + " : " + this.column + ")";
        }
    }
}
