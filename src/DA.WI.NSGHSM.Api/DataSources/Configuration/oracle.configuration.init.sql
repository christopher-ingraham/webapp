/***************************************************************************************************************************
  
  PURPOSE:   Script for table creation in Oracle
  
  REVISIONS:
  Ver        Date        Author           Description
  ---------  ----------  ---------------  ----------------------------------------------------------------------------------
  1.0        20/12/17    Tosi Silvia      Extracted from Erwin for oracle 
  1.1        10/01/18    Tosi Silvia      Removed flag After login from table CatViews
  1.2        05/02/18    Tosi Silvia      Constraints are created in create table clause
  1.3        14/02/18    Tosi Silvia      Removed table UCSConversionCoefficients
										  Added columns IsBase Multiplier and Offset
										  in table UCSUnits
  1.4        05/03/18    Tosi Silvia      Changed script to make it compatible with SQLite 
										  Removed creation of UserId unique index for FK in UCSUsersEnvironments
										  if was duplicated due to primary key (the column is  both primary and foreign key)
  1.5        25/07/18    Tosi Silvia      Added in all operational tables ModifiedAt and ModifiedBy columns  
  1.6        02/08/18    Tosi Silvia      Removed unique index from CatApplicationNodes Name/ApplicationId 
                                          Column Name is nullable 
  1.7        04/07/19    Tosi Silvia      Changed default value for table primary keys from 0 to null 
										  Added new table CfgUserViewPersistence
										  Changed default values for UCSUnits.Decimals column from 0 to -1
										  
  ************************************************************************************************************************/
  
CREATE TABLE SecUsers
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	UserName             NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	FullName             NVARCHAR2(255) DEFAULT  NULL  NULL ,
	Description          NVARCHAR2(255) DEFAULT  NULL  NULL ,
	Password             NVARCHAR2(255) DEFAULT  NULL  NULL ,
	IsPasswordChangeAllowed NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_1234782253 CHECK (IsPasswordChangeAllowed BETWEEN 0 AND 1),
	PasswordChangeDate   DATE DEFAULT  NULL  NULL ,
	PasswordNeverExpires NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_229435122 CHECK (PasswordNeverExpires BETWEEN 0 AND 1),
	LastLoginDate        DATE DEFAULT  NULL  NULL ,
	LastLogoutDate       DATE DEFAULT  NULL  NULL ,
	IsFromActiveDirectory NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_283163576 CHECK (IsFromActiveDirectory BETWEEN 0 AND 1),
	IsSuperUser          NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_759971050 CHECK (IsSuperUser BETWEEN 0 AND 1),
	IsDeleted            NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_942206983 CHECK (IsDeleted BETWEEN 0 AND 1),
	IsDisabled           NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_668192046 CHECK (IsDisabled BETWEEN 0 AND 1),
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkSecUsers_Id PRIMARY KEY (Id),CONSTRAINT  SecUsersUserNameUN UNIQUE (UserName)
);

CREATE TABLE CatModules
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	Name                 NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	Fqn                  NVARCHAR2(2000) DEFAULT  NULL  NULL ,
	Path                 NVARCHAR2(2000) DEFAULT  NULL  NULL ,
	IsShellLayout        NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_2136564375 CHECK (IsShellLayout BETWEEN 0 AND 1),
	IsServiceModule      NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_1180641092 CHECK (IsServiceModule BETWEEN 0 AND 1),
	OnStartup            NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_426129905 CHECK (OnStartup BETWEEN 0 AND 1),
	StartupOrder         INTEGER DEFAULT  NULL  NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkCatModules_Id PRIMARY KEY (Id),CONSTRAINT  CatModulesNameUN UNIQUE (Name)
);

CREATE TABLE CatViews
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	Name                 NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	Fqn                  NVARCHAR2(2000) DEFAULT  NULL  NULL ,
	ModuleId             INTEGER DEFAULT  0  NOT NULL ,
	KeepViewFrozen       NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_1698538876 CHECK (KeepViewFrozen BETWEEN 0 AND 1),
	IsApplicationView    NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_1565139769 CHECK (IsApplicationView BETWEEN 0 AND 1),
	OnStartup            NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_319357699 CHECK (OnStartup BETWEEN 0 AND 1),
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkCatViews_Id PRIMARY KEY (Id),
CONSTRAINT FkCatViews_ModuleId FOREIGN KEY (ModuleId) REFERENCES CatModules (Id)
);

CREATE INDEX XIF1CatViews ON CatViews
(ModuleId   ASC);

CREATE TABLE CatApplications
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	Name                 NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	LabelResourceKey     NVARCHAR2(255) DEFAULT  NULL  NULL ,
	DescriptionResourceKey NVARCHAR2(255) DEFAULT  NULL  NULL ,
	AreViewsFrozenByDefault NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_1582578237 CHECK (AreViewsFrozenByDefault BETWEEN 0 AND 1),
	IsAuditLogEnabled    NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_1117857411 CHECK (IsAuditLogEnabled BETWEEN 0 AND 1),
	LayoutViewId         INTEGER DEFAULT  0  NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkCatApplications_Id PRIMARY KEY (Id),CONSTRAINT  CatApplicationsNameUN UNIQUE (Name),
