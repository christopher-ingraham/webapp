-- SCHEMA
CREATE TABLE [Cities] (
    [Id]          INTEGER PRIMARY KEY AUTOINCREMENT,
    [Name]        NVARCHAR (100) NOT NULL,
    [CountryName] NVARCHAR (100) NOT NULL,
    [Population]  INT            NOT NULL,
    UNIQUE ([Name] ASC)
);

CREATE TABLE [Measurements] (
    [Id]           INTEGER PRIMARY KEY AUTOINCREMENT,
    [CityId]       INTEGER   NOT NULL,
    [MeasuredAt]   DATETIMEOFFSET NOT NULL,
    [Weather]      INT      NOT NULL,
    [TemperatureC] INT      NOT NULL,
    [PressureMB]   INT      NOT NULL,
    FOREIGN KEY ([CityId]) REFERENCES [Cities] ([Id])
);

-- INITIAL DATA
-- CITIES
INSERT INTO [Cities] ([Name], [CountryName], [Population]) VALUES ('Shanghai', 'China', 24153000);
INSERT INTO [Cities] ([Name], [CountryName], [Population]) VALUES ('Beijing', 'China', 18590000);
INSERT INTO [Cities] ([Name], [CountryName], [Population]) VALUES ('Karachi', 'Pakistan', 18000000);

-- MEASUREMENTS
insert into [Measurements] ([MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values ('2019-02-21 08:44:17+00:00', 0, 1, 22, 1002);
insert into [Measurements] ([MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values ('2019-01-23 04:27:25+00:00', 2, 1, 15, 1002);
insert into [Measurements] ([MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values ('2019-04-29 04:38:19+00:00', 0, 1, 15, 1019);

insert into [Measurements] ([MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values ('2019-02-22 09:44:17+00:00', 0, 2, 22, 2002);
insert into [Measurements] ([MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values ('2019-01-24 10:27:25+00:00', 2, 2, 15, 2002);
insert into [Measurements] ([MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values ('2019-04-25 11:38:19+00:00', 0, 2, 15, 2019);