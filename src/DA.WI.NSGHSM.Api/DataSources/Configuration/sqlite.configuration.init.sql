CREATE TABLE [CatApplications] (
    [Id] int DEFAULT 0 NOT NULL, 
    [Name] nvarchar(255) NOT NULL COLLATE NOCASE, 
    CONSTRAINT [PkCatApplications_Id] PRIMARY KEY ([Id]) 
);

CREATE TABLE [SecUsers] (
    [Id] INTEGER NOT NULL,
    [UserName] nvarchar(255) NOT NULL COLLATE NOCASE,
    [Password] nvarchar(255) NULL COLLATE NOCASE,
    [IsFromActiveDirectory] numeric(1,0) DEFAULT 0 NOT NULL,
    CONSTRAINT [PkSecUsers_Id] PRIMARY KEY ([Id])
);

CREATE TABLE [SecRoles] (
    [Id] int DEFAULT 0 NOT NULL, 
    [Name] nvarchar(255) NOT NULL COLLATE NOCASE, 
    CONSTRAINT [PkSecRoles_Id] PRIMARY KEY ([Id])
);

CREATE TABLE [SecUsersRoles] (
    [UserId] int DEFAULT 0 NOT NULL, 
    [RoleId] int DEFAULT 0 NOT NULL, 
    CONSTRAINT [PkSecUsersRoles_UserId_RoleId] PRIMARY KEY ([UserId],[RoleId]),
    CONSTRAINT [FkSecUsersRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [SecRoles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION, 
    CONSTRAINT [FkSecUsersRoles_UserId] FOREIGN KEY ([UserId]) REFERENCES [SecUsers] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE [CfgUserApplications] (
    [Id] int NOT NULL, 
    [UserId] int DEFAULT 0 NULL, 
    [ApplicationId] int DEFAULT 0 NULL, 
    CONSTRAINT [XPKCfgUserApplications] PRIMARY KEY ([Id]),
    CONSTRAINT [R_82] FOREIGN KEY ([UserId]) REFERENCES [SecUsers] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION, 
    CONSTRAINT [R_83] FOREIGN KEY ([ApplicationId]) REFERENCES [CatApplications] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE [SecGroups] (
    [Id] int DEFAULT 0 NOT NULL, 
    [Name] nvarchar(255) NOT NULL COLLATE NOCASE, 
    CONSTRAINT [PkSecGroups_Id] PRIMARY KEY ([Id])
);

CREATE TABLE [SecUsersGroups] (
    [UserId] int DEFAULT 0 NOT NULL, 
    [GroupId] int DEFAULT 0 NOT NULL, 
    CONSTRAINT [PkSecUsersGroups_UserId_GroupId] PRIMARY KEY ([UserId],[GroupId]), 
    CONSTRAINT [FkSecUsersGroups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [SecGroups] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION, 
    CONSTRAINT [FkSecUsersGroups_UserId] FOREIGN KEY ([UserId]) REFERENCES [SecUsers] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE [SecGroupsRoles] (
    [GroupId] int DEFAULT 0 NOT NULL, 
    [RoleId] int DEFAULT 0 NOT NULL, 
    CONSTRAINT [PkSecGroupsRoles_GroupId_RoleId] PRIMARY KEY ([GroupId],[RoleId]), 
    CONSTRAINT [FkSecGroupsRoles_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [SecGroups] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION, 
    CONSTRAINT [FkSecGroupsRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [SecRoles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
);

CREATE TABLE Log (
    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
    Timestamp TEXT, 
    Loglevel TEXT, 
    Callsite TEXT, 
    Message TEXT, 
    Exception TEXT
);


-- Applications Info
INSERT 
INTO CatApplications(Id,
                        Name)
VALUES(1,'TestApp1');

INSERT 
INTO CatApplications(Id,
                        Name)
VALUES(2,'TestApp2');



-- Users Info
INSERT 
INTO SecUsers(UserName,
                Password,
                IsFromActiveDirectory)
VALUES('admin',
        'admin',
        false);

INSERT 
INTO SecUsers(UserName,
                Password,
                IsFromActiveDirectory)
VALUES('user',
        'user',
        true);

INSERT 
INTO SecUsers(UserName,
                Password,
                IsFromActiveDirectory)
VALUES('developer',
        'developer',
        true);



-- Roles
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


-- Users To Application Cross Reference
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
INSERT 
INTO SecGroups(Id,
                Name)
VALUES(1,'Administrators');

INSERT 
INTO SecGroups(Id,
                Name)
VALUES(2,'Developers');


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