CONSTRAINT R_77 FOREIGN KEY (LayoutViewId) REFERENCES CatViews (Id)
);

CREATE INDEX XIF3CatApplications ON CatApplications
(LayoutViewId   ASC);

CREATE TABLE SecStations
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	Name                 NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	DescriptionResourceKey NVARCHAR2(255) DEFAULT  NULL  NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  XPKSecStations PRIMARY KEY (Id)
);

CREATE TABLE CfgUserApplications
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	UserId               INTEGER DEFAULT  0  NULL ,
	ApplicationId        INTEGER DEFAULT  0  NULL ,
	StationId            INTEGER DEFAULT  0  NULL ,
	XMLData              XMLType NULL ,
CONSTRAINT  XPKCfgUserApplications PRIMARY KEY (Id),
CONSTRAINT R_82 FOREIGN KEY (UserId) REFERENCES SecUsers (Id),
CONSTRAINT R_83 FOREIGN KEY (ApplicationId) REFERENCES CatApplications (Id),
CONSTRAINT R_84 FOREIGN KEY (StationId) REFERENCES SecStations (Id)
);

CREATE INDEX XIF1CfgUserApplications ON CfgUserApplications
(UserId   ASC);

CREATE INDEX XIF2CfgUserApplications ON CfgUserApplications
(ApplicationId   ASC);

CREATE INDEX XIF3CfgUserApplications ON CfgUserApplications
(StationId   ASC);

CREATE TABLE CfgUserApplicationViews
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	UserApplicationId    INTEGER DEFAULT  0  NULL ,
	XMLData              XMLType NULL ,
	ViewId               INTEGER DEFAULT  0  NULL ,
CONSTRAINT  XPKCfgUserApplicationViews PRIMARY KEY (Id),
CONSTRAINT R_86 FOREIGN KEY (UserApplicationId) REFERENCES CfgUserApplications (Id),
CONSTRAINT R_87 FOREIGN KEY (ViewId) REFERENCES CatViews (Id)
);

CREATE INDEX XIF1CfgUserApplicationViews ON CfgUserApplicationViews
(UserApplicationId   ASC);

CREATE INDEX XIF2CfgUserApplicationViews ON CfgUserApplicationViews
(ViewId   ASC);

CREATE TABLE CatResources
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	ResourceKey          NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	Class                NVARCHAR2(255) DEFAULT  NULL  NULL ,
	IsString             NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_1313752621 CHECK (IsString BETWEEN 0 AND 1),
CONSTRAINT  PkCatResources_Id PRIMARY KEY (Id),CONSTRAINT  CatResourcesKeyCultureUN UNIQUE (ResourceKey)
);

CREATE TABLE CatResourceValues
(
	ResourceId           INTEGER DEFAULT  0  NULL ,
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	StringValue          NVARCHAR2(2000) DEFAULT  NULL  NULL ,
	ObjectValue          LONG RAW DEFAULT  NULL  NULL ,
	Culture              NVARCHAR2(255) DEFAULT  NULL  NULL ,
CONSTRAINT  XPKCatResourceValues PRIMARY KEY (Id),CONSTRAINT  XAK1CatResourceValues UNIQUE (ResourceId,Culture),
CONSTRAINT R_79 FOREIGN KEY (ResourceId) REFERENCES CatResources (Id)
);

CREATE INDEX XIF1CatResourceValues ON CatResourceValues
(ResourceId   ASC);

CREATE TABLE CatViewParameters
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	ViewId               INTEGER DEFAULT  0  NOT NULL ,
	Name                 NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	DescriptionResourceKey NVARCHAR2(255) DEFAULT  NULL  NULL ,
	IsRequired           NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_133091688 CHECK (IsRequired BETWEEN 0 AND 1),
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkCatParameters_Id PRIMARY KEY (Id),
CONSTRAINT FK_CatViewParameters_CatViews FOREIGN KEY (ViewId) REFERENCES CatViews (Id)
);

CREATE INDEX XIF1CatViewParameters ON CatViewParameters
(ViewId   ASC);

CREATE TABLE CatCollapseModeTypes
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	Name                 NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	LabelResourceKey     NVARCHAR2(255) DEFAULT  NULL  NULL ,
	DescriptionResourceKey NVARCHAR2(255) DEFAULT  NULL  NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkCatCollapseModeTypes_Id PRIMARY KEY (Id),CONSTRAINT  CatCollapseModeTypesNameUN UNIQUE (Name)
);

CREATE TABLE CatViewRegions
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	ViewId               INTEGER DEFAULT  0  NOT NULL ,
	CollapseModeTypeId   INTEGER DEFAULT  0  NOT NULL ,
	IsDefaultViewRegion  NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_1823955306 CHECK (IsDefaultViewRegion BETWEEN 0 AND 1),
	Name                 NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkCatRegions_Id PRIMARY KEY (Id),
