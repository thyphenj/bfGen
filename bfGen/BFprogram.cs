using System.Collections.Generic;

namespace bfGen
{
    class BFprogram
    {
        public List<char> ProgString { get; set; }

        public BFprogram(string str)
        {
            ProgString = new List<char> ();

            foreach (char ch in str)
                ProgString.Add (ch);
        }

        public override string ToString()
        {
            string retval = "";
            foreach (char ch in ProgString)
                retval += ch;
            return retval;
        }
    }
}
