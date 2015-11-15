using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeriesPlayer.Utility.Extensions
{
    public static class MyExtensions
    {
        /// <summary>
        /// Searches for the string between the first and second string.
        /// </summary>
        /// <param name="startIndex">Startindex to start searching</param>
        /// <returns>returns an empty string if either first or second were not found</returns>
        public static string GetSubstringBetween(this string stringToSearch, int startIndex, string first, string second, out int endIndex)
        {
            int firstIndex = stringToSearch.IndexOf(first, startIndex) + first.Length;
            if ((firstIndex - first.Length) == -1)
            {
                endIndex = -1;
                return "";
            }
            int secondIndex = stringToSearch.IndexOf(second, firstIndex + 1);
            if (secondIndex == -1)
            {
                endIndex = -1;
                return "";
            }
            endIndex = secondIndex;
            return stringToSearch.Substring(firstIndex, secondIndex - firstIndex);
        }

        /// <summary>
        /// Searches for the string between the first and second string.
        /// </summary>
        /// <param name="startIndex">Startindex to start searching</param>
        /// <returns>returns an empty string if either first or second were not found</returns>
        public static string GetSubstringBetween(this string stringToSearch, int startIndex, string first, string second)
        {
            int dummy;
            return GetSubstringBetween(stringToSearch, startIndex, first, second, out dummy);
        }

        public static bool HasWritePermission(this DirectoryInfo dir)
        {
            DirectorySecurity security = dir.GetAccessControl();
            if (security == null) return false;

            AuthorizationRuleCollection rules = security.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
            if (rules == null) return false;

            foreach (FileSystemAccessRule rule in rules)
            {
                if ((FileSystemRights.Write & rule.FileSystemRights) != FileSystemRights.Write) continue;

                if (rule.AccessControlType == AccessControlType.Allow) return true;
                else if (rule.AccessControlType == AccessControlType.Deny) return false;
            }

            return false;
        }

        public static AutoCompleteStringCollection ToAutoCompleteStringCollection(this IEnumerable<string> enumerable)
        {
            if (enumerable == null) throw new ArgumentNullException("enumerable");
            var autoComplete = new AutoCompleteStringCollection();
            var list = new List<string>(enumerable.ToArray());
            list.Sort();
            autoComplete.AddRange(list.ToArray());
            return autoComplete;
        }

        public static void ScrollToYPosition(this Panel p, int pos)
        {
            using (Control c = new Control() { Parent = p, Height = 1, Top = pos })
            {
                p.ScrollControlIntoView(c);
            }
        }
    }
}
