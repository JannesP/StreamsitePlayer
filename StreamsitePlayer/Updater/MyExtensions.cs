using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
    static class MyExtensions
    {
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
    }
}
