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

namespace DA.WI.NSGHSM.Repo.Test.Example
{
    public class MeasurementRepoTest
    {
        internal ICityRepo<ExampleDataSource> cityRepo { get; private set; }
        internal IMeasurementRepo<ExampleDataSource> measurementRepo { get; private set; }

        private ServiceProvider serviceProvider;

        public MeasurementRepoTest()
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
        public void MeasurementRepo_FilterEmpty()
        {
            ListRequestDto<MeasurementListFilterDto> filter = new ListRequestDto<MeasurementListFilterDto>()
            {
                Filter = new MeasurementListFilterDto()
                {
                    CityId = null,
                    From = null,
                    To = null
                }
            };
            ListResultDto<MeasurementListItemDto> result = measurementRepo.ReadList(filter);

            Assert.Equal(6, result.Total);

            filter.Filter = null;
            result = measurementRepo.ReadList(filter);

            Assert.Equal(6, result.Total);
        }

        [Fact]
        public void MeasurementRepo_Filter_CityIdEmpty()
        {
            ListRequestDto<MeasurementListFilterDto> filter = new ListRequestDto<MeasurementListFilterDto>()
            {
                Filter = new MeasurementListFilterDto()
                {
                    CityId = null,
                    From = new DateTimeOffset(new DateTime(2019, 2, 20)),
                    To = new DateTimeOffset(new DateTime(2100, 1, 1))
                }
            };
            ListResultDto<MeasurementListItemDto> result = measurementRepo.ReadList(filter);

            Assert.Equal(4, result.Total);
        }

        [Fact]
        public void MeasurementRepo_Filter_FromDateEmpty()
        {
            ListRequestDto<MeasurementListFilterDto> filter = new ListRequestDto<MeasurementListFilterDto>()
            {
                Filter = new MeasurementListFilterDto()
                {
                    CityId = 1,
                    From = null,
                    To = new DateTimeOffset(new DateTime(2100, 1, 1))
                }
            };
            ListResultDto<MeasurementListItemDto> result = measurementRepo.ReadList(filter);

            Assert.Equal(3, result.Total);
        }

        [Fact]
        public void MeasurementRepo_Filter_ToDateEmpty()
        {
            ListRequestDto<MeasurementListFilterDto> filter = new ListRequestDto<MeasurementListFilterDto>()
            {
                Filter = new MeasurementListFilterDto()
                {
                    CityId = 1,
                    From = new DateTime(2019, 2, 20),
                    To = null
                }
            };
            ListResultDto<MeasurementListItemDto> result = measurementRepo.ReadList(filter);

            Assert.Equal(2, result.Total);
        }

        [Fact]
        public void MeasurementRepo_Filter_NoEmpty()
        {
            ListRequestDto<MeasurementListFilterDto> filter = new ListRequestDto<MeasurementListFilterDto>()
            {
                Filter = new MeasurementListFilterDto()
                {
                    CityId = 1,
                    From = new DateTimeOffset(new DateTime(2019, 4, 28)),
                    To = new DateTimeOffset(new DateTime(2019, 4, 30))
                }
            };
            ListResultDto<MeasurementListItemDto> result = measurementRepo.ReadList(filter);

            Assert.Equal(1, result.Total);
        }

        [Fact]
        public void ReadList_WithOrderByCityNameAsc()
        {
            ListRequestDto<MeasurementListFilterDto> listRequest = new ListRequestDto<MeasurementListFilterDto>() { };
            listRequest.Sort = new SortRequestDto();
            listRequest.Sort.Items = new SortRequestItemDto[1];
            listRequest.Sort.Items[0] = new SortRequestItemDto() { FieldName = "CityName" };


            ListResultDto<MeasurementListItemDto> returnedValue = this.measurementRepo.ReadList(listRequest);

            Assert.Equal(6, returnedValue.Total);
            Assert.Equal(1, returnedValue.Data[0].Id);
        }

        [Fact]
        public void ReadList_WithOrderByCityNameDesc()
        {
            ListRequestDto<MeasurementListFilterDto> listRequest = new ListRequestDto<MeasurementListFilterDto>() { };
            listRequest.Sort = new SortRequestDto();
            listRequest.Sort.Items = new SortRequestItemDto[1];
            listRequest.Sort.Items[0] = new SortRequestItemDto() { FieldName = "Name", IsDescending = true };


            ListResultDto<MeasurementListItemDto> returnedValue = this.measurementRepo.ReadList(listRequest);

            Assert.Equal(6, returnedValue.Total);
            Assert.Equal(1, returnedValue.Data[0].Id);
        }

        [Fact]
        public void MeasurementRepo_Update_Measurement()
        {
            MeasurementDtoForUpdate dto = new MeasurementDtoForUpdate()
            {
                CityId = 1,
                MeasuredAt = DateTimeOffset.Now,
                Weather = WeatherType.Sunny,
                TemperatureC = 0,
                PressureMB = 1000
            };

            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            {
                measurementRepo.Update(dto, 1);

                MeasurementDetailDto detail = measurementRepo.Read(1);
                Assert.Equal(dto.CityId, detail.CityId);
                Assert.Equal(dto.MeasuredAt, detail.MeasuredAt);
                Assert.Equal(dto.Weather, detail.Weather);
                Assert.Equal(dto.TemperatureC, detail.TemperatureC);
                Assert.Equal(dto.PressureMB, detail.PressureMB);

                ts.Complete();
            }
        }

        [Fact]
        public void MeasurementRepo_Delete_Measurement()
        {
            using (System.Transactions.TransactionScope ts = new System.Transactions.TransactionScope())
            {
                measurementRepo.Delete(1);
                Assert.Throws<NotFoundException>(() => measurementRepo.Read(1));
            }
        }

        [Fact]
        public void ReadList_WithEmptyRequest()
        {
            ListRequestDto<MeasurementListFilterDto> listRequest = new ListRequestDto<MeasurementListFilterDto>();
            ListResultDto<MeasurementListItemDto> returnedValue = this.measurementRepo.ReadList(listRequest);

            Assert.Equal(6, returnedValue.Total);

        }

        [Fact]
        public void ReadMeasure()
        {
            MeasurementDetailDto returnedValue = this.measurementRepo.Read(1);

        }
    }
}
