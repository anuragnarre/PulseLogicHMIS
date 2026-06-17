using System.Text.RegularExpressions;

namespace PulseLogicHMR.Utilities
{
    public static class PulseLogicStringExtension
    {
        // Returns boolean value (true/false) 
        public static bool Like(this string searchExpression, string searchKey)
        {
            return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(searchKey, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(searchExpression);
        }
    }
}
