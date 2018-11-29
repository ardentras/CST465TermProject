SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Note](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[Text] [varchar](MAX),
	[UserID] [int],
	[Timestamp] [datetime] default(getdate()),
 CONSTRAINT [PK__Note] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Passwords](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[Username] [varchar](MAX),
	[PasswordValue] [varchar](MAX),
	[UserID] [int],
	[Timestamp] [datetime] default(getdate()),
 CONSTRAINT [PK__Passwords] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Website](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[Username] [varchar](MAX) NOT NULL,
	[URL] [varchar](MAX),
	[PasswordValue] [varchar](MAX),
	[UserID] [int],
	[Timestamp] [datetime] default(getdate()),
 CONSTRAINT [PK_Website] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE PROCEDURE Note_GetList
AS
SELECT * FROM Note;
GO
CREATE PROCEDURE Note_Insert
(
	@Name varchar(150),
	@Text varchar(MAX),
	@UserID int
)
AS
INSERT INTO Note([Name], [Text], [UserID]) 
VALUES (@Name, @Text, @UserID);
GO
CREATE PROCEDURE Note_Delete
(
	@ID int
)
AS
DELETE FROM Note WHERE ID = @ID;
GO
CREATE PROCEDURE Note_Update
(
	@ID int,
	@Name varchar(150),
	@Text varchar(MAX)
)
AS
UPDATE Note SET [Name]=@Name, [Text]=@Text WHERE [ID]=@ID;
GO
CREATE PROCEDURE Password_GetList
AS
SELECT * FROM Passwords;
GO
CREATE PROCEDURE Password_Insert
(
	@Name varchar(150),
	@Username varchar(MAX),
	@PasswordValue varchar(MAX),
	@UserID int
)
AS
INSERT INTO Passwords([Name], [Username], [PasswordValue], [UserID]) 
VALUES (@Name, @Username, @PasswordValue, @UserID);
GO
CREATE PROCEDURE Password_Delete
(
	@ID int
)
AS
DELETE FROM Passwords WHERE ID = @ID;
GO
CREATE PROCEDURE Password_Update
(
	@ID int,
	@Name varchar(150),
	@Username varchar(MAX),
	@PasswordValue varchar(MAX)
)
AS
UPDATE Passwords SET [Name]=@Name, [Username]=@Username, [PasswordValue]=@PasswordValue WHERE [ID]=@ID;
GO
CREATE PROCEDURE Website_GetList
AS
SELECT * FROM Website;
GO
CREATE PROCEDURE Website_Insert
(
	@Name varchar(150),
	@Username varchar(MAX),
	@URL varchar(MAX),
	@PasswordValue varchar(MAX),
	@UserID int
)
AS
INSERT INTO Website([Name], [Username], [URL], [PasswordValue], [UserID]) 
VALUES (@Name, @Username, @URL, @PasswordValue, @UserID);
GO
CREATE PROCEDURE Website_Delete
(
	@ID int
)
AS
DELETE FROM Website WHERE ID = @ID;
GO
CREATE PROCEDURE Website_Update
(
	@ID int,
	@Name varchar(150),
	@Username varchar(MAX),
	@URL varchar(MAX),
	@PasswordValue varchar(MAX)
)
AS
UPDATE Website SET [Name]=@Name, [URL]=@URL, [Username]=@Username, [PasswordValue]=@PasswordValue WHERE [ID]=@ID;
GO