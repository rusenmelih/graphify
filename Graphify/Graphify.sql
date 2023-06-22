CREATE DATABASE [GraphifyDB]
GO

USE [GraphifyDB]
GO

CREATE TABLE Users(
	[UserID]		INT				IDENTITY(1, 1),
	[Email]			VARCHAR(255)	NOT NULL,
	[Name]			VARCHAR(128)	NOT NULL,
	[ProfilUrl]		VARCHAR(255),
	[RefreshToken]	CHAR(88),
	[RefreshTokenExp]	DATETIME,
	[CreationDate]	DATETIME		NOT NULL	DEFAULT(GETDATE()),

	CONSTRAINT	PK_Users_UserID		PRIMARY KEY(UserID),
	CONSTRAINT	UN_Users_Email		UNIQUE(Email),
	INDEX	IX_Users_Email		NONCLUSTERED(Email ASC),
	INDEX	IX_Users_Name		NONCLUSTERED([Name] ASC),
	INDEX	IX_Users_CreationDate	NONCLUSTERED(CreationDate DESC),
);
GO

CREATE TABLE UserCredentials(
	[UserID]		INT				NOT NULL,
	[PasswordHash]	CHAR(88)		NOT NULL,
	[PasswordSalt]	CHAR(88)		NOT NULL,
	[UpdateDate]	DATETIME		NOT NULL	DEFAULT(GETDATE()),

	CONSTRAINT	PK_UserCredentials_UserID	PRIMARY KEY(UserID),
	CONSTRAINT	FK_UserCredentials_UserID	FOREIGN KEY(UserID)		REFERENCES	Users(UserID),
);
GO

CREATE TABLE LoginHistories(
	[HistoryID]		INT				IDENTITY(1, 1),
	[UserID]		INT				NOT NULL,
	[Status]		BIT				NOT NULL,
	[LogDate]		DATETIME		NOT NULL	DEFAULT(GETDATE()),
	[Ip]			VARCHAR(255),
	[Device]		VARCHAR(MAX),
	[Location]		VARCHAR(MAX),
	[From]			VARCHAR(64),
	
	CONSTRAINT	PK_LoginHistories_HistoryID		PRIMARY KEY(HistoryID),
	CONSTRAINT	FK_LoginHistories_UserID		FOREIGN KEY(UserID)		REFERENCES	Users(UserID),
	INDEX	IX_LoginHistories_UserID	NONCLUSTERED(UserID ASC),
	INDEX	IX_LoginHistories_Status	NONCLUSTERED([Status] ASC),
	INDEX	IX_LoginHistories_LogDate	NONCLUSTERED(LogDate DESC),
);
GO

CREATE TABLE WorkPages(
	[PageID]		INT				IDENTITY(1, 1),
	[OwnerID]		INT				NOT NULL,
	[Identifier]	CHAR(36)		NOT NULL,
	[PageName]		VARCHAR(255)	NOT NULL,
	[FilePath]		VARCHAR(255)	NOT NULL,
	[IsDeleted]		BIT				NOT NULL	DEFAULT(0),
	[DeletionDate]	DATETIME,
	[UpdateDate]	DATETIME		NOT NULL	DEFAULT(GETDATE()),
	[CreationDate]	DATETIME		NOT NULL	DEFAULT(GETDATE()),

	CONSTRAINT	PK_WorkPages_PageID		PRIMARY KEY(PageID),
	CONSTRAINT	FK_WorkPages_OwnerID	FOREIGN KEY(OwnerID)	REFERENCES	Users(UserID),
	CONSTRAINT	UN_WorkPages_Identifier	UNIQUE([Identifier]),
	INDEX	IX_WorkPages_OwnerID		NONCLUSTERED(OwnerID ASC),
	INDEX	IX_WorkPages_Identifier		NONCLUSTERED([Identifier] ASC),
	INDEX	IX_WorkPages_IsDeleted		NONCLUSTERED(IsDeleted DESC),
	INDEX	IX_WorkPages_CreationDate	NONCLUSTERED(CreationDate ASC),
	INDEX	IX_WorkPages_PageName		NONCLUSTERED(PageName ASC),
);
GO

