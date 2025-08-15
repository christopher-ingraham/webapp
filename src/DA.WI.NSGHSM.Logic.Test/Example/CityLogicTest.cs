using DA.WI.NSGHSM.Core.Exceptions;
using DA.WI.NSGHSM.Core.Extensions;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Example;
using DA.WI.NSGHSM.IRepo.Example;
using DA.WI.NSGHSM.Logic.Example;
using DA.WI.NSGHSM.Repo._Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Xunit;

namespace DA.WI.NSGHSM.Logic.Test.Example
{
    public class CityLogicTest
    {
        private CityRepoMock cityRepo;
        private MeasurementRepoMock measurementRepo;
        //private UnitOfWorkMock uow;

        public CityLogicTest()
        {
            cityRepo = new CityRepoMock();
            measurementRepo = new MeasurementRepoMock();
            //uow = new UnitOfWorkMock();
        }

        [Fact]
        public void Create_NameNullOrEmpty_ThrowsBadRequest()
        {
            var sut = new CityLogic(cityRepo, measurementRepo);

            // Name null
            Assert.Throws<BadRequestException>(() => sut.Create(new CityForInsertDto() { CountryName = "Italia", Population = 1 }));
            //Assert.Equal(1, uow.BeginScopeCalls);
            //Assert.Equal(0, uow.CommitCalls);
            //Assert.Equal(1, uow.DisposeCalls);
            // Name empty
            Assert.Throws<BadRequestException>(() => sut.Create(new CityForInsertDto() { Name = "", CountryName = "Italia", Population = 1 }));
            //Assert.Equal(2, uow.BeginScopeCalls);
            //Assert.Equal(0, uow.CommitCalls);
            //Assert.Equal(2, uow.DisposeCalls);
        }

        [Fact]
        public void Create_CountryNameNullOrEmpty_ThrowsBadRequest()
        {
            var sut = new CityLogic(cityRepo, measurementRepo);

            // CountryName null
            Assert.Throws<BadRequestException>(() => sut.Create(new CityForInsertDto() { Name = "Roma", Population = 1 }));
            //Assert.Equal(1, uow.BeginScopeCalls);
            //Assert.Equal(0, uow.CommitCalls);
            //Assert.Equal(1, uow.DisposeCalls);
            // CountryName empty
            Assert.Throws<BadRequestException>(() => sut.Create(new CityForInsertDto() { Name = "Roma" , CountryName = "", Population = 1 }));
            //Assert.Equal(2, uow.BeginScopeCalls);
            //Assert.Equal(0, uow.CommitCalls);
            //Assert.Equal(2, uow.DisposeCalls);
        }

        [Fact]
        public void Create_PopulationNullOrNegative_ThrowsBadRequest()
        {
            var sut = new CityLogic(cityRepo, measurementRepo);

            // Population null
            Assert.Throws<BadRequestException>(() => sut.Create(new CityForInsertDto() { Name = "Roma", CountryName = "Italia" }));
            //Assert.Equal(1, uow.BeginScopeCalls);
            //Assert.Equal(0, uow.CommitCalls);
            //Assert.Equal(1, uow.DisposeCalls);
            // Population 0
            Assert.Throws<BadRequestException>(() => sut.Create(new CityForInsertDto() { Name = "Roma", CountryName = "Italia", Population = 0 }));
            //Assert.Equal(2, uow.BeginScopeCalls);
            //Assert.Equal(0, uow.CommitCalls);
            //Assert.Equal(2, uow.DisposeCalls);
            // Population negative
            Assert.Throws<BadRequestException>(() => sut.Create(new CityForInsertDto() { Name = "Roma", CountryName = "Italia", Population = -1 }));
            //Assert.Equal(3, uow.BeginScopeCalls);
            //Assert.Equal(0, uow.CommitCalls);
            //Assert.Equal(3, uow.DisposeCalls);
        }

        //[Fact]
        //public void Create_CityAlreadyExists_ThrowsBadRequest()
        //{
        //    var sut = new CityLogic(cityRepo, measurementRepo, uow);

        //    // Already existing entity
        //    Assert.Throws<BadRequestException>(() => sut.Create(new CityForInsertDto() { Name = "Roma", CountryName = "Italia", Population = 1 }));
        //    Assert.Equal(1, uow.BeginScopeCalls);
        //    Assert.Equal(0, uow.CommitCalls);
        //    Assert.Equal(1, uow.DisposeCalls);
        //}

