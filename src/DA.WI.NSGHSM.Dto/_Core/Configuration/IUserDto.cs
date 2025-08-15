using System;
using System.Collections.Generic;
using System.Text;

namespace DA.WI.NSGHSM.Dto._Core.Configuration
{
    public interface IUserDto
    {
        string UserName { get; set; }

        bool IsFromActiveDirectory { get; set; }
    }

    public interface IUserDetailDto
    {
        List<string> Roles { get; set; }
    }
}
    