CREATE TABLE WorkPageHistory(
	PageHistoryID		INT			IDENTITY(1, 1),
	PageID				INT			NOT NULL,
	CommandFilePath		VARCHAR(255)	NOT NULL,
	HistoryDate			DATETIME	NOT NULL	DEFAULT(GETDATE()),

	CONSTRAINT	PK_WorkPageHistory_PageHistoryID	PRIMARY KEY(PageHistoryID),
	CONSTRAINT	FK_WorkPageHistory_PageID		FOREIGN KEY(PageID)		REFERENCES	WorkPages(PageID),
	INDEX	IX_WorkPageHistory_PageID	NONCLUSTERED(PageID ASC),
	INDEX	IX_WorkPageHistory_HistoryDate	NONCLUSTERED(HistoryDate DESC),
);
GO

CREATE TABLE SharedPageRelations(
	SharedPageRelationID	INT			IDENTITY(1, 1),
	PageID					INT			NOT NULL,
	UserID					INT			NOT NULL,
	[Identifier]			CHAR(36)	NOT NULL,
	SharedDate				DATETIME	NOT NULL	DEFAULT(GETDATE()),
	ExpirationDate			DATETIME	NOT NULL,

	CONSTRAINT	PK_SharedPageRelations_SharedPageRelationID	PRIMARY KEY(SharedPageRelationID),
	CONSTRAINT	FK_SharedPageRelations_PageID	FOREIGN KEY(PageID)	REFERENCES	WorkPages(PageID),
	CONSTRAINT	FK_SharedPageRelations_UserID	FOREIGN KEY(UserID)	REFERENCES	Users(UserID),
	INDEX	IX_SharedPageRelations_PageID	NONCLUSTERED(PageID ASC),
	INDEX	IX_SharedPageRelations_UserID	NONCLUSTERED(UserID ASC),
	INDEX	IX_SharedPageRelations_Identifier	NONCLUSTERED([Identifier] ASC),
	INDEX	IX_SharedPageRelations_ExpirationDate	NONCLUSTERED(ExpirationDate DESC),
);
GO

CREATE TABLE UserWorkshipRelations(
	UserWorkshipRelationID	INT			IDENTITY(1, 1),
	UserID					INT			NOT NULL,
	PageID					INT			NOT NULL,
	ApprovedDate			DATETIME	NOT NULL	DEFAULT(GETDATE()),

	CONSTRAINT	PK_UserWorkshipRelations_UserWorkshipRelationID	PRIMARY KEY(UserWorkshipRelationID),
	CONSTRAINT	FK_UserWorkshipRelations_UserID	FOREIGN KEY(UserID)	REFERENCES	Users(UserID),
	CONSTRAINT	FK_UserWorkshipRelations_PageID	FOREIGN KEY(PageID)	REFERENCES	WorkPages(PageID),
	INDEX	IX_UserWorkshipRelations_UserID	NONCLUSTERED(UserID ASC),
	INDEX	IX_UserWorkshipRelations_PageID	NONCLUSTERED(PageID ASC),
	INDEX	IX_UserWorkshipRelations_ApprovedDate	NONCLUSTERED(ApprovedDate DESC),
);
GO

CREATE TABLE LogDBErrors(
	ErrorID				INT				IDENTITY(1, 1),
	UserName			NVARCHAR(160)	NOT NULL,
	ErrorNumber			INT				NOT NULL,
	ErrorState			INT				NOT NULL,
	ErrorSeverity		INT				NOT NULL,
	ErrorLine			INT				NOT NULL,
	ErrorProcedure		NVARCHAR(MAX)	NOT NULL,
	ErrorMessage		NVARCHAR(MAX)	NOT NULL,
	ErrorDateTime		DATETIME		NOT NULL,

	CONSTRAINT	PK_LogDBErrors_ErrorID	PRIMARY KEY(ErrorID),
	INDEX	IX_LogDBErrors_ErrorDateTime	NONCLUSTERED (ErrorDateTime DESC),
);