CONSTRAINT FK_CatViewRegions_CatViews FOREIGN KEY (ViewId) REFERENCES CatViews (Id),
CONSTRAINT FkCatRegions_CollapseModeTypes FOREIGN KEY (CollapseModeTypeId) REFERENCES CatCollapseModeTypes (Id)
);

CREATE INDEX XIF1CatViewRegions ON CatViewRegions
(ViewId   ASC);

CREATE INDEX XIF2CatViewRegions ON CatViewRegions
(CollapseModeTypeId   ASC);

CREATE TABLE CatApplicationRegions
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	ApplicationId        INTEGER DEFAULT  0  NOT NULL ,
	RegionId             INTEGER DEFAULT  0  NOT NULL ,
	CollapseModeTypeId   INTEGER DEFAULT  0  NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PK_CatApplicationRegions PRIMARY KEY (Id),
CONSTRAINT FK_CatApplicationRegions_CatAp FOREIGN KEY (ApplicationId) REFERENCES CatApplications (Id),
CONSTRAINT FK_CatApplicationRegions_Regio FOREIGN KEY (RegionId) REFERENCES CatViewRegions (Id),
CONSTRAINT R_78 FOREIGN KEY (CollapseModeTypeId) REFERENCES CatCollapseModeTypes (Id)
);

CREATE INDEX XIF1CatApplicationRegions ON CatApplicationRegions
(ApplicationId   ASC);

CREATE INDEX XIF3CatApplicationRegions ON CatApplicationRegions
(RegionId   ASC);

CREATE INDEX XIF4CatApplicationRegions ON CatApplicationRegions
(CollapseModeTypeId   ASC);

CREATE TABLE CatApplicationRegionViews
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	ViewId               INTEGER DEFAULT  0  NULL ,
	ApplicationRegionId  INTEGER DEFAULT  0  NULL ,
	IsDefaultView        NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_1137557763 CHECK (IsDefaultView BETWEEN 0 AND 1),
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  XPKCatApplicationRegionViews PRIMARY KEY (Id),
CONSTRAINT R_60 FOREIGN KEY (ViewId) REFERENCES CatViews (Id),
CONSTRAINT R_62 FOREIGN KEY (ApplicationRegionId) REFERENCES CatApplicationRegions (Id)
);

CREATE INDEX XIF1CatApplicationRegionViews ON CatApplicationRegionViews
(ViewId   ASC);

CREATE INDEX XIF3CatApplicationRegionViews ON CatApplicationRegionViews
(ApplicationRegionId   ASC);

CREATE TABLE CatAppRegionViewParameters
(
	ViewParameterId      INTEGER DEFAULT  0  NOT NULL ,
	Value                NVARCHAR2(2000) DEFAULT  NULL  NULL ,
	ApplicationRegionViewId INTEGER DEFAULT  0  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PK_CatApplicationRegionParamet PRIMARY KEY (ApplicationRegionViewId,ViewParameterId),
CONSTRAINT FK_CatApplicationViewParamet FOREIGN KEY (ViewParameterId) REFERENCES CatViewParameters (Id),
CONSTRAINT R_63 FOREIGN KEY (ApplicationRegionViewId) REFERENCES CatApplicationRegionViews (Id)
);

CREATE INDEX XIF2CatAppRegionViewParameters ON CatAppRegionViewParameters
(ViewParameterId   ASC);

CREATE INDEX XIF3CatAppRegionViewParameters ON CatAppRegionViewParameters
(ApplicationRegionViewId   ASC);

CREATE TABLE SecGroups
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	Name                 NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	DescriptionResourceKey NVARCHAR2(255) DEFAULT  NULL  NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkSecGroups_Id PRIMARY KEY (Id)
);

CREATE TABLE SecRoles
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	Name                 NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	DescriptionResourceKey NVARCHAR2(255) DEFAULT  NULL  NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkSecRoles_Id PRIMARY KEY (Id)
);

CREATE TABLE SecGroupsRoles
(
	GroupId              INTEGER DEFAULT  0  NOT NULL ,
	RoleId               INTEGER DEFAULT  0  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkSecGroupsRoles_GroupId_RoleI PRIMARY KEY (GroupId,RoleId),
CONSTRAINT FkSecGroupsRoles_GroupId FOREIGN KEY (GroupId) REFERENCES SecGroups (Id),
CONSTRAINT FkSecGroupsRoles_RoleId FOREIGN KEY (RoleId) REFERENCES SecRoles (Id)
);

CREATE INDEX XIF1SecGroupsRoles ON SecGroupsRoles
(GroupId   ASC);

CREATE INDEX XIF2SecGroupsRoles ON SecGroupsRoles
(RoleId   ASC);

CREATE TABLE CatResourceLocations
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	ResourceId           INTEGER DEFAULT  0  NULL ,
	Module               NVARCHAR2(255) DEFAULT  NULL  NULL ,
	Class                NVARCHAR2(255) DEFAULT  NULL  NULL ,
	IsInXAML             NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_118299157 CHECK (IsInXAML BETWEEN 0 AND 1),
CONSTRAINT  XPKCatResourceLocations PRIMARY KEY (Id),
CONSTRAINT R_64 FOREIGN KEY (ResourceId) REFERENCES CatResources (Id)
);

