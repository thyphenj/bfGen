using System;
using System.Linq;
using System.Collections.Generic;

namespace bfGen
{
    class Program
    {
        static void Main(string[] args)
        {
            int programLength = 6;

            List<char> theOperators = new List<char> { '.', '>', '<', '[', ']', '+', '-' }; //there is NO "read" here

            var BFprogList = new BFProgramList ();

            int numOperators = theOperators.Count;
            List<string> thePrograms = new List<string> ();

            int NumberOfProgramStrings = Convert.ToInt32 (Math.Pow (numOperators, programLength));

            for (int i = 0; i < NumberOfProgramStrings; i++)
            {
                int worker = i;
                char[] possibleProgramString = new char[programLength];

                for (int j = 0; j < programLength; j++)
                {
                    possibleProgramString[j] = theOperators[worker % numOperators];
                    worker /= numOperators;
                }
                BFprogList.Add (new BFprogram (new string (possibleProgramString)));

                if (IsValid (possibleProgramString))
                {
                    string opt = Optimise (possibleProgramString);
                    thePrograms.Add (opt);
                }
            }
            List<string> dis = thePrograms.Distinct ().ToList ();
            dis.Sort ((a, b) => (a.Length.CompareTo (b.Length)));

            int k = 0;
            foreach (string s in dis)
                Console.WriteLine ($"{(++k).ToString ().PadLeft (5)}  {s}");

            Console.ReadLine ();
        }
        private static string Optimise(char[] res)
        {
            List<char> worker = new List<char> (res);
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
                        worker.RemoveAt (i);
                        worker.RemoveAt (i - 1);
                        again = true;           // we haven't necessarily finished yet - try again
                        break;
                    }
                }

                if (worker.Count > 0)           // we might have already removed EVERYTHING!
                {
                    //if we start a loop with zero - we can diss the loop!
                    bool weHaveALoop = false;
                    int startingBracePosition = 0;

                    char ch = worker[startingBracePosition];
                    while (++startingBracePosition < worker.Count && ch != '+' && ch != '-')
                    {
                        if (ch == '[')
                        {
                            startingBracePosition--;
                            weHaveALoop = true;
                            break;
                        }
                        ch = worker[startingBracePosition];
                    };

                    if (weHaveALoop && worker.Count > 0)
                    {
                        int depth = 1;
                        int endingBracePosition = startingBracePosition;
                        while (depth > 0)
                            switch (worker[++endingBracePosition])
                            {
                                case '[':
                                    depth++;
                                    break;
                                case ']':
                                    depth--;
                                    break;
                                default:
                                    break;
                            }
                        for (int ind = endingBracePosition; ind >= startingBracePosition; ind--)
                            worker.RemoveAt (ind);

                        again = true;
                    }
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
                        if (inLoopCount <= 0)
                            return false;
                        inLoopCount--;
                        break;
                    case '.':
                        producesOutput = true;
                        break;
                    default:
                        break;
                }
            if (inLoopCount > 0)
                return false;

            return producesOutput;
        }
    }
}