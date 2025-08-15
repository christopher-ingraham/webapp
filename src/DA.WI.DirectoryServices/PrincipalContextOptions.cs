using System;
using System.Collections.Generic;
using System.Text;

namespace DA.WI.DirectoryServices
{
    public class PrincipalContextOptions<TPrincipalContextManagerImplementation>
    {
        public bool IsEnabled { get; set; }
        public string Domain { get; set; }

        public string ServiceUserName { get; set; }
        public string ServiceUserPassword { get; set; }
    }

}