CREATE INDEX XIF1CatResourceLocations ON CatResourceLocations
(ResourceId   ASC);

CREATE TABLE SecOtherInfo
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	Name                 NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	LabelResourceKey     NVARCHAR2(255) DEFAULT  NULL  NULL ,
	DescriptionResourceKey NVARCHAR2(255) DEFAULT  NULL  NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkSecOtherInfo_Id PRIMARY KEY (Id),CONSTRAINT  SecOtherInfoNameUN UNIQUE (Name)
);

CREATE TABLE SecUserOtherInfo
(
	UserId               INTEGER DEFAULT  0  NOT NULL ,
	Value                NVARCHAR2(2000) DEFAULT  NULL  NULL ,
	OtherInfoId          INTEGER DEFAULT  0  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkSecUsersProfile_Id PRIMARY KEY (OtherInfoId,UserId),
CONSTRAINT FkSecUsersProfile_UserId FOREIGN KEY (UserId) REFERENCES SecUsers (Id),
CONSTRAINT R_58 FOREIGN KEY (OtherInfoId) REFERENCES SecOtherInfo (Id)
);

CREATE INDEX XIF1SecUserOtherInfo ON SecUserOtherInfo
(UserId   ASC);

CREATE INDEX XIF2SecUserOtherInfo ON SecUserOtherInfo
(OtherInfoId   ASC);

CREATE TABLE CfgUserViewPersistence
(
	UserApplicationId    INTEGER DEFAULT  0  NULL ,
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	XMLData              XMLType NULL ,
	LogicalVisualTreePath NVARCHAR2(2000) NULL ,
	UIElementName        NVARCHAR2(255) NULL ,
CONSTRAINT  XPKCfgUserViewPersistence PRIMARY KEY (Id),
CONSTRAINT R_91 FOREIGN KEY (UserApplicationId) REFERENCES CfgUserApplications (Id)
);

CREATE INDEX XIF1CfgUserViewPersistence ON CfgUserViewPersistence
(UserApplicationId   ASC);

CREATE INDEX XIE1CfgUserViewPersistence ON CfgUserViewPersistence
(UserApplicationId   ASC,UIElementName   ASC);

CREATE TABLE CatApplicationNodes
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	ApplicationId        INTEGER DEFAULT  0  NOT NULL ,
	Sequence             INTEGER DEFAULT  0  NOT NULL ,
	ParentNodeId         INTEGER DEFAULT  NULL  NULL ,
	ImageResourceKey     NVARCHAR2(255) DEFAULT  NULL  NULL ,
	ViewId               INTEGER DEFAULT  NULL  NULL ,
	RegionId             INTEGER DEFAULT  NULL  NULL ,
	TargetService        NVARCHAR2(255) DEFAULT  NULL  NULL ,
	TargetDatabase       NVARCHAR2(255) DEFAULT  NULL  NULL ,
	ShortcutKey          INTEGER DEFAULT  0  NOT NULL ,
	ShortcutKeyModifiers INTEGER DEFAULT  0  NOT NULL ,
	Name                 NVARCHAR2(255) DEFAULT  NULL  NULL ,
	LabelResourceKey     NVARCHAR2(255) DEFAULT  NULL  NULL ,
	DescriptionResourceKey NVARCHAR2(255) DEFAULT  NULL  NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkCatNodes_Id PRIMARY KEY (Id),
CONSTRAINT FK_CatApplicationNodes_View FOREIGN KEY (ViewId) REFERENCES CatViews (Id),
CONSTRAINT FkCatNodes_ApplicationId FOREIGN KEY (ApplicationId) REFERENCES CatApplications (Id),
CONSTRAINT FkCatNodes_NodeId FOREIGN KEY (ParentNodeId) REFERENCES CatApplicationNodes (Id),
CONSTRAINT FK_CatApplicationNodes_Region FOREIGN KEY (RegionId) REFERENCES CatViewRegions (Id)
);

CREATE INDEX XIF2CatApplicationNodes ON CatApplicationNodes
(ViewId   ASC);

CREATE INDEX XIF3CatApplicationNodes ON CatApplicationNodes
(ApplicationId   ASC);

CREATE INDEX XIF4CatApplicationNodes ON CatApplicationNodes
(ParentNodeId   ASC);

CREATE INDEX XIF1CatApplicationNodes ON CatApplicationNodes
(RegionId   ASC);

CREATE TABLE CatApplicationNodeParameters
(
	ApplicationNodeId    INTEGER DEFAULT  0  NOT NULL ,
	ViewParameterId      INTEGER DEFAULT  0  NOT NULL ,
	Value                NVARCHAR2(2000) DEFAULT  NULL  NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkCatNodesParameters_NodeId_Pa PRIMARY KEY (ApplicationNodeId,ViewParameterId),
CONSTRAINT FkCatNodesParameters_NodeId FOREIGN KEY (ApplicationNodeId) REFERENCES CatApplicationNodes (Id),
CONSTRAINT FkCatNodesParameters_Parameter FOREIGN KEY (ViewParameterId) REFERENCES CatViewParameters (Id)
);

