using DA.DB.Utils;
using DA.WI.NSGHSM.Core.App;
using DA.WI.NSGHSM.Core.Exceptions;
using DA.WI.NSGHSM.Core.Services;
using DA.WI.NSGHSM.Dto._Core;
using DA.WI.NSGHSM.Dto.Example;
using DA.WI.NSGHSM.IRepo.Example;
using DA.WI.NSGHSM.Repo._Core;
using DA.WI.NSGHSM.Repo._Core.Services;
using DA.WI.NSGHSM.Repo.Example;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using Xunit;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace DA.WI.NSGHSM.Repo.Test.Example
{
    public class CityRepoTest : IDisposable
    {
        internal ICityRepo<ExampleDataSource> cityRepo { get; private set; }
        internal IMeasurementRepo<ExampleDataSource> measurementRepo { get; private set; }

        private ServiceProvider serviceProvider;
        
        public CityRepoTest()
        {
            var services = new ServiceCollection();

            // Configuration config = ConfigureSQLiteDb();
            // Replace with the following if you chose to use SQL Server
            Configuration config = ConfigureSQLServerDb();
            
            services.AddSingleton(typeof(ILogger<>), typeof(ConsoleLogger<>));

            services.AddDataSource<ExampleDataSource>(config);

            services.AddTransient(typeof(DataSourceInitializer<ExampleDataSource>));
            services.AddTransient(typeof(CityRepo<ExampleDataSource>));
            services.AddTransient(typeof(MeasurementRepo<ExampleDataSource>));

            serviceProvider = services.BuildServiceProvider();
            
            serviceProvider.GetService<DataSourceInitializer<ExampleDataSource>>().Execute();
            
            cityRepo = serviceProvider.GetService<CityRepo<ExampleDataSource>>();
            measurementRepo = serviceProvider.GetService<MeasurementRepo<ExampleDataSource>>();
        }

        private Configuration ConfigureSQLServerDb()
        {
            var config = new Configuration()
            {
                DataSources = new Configuration.DataSourcesConfig()
            };

            string exampleDataSourcePath = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), @"Example\DataSources\sqlserver.example_Test.init.sql");
            config.DataSources["Example"] = new Configuration.DataSourceConfig
            {
                Provider = Configuration.DataSourceConfig.ProviderType.SqlServer,
                ConnectionString = "DbType=SqlServer;Host=(localdb)\\MSSQLLocalDB;Database=NSGHSM_Example_Test;",
                InitMode = Configuration.DataSourceConfig.InitModeType.Always,
                InitScriptPath = exampleDataSourcePath
            };

            return config;
        }

        private string GetSQLiteFilePath()
        {
            return Path.Combine
            (
                Path.GetDirectoryName(Assembly.GetCallingAssembly().Location),
                @"Example\DataSources\NSGHSM.db"
            );
        }

        private Configuration ConfigureSQLiteDb()
        {
            var config = new Configuration()
            {
                DataSources = new Configuration.DataSourcesConfig()
            };

            string exampleInitScriptPath = Path.Combine(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), @"Example\DataSources\sqlite.example_Test.init.sql");
            string sqliteFilePath = GetSQLiteFilePath();

            config.DataSources["Example"] = new Configuration.DataSourceConfig
            {
                Provider = Configuration.DataSourceConfig.ProviderType.Sqlite,
                ConnectionString = $"DbType=SQLite;Host={sqliteFilePath}",
                InitMode = Configuration.DataSourceConfig.InitModeType.Always,
                InitScriptPath = exampleInitScriptPath
            };

            return config;
        }

        public void Dispose()
        {
            serviceProvider.Dispose();

            // Comment the following if you chose to use SQL Server
            // DeleteSQLiteFile();
        }

        private void DeleteSQLiteFile()
        {
            // Before deleting the file we need to forcibly dispose its users
            // See: https://stackoverflow.com/a/8513453/6573760
            GC.Collect();
            GC.WaitForPendingFinalizers();

            string sqliteFilePath = GetSQLiteFilePath();
            File.Delete(sqliteFilePath);
        }

        [Fact]
        public void CityRepo_Create_City_Test()
        {
            CityForInsertDto newcity1 = new CityForInsertDto()
            {
                //Id = 4,
                Name = "City_name_test",
                CountryName = "Country_Name_Test",
                Population = 1234
            };

            CityForInsertDto newcity2 = new CityForInsertDto()
            {
                //Id = 1234,
                Name = "City_name_test2",
                CountryName = "Country_Name_Test",
                Population = 1234
            };

            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            {

                long newcityid = cityRepo.Create(newcity1);

                CityDto addedCity = cityRepo.Read(newcityid);

                Assert.Equal(newcityid, addedCity.Id);


                newcityid = cityRepo.Create(newcity2);
                addedCity = cityRepo.Read(newcityid);
                Assert.Equal(newcityid, addedCity.Id);

                ts.Complete();
            }

        }

        [Fact]
        public void Read_ForExistentId_ReturnsCorrectCity()
        {
            CityDto result = cityRepo.Read(1);

            Assert.NotNull(result);
            Assert.Equal("Shanghai", result.Name);
            Assert.Equal("China", result.CountryName);
            Assert.Equal(24153000, result.Population);
        }

        [Fact]
        public void Read_ForNonExistentId_ThrowsNotFoundException()
        {
            Assert.Throws<NotFoundException>(() => cityRepo.Read(-1));
        }

        [Fact]
        public void ReadList_WithEmptyRequest()
        {
            ListRequestDto<CityListFilterDto> listRequest = new ListRequestDto<CityListFilterDto>();
            ListResultDto<CityListItemDto> returnedValue = this.cityRepo.ReadList(listRequest);

            Assert.Equal(3, returnedValue.Total);

        }

        [Fact]
        public void ReadList_WithSearchTextFilter_Empty()
        {
            ListRequestDto<CityListFilterDto> listRequest = new ListRequestDto<CityListFilterDto>() { Filter = new CityListFilterDto { SearchText = "" } };
            ListResultDto<CityListItemDto> returnedValue = this.cityRepo.ReadList(listRequest);

            Assert.Equal(3, returnedValue.Total);
        }

        [Fact]
        public void ReadList_WithSearchTextFilter_Shanghai()
        {
            ListRequestDto<CityListFilterDto> listRequest = new ListRequestDto<CityListFilterDto>() { Filter = new CityListFilterDto { SearchText = "shanghai" } };
            ListResultDto<CityListItemDto> returnedValue = this.cityRepo.ReadList(listRequest);

            Assert.Equal(1, returnedValue.Total);
            Assert.Equal("Shanghai", returnedValue.Data[0].Name);
            Assert.Equal("China", returnedValue.Data[0].CountryName);
            Assert.Equal(24153000, returnedValue.Data[0].Population);
        }

        [Fact]
        public void ReadList_WithSearchTextFilter_Ch()
        {
            ListRequestDto<CityListFilterDto> listRequest = new ListRequestDto<CityListFilterDto>() { Filter = new CityListFilterDto { SearchText = "%ch%" } };
            ListResultDto<CityListItemDto> returnedValue = this.cityRepo.ReadList(listRequest);

            Assert.Equal(3, returnedValue.Total);
        }

        [Fact]
        public void ReadList_WithSearchTextFilter_China()
        {
            ListRequestDto<CityListFilterDto> listRequest = new ListRequestDto<CityListFilterDto>() { Filter = new CityListFilterDto { SearchText = "china" } };
            ListResultDto<CityListItemDto> returnedValue = this.cityRepo.ReadList(listRequest);

            Assert.Equal(2, returnedValue.Total);
        }

        [Fact]
        public void ReadList_WithSearchCityNameFilter_Empty()
        {
            ListRequestDto<CityListFilterDto> listRequest = new ListRequestDto<CityListFilterDto>() { Filter = new CityListFilterDto { SearchCityName = "" } };
            ListResultDto<CityListItemDto> returnedValue = this.cityRepo.ReadList(listRequest);

            Assert.Equal(3, returnedValue.Total);
        }

        [Fact]
        public void ReadList_WithSearchCityNameFilter_Shanghai()
        {
            ListRequestDto<CityListFilterDto> listRequest = new ListRequestDto<CityListFilterDto>() { Filter = new CityListFilterDto { SearchCityName = "shanghai" } };
            ListResultDto<CityListItemDto> returnedValue = this.cityRepo.ReadList(listRequest);

            Assert.Equal(1, returnedValue.Total);
            Assert.Equal("Shanghai", returnedValue.Data[0].Name);
            Assert.Equal("China", returnedValue.Data[0].CountryName);
            Assert.Equal(24153000, returnedValue.Data[0].Population);
        }

        [Fact]
        public void ReadList_WithSearchCityNameFilter_China()
        {
            ListRequestDto<CityListFilterDto> listRequest = new ListRequestDto<CityListFilterDto>() { Filter = new CityListFilterDto { SearchCityName = "china" } };
            ListResultDto<CityListItemDto> returnedValue = this.cityRepo.ReadList(listRequest);

            Assert.Equal(0, returnedValue.Total);
        }

        [Fact]
        public void ReadList_WithSearchCityNameFilter_Sh()
        {
            ListRequestDto<CityListFilterDto> listRequest = new ListRequestDto<CityListFilterDto>() { Filter = new CityListFilterDto { SearchCityName = "%sh%" } };
            ListResultDto<CityListItemDto> returnedValue = this.cityRepo.ReadList(listRequest);

            Assert.Equal(1, returnedValue.Total);
            Assert.Equal("Shanghai", returnedValue.Data[0].Name);
            Assert.Equal("China", returnedValue.Data[0].CountryName);
            Assert.Equal(24153000, returnedValue.Data[0].Population);
        }

        [Fact]
        public void UpdateCityName()
        {
            ListRequestDto<CityListFilterDto> listRequest = new ListRequestDto<CityListFilterDto>();
            ListResultDto<CityListItemDto> citiesList = this.cityRepo.ReadList(listRequest);

            //test preconditions evaluation
            if (citiesList == null || citiesList.Total == 0)
                return;

            CityForUpdateDto testCityDto = new CityForUpdateDto
            {
                CountryName = citiesList.Data[0].CountryName,
                Name = citiesList.Data[0].Name,
                Population = citiesList.Data[0].Population
            };

            string previousValue = testCityDto.Name;

            string newValue = previousValue + "_upd";
            testCityDto.Name = newValue;

            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            {
                this.cityRepo.Update(testCityDto, citiesList.Data[0].Id);
                CityDto updtestCityDto = this.cityRepo.Read(citiesList.Data[0].Id);

                Assert.Equal(newValue, updtestCityDto.Name);
                ts.Complete();
            }
        }

        [Fact]
        public void UpdateCityPopulation()
        {
            ListRequestDto<CityListFilterDto> listRequest = new ListRequestDto<CityListFilterDto>();
            ListResultDto<CityListItemDto> citiesList = this.cityRepo.ReadList(listRequest);

            //test preconditions evaluation
            if (citiesList == null || citiesList.Total == 0)
                return;

            CityForUpdateDto testCityDto = new CityForUpdateDto
            {
                CountryName = citiesList.Data[0].CountryName,
                Name = citiesList.Data[0].Name,
                Population = citiesList.Data[0].Population
            };

            int previousValue = testCityDto.Population;

            int newValue = previousValue + 2;
            testCityDto.Population = newValue;

            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            {
                this.cityRepo.Update(testCityDto, citiesList.Data[0].Id);
                CityDetailDto updtestCityDto = this.cityRepo.Read(citiesList.Data[0].Id);

                Assert.Equal(newValue, updtestCityDto.Population);
                ts.Complete();
            }
        }

        [Fact]
        public void UpdateCityId()
        {
            ListRequestDto<CityListFilterDto> listRequest = new ListRequestDto<CityListFilterDto>();
            ListResultDto<CityListItemDto> citiesList = this.cityRepo.ReadList(listRequest);

            //test preconditions evaluation
            if (citiesList == null || citiesList.Total == 0)
                return;

            CityForUpdateDto testCityDto = new CityForUpdateDto
            {
                CountryName = citiesList.Data[0].CountryName,
                Name = citiesList.Data[0].Name,
                Population = citiesList.Data[0].Population
            };

            long previousValue = citiesList.Data[0].Id;

            long newValue = previousValue + 2;

            try
            {
                using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
                {
                    this.cityRepo.Update(testCityDto, newValue);
                    ts.Complete();
                }
                Assert.True(false);
            }
            catch (Exception)
            {
                Assert.True(true);
            }

        }

        [Fact]
        public void UpdateNotExistingDto()
        {
            ListRequestDto<CityListFilterDto> listRequest = new ListRequestDto<CityListFilterDto>();
            ListResultDto<CityListItemDto> citiesList = this.cityRepo.ReadList(listRequest);

            //test preconditions evaluation
            if (citiesList == null || citiesList.Total == 0)
                return;

            CityForUpdateDto testCityDto = new CityForUpdateDto
            {
                CountryName = citiesList.Data[0].CountryName,
                Name = citiesList.Data[0].Name,
                Population = citiesList.Data[0].Population
            };

            long previousValue = citiesList.Data[0].Id;

            long newValue = long.MaxValue - 1;
            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            {
                this.cityRepo.Update(testCityDto, newValue);

                try
                {

                    CityDto updtestCityDto = this.cityRepo.Read(newValue);

                    Assert.True(false);
                }
                catch (Exception)
                {
                    Assert.True(true);
                }
                ts.Complete();
            }

        }

        [Fact]
        public void ReadList_WithOrderByCityNameAsc()
        {
            ListRequestDto<CityListFilterDto> listRequest = new ListRequestDto<CityListFilterDto>() { };
            listRequest.Sort = new SortRequestDto();
            listRequest.Sort.Items = new SortRequestItemDto[1];
            listRequest.Sort.Items[0] = new SortRequestItemDto() { FieldName = "Name" };


            ListResultDto<CityListItemDto> returnedValue = this.cityRepo.ReadList(listRequest);

            Assert.Equal(3, returnedValue.Total);
            Assert.Equal(1, returnedValue.Data[0].Id);
        }

        [Fact]
        public void ReadList_WithOrderByCityNameDesc()
        {
            ListRequestDto<CityListFilterDto> listRequest = new ListRequestDto<CityListFilterDto>() { };
            listRequest.Sort = new SortRequestDto();
            listRequest.Sort.Items = new SortRequestItemDto[1];
            listRequest.Sort.Items[0] = new SortRequestItemDto() { FieldName = "Name", IsDescending = true };


            ListResultDto<CityListItemDto> returnedValue = this.cityRepo.ReadList(listRequest);

            Assert.Equal(3, returnedValue.Total);
            Assert.Equal(1, returnedValue.Data[0].Id);
        }
        
        [Fact]
        public void CityRepo_Delete_Measurement()
        {
            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            {
                cityRepo.Delete(3);
                Assert.Throws<NotFoundException>(() => cityRepo.Read(3));
            }
        }
    }
}
