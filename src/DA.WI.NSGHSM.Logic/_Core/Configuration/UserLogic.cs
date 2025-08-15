using DA.WI.NSGHSM.Core.Exceptions;
using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.Dto._Core.Configuration;
using DA.WI.NSGHSM.IRepo._Core.Configuration;
using DA.WI.NSGHSM.Repo;
using System.Collections.Generic;
using System.Transactions;

namespace DA.WI.NSGHSM.Logic._Core.Configuration
{
    public class UserLogic
    {
        private readonly IUserRepo<ConfigurationDataSource> repo;

        public UserLogic(IUserRepo<ConfigurationDataSource> repo)
        {
            this.repo = repo;
        }

        public IEnumerable<UserDto> Read()
        {
            return repo.Read();
        }

        public UserDto Read(long id)
        {
            return repo.Read(id);
        }

        public UserDto Create(UserCreateDto dto)
        {
            UserDto created = null;
            using (TransactionScope ts = new TransactionScope())
            {
                created = repo.Create(dto);

                // for test purposes validate is moved after insert in order to fail if username is empty 
                // and check that the transaction is rolled back and the connection is closed
                ValidateOrThrow(dto);

                ts.Complete();
            }
            return created;
        }

        private void ValidateOrThrow(UserCreateDto dto)
        {
            if (dto.UserName.IsNullOrEmpty() == true)
            {
                throw new BadRequestException(BadRequestType.REQUIRED_FIELD, new { field = nameof(dto.UserName) });
            }
        }
    }
}
