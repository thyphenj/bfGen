using System.Collections.Generic;

namespace BFgenerator
{
    class BFprogram
    {
        public int ProgInt { get; set; }
        public string ProgStr { get; set; }
        public string ProgStrOpt { get; set; }

        private bool validStr;

        public BFprogram(int ind)
        {
            ProgInt = ind;
            ProgStr = BF.IntToStr(ProgInt);

            checkValid();
            if (validStr)
            {
                ProgStrOpt = Optimise();
                checkValid();
            }
        }

        private void checkValid()
        {
            int inLoopCount = 0;
            bool producesOutput = false;

            foreach (char ch in ProgStr)
                if (ch == BF.brace)
                    inLoopCount++;
                else if (ch == BF.write)
                    producesOutput = true;
                else if (ch == BF.close)
                {
                    if (inLoopCount <= 0)
                    {
                        validStr = false;
                        return;
                    }
                    inLoopCount--;
                }
            if (inLoopCount > 0)
            {
                validStr = false;
                return;
            }
            validStr = producesOutput;
            return;
        }

        private string Optimise()
        {
            string strWorker = (string)ProgStr.Clone();
            bool again;
            string returnString;

            do
            {
                again = false;

                returnString = "";

                //some combinations do NOTHING!
                int i = 0;
                while (i < strWorker.Length - 1)
                {
                    if (
                        (strWorker[i] == BF.right && strWorker[i + 1] == BF.left) ||
                        (strWorker[i] == BF.left && strWorker[i + 1] == BF.right) ||
                        (strWorker[i] == BF.plus && strWorker[i + 1] == BF.minus) ||
                        (strWorker[i] == BF.minus && strWorker[i + 1] == BF.plus)
                        )
                    {
                        i += 2;
                    }
                    else
                    {
                        returnString += strWorker[i++];
                    }
                }
                if (i < strWorker.Length)
                    returnString += strWorker[i];

                if (strWorker.Length > 0)           // we might have already removed EVERYTHING!
                {
                    //do we have a loop 
                    int startingBracePosition = strWorker.IndexOf(BF.brace); 

                    //do we do anything of import before the loop
                    if (strWorker.Length > 0 &&
                        startingBracePosition >= 0 &&
                        strWorker.IndexOf(BF.plus, 0, startingBracePosition) < 0 &&
                        strWorker.IndexOf(BF.minus, 0, startingBracePosition) < 0 &&
                        strWorker.IndexOf(BF.read, 0, startingBracePosition) < 0)
                    {
                        int depth = 1;
                        int endingBracePosition = startingBracePosition;
                        while (depth > 0)
                        {
                            char ch = strWorker[++endingBracePosition];
                            if (ch == BF.brace)
                                depth++;
                            else if (ch == BF.close)
                                depth--;
                        }

                        string tmp = "";
                        for (int ind = 0; ind < strWorker.Length; ind++)
                            if (ind < startingBracePosition || ind > endingBracePosition)
                                tmp += strWorker[ind];
                        strWorker = tmp;
                        returnString = strWorker;

                        again = true;
                    }

                }
            } while (again);

            return returnString;
        }

        public bool IsValid()
        {
            return validStr;
        }
        public override string ToString()
        {
            string retval = $"{ProgInt.ToString().PadLeft(10)}    {ProgStr}";

            if (validStr)
                retval += $"  {ProgStrOpt}";

            return retval;
        }

    }
}
