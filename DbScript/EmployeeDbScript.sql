USE [master]
GO
/****** Object:  Database [EmployeeDb]    Script Date: 28-04-2025 03:09:23 ******/
CREATE DATABASE [EmployeeDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'EmployeeDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\EmployeeDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'EmployeeDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\EmployeeDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [EmployeeDb] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EmployeeDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EmployeeDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EmployeeDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EmployeeDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EmployeeDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EmployeeDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [EmployeeDb] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [EmployeeDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EmployeeDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EmployeeDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EmployeeDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EmployeeDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EmployeeDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EmployeeDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EmployeeDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EmployeeDb] SET  ENABLE_BROKER 
GO
ALTER DATABASE [EmployeeDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EmployeeDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EmployeeDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EmployeeDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EmployeeDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EmployeeDb] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [EmployeeDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EmployeeDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [EmployeeDb] SET  MULTI_USER 
GO
ALTER DATABASE [EmployeeDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EmployeeDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EmployeeDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EmployeeDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EmployeeDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EmployeeDb] SET QUERY_STORE = OFF
GO
USE [EmployeeDb]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 28-04-2025 03:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 28-04-2025 03:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](100) NULL,
	[Email] [varchar](50) NULL,
	[Phone] [varchar](10) NULL,
	[Password] [nvarchar](20) NULL,
	[Department] [varchar](50) NULL,
	[Salary] [decimal](18, 2) NULL,
	[CreatedDate] [datetime] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: 28-04-2025 03:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Logs](
	[LogId] [bigint] IDENTITY(1,1) NOT NULL,
	[Level] [varchar](10) NULL,
	[CorelationId] [varchar](36) NULL,
	[UserId] [varchar](50) NULL,
	[RequestURL] [varchar](250) NULL,
	[AdditionalInfo] [varchar](max) NULL,
	[RequestType] [varchar](20) NULL,
	[IPAddress] [varchar](20) NULL,
	[FunctionName] [varchar](150) NULL,
	[StatusCode] [varchar](10) NULL,
	[Type] [varchar](5000) NULL,
	[Message] [varchar](5000) NULL,
	[StackTrace] [varchar](5000) NULL,
	[InnerException] [varchar](5000) NULL,
	[APITime] [int] NULL,
	[LoggedOnDate] [datetime] NULL,
	[CallSite] [varchar](200) NULL,
 CONSTRAINT [pk_logs] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SearchHistory]    Script Date: 28-04-2025 03:09:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SearchHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Department] [varchar](50) NULL,
	[Name] [varchar](100) NULL,
	[Salary] [decimal](18, 2) NULL,
	[FromDate] [varchar](50) NULL,
	[ToDate] [varchar](50) NULL,
	[PageNumber] [int] NULL,
	[PageSize] [int] NULL,
	[SortBy] [varchar](50) NULL,
	[SortOrder] [varchar](50) NULL,
	[UserId] [varchar](50) NULL,
	[SearchDate] [datetime] NULL,
	[ResultCount] [int] NULL,
 CONSTRAINT [PK_SearchHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250426100540_dbinit', N'9.0.4')
GO
INSERT [dbo].[Employees] ([Id], [Name], [Email], [Phone], [Password], [Department], [Salary], [CreatedDate], [IsActive]) VALUES (N'38ca9890-52ce-4644-88e4-155ea3eae42d', N'Admin', N'Admin@Yahoo.com', N'8767543423', N'Admin@786', N'Admin', CAST(48000.00 AS Decimal(18, 2)), CAST(N'2025-04-27T14:15:06.737' AS DateTime), 1)
INSERT [dbo].[Employees] ([Id], [Name], [Email], [Phone], [Password], [Department], [Salary], [CreatedDate], [IsActive]) VALUES (N'5e46beea-3463-48a9-aa74-6bdac8de0743', N'Rakesh', N'Rakesh@Yahoo.com', N'7678565434', N'Rakesh@051', N'Helpdesk', CAST(20000.00 AS Decimal(18, 2)), CAST(N'2025-04-27T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[Employees] ([Id], [Name], [Email], [Phone], [Password], [Department], [Salary], [CreatedDate], [IsActive]) VALUES (N'3379dd31-4ca2-451f-81d5-8e8e394ba9ac', N'Abhishek Joshi', N'AbhishekJoshi@Yahoo.com', N'8987898897', N'AbhishekJoshi@256', N'Technology', CAST(50000.00 AS Decimal(18, 2)), CAST(N'2025-04-25T20:13:47.620' AS DateTime), 1)
INSERT [dbo].[Employees] ([Id], [Name], [Email], [Phone], [Password], [Department], [Salary], [CreatedDate], [IsActive]) VALUES (N'296fcadb-3dcf-47de-8ef0-c9a8fca0f74c', N'Abhishek', N'Abhishek@Yahoo.com', N'8987898767', N'Abhishek@345', N'HR', CAST(75000.00 AS Decimal(18, 2)), CAST(N'2025-04-27T20:13:47.620' AS DateTime), 1)
INSERT [dbo].[Employees] ([Id], [Name], [Email], [Phone], [Password], [Department], [Salary], [CreatedDate], [IsActive]) VALUES (N'207c4306-8464-41d1-8817-ee17cfda8daa', N'Yogesh', N'Yogesh@Yahoo.com', N'7678787678', N'Yogesh@456', N'KYC', CAST(65000.00 AS Decimal(18, 2)), CAST(N'2025-04-26T20:12:49.463' AS DateTime), 1)
GO
SET IDENTITY_INSERT [dbo].[Logs] ON 

INSERT [dbo].[Logs] ([LogId], [Level], [CorelationId], [UserId], [RequestURL], [AdditionalInfo], [RequestType], [IPAddress], [FunctionName], [StatusCode], [Type], [Message], [StackTrace], [InnerException], [APITime], [LoggedOnDate], [CallSite]) VALUES (1, N'Trace', N'6cfa3fb1-e903-452f-8bc4-279632068044', N'', N'https://localhost:44337/api/Employee/SearchEmployee', N'{
  "department": "",
  "name": "Abhishek",
  "salary": 0,
  "fromDate": "",
  "toDate": "",
  "recordsPerRequest": 0,
  "sortBy": "",
  "sortOrder": "",
  "userId": ""
}', N'Request', N'::1', N'SearchEmployee', N'', N'', N'', N'', N'', 0, CAST(N'2025-04-28T03:07:39.267' AS DateTime), N'SearchApp.Core.CustomLogger.AddlogInfo')
INSERT [dbo].[Logs] ([LogId], [Level], [CorelationId], [UserId], [RequestURL], [AdditionalInfo], [RequestType], [IPAddress], [FunctionName], [StatusCode], [Type], [Message], [StackTrace], [InnerException], [APITime], [LoggedOnDate], [CallSite]) VALUES (2, N'Trace', N'6cfa3fb1-e903-452f-8bc4-279632068044', N'', N'https://localhost:44337/api/Employee/SearchEmployee', N'{"statusCode":200,"message":"Request successful.","isError":false,"responseException":null,"result":[{"id":"3379dd31-4ca2-451f-81d5-8e8e394ba9ac","name":"Abhishek Joshi","email":"AbhishekJoshi@Yahoo.com","phone":"8987898897","department":"Technology","salary":50000.00,"createdDate":"2025-04-25T20:13:47.62"},{"id":"296fcadb-3dcf-47de-8ef0-c9a8fca0f74c","name":"Abhishek","email":"Abhishek@Yahoo.com","phone":"8987898767","department":"HR","salary":75000.00,"createdDate":"2025-04-27T20:13:47.62"}]}', N'Response', N'::1', N'SearchEmployee', N'200', N'', N'', N'', N'', 5833, CAST(N'2025-04-28T03:07:42.233' AS DateTime), N'SearchApp.Core.CustomLogger.AddlogInfo')
SET IDENTITY_INSERT [dbo].[Logs] OFF
GO
SET IDENTITY_INSERT [dbo].[SearchHistory] ON 

INSERT [dbo].[SearchHistory] ([Id], [Department], [Name], [Salary], [FromDate], [ToDate], [PageNumber], [PageSize], [SortBy], [SortOrder], [UserId], [SearchDate], [ResultCount]) VALUES (1, N'', N'', CAST(0.00 AS Decimal(18, 2)), N'2025-04-26', N'2025-04-26', 0, 0, N'', N'', N'', CAST(N'2025-04-27T13:29:31.897' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[SearchHistory] OFF
GO
/****** Object:  StoredProcedure [dbo].[InsertLog]    Script Date: 28-04-2025 03:09:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[InsertLog] 
(
		    @Level varchar(10)
           ,@CorelationId varchar(36)          
           ,@UserId varchar(50)
           ,@RequestURL varchar(250)
           ,@AdditionalInfo varchar(max)
           ,@RequestType varchar(20)
           ,@IPAddress varchar(20)
           ,@FunctionName varchar(150)
           ,@StatusCode varchar(10)
           ,@Type varchar(5000)
           ,@Message varchar(5000)
           ,@StackTrace varchar(5000)
           ,@InnerException varchar(5000)
           ,@APITime int
		   ,@CallSite varchar(200)
)
as
begin

-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
   SET NOCOUNT ON;

  INSERT INTO [dbo].[Logs]
           ([Level]
           ,[CorelationId]          
           ,[UserId]
           ,[RequestURL]
           ,[AdditionalInfo]
           ,[RequestType]
           ,[IPAddress]
           ,[FunctionName]
           ,[StatusCode]
           ,[Type]
           ,[Message]
           ,[StackTrace]
           ,[InnerException]
           ,[APITime]
		   ,[LoggedOnDate]
		   ,[CallSite])
     VALUES
           (@Level
           ,@CorelationId
           ,@UserId
           ,@RequestURL
           ,@AdditionalInfo
           ,@RequestType
           ,@IPAddress
           ,@FunctionName
           ,@StatusCode
           ,@Type
           ,@Message
           ,@StackTrace
           ,@InnerException
           ,@APITime
		   ,GETDATE()
		   ,@CallSite)
end
GO
USE [master]
GO
ALTER DATABASE [EmployeeDb] SET  READ_WRITE 
GO