CREATE INDEX XIF1CatApplicationNodeParamete ON CatApplicationNodeParameters
(ApplicationNodeId   ASC);

CREATE INDEX XIF2CatApplicationNodeParamete ON CatApplicationNodeParameters
(ViewParameterId   ASC);

CREATE TABLE CatApplicationAutoLoadModules
(
	ModuleId             INTEGER DEFAULT  0  NOT NULL ,
	ApplicationId        INTEGER DEFAULT  0  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  XPKCatApplicationAutoLoadModul PRIMARY KEY (ApplicationId,ModuleId),
CONSTRAINT R_80 FOREIGN KEY (ModuleId) REFERENCES CatModules (Id),
CONSTRAINT R_81 FOREIGN KEY (ApplicationId) REFERENCES CatApplications (Id)
);

CREATE INDEX XIF1CatApplicationAutoLoadModu ON CatApplicationAutoLoadModules
(ModuleId   ASC);

CREATE INDEX XIF2CatApplicationAutoLoadModu ON CatApplicationAutoLoadModules
(ApplicationId   ASC);

CREATE TABLE CatApplicationAfterLoginViews
(
	ApplicationId        INTEGER DEFAULT  0  NOT NULL ,
	ViewId               INTEGER DEFAULT  0  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  XPKCatApplicationAfterLoginVie PRIMARY KEY (ApplicationId,ViewId),
CONSTRAINT R_89 FOREIGN KEY (ApplicationId) REFERENCES CatApplications (Id),
CONSTRAINT R_90 FOREIGN KEY (ViewId) REFERENCES CatViews (Id)
);

CREATE INDEX XIF1CatApplicationAfterLoginVi ON CatApplicationAfterLoginViews
(ApplicationId   ASC);

CREATE INDEX XIF2CatApplicationAfterLoginVi ON CatApplicationAfterLoginViews
(ViewId   ASC);

CREATE TABLE CatModuleDependencies
(
	ModuleId             INTEGER DEFAULT  0  NOT NULL ,
	DependencyModuleId   INTEGER DEFAULT  0  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkCatModulesDependencies_Modul PRIMARY KEY (ModuleId,DependencyModuleId),
CONSTRAINT FkCatModulesDependencies_Depen FOREIGN KEY (DependencyModuleId) REFERENCES CatModules (Id),
CONSTRAINT FkCatModulesDependencies_Modul FOREIGN KEY (ModuleId) REFERENCES CatModules (Id)
);

CREATE INDEX XIF1CatModuleDependencies ON CatModuleDependencies
(DependencyModuleId   ASC);

CREATE INDEX XIF2CatModuleDependencies ON CatModuleDependencies
(ModuleId   ASC);

CREATE TABLE SecApplicationEnvironments
(
	StationId            INTEGER DEFAULT  NULL  NULL ,
	RoleId               INTEGER DEFAULT  0  NOT NULL ,
	ApplicationId        INTEGER DEFAULT  0  NOT NULL ,
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  XPKSecApplicationEnvironments PRIMARY KEY (Id),
CONSTRAINT R_44 FOREIGN KEY (StationId) REFERENCES SecStations (Id),
CONSTRAINT R_46 FOREIGN KEY (RoleId) REFERENCES SecRoles (Id),
CONSTRAINT R_47 FOREIGN KEY (ApplicationId) REFERENCES CatApplications (Id)
);

CREATE INDEX XIF1SecApplicationEnvironments ON SecApplicationEnvironments
(StationId   ASC);

CREATE INDEX XIF2SecApplicationEnvironments ON SecApplicationEnvironments
(RoleId   ASC);

CREATE INDEX XIF3SecApplicationEnvironments ON SecApplicationEnvironments
(ApplicationId   ASC);

CREATE TABLE SecPermissionTags
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	LabelResourceKey     NVARCHAR2(255) DEFAULT  NULL  NULL ,
	DescriptionResourceKey NVARCHAR2(255) DEFAULT  NULL  NULL ,
	Name                 NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  XPKSecPermissionTags PRIMARY KEY (Id),CONSTRAINT  XAK1SecPermissionTags_UnName UNIQUE (Name)
);

CREATE TABLE SecAppEnvPermissionTags
(
	EnvironmentId        INTEGER DEFAULT  0  NOT NULL ,
	PermissionTagId      INTEGER DEFAULT  0  NOT NULL ,
	IsToBeAdded          NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_334729101 CHECK (IsToBeAdded BETWEEN 0 AND 1),
	IsToBeRemoved        NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_1126140144 CHECK (IsToBeRemoved BETWEEN 0 AND 1),
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  XPKSecAppEnvPermissionTags PRIMARY KEY (EnvironmentId,PermissionTagId),
CONSTRAINT R_69 FOREIGN KEY (EnvironmentId) REFERENCES SecApplicationEnvironments (Id),
CONSTRAINT R_70 FOREIGN KEY (PermissionTagId) REFERENCES SecPermissionTags (Id)
);

CREATE INDEX XIF1SecAppEnvPermissionTags ON SecAppEnvPermissionTags
(EnvironmentId   ASC);

