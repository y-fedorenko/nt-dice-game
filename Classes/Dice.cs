using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DiceGame
{
    class Dice
    {
        public int[] FaceValues { get; private set; }

        private Dice(int[] faceValues)
        {
            FaceValues = faceValues;
        }

        public static Dice Create(string inputString)
        {
            string[] values = inputString.Split(',');
            if (values.Length != 6 || !values.All(v => int.TryParse(v, out _)))
            {
                Console.WriteLine("Error: Invalid dice face values. Provide exactly 6 comma-separated numbers.");
                Environment.Exit(0);
            }
            return new Dice(values.Select(int.Parse).ToArray());
        }

        public override string ToString()
        {
            return string.Join(",", FaceValues);
        }

        public static decimal GetWinChances(Dice d1, Dice d2)
        {
            int winCases = 0;
            int totalCases = d1.FaceValues.Length * d2.FaceValues.Length;
            for (int i = 0; i < d1.FaceValues.Length; i++)
            {
                for (int j = 0; j < d2.FaceValues.Length; j++)
                {
                    if (d1.FaceValues[i] > d2.FaceValues[j])
                        winCases++;
                }
            }
            return (decimal)winCases/totalCases;
        }
    }
}