        [Fact]
        public void Create_ValidCity()
        {
            var sut = new CityLogic(cityRepo, measurementRepo);

            // Succesfull Create
            CityDetailDto entity = sut.Create(new CityForInsertDto() { Name = "Londra", CountryName = "Inghilterra", Population = 2 });
            Assert.Equal(1, entity.Id);
            Assert.Equal("Roma", entity.Name);
            Assert.Equal("Italia", entity.CountryName);
            Assert.Equal(111, entity.Population);
            //Assert.Equal(1, uow.BeginScopeCalls);
            //Assert.Equal(1, uow.CommitCalls);
            //Assert.Equal(1, uow.DisposeCalls);
        }

        [Fact]
        public void Update_NameNullOrEmpty_ThrowsBadRequest()
        {
            var sut = new CityLogic(cityRepo, measurementRepo);

            // Name null
            Assert.Throws<BadRequestException>(() => sut.Update(new CityForUpdateDto() { CountryName = "Italia", Population = 1 },1));
            //Assert.Equal(1, uow.BeginScopeCalls);
            //Assert.Equal(0, uow.CommitCalls);
            //Assert.Equal(1, uow.DisposeCalls);
            Assert.Equal(0, cityRepo.UpdateCalls);
            // Name empty
            Assert.Throws<BadRequestException>(() => sut.Update(new CityForUpdateDto() { Name = "", CountryName = "Italia", Population = 1 },2));
            //Assert.Equal(2, uow.BeginScopeCalls);
            //Assert.Equal(0, uow.CommitCalls);
            //Assert.Equal(2, uow.DisposeCalls);
            Assert.Equal(0, cityRepo.UpdateCalls);
        }

        [Fact]
        public void Update_CountryNameNullOrEmpty_ThrowsBadRequest()
        {
            var sut = new CityLogic(cityRepo, measurementRepo);

            // CountryName null
            Assert.Throws<BadRequestException>(() => sut.Update(new CityForUpdateDto() { Name = "Roma", Population = 1 },1));
            //Assert.Equal(1, uow.BeginScopeCalls);
            //Assert.Equal(0, uow.CommitCalls);
            //Assert.Equal(1, uow.DisposeCalls);
            Assert.Equal(0, cityRepo.UpdateCalls);
            // CountryName empty
            Assert.Throws<BadRequestException>(() => sut.Update(new CityForUpdateDto() { Name = "Roma", CountryName = "", Population = 1 },2));
            //Assert.Equal(2, uow.BeginScopeCalls);
            //Assert.Equal(0, uow.CommitCalls);
            //Assert.Equal(2, uow.DisposeCalls);
            Assert.Equal(0, cityRepo.UpdateCalls);
        }

        [Fact]
        public void Update_PopulationNullOrNegative_ThrowsBadRequest()
        {
            var sut = new CityLogic(cityRepo, measurementRepo);

            // Population null
            Assert.Throws<BadRequestException>(() => sut.Update(new CityForUpdateDto() { Name = "Roma", CountryName = "Italia" },1));
            //Assert.Equal(1, uow.BeginScopeCalls);
            //Assert.Equal(0, uow.CommitCalls);
            //Assert.Equal(1, uow.DisposeCalls);
            Assert.Equal(0, cityRepo.UpdateCalls);
            // Population 0
            Assert.Throws<BadRequestException>(() => sut.Update(new CityForUpdateDto() { Name = "Roma", CountryName = "Italia", Population = 0 },2));
            //Assert.Equal(2, uow.BeginScopeCalls);
            //Assert.Equal(0, uow.CommitCalls);
            //Assert.Equal(2, uow.DisposeCalls);
            Assert.Equal(0, cityRepo.UpdateCalls);
            // Population negative
            Assert.Throws<BadRequestException>(() => sut.Update(new CityForUpdateDto() { Name = "Roma", CountryName = "Italia", Population = -1 },3));
            //Assert.Equal(3, uow.BeginScopeCalls);
            //Assert.Equal(0, uow.CommitCalls);
            //Assert.Equal(3, uow.DisposeCalls);
            Assert.Equal(0, cityRepo.UpdateCalls);
        }

        //[Fact]
        //public void Update_CityDoesNotExists_ThrowsBadRequest()
        //{
        //    var sut = new CityLogic(cityRepo, measurementRepo, uow);

        //    // Already existing entity
        //    Assert.Throws<NotFoundException>(() => sut.Update(new CityForUpdateDto() { Name = "Milano", CountryName = "Italia", Population = 1 },1));
        //    Assert.Equal(1, uow.BeginScopeCalls);
        //    Assert.Equal(0, uow.CommitCalls);
        //    Assert.Equal(1, uow.DisposeCalls);
        //    Assert.Equal(0, cityRepo.UpdateCalls);
        //}

