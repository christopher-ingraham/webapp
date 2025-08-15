using DA.WI.NSGHSM.Core.Exceptions;
using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.Dto._Core.Configuration;
using DA.WI.NSGHSM.IRepo._Core.Configuration;
using DA.WI.NSGHSM.Logic._Core.Configuration;
using DA.WI.NSGHSM.Repo._Core;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xunit;

namespace DA.WI.NSGHSM.Logic.Test._Core.Configuration
{
    public class UserLogicTest
    {
        private UserRepoMock userRepo;

        public UserLogicTest()
        {
            userRepo = new UserRepoMock();
        }


        [Fact]
        public void Read_ReturnsAllUsers()
        {
            userRepo.ReadResult = new[]
            {
                new UserDto { Id = 1, UserName = "User1" },
                new UserDto { Id = 2, UserName = "User2" },
            };
            
            var sut = new UserLogic(userRepo);

            var result = sut.Read();

            var expected = userRepo.ReadResult.JsonClone();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Create_UserNameEmpty_ThrowsBadRequestException()
        {
            var sut = new UserLogic(userRepo);

            Assert.Throws<BadRequestException>(() => sut.Create(new UserCreateDto()));
        }

        [Fact]
        public void Create_UserNameNotEmpty_ReturnsCreatedUser()
        {
            var expected = new UserDto { Id = 1, UserName = "testName" };
            userRepo.CreateResult = expected;

            var sut = new UserLogic(userRepo);

           var result =sut.Create(new UserCreateDto() { UserName = "testName"});

            Assert.NotNull(result);
            result.Should().BeEquivalentTo(expected);
        }

        private class UserRepoMock : IUserRepo<Repo.ConfigurationDataSource>
        {
            public IEnumerable<UserDto> ReadResult { get; set; }
            public UserDto ReadFromIdResult { get; set; }
            public UserDto CreateResult { get; set; }


            public IEnumerable<UserDto> Read()
            {
                return ReadResult.JsonClone();
            }

            public UserDto Read(int id)
            {
                return ReadFromIdResult.JsonClone();
            }

            public UserDto Create(UserCreateDto dto)
            {
                return CreateResult.JsonClone();
            }

            public UserDto Read(long id)
            {
                throw new NotImplementedException();
            }

            public UserDto GetUserByName(string userName)
            {
                throw new NotImplementedException();
            }
        }

        //private class UnitOfWorkMock : IUnitOfWork, IUnitOfWorkScope
        //{
        //    public int BeginScopeCalls { get; set; }

        //    public IUnitOfWorkScope BeginScope()
        //    {
        //        BeginScopeCalls = BeginScopeCalls + 1;
        //        return this;
        //    }

        //    public int CommitCalls { get; set; }

        //    public void Commit()
        //    {
        //        CommitCalls = CommitCalls + 1;
        //    }

        //    public int DisposeCalls { get; set; }

        //    public void Dispose()
        //    {
        //        DisposeCalls = DisposeCalls + 1;
        //    }

        //    public int RollbackCalls { get; set; }

        //    public void Rollback()
        //    {
        //        RollbackCalls = RollbackCalls + 1;
        //    }

        //    public IDbCommand CreateCommand()
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}
