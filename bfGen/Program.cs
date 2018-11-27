using System;
using System.Linq;
using System.Collections.Generic;

namespace BFgenerator
{
    class Program
    {
        public static int progStrLen = 7;

        static void Main(string[] args)
        {
            List<BFprogram> progList = new List<BFprogram>();

            for (int i = 0; i < Convert.ToInt32(Math.Pow(BF.opcodesLen, progStrLen)); i++)
            {
                progList.Add(new BFprogram(i));
            }

            foreach ( var s in progList.Where(a=>a.IsValid()))
            {
                Console.WriteLine($"{s.ToString()}");
            }
            Console.ReadLine();
        }
    }
}