        [Fact]
        public void Update_ValidCity()
        {
            var sut = new CityLogic(cityRepo, measurementRepo);

            // Succesfull Create
            CityDetailDto entity = sut.Update(new CityForUpdateDto() { Name = "Roma", CountryName = "Italia", Population = 1 },1);
            Assert.Equal(1, entity.Id);
            Assert.Equal("Roma", entity.Name);
            Assert.Equal("Italia", entity.CountryName);
            Assert.Equal(111, entity.Population);
            //Assert.Equal(1, uow.BeginScopeCalls);
            //Assert.Equal(1, uow.CommitCalls);
            //Assert.Equal(1, uow.DisposeCalls);
            Assert.Equal(1, cityRepo.UpdateCalls);
        }
        
        [Fact]
        public void Delete_RepoCannotRead_ThrowsWhatRepoReadThrows()
        {
            var cityLogic = new CityLogic(cityRepo, measurementRepo);

            const long anyId = 3;

            cityRepo.ReadWillThrow = new NotFoundException(typeof(CityDto), anyId);

            Assert.Throws<NotFoundException>(() => cityLogic.Delete(anyId));
        }

        [Fact]
        public void Delete_CityWithMeasurement_ThrowsCorrectBadRequestException()
        {
            var cityLogic = new CityLogic(cityRepo, measurementRepo);

            const long anyCityId = 3;
            
            var singleMeasurementResult = new ListResultDto<MeasurementListItemDto>
            {
                Data = new MeasurementListItemDto[1] 
                {
                    new MeasurementListItemDto
                    {
                        CityId = anyCityId
                    }
                },
                Total = 1
            };

            measurementRepo.ReadListWillReturn = singleMeasurementResult;

            BadRequestException exception = Assert.Throws<BadRequestException>(() => cityLogic.Delete(anyCityId));
            Assert.Equal(BadRequestType.RELATED_RESOURCES_EXIST, exception.ErrorType);
        }

        [Fact]
        public void Delete_ExistentId_CallsRepoDeleteExactlyOnce()
        {
            var cityLogic = new CityLogic(cityRepo, measurementRepo);

            const long anyId = 3;

            cityLogic.Delete(anyId);

            Assert.Single(cityRepo.DeleteCalls);
            Assert.Equal(anyId, cityRepo.DeleteCalls.Single());
        }

        private class CityRepoMock : ICityRepo<Repo.ExampleDataSource>
        {
            public IEnumerable<CityDto> Cities { get; set; }

            public Exception ReadWillThrow { get; set; }
            
            public CityDto CreateResult { get; set; }
            public int UpdateCalls { get; set; }
            
            public List<long> DeleteCalls = new List<long>();

            public long Create(CityForInsertDto dto)
            {
                return 1;
            }

            public void Delete(long id)
            {
                DeleteCalls.Add(id);
            }

            public CityDetailDto Read(long id)
            {
                if (ReadWillThrow != null)
                    throw ReadWillThrow;

                return new CityDetailDto()
                {
                    Id = 1,
                    Name = "Roma",
                    CountryName = "Italia",
                    Population = 111
                };
            }

            public ListResultDto<CityListItemDto> ReadList(ListRequestDto<CityListFilterDto> listRequest)
            {
                throw new NotImplementedException();
            }

            public void Update(CityForUpdateDto dto,long id)
            {
                UpdateCalls += 1;
            }

            public bool Exists(string cityName)
            {
                return cityName == "Shangai";
            }

            public bool ExistsId(long id)
            {
                return id == 1;
            }

        }

        private class MeasurementRepoMock : IMeasurementRepo<Repo.ExampleDataSource>
        {
            internal ListResultDto<MeasurementListItemDto> ReadListWillReturn = new ListResultDto<MeasurementListItemDto>();

            public long Create(MeasurementDtoForInsert dto)
            {
                throw new NotImplementedException();
            }

            public void Delete(long id)
            {
                throw new NotImplementedException();
            }

            public MeasurementDetailDto Read(long id)
            {
                throw new NotImplementedException();
            }

            public ListResultDto<MeasurementListItemDto> ReadList(ListRequestDto<MeasurementListFilterDto> listRequest)
            {
                return ReadListWillReturn;
            }

            public void Update(MeasurementDtoForUpdate dto, long id)
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
