using System;

namespace ParserContracts223
{
    public class Tools
    {
        public static string ClearString(string s)
        {
            string st = s;
            st = st.Trim();
            if (st.StartsWith("[", StringComparison.Ordinal))
            {
                st = st.Remove(0, 1);
            }
            if (st.IndexOf(',', (st.Length - 1)) != -1)
            {
                st = st.Remove(st.Length - 1);
            }
            if (st.IndexOf(']', (st.Length - 1)) != -1)
            {
                st = st.Remove(st.Length - 1);
            }

            return st;
        }
    }
}