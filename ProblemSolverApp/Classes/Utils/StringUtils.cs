using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemSolverApp.Classes.Utils
{
    public class StringUtils
    {
        public static string StringToHtml(string s)
        {
            return s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }

        public static string HtmlToString(string html)
        {
            return html.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&apos;", "'");
        }

        public static bool IsTextValidName(string text)
        {
            if (text.Contains("&") || text.Contains("<") || text.Contains(">") || text.Contains("\"") || text.Contains("'"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