CREATE INDEX XIF2SecAppEnvPermissionTags ON SecAppEnvPermissionTags
(PermissionTagId   ASC);

CREATE TABLE UCSEnvironments
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	Name                 NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	LabelResourceKey     NVARCHAR2(255) DEFAULT  NULL  NULL ,
	DescriptionResourceKey NVARCHAR2(255) DEFAULT  NULL  NULL ,
	IsEntryDefault       NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_17788654 CHECK (IsEntryDefault BETWEEN 0 AND 1),
	IsExitDefault        NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_718611904 CHECK (IsExitDefault BETWEEN 0 AND 1),
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  XPKUCSEnvironments PRIMARY KEY (Id)
);

CREATE TABLE UCSUsersEnvironments
(
	EnvironmentId        INTEGER DEFAULT  0  NULL ,
	UserId               INTEGER DEFAULT  0  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  XPKUCSUsersEnvironments PRIMARY KEY (UserId),
CONSTRAINT R_73 FOREIGN KEY (EnvironmentId) REFERENCES UCSEnvironments (Id),
CONSTRAINT R_74 FOREIGN KEY (UserId) REFERENCES SecUsers (Id)
);

CREATE INDEX XIF1UCSUsersEnvironments ON UCSUsersEnvironments
(EnvironmentId   ASC);

CREATE TABLE UCSQuantities
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	LabelResourceKey     NVARCHAR2(255) DEFAULT  NULL  NULL ,
	DescriptionResourceKey NVARCHAR2(255) DEFAULT  NULL  NULL ,
	Name                 NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  XPKUCSQuantities PRIMARY KEY (Id)
);

CREATE TABLE UCSUnits
(
	QuantityId           INTEGER DEFAULT  0  NOT NULL ,
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	Name                 NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	LabelResourceKey     NVARCHAR2(255) DEFAULT  NULL  NULL ,
	DescriptionResourceKey NVARCHAR2(255) DEFAULT  NULL  NULL ,
	Sequence             INTEGER DEFAULT  0  NOT NULL ,
	SymbolResourceKey    NVARCHAR2(255) DEFAULT  NULL  NULL ,
	Decimals             INTEGER DEFAULT  -1  NULL ,
	Multiplier           NUMBER(38,15) DEFAULT  0  NULL ,
	Offset               NUMBER(38,15) DEFAULT  0  NULL ,
	IsBase               NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_1673770080 CHECK (IsBase BETWEEN 0 AND 1),
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  XPKUCSUnits PRIMARY KEY (Id),
CONSTRAINT R_53 FOREIGN KEY (QuantityId) REFERENCES UCSQuantities (Id)
);

CREATE INDEX XIF1UCSUnits ON UCSUnits
(QuantityId   ASC);

CREATE TABLE UCSEnvironmentsUnits
(
	EnvironmentId        INTEGER DEFAULT  0  NOT NULL ,
	UnitId               INTEGER DEFAULT  0  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  XPKUCSEnvironmentsUnits PRIMARY KEY (EnvironmentId,UnitId),
CONSTRAINT R_56 FOREIGN KEY (EnvironmentId) REFERENCES UCSEnvironments (Id),
CONSTRAINT R_57 FOREIGN KEY (UnitId) REFERENCES UCSUnits (Id)
);

CREATE INDEX XIF1UCSEnvironmentsUnits ON UCSEnvironmentsUnits
(EnvironmentId   ASC);

CREATE INDEX XIF2UCSEnvironmentsUnits ON UCSEnvironmentsUnits
(UnitId   ASC);

CREATE TABLE AuditLogs
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	ApplicationName      NVARCHAR2(255) DEFAULT  NULL  NULL ,
	UserName             NVARCHAR2(255) DEFAULT  NULL  NULL ,
	StationName          NVARCHAR2(255) DEFAULT  NULL  NULL ,
	ViewName             NVARCHAR2(255) DEFAULT  NULL  NULL ,
	StartDate            DATE DEFAULT  NULL  NULL ,
	Info                 NVARCHAR2(2000) DEFAULT  NULL  NULL ,
	Type                 NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	PID                  INTEGER DEFAULT  0  NOT NULL ,
	StopDate             DATE DEFAULT  NULL  NULL ,
	ModuleName           NVARCHAR2(255) DEFAULT  NULL  NULL ,
CONSTRAINT  XPKAuditLogs PRIMARY KEY (Id)
);

CREATE TABLE CatViewCommands
(
	Id                   INTEGER DEFAULT  NULL  NOT NULL ,
	ViewId               INTEGER DEFAULT  0  NOT NULL ,
	Name                 NVARCHAR2(255) DEFAULT  NULL  NOT NULL ,
	Fqn                  NVARCHAR2(2000) DEFAULT  NULL  NULL ,
	LabelResourceKey     NVARCHAR2(255) DEFAULT  NULL  NULL ,
	DescriptionResourceKey NVARCHAR2(255) DEFAULT  NULL  NULL ,
	ShortcutKey          INTEGER DEFAULT  0  NOT NULL ,
	ShortcutKeyModifiers INTEGER DEFAULT  0  NOT NULL ,
	ImageResourceKey     NVARCHAR2(255) DEFAULT  NULL  NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkCatViewCommands_Id PRIMARY KEY (Id),
CONSTRAINT FkCatViewCommands_ViewId FOREIGN KEY (ViewId) REFERENCES CatViews (Id)
);

