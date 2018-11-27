using System;
using System.Collections.Generic;
using System.Text;

namespace BFgenerator
{
    static class BF
    {
        public static char plus = '+';
        public static char minus = '-';
        public static char left = '<';
        public static char right = '>';
        public static char brace = '[';
        public static char close = ']';
        public static char write = '.';
        public static char read = ',';

//        static List<char> opcodeList = new List<char> { plus, minus, left, right, brace, close, write, read };
        static List<char> opcodeList = new List<char> { plus, minus, left, right, brace, close, write };

        public static int opcodesLen = opcodeList.Count;

        public static string IntToStr(int anInt)
        {
            int worker = anInt;
            string retval = "";

            while (retval.Length < Program.progStrLen)
            {
                retval = opcodeList[worker % opcodesLen] + retval;
                worker /= opcodesLen;
            }

            return retval;
        }
    }
}
