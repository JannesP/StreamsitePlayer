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
            if (startIndex >= stringToSearch.Length || startIndex <= -1)
            {
                endIndex = -1;
                return "";
            }
            int firstIndex = stringToSearch.IndexOf(first, startIndex);
            if ((firstIndex) == -1)
            {
                endIndex = -1;
                return "";
            }
            else
            {
                firstIndex += first.Length;
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

        public static byte[] ToByteArray(this int val)
        {
            byte[] bytes = new byte[4];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)((val >> i * 8) & 0xFF);
            }
            return bytes;
        }

        public static int ReadInt(this byte[] array, int pos)
        {
            if (array.Length >= (pos + 4))
            {
                int val = 0;
                for (int i = 0; i < 4; i++)
                {
                    val |= (((int)array[pos + i]) << i * 8);
                }
                return val;
            }
            else
            {
                return 0;
            }
        }

        public static string ToReadableString(this byte[] array)
        {
            string readable = "[ ";
            for (int i = 0; i < array.Length; i++)
            {
                readable += array[i].ToString();
                if (i != (array.Length - 1))
                {
                    readable += ", ";
                }
            }

            return readable + " ]";
        }

        public static void AddAll<T>(this ICollection<T> target, IEnumerable<T> source)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (source == null)
                throw new ArgumentNullException("source");
            foreach (T element in source)
            {
                try
                {
                    target.Add(element);
                }
                catch { }
            }
        }

        public static void ShowParentCentered(this Form newForm, Form owner)
        {
            int x = owner.Width / 2 - newForm.Width / 2 + owner.Location.X;
            int y = owner.Height / 2 - newForm.Height / 2 + owner.Location.Y;
            newForm.Location = new System.Drawing.Point(x, y);
            newForm.Show(owner);
        }
    }
}
