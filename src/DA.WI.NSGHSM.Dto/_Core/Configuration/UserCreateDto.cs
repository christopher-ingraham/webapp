using System;
using System.Collections.Generic;
using System.Text;

namespace DA.WI.NSGHSM.Dto._Core.Configuration
{
    public class UserCreateDto : IUserDto
    {
        public string UserName { get; set; }

        public bool IsFromActiveDirectory { get; set; }
    }
}
