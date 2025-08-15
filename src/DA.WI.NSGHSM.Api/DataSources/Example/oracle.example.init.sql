-- SCHEMA
CREATE TABLE Cities (
    Id          NUMBER(19)          NOT NULL,
    Name        NVARCHAR2 (100) NOT NULL,
    CountryName NVARCHAR2 (100) NOT NULL,
    Population  NUMBER(10)            NOT NULL,
    PRIMARY KEY (Id),
    UNIQUE (Name)
);

CREATE TABLE Todo (
    Id         NUMBER(19)      NOT NULL,
    Name       NVARCHAR2 (100) NOT NULL,
    Address    NVARCHAR2 (100) NOT NULL,
    tododate       DATE            NOT NULL,
    Time       TIMESTAMP(6)    NOT NULL,
    PRIMARY KEY (Id),
    UNIQUE (Name)
);

CREATE SEQUENCE Todo_seq START WITH 200 INCREMENT BY 1;

CREATE OR REPLACE TRIGGER Todo_seq_tr
 BEFORE INSERT ON Todo FOR EACH ROW
 WHEN (NEW.Id IS NULL)
BEGIN
 SELECT Todo_seq.NEXTVAL INTO :NEW.Id FROM DUAL;
END;


-- Generate ID using sequence 
CREATE SEQUENCE Cities_seq START WITH 200 INCREMENT BY 1;


CREATE TABLE Measurements (
    Id           NUMBER(19)		  NOT NULL,
    CityId       NUMBER(19)			NOT NULL,
    MeasuredAt   TIMESTAMP(6) WITH TIME ZONE	NOT NULL,
    Weather      NUMBER(10)				NOT NULL,
    TemperatureC NUMBER(10)				NOT NULL,
    PressureMB   NUMBER(10)				NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (CityId) REFERENCES Cities (Id)
);

-- Generate ID using sequence
CREATE SEQUENCE Measurements_seq START WITH 200 INCREMENT BY 1;

INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (1,'Shanghai', 'China', 24153000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (2,'Beijing', 'China', 18590000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (3,'Karachi', 'Pakistan', 18000000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (4,'Istanbul', 'Turkey', 14657000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (5,'Dhaka', 'Bangladesh', 14543000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (6,'Tokyo', 'Japan', 13617000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (7,'Moscow', 'Russia', 13197596);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (8,'Manila', 'Philippines', 12877000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (9,'Tianjin', 'China', 12784000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (10,'Mumbai', 'India', 12400000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (11,'São Paulo', 'Brazil', 12038000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (12,'Shenzhen', 'China', 11908000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (13,'Guangzhou', 'China', 11548000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (14,'Delhi', 'India', 11035000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (15,'Wuhan', 'China', 10608000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (16,'Lahore', 'Pakistan', 10355000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (17,'Seoul', 'South Korea', 10290000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (18,'Chengdu', 'China', 10152000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (19,'Kinshasa', 'Democratic Republic of the Congo', 10125000);
INSERT INTO Cities (Id, Name, CountryName, Population) VALUES (20,'Lima', 'Peru', 9752000);


INSERT INTO Todo (Id, Name, Address, todoDate, Time) VALUES (1,'Todo1', 'Address1',  TO_DATE('2019-01-12', 'YYYY-MM-DD'), TO_TIMESTAMP('2019-12-15 08:23:12', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO Todo (Id, Name, Address, todoDate, Time) VALUES (1,'Todo2', 'Address2',  TO_DATE('2019-03-11', 'YYYY-MM-DD'), TO_TIMESTAMP('2019-11-14 09:12:45', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO Todo (Id, Name, Address, todoDate, Time) VALUES (1,'Todo3', 'Address3',  TO_DATE('2019-04-24', 'YYYY-MM-DD'), TO_TIMESTAMP('2019-01-16 04:42:21', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO Todo (Id, Name, Address, todoDate, Time) VALUES (1,'Todo4', 'Address4',  TO_DATE('2019-05-21', 'YYYY-MM-DD'), TO_TIMESTAMP('2019-03-11 06:12:12', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO Todo (Id, Name, Address, todoDate, Time) VALUES (1,'Todo5', 'Address5',  TO_DATE('2019-06-21', 'YYYY-MM-DD'), TO_TIMESTAMP('2019-07-17 07:46:11', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO Todo (Id, Name, Address, todoDate, Time) VALUES (1,'Todo6', 'Address6',  TO_DATE('2019-04-13', 'YYYY-MM-DD'), TO_TIMESTAMP('2019-06-18 02:23:15', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO Todo (Id, Name, Address, todoDate, Time) VALUES (1,'Todo7', 'Address7',  TO_DATE('2019-03-14', 'YYYY-MM-DD'), TO_TIMESTAMP('2019-07-23 05:12:17', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO Todo (Id, Name, Address, todoDate, Time) VALUES (1,'Todo8', 'Address8',  TO_DATE('2019-07-18', 'YYYY-MM-DD'), TO_TIMESTAMP('2019-06-11 08:16:35', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO Todo (Id, Name, Address, todoDate, Time) VALUES (1,'Todo9', 'Address9',  TO_DATE('2019-08-28', 'YYYY-MM-DD'), TO_TIMESTAMP('2019-09-21 09:35:46', 'YYYY-MM-DD HH24:MI:SS'));
INSERT INTO Todo (Id, Name, Address, todoDate, Time) VALUES (1,'Todo10', 'Address10',  TO_DATE('2019-05-28', 'YYYY-MM-DD'), TO_TIMESTAMP('2019-08-21 05:48:35', 'YYYY-MM-DD HH24:MI:SS'));


insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (1, TO_TIMESTAMP('2019-02-21 08:44:17', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 22, 1002);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (2, TO_TIMESTAMP('2019-01-23 04:27:25', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 15, 1002);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (3, TO_TIMESTAMP('2019-04-29 04:38:19', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 15, 1019);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (4, TO_TIMESTAMP('2019-04-17 02:07:51', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 22, 1020);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (5, TO_TIMESTAMP('2019-04-10 18:08:03', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 19, 1001);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (6, TO_TIMESTAMP('2019-05-05 19:05:07', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 16, 1019);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (7, TO_TIMESTAMP('2019-04-17 17:11:46', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 17, 1000);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (8, TO_TIMESTAMP('2019-02-27 08:08:11', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 16, 1015);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (9, TO_TIMESTAMP('2019-01-24 05:00:30', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 22, 1018);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (10, TO_TIMESTAMP('2019-04-22 22:17:09', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 23, 1004);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (11, TO_TIMESTAMP('2019-01-18 08:41:23', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 22, 1000);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (12, TO_TIMESTAMP('2019-03-22 21:55:03', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 15, 1007);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (13, TO_TIMESTAMP('2019-04-27 13:00:18', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 18, 1011);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (14, TO_TIMESTAMP('2019-02-20 17:33:06', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 18, 1010);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (15, TO_TIMESTAMP('2019-05-12 16:22:37', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 20, 1002);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (16, TO_TIMESTAMP('2019-03-17 18:46:24', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 21, 1020);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (17, TO_TIMESTAMP('2019-05-06 03:37:47', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 22, 1009);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (18, TO_TIMESTAMP('2019-05-05 06:53:59', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 25, 1003);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (19, TO_TIMESTAMP('2019-01-07 07:41:35', 'YYYY-MM-DD HH24:MI:SS'), 0, 2, 17, 1000);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (20, TO_TIMESTAMP('2019-04-26 23:38:50', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 20, 1008);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (21, TO_TIMESTAMP('2019-03-30 11:26:29', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 17, 1018);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (22, TO_TIMESTAMP('2019-03-07 17:21:24', 'YYYY-MM-DD HH24:MI:SS'), 1, 2, 23, 1008);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (23, TO_TIMESTAMP('2019-03-06 20:10:30', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 19, 1011);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (24, TO_TIMESTAMP('2019-01-18 00:27:17', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 22, 1011);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (25, TO_TIMESTAMP('2019-01-17 07:24:57', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 16, 1004);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (26, TO_TIMESTAMP('2019-05-17 10:30:09', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 16, 1009);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (27, TO_TIMESTAMP('2019-05-02 22:32:33', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 18, 1003);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (28, TO_TIMESTAMP('2019-05-06 04:14:43', 'YYYY-MM-DD HH24:MI:SS'), 1, 2, 24, 1016);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (29, TO_TIMESTAMP('2019-04-13 07:10:49', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 16, 1017);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (30, TO_TIMESTAMP('2019-04-19 14:27:50', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 22, 1020);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (31, TO_TIMESTAMP('2019-03-16 14:11:44', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 22, 1003);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (32, TO_TIMESTAMP('2019-02-27 03:43:54', 'YYYY-MM-DD HH24:MI:SS'), 1, 3, 17, 1018);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (33, TO_TIMESTAMP('2019-04-22 18:54:02', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 18, 1001);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (34, TO_TIMESTAMP('2019-03-28 16:36:00', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 23, 1000);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (35, TO_TIMESTAMP('2019-01-05 23:15:48', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 22, 1012);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (36, TO_TIMESTAMP('2019-03-18 00:05:08', 'YYYY-MM-DD HH24:MI:SS'), 2, 3, 24, 1000);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (37, TO_TIMESTAMP('2019-04-25 04:38:44', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 23, 1017);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (38, TO_TIMESTAMP('2019-02-17 04:00:04', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 22, 1010);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (39, TO_TIMESTAMP('2019-02-19 18:19:29', 'YYYY-MM-DD HH24:MI:SS'), 2, 4, 24, 1005);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (40, TO_TIMESTAMP('2019-01-15 14:16:26', 'YYYY-MM-DD HH24:MI:SS'), 1, 4, 24, 1016);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (41, TO_TIMESTAMP('2019-02-20 21:55:36', 'YYYY-MM-DD HH24:MI:SS'), 0, 4, 19, 1003);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (42, TO_TIMESTAMP('2019-03-11 18:13:16', 'YYYY-MM-DD HH24:MI:SS'), 1, 4, 18, 1017);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (43, TO_TIMESTAMP('2019-05-16 21:15:32', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 20, 1012);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (44, TO_TIMESTAMP('2019-01-23 08:50:50', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 15, 1003);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (45, TO_TIMESTAMP('2019-04-12 07:37:35', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 18, 1005);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (46, TO_TIMESTAMP('2019-02-25 16:10:43', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 18, 1020);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (47, TO_TIMESTAMP('2019-01-12 16:47:34', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 25, 1000);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (48, TO_TIMESTAMP('2019-01-17 17:21:32', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 19, 1018);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (49, TO_TIMESTAMP('2019-05-13 08:13:09', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 24, 1015);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (50, TO_TIMESTAMP('2019-01-08 18:05:38', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 18, 1012);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (51, TO_TIMESTAMP('2019-01-29 18:02:52', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 24, 1003);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (52, TO_TIMESTAMP('2019-03-07 00:38:42', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 20, 1017);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (53, TO_TIMESTAMP('2019-03-07 14:24:01', 'YYYY-MM-DD HH24:MI:SS'), 1, 5, 23, 1009);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (54, TO_TIMESTAMP('2019-05-07 04:53:02', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 19, 1016);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (55, TO_TIMESTAMP('2019-05-13 01:59:03', 'YYYY-MM-DD HH24:MI:SS'), 0, 6, 23, 1008);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (56, TO_TIMESTAMP('2019-01-06 03:06:18', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 19, 1014);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (57, TO_TIMESTAMP('2019-03-26 15:14:31', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 19, 1012);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (58, TO_TIMESTAMP('2019-02-02 19:09:28', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 25, 1008);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (59, TO_TIMESTAMP('2019-05-04 14:33:53', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 16, 1011);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (60, TO_TIMESTAMP('2019-02-01 11:52:38', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 17, 1008);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (61, TO_TIMESTAMP('2019-05-05 22:24:45', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 24, 1011);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (62, TO_TIMESTAMP('2019-03-10 16:13:54', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 18, 1007);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (63, TO_TIMESTAMP('2019-02-21 17:57:03', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 24, 1020);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (64, TO_TIMESTAMP('2019-03-31 17:18:35', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 22, 1013);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (65, TO_TIMESTAMP('2019-02-03 22:49:17', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 20, 1009);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (66, TO_TIMESTAMP('2019-03-22 00:44:15', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 15, 1017);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (67, TO_TIMESTAMP('2019-04-11 12:57:24', 'YYYY-MM-DD HH24:MI:SS'), 0, 7, 15, 1006);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (68, TO_TIMESTAMP('2019-01-25 08:22:00', 'YYYY-MM-DD HH24:MI:SS'), 2, 7, 16, 1019);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (69, TO_TIMESTAMP('2019-04-24 22:05:49', 'YYYY-MM-DD HH24:MI:SS'), 0, 7, 22, 1004);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (70, TO_TIMESTAMP('2019-03-31 20:55:33', 'YYYY-MM-DD HH24:MI:SS'), 2, 7, 24, 1016);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (71, TO_TIMESTAMP('2019-04-12 19:10:11', 'YYYY-MM-DD HH24:MI:SS'), 2, 2, 16, 1013);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (72, TO_TIMESTAMP('2019-03-31 19:51:16', 'YYYY-MM-DD HH24:MI:SS'), 1, 2, 18, 1012);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (73, TO_TIMESTAMP('2019-04-04 00:40:43', 'YYYY-MM-DD HH24:MI:SS'), 1, 2, 18, 1009);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (74, TO_TIMESTAMP('2019-01-18 06:58:37', 'YYYY-MM-DD HH24:MI:SS'), 0, 2, 15, 1007);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (75, TO_TIMESTAMP('2019-03-28 08:32:29', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 15, 1001);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (76, TO_TIMESTAMP('2019-04-24 20:21:08', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 20, 1019);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (77, TO_TIMESTAMP('2019-02-13 02:07:07', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 20, 1000);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (78, TO_TIMESTAMP('2019-03-04 01:24:41', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 16, 1001);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (79, TO_TIMESTAMP('2019-01-11 23:02:34', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 25, 1012);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (80, TO_TIMESTAMP('2019-04-29 08:23:26', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 16, 1016);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (81, TO_TIMESTAMP('2019-04-29 04:21:31', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 20, 1017);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (82, TO_TIMESTAMP('2019-05-13 04:42:19', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 24, 1009);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (83, TO_TIMESTAMP('2019-03-24 23:33:42', 'YYYY-MM-DD HH24:MI:SS'), 1, 2, 19, 1010);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (84, TO_TIMESTAMP('2019-02-26 23:46:26', 'YYYY-MM-DD HH24:MI:SS'), 0, 2, 23, 1009);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (85, TO_TIMESTAMP('2019-03-29 17:57:53', 'YYYY-MM-DD HH24:MI:SS'), 2, 2, 18, 1001);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (86, TO_TIMESTAMP('2019-05-23 23:21:17', 'YYYY-MM-DD HH24:MI:SS'), 2, 2, 19, 1012);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (87, TO_TIMESTAMP('2019-01-11 01:14:32', 'YYYY-MM-DD HH24:MI:SS'), 1, 2, 15, 1005);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (88, TO_TIMESTAMP('2019-03-11 10:04:01', 'YYYY-MM-DD HH24:MI:SS'), 2, 2, 25, 1004);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (89, TO_TIMESTAMP('2019-04-25 14:36:43', 'YYYY-MM-DD HH24:MI:SS'), 2, 2, 24, 1003);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (90, TO_TIMESTAMP('2019-05-11 06:40:22', 'YYYY-MM-DD HH24:MI:SS'), 0, 2, 23, 1016);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (91, TO_TIMESTAMP('2019-04-04 18:02:24', 'YYYY-MM-DD HH24:MI:SS'), 2, 2, 24, 1020);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (92, TO_TIMESTAMP('2019-01-15 13:24:06', 'YYYY-MM-DD HH24:MI:SS'), 0, 2, 20, 1005);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (93, TO_TIMESTAMP('2019-05-08 06:03:00', 'YYYY-MM-DD HH24:MI:SS'), 0, 2, 21, 1005);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (94, TO_TIMESTAMP('2019-05-24 03:51:15', 'YYYY-MM-DD HH24:MI:SS'), 1, 2, 22, 1011);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (95, TO_TIMESTAMP('2019-03-03 23:08:48', 'YYYY-MM-DD HH24:MI:SS'), 1, 2, 17, 1005);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (96, TO_TIMESTAMP('2019-04-12 23:28:15', 'YYYY-MM-DD HH24:MI:SS'), 2, 2, 25, 1004);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (97, TO_TIMESTAMP('2019-04-16 16:01:20', 'YYYY-MM-DD HH24:MI:SS'), 0, 2, 19, 1010);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (98, TO_TIMESTAMP('2019-03-31 13:03:43', 'YYYY-MM-DD HH24:MI:SS'), 0, 2, 18, 1010);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (99, TO_TIMESTAMP('2019-03-28 19:06:31', 'YYYY-MM-DD HH24:MI:SS'), 2, 1, 23, 1008);
insert into Measurements (Id, MeasuredAt, Weather, CityId, TemperatureC, PressureMB) values (100, TO_TIMESTAMP('2019-04-25 16:48:39', 'YYYY-MM-DD HH24:MI:SS'), 0, 1, 18, 1002);