CREATE INDEX XIF1CatViewCommands ON CatViewCommands
(ViewId   ASC);

CREATE TABLE SecViewCommandPermissionTags
(
	PermissionTagId      INTEGER DEFAULT  0  NOT NULL ,
	ViewCommandId        INTEGER DEFAULT  0  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  XPKSecViewCommandPermissionTag PRIMARY KEY (PermissionTagId,ViewCommandId),
CONSTRAINT R_71 FOREIGN KEY (PermissionTagId) REFERENCES SecPermissionTags (Id),
CONSTRAINT R_72 FOREIGN KEY (ViewCommandId) REFERENCES CatViewCommands (Id)
);

CREATE INDEX XIF1SecViewCommandPermissionTa ON SecViewCommandPermissionTags
(PermissionTagId   ASC);

CREATE INDEX XIF2SecViewCommandPermissionTa ON SecViewCommandPermissionTags
(ViewCommandId   ASC);

CREATE TABLE SecUsersRoles
(
	UserId               INTEGER DEFAULT  0  NOT NULL ,
	RoleId               INTEGER DEFAULT  0  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkSecUsersRoles_UserId_RoleId PRIMARY KEY (UserId,RoleId),
CONSTRAINT FkSecUsersRoles_RoleId FOREIGN KEY (RoleId) REFERENCES SecRoles (Id),
CONSTRAINT FkSecUsersRoles_UserId FOREIGN KEY (UserId) REFERENCES SecUsers (Id)
);

CREATE INDEX XIF1SecUsersRoles ON SecUsersRoles
(RoleId   ASC);

CREATE INDEX XIF2SecUsersRoles ON SecUsersRoles
(UserId   ASC);

CREATE TABLE SecUsersGroups
(
	UserId               INTEGER DEFAULT  0  NOT NULL ,
	GroupId              INTEGER DEFAULT  0  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkSecUsersGroups_UserId_GroupI PRIMARY KEY (UserId,GroupId),
CONSTRAINT FkSecUsersGroups_GroupId FOREIGN KEY (GroupId) REFERENCES SecGroups (Id),
CONSTRAINT FkSecUsersGroups_UserId FOREIGN KEY (UserId) REFERENCES SecUsers (Id)
);

CREATE INDEX XIF1SecUsersGroups ON SecUsersGroups
(GroupId   ASC);

CREATE INDEX XIF2SecUsersGroups ON SecUsersGroups
(UserId   ASC);

CREATE TABLE SecRolePermissionTags
(
	RoleId               INTEGER DEFAULT  0  NOT NULL ,
	PermissionTagId      INTEGER DEFAULT  0  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  XPKSecRolePermissionTags PRIMARY KEY (RoleId,PermissionTagId),
CONSTRAINT R_65 FOREIGN KEY (RoleId) REFERENCES SecRoles (Id),
CONSTRAINT R_66 FOREIGN KEY (PermissionTagId) REFERENCES SecPermissionTags (Id)
);

CREATE INDEX XIF1SecRolePermissionTags ON SecRolePermissionTags
(RoleId   ASC);

CREATE INDEX XIF2SecRolePermissionTags ON SecRolePermissionTags
(PermissionTagId   ASC);

CREATE TABLE SecApplicationNodes
(
	NodeId               INTEGER DEFAULT  0  NOT NULL ,
	IsEnabled            NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_165887450 CHECK (IsEnabled BETWEEN 0 AND 1),
	ApplicationEnvironmentId INTEGER DEFAULT  0  NOT NULL ,
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  PkSecApplicationsRolesNodes_Ap PRIMARY KEY (ApplicationEnvironmentId,NodeId),
CONSTRAINT FkSecApplicationsRolesNodes_No FOREIGN KEY (NodeId) REFERENCES CatApplicationNodes (Id),
CONSTRAINT R_48 FOREIGN KEY (ApplicationEnvironmentId) REFERENCES SecApplicationEnvironments (Id)
);

CREATE INDEX XIF2SecApplicationNodes ON SecApplicationNodes
(NodeId   ASC);

CREATE INDEX XIF3SecApplicationNodes ON SecApplicationNodes
(ApplicationEnvironmentId   ASC);

