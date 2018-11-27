using System;
using System.Collections.Generic;
using System.Text;

namespace bfGen
{
    class BFProgramList
    {
        List<BFprogram> ProgramList;

        public BFProgramList()
        {
            ProgramList = new List<BFprogram> ();
        }

        public void Add(BFprogram prog)
        {
            if (IsValid (prog))
                ProgramList.Add (Optimise (prog));
        }

        private static bool IsValid(BFprogram prog)
        {
            int inLoopCount = 0;
            bool producesOutput = false;

            foreach (char ch in prog.ToString ())
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

        private static BFprogram Optimise(BFprogram prog)
        {
            string returnString = "";
            string strWorker = prog.ToString ();
            bool again;

            do
            {
                again = false;

                //some combinations do NOTHING!
                int i = 0;
                while (i < strWorker.Length - 1)
                {
                    if (
                        (strWorker[i] == '>' && strWorker[i + 1] == '<') ||
                        (strWorker[i] == '<' && strWorker[i + 1] == '>') ||
                        (strWorker[i] == '+' && strWorker[i + 1] == '-') ||
                        (strWorker[i] == '-' && strWorker[i + 1] == '+')
                        )
                    {
                        i += 2;
                    }
                    else
                    {
                        returnString += strWorker[i++];
                        i++;
                    }
                }

                if (strWorker.Length > 0)           // we might have already removed EVERYTHING!
                {
                    //if we start a loop with zero - we can diss the loop!
                    bool weHaveALoop = false;
                    int startingBracePosition = 0;

                    char ch = strWworker[startingBracePosition];
                    while (++startingBracePosition < strWworker.Count && ch != '+' && ch != '-')
                    {
                        if (ch == '[')
                        {
                            startingBracePosition--;
                            weHaveALoop = true;
                            break;
                        }
                        ch = strWworker[startingBracePosition];
                    };

                    if (weHaveALoop && strWworker.Count > 0)
                    {
                        int depth = 1;
                        int endingBracePosition = startingBracePosition;
                        while (depth > 0)
                            switch (strWworker[++endingBracePosition])
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
                            strWworker.RemoveAt (ind);

                        again = true;
                    }
                }
            } while (again);

            string retval = "";
            foreach (char ch in strWworker)
                retval += ch;
            return retval;
        }

    }
}
