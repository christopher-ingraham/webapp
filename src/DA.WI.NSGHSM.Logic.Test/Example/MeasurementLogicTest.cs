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
using System.Text;
using Xunit;

namespace DA.WI.NSGHSM.Logic.Test.Example
{
    public class MeasurementLogicTest
    {
        private MeasurementRepoMock MeasurementRepo;

        public MeasurementLogicTest()
        {
            MeasurementRepo = new MeasurementRepoMock();
        }


        [Fact]
        public void Create_ValidMeasurement()
        {
            var sut = new MeasurementLogic(MeasurementRepo);

            // Succesfull Create
            MeasurementDetailDto entity = sut.Create(new MeasurementDtoForInsert()
            {
                CityId = 1,
                MeasuredAt = System.DateTime.Parse("10/11/2016"),
                PressureMB = 200,
                TemperatureC = 280,
                Weather = WeatherType.Sunny
            });
        }

        [Fact]
        public void Create_InValidMeasurement_Date()
        {
            var sut = new MeasurementLogic(MeasurementRepo);

            Assert.Throws<BadRequestException>(() => {
                MeasurementDetailDto entity = sut.Create(new MeasurementDtoForInsert()
                {
                    CityId = 1,
                    MeasuredAt = System.DateTime.Now.AddDays(1),
                    PressureMB = 200,
                    TemperatureC = 280,
                    Weather = WeatherType.Sunny
                });
            });
        }

        [Fact]
        public void Create_InValidTemperature_Date()
        {
            var sut = new MeasurementLogic(MeasurementRepo);

            Assert.Throws<BadRequestException>(() => {
                MeasurementDetailDto entity = sut.Create(new MeasurementDtoForInsert()
                {
                    CityId = 1,
                    MeasuredAt = System.DateTime.Now.AddDays(1),
                    PressureMB = 200,
                    TemperatureC = 500,
                    Weather = WeatherType.Sunny
                });
            });
        }

        [Fact]
        public void Delete_RepoCannotRead_ThrowsWhatRepoReadThrows()
        {
            var measurementLogic = new MeasurementLogic(MeasurementRepo);

            const long anyId = 3000;

            MeasurementRepo.ReadWillThrow = new NotFoundException(typeof(MeasurementDto), anyId);

            Assert.Throws<NotFoundException>(() => measurementLogic.Delete(anyId));
        }

        private class MeasurementRepoMock : IMeasurementRepo<Repo.ExampleDataSource>
        {
            public IEnumerable<MeasurementDto> ReadResult { get; set; }
            public MeasurementDto CreateResult { get; set; }
            public int UpdateCalls { get; set; }

            public Exception ReadWillThrow { get; set; }

            public long Create(MeasurementDtoForInsert dto)
            {
                return 1;
            }

            public void Delete(long id)
            {
                throw new NotImplementedException();
            }

            public MeasurementDetailDto Read(long id)
            {

                if (ReadWillThrow != null)
                    throw ReadWillThrow;

                return new MeasurementDetailDto()
                {
                    City = new CityLookupDto()
                    {
                        Id = 1,
                        Name = "Buttrio"
                    },
                    Id = 1,
                    CityId = 1,
                    MeasuredAt = System.DateTime.Now,
                    PressureMB = 200,
                    TemperatureC = 300,
                    Weather = WeatherType.Sunny
                };
            }

            public ListResultDto<MeasurementListItemDto> ReadList(ListRequestDto<MeasurementListFilterDto> listRequest)
            {
                throw new NotImplementedException();
            }

            public void Update(MeasurementDtoForUpdate dto, long id)
            {
                UpdateCalls += 1;
            }

            public bool Exists(MeasurementDto dto)
            {
                throw new NotImplementedException();
            }
        }


    }
}
