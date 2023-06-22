USE [GraphifyDB]
GO

CREATE PROCEDURE Register 
	@email VARCHAR(255), @name VARCHAR(128), @passwordHash CHAR(88), @passwordSalt CHAR(88)
AS
BEGIN TRY
	DECLARE @valid INT;
	SELECT @valid = u.UserID
		FROM Users AS u
		WHERE u.Email = @email;

	IF (@valid IS NULL)
	BEGIN
		INSERT INTO Users(Email, [Name])
			VALUES(@email, @name);

		DECLARE @userID INT;
		SET @userID = SCOPE_IDENTITY();

		INSERT INTO UserCredentials(UserID, PasswordHash, PasswordSalt)
			VALUES(@userID, @passwordHash, @passwordSalt);
		
		SELECT CAST(1 AS bit) AS [Success];
	END
	ELSE
	BEGIN
		SELECT CAST(0 AS bit) AS [Success];
	END
END TRY
BEGIN CATCH
	INSERT INTO dbo.LogDBErrors(Username, ErrorNumber, ErrorState, ErrorSeverity, ErrorLine, ErrorProcedure, ErrorMessage, ErrorDatetime)
		VALUES (SUSER_SNAME(), ERROR_NUMBER(), ERROR_STATE(), ERROR_SEVERITY(), ERROR_LINE(), ERROR_PROCEDURE(), ERROR_MESSAGE(), GETDATE());
END CATCH
GO

CREATE PROCEDURE GetUserCredentialsByEmail @email VARCHAR(255)
AS
BEGIN
	SELECT u.UserID, u.Email, u.[Name], uc.PasswordHash, uc.PasswordSalt
		FROM Users AS u
		INNER JOIN UserCredentials AS uc ON uc.UserID = u.UserID
		WHERE u.Email = @email;
END
GO

CREATE PROCEDURE CreateWorkPage
	@ownerID INT, @identifier CHAR(36), @pageName VARCHAR(255), @path VARCHAR(255)
AS
BEGIN TRY
	INSERT INTO WorkPages(OwnerID, Identifier, PageName, FilePath)
		VALUES(@ownerID, @identifier, @pageName, @path);

	SELECT CAST(1 AS bit) AS [Success];
END TRY
BEGIN CATCH
	INSERT INTO dbo.LogDBErrors(Username, ErrorNumber, ErrorState, ErrorSeverity, ErrorLine, ErrorProcedure, ErrorMessage, ErrorDatetime)
		VALUES (SUSER_SNAME(), ERROR_NUMBER(), ERROR_STATE(), ERROR_SEVERITY(), ERROR_LINE(), ERROR_PROCEDURE(), ERROR_MESSAGE(), GETDATE());
END CATCH
GO

CREATE PROCEDURE GetWorkPages @ownerID INT
AS
BEGIN
	SELECT wp.PageID, wp.OwnerID, wp.PageName, wp.Identifier, wp.FilePath, wp.CreationDate, wp.UpdateDate
		FROM WorkPages AS wp
		WHERE wp.OwnerID = @ownerID AND wp.IsDeleted = 0
		ORDER BY wp.UpdateDate DESC;
END
GO

CREATE PROCEDURE GetWorkpage @identifier CHAR(36)
AS
BEGIN
	SELECT wp.PageID, wp.OwnerID, wp.PageName, wp.Identifier, wp.FilePath, wp.CreationDate, wp.UpdateDate
		FROM WorkPages AS wp
		WHERE wp.Identifier = @identifier AND wp.IsDeleted = 0;
END
GO

