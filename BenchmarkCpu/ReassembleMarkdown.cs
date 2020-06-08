using System;
using System.Linq;
using System.Text;

namespace BenchmarkCpu
{
    public class ReassembleMarkdown
    {
        public static readonly char[] markdownCharacters = { '*', '_', '[', ']', '(', ')', '\\' };

        public string OldEscapeMarkDownCharacters(string s)
        {
            for (int i = s.Length - 1; i >= 0; i--)
            {
                if (markdownCharacters.Contains(s[i]))
                {
                    s = s.Insert(i, "\\");
                }
            }
            return s;
        }

        public string NewEscapeMarkDownCharacters(string s)
        {
            var sb = escapeMarkdownCharacters(s);
            return sb.ToString();
        }

        public StringBuilder escapeMarkdownCharacters(string s)
        {
            StringBuilder sb = new StringBuilder(s.Length * 2);

            foreach (Char ch in s)
            {
                switch (ch)
                {
                    case '*':
                        sb.Append(@"\*");

                        break;
                    case '_':
                        sb.Append(@"\_");

                        break;
                    case '[':
                        sb.Append(@"\[");

                        break;
                    case ']':
                        sb.Append(@"\]");

                        break;
                    case '(':
                        sb.Append(@"\(");

                        break;
                    case ')':
                        sb.Append(@"\)");

                        break;
                    case '\\':
                        sb.Append(@"\\");

                        break;
                    default:
                        sb.Append(ch);

                        break;
                }
            }

            return sb;
        }
    }
}