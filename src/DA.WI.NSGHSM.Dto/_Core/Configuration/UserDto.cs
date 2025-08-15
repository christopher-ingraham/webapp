using System;
using System.Collections.Generic;
using System.Text;

namespace DA.WI.NSGHSM.Dto._Core.Configuration
{
    public class UserDto : IIdDto, IUserDto, IUserDetailDto
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        // grants that the pasword will never be serialized to consumers ;)
        public bool ShouldSerializePassword() => false;
        public string Password { get; set; }

        public bool IsFromActiveDirectory { get; set; }

        public List<string> Roles { get; set; }
    }
}
