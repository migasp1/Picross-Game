using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Picross
{
    class Program
    {
        //List<List<List<int>>>
        public static List<List<List<int>>> ReadSpecificationsFrom(string textFilePath)
        {
            string text = File.ReadAllText(textFilePath);
            Console.WriteLine(text);
            var input = text;
            input = input.Replace(" ", string.Empty);
            input = input.Remove(0, 1);
            input = input.Remove(input.Length - 1);
            string vertical = input.Split('|')[1];
            string horizontal = input.Split('|')[0];
            List<List<int>> rows = GetSpecificationsHV(horizontal);
            List<List<int>> columns = GetSpecificationsHV(vertical);
            List<List<List<int>>> specifications = new List<List<List<int>>>();
            specifications.Add(rows);
            specifications.Add(columns);
            return specifications;
        }

        public static List<List<int>> GetSpecificationsHV(string spec)
        {
            var verticalSpec = new List<List<int>>();
            var input = spec;
            input = input.Replace(" ", string.Empty);
            input = input.Remove(0, 1);
            input = input.Remove(input.Length - 1);

            bool inTuple = false;
            string numberCollator = string.Empty;
            var aux = new List<int>();
            for (var j = 0; j < spec.Length; j++)
            {
                if (spec[j].Equals('(') && !inTuple)
                {
                    inTuple = true;
                    aux.Clear();
                }

                else if (spec[j].Equals(')') && inTuple)
                {
                    inTuple = false;
                    aux.Add(int.Parse(numberCollator));
                    numberCollator = string.Empty;
                    verticalSpec.Add(aux.ToList());
                    aux.Clear();
                }
                else if (spec[j].Equals(',') && inTuple)
                {
                    if (int.TryParse(numberCollator, out int integer))
                        aux.Add(integer);
                    numberCollator = string.Empty;
                }
                else if (inTuple && char.IsDigit(spec[j]))
                {
                    numberCollator += spec[j];
                }
            }
            return verticalSpec;
        }

        static void Main(string[] args)
        {      
            //    while (!GameBoard.IsCorrectlyCompleted(gb2))
            //    {
            //        Move.AskForMove();
            //        GameBoard.PrintGameBoard(, ,);
            //        if (GameBoard.IsCorrectlyCompleted())
            //        {
            //            Console.WriteLine("Congratulations, you have solved the nanogram correctly!");
            //            break;
            //        }
            //    }
            //}
            //catch (Exception ae)
            //{
            //    Console.WriteLine(ae.Message);
            //}
           
        }
    }
}