CREATE TABLE SecAppNodePermissionTags
(
	PermissionTagId      INTEGER DEFAULT  0  NOT NULL ,
	NodeId               INTEGER DEFAULT  0  NOT NULL ,
	ApplicationEnvironmentId INTEGER DEFAULT  0  NOT NULL ,
	IsToBeAdded          NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_1946915947 CHECK (IsToBeAdded BETWEEN 0 AND 1),
	IsToBeRemoved        NUMBER(1) DEFAULT  0  NOT NULL  CONSTRAINT  Boolean_Check_1729564911 CHECK (IsToBeRemoved BETWEEN 0 AND 1),
	ModifiedBy           INTEGER DEFAULT  0  NULL ,
	ModifiedAt           DATE DEFAULT  NULL  NULL ,
CONSTRAINT  XPKSecAppNodePermissionTags PRIMARY KEY (PermissionTagId,ApplicationEnvironmentId,NodeId),
CONSTRAINT R_67 FOREIGN KEY (PermissionTagId) REFERENCES SecPermissionTags (Id),
CONSTRAINT R_68 FOREIGN KEY (ApplicationEnvironmentId, NodeId) REFERENCES SecApplicationNodes (ApplicationEnvironmentId, NodeId)
);

CREATE INDEX XIF1SecAppNodePermissionTags ON SecAppNodePermissionTags
(PermissionTagId   ASC);

CREATE INDEX XIF2SecAppNodePermissionTags ON SecAppNodePermissionTags
(ApplicationEnvironmentId   ASC,NodeId   ASC);

CREATE VIEW     SecApplicationsUsersRolesView AS
SELECT          SecApplicationEnvironments.Id            AS ApplicationEnvironmentId, 
                SecApplicationEnvironments.ApplicationId AS ApplicationId, 
                SecApplicationEnvironments.StationId     AS StationId, 
                SecStations.Name                         AS StationName,
                SecUsersRoles.UserId                     AS UserId, 
                SecApplicationEnvironments.RoleId        AS RoleId
FROM            SecApplicationEnvironments 
INNER      JOIN SecUsersRoles ON SecUsersRoles.RoleId = SecApplicationEnvironments.RoleId
LEFT OUTER JOIN SecStations   ON SecStations.Id       = SecApplicationEnvironments.StationId
UNION
SELECT          SecApplicationEnvironments.Id            AS ApplicationEnvironmentId, 
                SecApplicationEnvironments.ApplicationId AS ApplicationId, 
                SecApplicationEnvironments.StationId     AS StationId,
                SecStations.Name                         AS StationName,
                SecUsersGroups.UserId                    AS UserId, 
                SecApplicationEnvironments.RoleId        AS RoleId
FROM            SecApplicationEnvironments 
INNER      JOIN SecGroupsRoles ON SecGroupsRoles.RoleId  = SecApplicationEnvironments.RoleId 
INNER      JOIN SecUsersGroups ON SecUsersGroups.GroupId = SecGroupsRoles.GroupId
LEFT OUTER JOIN SecStations    ON SecStations.Id         = SecApplicationEnvironments.StationId;



INSERT INTO CatApplications(Id, Name, LayoutViewId)
VALUES(1,'TestApp1', NULL);


INSERT INTO CatApplications(Id, Name, LayoutViewId)
VALUES(2,'TestApp2', NULL);

-- Users Info

INSERT INTO SecUsers(Id, UserName, Password, IsFromActiveDirectory)
VALUES(1, 'admin', 'admin', 0);


INSERT INTO SecUsers(Id, UserName, Password, IsFromActiveDirectory)
VALUES(2, 'user', 'user', 1);


INSERT INTO SecUsers(Id, UserName, Password, IsFromActiveDirectory)
VALUES(3, 'developer', 'developer', 1);

-- Roles

INSERT INTO SecRoles(Id, Name)
VALUES(1,'Admin');


INSERT INTO SecRoles(Id, Name)
VALUES(2,'User');


INSERT INTO SecRoles(Id, Name)
VALUES(3,'Developer');

-- Users To Application Cross Reference

INSERT INTO CfgUserApplications(Id, UserId, ApplicationId, StationId)
VALUES(1,1,1, NULL);


INSERT INTO CfgUserApplications(Id, UserId, ApplicationId, StationId)
VALUES(2,2,2, NULL);

-- Users To Roles Cross Reference
 -- User=user, Role=User

INSERT INTO SecUsersRoles(UserId, RoleId)
VALUES(2,2);

-- User=developer, Role=User

INSERT INTO SecUsersRoles(UserId, RoleId)
VALUES(3,2);

-- Groups

INSERT INTO SecGroups(Id, Name)
VALUES(1,'Administrators');


INSERT INTO SecGroups(Id, Name)
VALUES(2,'Developers');

-- Users To Groups Cross Reference
 -- User=admin, Group=Administrators

INSERT INTO SecUsersGroups(UserId, GroupId)
VALUES(1,1);

-- User=developer, Group=Developers

INSERT INTO SecUsersGroups(UserId, GroupId)
VALUES(3,2);

-- Groups To Roles Cross Reference
 -- Group=Administrators, Role=Admin

INSERT INTO SecGroupsRoles(GroupId, RoleId)
VALUES(1,1);

-- Group=Administrators, Role=User

INSERT INTO SecGroupsRoles(GroupId, RoleId)
VALUES(1,2);

-- Group=Administrators, Role=Developer

INSERT INTO SecGroupsRoles(GroupId, RoleId)
VALUES(1,3);

-- Group=Developers, Role=Developer

INSERT INTO SecGroupsRoles(GroupId, RoleId)
VALUES(2,3);


