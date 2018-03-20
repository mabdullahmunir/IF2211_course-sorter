using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coursesorter
{
    class Utility
    {
        public static string format1(bool[,] adj, string[] lbl, string[] nname)
        {
            StringBuilder s = new StringBuilder();
            s.Append("digraph g\n{\nforcelabels=true;\nsize=\"4,4\";\nrankdir=\"LR\";\n");
            for (int i=0; i<nname.Length; i++)
            {
                string trimmed = "";
                if (lbl[i] != null)
                    trimmed = lbl[i].Trim();
                s.Append(nname[i] + "[ style=filled,xlabel=\"" + trimmed + "\",");
                int len = trimmed.Split(' ').Length;
                if ((len == 1) && (!trimmed.Equals("")))
                    s.Append("color=red");
                else if (len == 2)
                    s.Append("color=blue");
                s.Append("];\n");
            }
            for (int i=0; i<nname.Length; i++)
            {
                for (int j=0; j<nname.Length; j++)
                {
                    if (adj[i, j])
                    {
                        s.Append(nname[i] + "->" + nname[j] + ";\n");
                    }
                }
            }
            s.Append("}");

            return s.ToString();
        }

        public static string format2(bool[,] adj, ArrayList sortednode, string[] nname)
        {
            StringBuilder s = new StringBuilder();
            s.Append("digraph g\n{\nforcelabels=true;\nsize=\"4,4\"\nrankdir=\"LR\";\n");
            for (int i = 0; i < nname.Length; i++)
            {
                s.Append(nname[i] + "[style=filled,");
                if (sortednode.Contains(i))
                    s.Append("color=blue");
                s.Append("];\n");
            }
            for (int i = 0; i < nname.Length; i++)
            {
                for (int j = 0; j < nname.Length; j++)
                {
                    if (adj[i, j])
                    {
                        s.Append(nname[i] + "->" + nname[j] + "[");
                        if (sortednode.Contains(i))
                            s.Append("style=dotted");
                        s.Append("];\n");
                    }
                }
            }
            s.Append("}");

            return s.ToString();
        }

        public static int findarray(string[] liststr, string search)
        {
            for (int i = 0; i < liststr.Length; i++)
            {
                if (search.Equals(liststr[i]))
                    return i;
            }
            return -1;
        }

        public static string RemoveWhiteSpace(string input)
        {
            var len = input.Length;
            var src = input.ToCharArray();
            int dstIdx = 0;
            for (int i = 0; i < len; i++)
            {
                var ch = src[i];
                switch (ch)
                {
                    case '\u0020':
                    case '\u00A0':
                    case '\u1680':
                    case '\u2000':
                    case '\u2001':
                    case '\u2002':
                    case '\u2003':
                    case '\u2004':
                    case '\u2005':
                    case '\u2006':
                    case '\u2007':
                    case '\u2008':
                    case '\u2009':
                    case '\u200A':
                    case '\u202F':
                    case '\u205F':
                    case '\u3000':
                    case '\u2028':
                    case '\u2029':
                    case '\u0009':
                    case '\u000A':
                    case '\u000B':
                    case '\u000C':
                    case '\u000D':
                    case '\u0085':
                        continue;
                    default:
                        src[dstIdx++] = ch;
                        break;
                }
            }
            return new string(src, 0, dstIdx);
        }

    }
}
