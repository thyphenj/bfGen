using System;
using System.Collections.Generic;

namespace bfGen
{
    class Program
    {
        static void Main(string[] args)
        {
            List<char> theOperators = new List<char> { '.', '>', '<', '[', ']', '+', '-' }; //there is NO "read" here
            int numOperators = theOperators.Count;

            int programLength = 5;

            int NumberOfProgramStrings = Convert.ToInt32(Math.Pow(numOperators, programLength));
            int validProgramNumber = 0;
            for (int i = 0; i < NumberOfProgramStrings; i++)
            {
                int worker = i;
                char[] possibleProgramString = new char[programLength];

                for (int j = 0; j < programLength; j++)
                {
                    possibleProgramString[j] = theOperators[worker % numOperators];
                    worker /= numOperators;
                }
                if (IsValid(possibleProgramString))
                {
                    string opt = Optimise(possibleProgramString);
                    Console.WriteLine($"{(++validProgramNumber).ToString().PadLeft(programLength + 2)} {i.ToString().PadLeft(programLength + 2)}       {new string(possibleProgramString)}       {opt}");
                }
            }
            Console.ReadLine();
        }
        private static string Optimise(char[] res)
        {
            List<char> worker = new List<char>(res);
            bool again;

            do
            {
                again = false;

                //some combinations do NOTHING!
                for (int i = worker.Count - 1; i > 0; i--)
                {
                    if (
                        (worker[i] == '>' && worker[i - 1] == '<') ||
                        (worker[i] == '<' && worker[i - 1] == '>') ||
                        (worker[i] == '+' && worker[i - 1] == '-') ||
                        (worker[i] == '-' && worker[i - 1] == '+')
                        )
                    {
                        worker.RemoveAt(i);
                        worker.RemoveAt(i - 1);
                        again = true;
                        break;
                    }
                }

                //if we start a loop with zero - we can diss the loop!
                int startingBracePosition = 0;
                bool loopStart = false;
                do
                {
                    if (worker[startingBracePosition] == '[' )
                    {
                        loopStart = true;
                        break;
                    }
                    if ( worker[startingBracePosition] == '+' || worker[startingBracePosition] == '-')
                    {
                        loopStart = false;
                        break;
                    }
                } while (loopStart);

                while (loopStart && worker.Count > 0)
                {
                    int depth = 1;
                    int endingBracePosition = startingBracePosition;
                    while (depth > 0)
                        switch (worker[++endingBracePosition])
                        {
                            case '[': depth++; break;
                            case ']': depth--; break;
                            default: break;
                        }
                    for ( int ind = endingBracePosition; ind >= startingBracePosition; ind--)
                        worker.RemoveAt(ind);

                    loopStart = false;
                    again = true;
                }
            } while (again);

            string retval = "";
            foreach (char ch in worker)
                retval += ch;
            return retval;
        }
        private static bool IsValid(char[] res)
        {
            int inLoopCount = 0;
            bool producesOutput = false;

            foreach (char ch in res)
                switch (ch)
                {
                    case '[':
                        inLoopCount++;
                        break;
                    case ']':
                        if (inLoopCount <= 0) return false;
                        inLoopCount--;
                        break;
                    case '.':
                        producesOutput = true;
                        break;
                    default:
                        break;
                }
            if (inLoopCount > 0) return false;

            return producesOutput;
        }
    }
}