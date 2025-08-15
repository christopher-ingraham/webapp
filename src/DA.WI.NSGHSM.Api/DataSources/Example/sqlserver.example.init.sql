-- SCHEMA
CREATE TABLE [dbo].[Cities] (
    [Id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [CountryName] NVARCHAR (100) NOT NULL,
    [Population]  INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
);

CREATE TABLE [dbo].[Measurements] (
    [Id]           BIGINT			IDENTITY (1, 1) NOT NULL,
    [CityId]       BIGINT			NOT NULL,
    [MeasuredAt]   DATETIMEOFFSET	NOT NULL,
    [Weather]      INT				NOT NULL,
    [TemperatureC] INT				NOT NULL,
    [PressureMB]   INT				NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([CityId]) REFERENCES [dbo].[Cities] ([Id])
);

-- INITIAL DATA
-- CITIES
SET IDENTITY_INSERT [dbo].[Cities] ON;
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (1,'Shanghai', 'China', 24153000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (2,'Beijing', 'China', 18590000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (3,'Karachi', 'Pakistan', 18000000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (4,'Istanbul', 'Turkey', 14657000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (5,'Dhaka', 'Bangladesh', 14543000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (6,'Tokyo', 'Japan', 13617000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (7,'Moscow', 'Russia', 13197596);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (8,'Manila', 'Philippines', 12877000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (9,'Tianjin', 'China', 12784000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (10,'Mumbai', 'India', 12400000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (11,'São Paulo', 'Brazil', 12038000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (12,'Shenzhen', 'China', 11908000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (13,'Guangzhou', 'China', 11548000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (14,'Delhi', 'India', 11035000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (15,'Wuhan', 'China', 10608000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (16,'Lahore', 'Pakistan', 10355000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (17,'Seoul', 'South Korea', 10290000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (18,'Chengdu', 'China', 10152000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (19,'Kinshasa', 'Democratic Republic of the Congo', 10125000);
INSERT INTO [dbo].[Cities] ([Id], [Name], [CountryName], [Population]) VALUES (20,'Lima', 'Peru', 9752000);
SET IDENTITY_INSERT [dbo].[Cities] OFF;

-- MEASUREMENTS
SET IDENTITY_INSERT [dbo].[Measurements] ON;
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (1, '2019-02-21 08:44:17', 0, 1, 22, 1002);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (2, '2019-01-23 04:27:25', 2, 1, 15, 1002);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (3, '2019-04-29 04:38:19', 0, 1, 15, 1019);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (4, '2019-04-17 02:07:51', 2, 1, 22, 1020);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (5, '2019-04-10 18:08:03', 2, 1, 19, 1001);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (6, '2019-05-05 19:05:07', 1, 1, 16, 1019);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (7, '2019-04-17 17:11:46', 2, 1, 17, 1000);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (8, '2019-02-27 08:08:11', 1, 1, 16, 1015);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (9, '2019-01-24 05:00:30', 0, 1, 22, 1018);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (10, '2019-04-22 22:17:09', 0, 1, 23, 1004);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (11, '2019-01-18 08:41:23', 0, 1, 22, 1000);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (12, '2019-03-22 21:55:03', 1, 1, 15, 1007);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (13, '2019-04-27 13:00:18', 2, 1, 18, 1011);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (14, '2019-02-20 17:33:06', 0, 1, 18, 1010);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (15, '2019-05-12 16:22:37', 1, 1, 20, 1002);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (16, '2019-03-17 18:46:24', 2, 1, 21, 1020);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (17, '2019-05-06 03:37:47', 0, 1, 22, 1009);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (18, '2019-05-05 06:53:59', 2, 1, 25, 1003);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (19, '2019-01-07 07:41:35', 0, 2, 17, 1000);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (20, '2019-04-26 23:38:50', 2, 1, 20, 1008);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (21, '2019-03-30 11:26:29', 2, 1, 17, 1018);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (22, '2019-03-07 17:21:24', 1, 2, 23, 1008);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (23, '2019-03-06 20:10:30', 1, 1, 19, 1011);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (24, '2019-01-18 00:27:17', 2, 1, 22, 1011);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (25, '2019-01-17 07:24:57', 1, 1, 16, 1004);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (26, '2019-05-17 10:30:09', 2, 1, 16, 1009);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (27, '2019-05-02 22:32:33', 1, 1, 18, 1003);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (28, '2019-05-06 04:14:43', 1, 2, 24, 1016);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (29, '2019-04-13 07:10:49', 1, 1, 16, 1017);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (30, '2019-04-19 14:27:50', 2, 1, 22, 1020);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (31, '2019-03-16 14:11:44', 1, 1, 22, 1003);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (32, '2019-02-27 03:43:54', 1, 3, 17, 1018);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (33, '2019-04-22 18:54:02', 1, 1, 18, 1001);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (34, '2019-03-28 16:36:00', 0, 1, 23, 1000);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (35, '2019-01-05 23:15:48', 1, 1, 22, 1012);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (36, '2019-03-18 00:05:08', 2, 3, 24, 1000);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (37, '2019-04-25 04:38:44', 1, 1, 23, 1017);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (38, '2019-02-17 04:00:04', 0, 1, 22, 1010);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (39, '2019-02-19 18:19:29', 2, 4, 24, 1005);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (40, '2019-01-15 14:16:26', 1, 4, 24, 1016);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (41, '2019-02-20 21:55:36', 0, 4, 19, 1003);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (42, '2019-03-11 18:13:16', 1, 4, 18, 1017);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (43, '2019-05-16 21:15:32', 1, 1, 20, 1012);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (44, '2019-01-23 08:50:50', 1, 1, 15, 1003);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (45, '2019-04-12 07:37:35', 0, 1, 18, 1005);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (46, '2019-02-25 16:10:43', 0, 1, 18, 1020);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (47, '2019-01-12 16:47:34', 1, 1, 25, 1000);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (48, '2019-01-17 17:21:32', 1, 1, 19, 1018);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (49, '2019-05-13 08:13:09', 2, 1, 24, 1015);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (50, '2019-01-08 18:05:38', 2, 1, 18, 1012);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (51, '2019-01-29 18:02:52', 0, 1, 24, 1003);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (52, '2019-03-07 00:38:42', 1, 1, 20, 1017);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (53, '2019-03-07 14:24:01', 1, 5, 23, 1009);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (54, '2019-05-07 04:53:02', 0, 1, 19, 1016);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (55, '2019-05-13 01:59:03', 0, 6, 23, 1008);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (56, '2019-01-06 03:06:18', 2, 1, 19, 1014);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (57, '2019-03-26 15:14:31', 2, 1, 19, 1012);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (58, '2019-02-02 19:09:28', 1, 1, 25, 1008);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (59, '2019-05-04 14:33:53', 2, 1, 16, 1011);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (60, '2019-02-01 11:52:38', 2, 1, 17, 1008);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (61, '2019-05-05 22:24:45', 0, 1, 24, 1011);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (62, '2019-03-10 16:13:54', 1, 1, 18, 1007);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (63, '2019-02-21 17:57:03', 0, 1, 24, 1020);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (64, '2019-03-31 17:18:35', 1, 1, 22, 1013);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (65, '2019-02-03 22:49:17', 0, 1, 20, 1009);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (66, '2019-03-22 00:44:15', 2, 1, 15, 1017);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (67, '2019-04-11 12:57:24', 0, 7, 15, 1006);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (68, '2019-01-25 08:22:00', 2, 7, 16, 1019);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (69, '2019-04-24 22:05:49', 0, 7, 22, 1004);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (70, '2019-03-31 20:55:33', 2, 7, 24, 1016);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (71, '2019-04-12 19:10:11', 2, 2, 16, 1013);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (72, '2019-03-31 19:51:16', 1, 2, 18, 1012);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (73, '2019-04-04 00:40:43', 1, 2, 18, 1009);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (74, '2019-01-18 06:58:37', 0, 2, 15, 1007);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (75, '2019-03-28 08:32:29', 2, 1, 15, 1001);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (76, '2019-04-24 20:21:08', 0, 1, 20, 1019);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (77, '2019-02-13 02:07:07', 0, 1, 20, 1000);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (78, '2019-03-04 01:24:41', 1, 1, 16, 1001);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (79, '2019-01-11 23:02:34', 0, 1, 25, 1012);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (80, '2019-04-29 08:23:26', 2, 1, 16, 1016);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (81, '2019-04-29 04:21:31', 0, 1, 20, 1017);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (82, '2019-05-13 04:42:19', 0, 1, 24, 1009);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (83, '2019-03-24 23:33:42', 1, 2, 19, 1010);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (84, '2019-02-26 23:46:26', 0, 2, 23, 1009);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (85, '2019-03-29 17:57:53', 2, 2, 18, 1001);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (86, '2019-05-23 23:21:17', 2, 2, 19, 1012);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (87, '2019-01-11 01:14:32', 1, 2, 15, 1005);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (88, '2019-03-11 10:04:01', 2, 2, 25, 1004);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (89, '2019-04-25 14:36:43', 2, 2, 24, 1003);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (90, '2019-05-11 06:40:22', 0, 2, 23, 1016);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (91, '2019-04-04 18:02:24', 2, 2, 24, 1020);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (92, '2019-01-15 13:24:06', 0, 2, 20, 1005);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (93, '2019-05-08 06:03:00', 0, 2, 21, 1005);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (94, '2019-05-24 03:51:15', 1, 2, 22, 1011);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (95, '2019-03-03 23:08:48', 1, 2, 17, 1005);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (96, '2019-04-12 23:28:15', 2, 2, 25, 1004);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (97, '2019-04-16 16:01:20', 0, 2, 19, 1010);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (98, '2019-03-31 13:03:43', 0, 2, 18, 1010);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (99, '2019-03-28 19:06:31', 2, 1, 23, 1008);
insert into [dbo].[Measurements] ([Id], [MeasuredAt], [Weather], [CityId], [TemperatureC], [PressureMB]) values (100, '2019-04-25 16:48:39', 0, 1, 18, 1002);

SET IDENTITY_INSERT [dbo].[Measurements] OFF;
