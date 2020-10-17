using System.Collections.Generic;
using System.DirectoryServices;

namespace RetroFidget.Data.Authorize
{
    public class UserInfo
    {

        private DirectoryEntry User { get; set; }
        public List<string> ADGroups { get; set; }

        public UserInfo(string SID)
        {
            ADGroups = new List<string>();
            //Create Disposable Directory Searcher to retrieve Current User with SID pulled from Smart Card
            using (DirectorySearcher comps = new DirectorySearcher(new DirectoryEntry("LDAP://OU=Campbell,OU=Installations,DC=nase,DC=ds,DC=army,DC=mil")))
            {
                comps.Filter = "(&(objectClass=user)(objectSID=" + SID + "))";
                User = comps.FindOne().GetDirectoryEntry();
            }
            //Retrieve the Name of AD Groups Current User is a member of. Please note Domain Users Group is exclued from this list.
            foreach (object group in User.Properties["memberOf"])
                ADGroups.Add(group.ToString()[3..].Split(",OU=")[0]);
        }
    }
}
