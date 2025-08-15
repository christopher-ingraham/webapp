CREATE TABLE [CatApplications] (
    [Id] BIGINT PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(255) NOT NULL, 
);

CREATE TABLE [SecUsers] (
    [Id] BIGINT PRIMARY KEY IDENTITY,
    [UserName] NVARCHAR(255) NOT NULL,
    [Password] NVARCHAR(255) NULL,
    [IsFromActiveDirectory] BIT DEFAULT 0 NOT NULL,
);

CREATE TABLE [SecRoles] (
    [Id] BIGINT PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(255) NOT NULL, 
);

CREATE TABLE [SecUsersRoles] (
    [UserId] BIGINT NOT NULL, 
    [RoleId] BIGINT NOT NULL, 
    CONSTRAINT [PkSecUsersRoles_UserId_RoleId] PRIMARY KEY ([UserId],[RoleId]),
    CONSTRAINT [FkSecUsersRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [SecRoles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION, 
    CONSTRAINT [FkSecUsersRoles_UserId] FOREIGN KEY ([UserId]) REFERENCES [SecUsers] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE [CfgUserApplications] (
    [Id] BIGINT PRIMARY KEY IDENTITY, 
    [UserId] BIGINT DEFAULT 0 NULL, 
    [ApplicationId] BIGINT DEFAULT 0 NULL, 
    CONSTRAINT [R_82] FOREIGN KEY ([UserId]) REFERENCES [SecUsers] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION, 
    CONSTRAINT [R_83] FOREIGN KEY ([ApplicationId]) REFERENCES [CatApplications] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE [SecGroups] (
    [Id] BIGINT PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(255) NOT NULL, 
);

CREATE TABLE [SecUsersGroups] (
    [UserId] BIGINT DEFAULT 0 NOT NULL, 
    [GroupId] BIGINT DEFAULT 0 NOT NULL, 
    CONSTRAINT [PkSecUsersGroups_UserId_GroupId] PRIMARY KEY ([UserId],[GroupId]), 
    CONSTRAINT [FkSecUsersGroups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [SecGroups] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION, 
    CONSTRAINT [FkSecUsersGroups_UserId] FOREIGN KEY ([UserId]) REFERENCES [SecUsers] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE [SecGroupsRoles] (
    [GroupId] BIGINT DEFAULT 0 NOT NULL, 
    [RoleId] BIGINT DEFAULT 0 NOT NULL, 
    CONSTRAINT [PkSecGroupsRoles_GroupId_RoleId] PRIMARY KEY ([GroupId],[RoleId]), 
    CONSTRAINT [FkSecGroupsRoles_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [SecGroups] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION, 
    CONSTRAINT [FkSecGroupsRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [SecRoles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);
  
-- Applications Info

SET IDENTITY_INSERT CatApplications ON; 

INSERT 
INTO CatApplications(Id,
                        Name)
VALUES(1,'TestApp1');

INSERT 
INTO CatApplications(Id,
                        Name)
VALUES(2,'TestApp2');

SET IDENTITY_INSERT CatApplications OFF;   


SET IDENTITY_INSERT SecUsers ON;   

-- Users Info
INSERT 
INTO SecUsers(Id,
				UserName,
                Password,
                IsFromActiveDirectory)
VALUES(1,
		'admin',
        'admin',
        0);

INSERT 
INTO SecUsers(Id,
				UserName,
                Password,
                IsFromActiveDirectory)
VALUES(2,
		'user',
        'user',
        1);

INSERT 
INTO SecUsers(Id,
				UserName,
                Password,
                IsFromActiveDirectory)
VALUES(3,
		'developer',
        'developer',
        1);

SET IDENTITY_INSERT SecUsers OFF;   


-- Roles

 
SET IDENTITY_INSERT SecRoles ON;   

INSERT 
INTO SecRoles(Id,
                Name)
VALUES(1,'Admin');

INSERT 
INTO SecRoles(Id,
                Name)
VALUES(2,'User');

INSERT 
INTO SecRoles(Id,
                Name)
VALUES(3,'Developer');

SET IDENTITY_INSERT SecRoles OFF;   


-- Users To Application Cross Reference

SET IDENTITY_INSERT CfgUserApplications ON;   

INSERT 
INTO CfgUserApplications(Id,
                            UserId,
                            ApplicationId)
VALUES(1,1,1);

INSERT 
INTO CfgUserApplications(Id,
                            UserId,
                            ApplicationId)
VALUES(2,2,2);

SET IDENTITY_INSERT CfgUserApplications OFF;   

-- Users To Roles Cross Reference

-- User=user, Role=User 
INSERT 
INTO SecUsersRoles(UserId,
                    RoleId)
VALUES(2,2);

-- User=developer, Role=User 
INSERT 
INTO SecUsersRoles(UserId,
                    RoleId)
VALUES(3,2);




-- Groups

SET IDENTITY_INSERT SecGroups ON;   

INSERT 
INTO SecGroups(Id,
                Name)
VALUES(1,'Administrators');

INSERT 
INTO SecGroups(Id,
                Name)
VALUES(2,'Developers');

SET IDENTITY_INSERT SecGroups OFF;   


-- Users To Groups Cross Reference

-- User=admin, Group=Administrators 
INSERT 
INTO SecUsersGroups(UserId,
                    GroupId)
VALUES(1,1);

-- User=developer, Group=Developers 
INSERT 
INTO SecUsersGroups(UserId,
                    GroupId)
VALUES(3,2);


-- Groups To Roles Cross Reference

-- Group=Administrators, Role=Admin 
INSERT 
INTO SecGroupsRoles(GroupId,
                    RoleId)
VALUES(1,1);

-- Group=Administrators, Role=User
INSERT 
INTO SecGroupsRoles(GroupId,
                    RoleId)
VALUES(1,2);

-- Group=Administrators, Role=Developer
INSERT 
INTO SecGroupsRoles(GroupId,
                    RoleId)
VALUES(1,3);

-- Group=Developers, Role=Developer
INSERT 
INTO SecGroupsRoles(GroupId,
                    RoleId)
VALUES(2,3);




