using DA.WI.NSGHSM.Dto._Core.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA.WI.NSGHSM.IRepo._Core.Configuration
{
    public interface IUserRepo<TDataSource>
    {
        IEnumerable<UserDto> Read();

        UserDto Read(long id);

        UserDto GetUserByName(string userName);
        UserDto Create(UserCreateDto dto);
    }
}
