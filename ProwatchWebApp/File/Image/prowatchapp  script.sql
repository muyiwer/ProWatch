USE [master]
GO
/****** Object:  Database [ProWatchApp]    Script Date: 02-Apr-18 10:57:33 AM ******/
CREATE DATABASE [ProWatchApp]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ProWatchApp', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\ProWatchApp.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ProWatchApp_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\ProWatchApp_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ProWatchApp] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ProWatchApp].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ProWatchApp] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ProWatchApp] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ProWatchApp] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ProWatchApp] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ProWatchApp] SET ARITHABORT OFF 
GO
ALTER DATABASE [ProWatchApp] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ProWatchApp] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ProWatchApp] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ProWatchApp] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ProWatchApp] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ProWatchApp] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ProWatchApp] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ProWatchApp] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ProWatchApp] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ProWatchApp] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ProWatchApp] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ProWatchApp] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ProWatchApp] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ProWatchApp] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ProWatchApp] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ProWatchApp] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ProWatchApp] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ProWatchApp] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ProWatchApp] SET  MULTI_USER 
GO
ALTER DATABASE [ProWatchApp] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ProWatchApp] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ProWatchApp] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ProWatchApp] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [ProWatchApp] SET DELAYED_DURABILITY = DISABLED 
GO
USE [ProWatchApp]
GO
/****** Object:  Table [dbo].[project]    Script Date: 02-Apr-18 10:57:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[project](
	[projectID] [int] IDENTITY(1,1) NOT NULL,
	[projectname] [varchar](255) NOT NULL,
	[projectDescription] [varchar](255) NULL,
	[projectMateril] [varchar](255) NOT NULL,
	[fileUrl] [varchar](255) NULL,
	[dateCreated] [datetime2](7) NULL,
	[dateModified] [datetime2](7) NULL,
	[userID] [int] NOT NULL,
	[projectStatusID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[projectID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[projectStatus]    Script Date: 02-Apr-18 10:57:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[projectStatus](
	[projectStatusID] [int] IDENTITY(1,1) NOT NULL,
	[projectStatusName] [varchar](255) NOT NULL,
	[dateModifier] [datetime2](7) NULL,
	[dateCreated] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[projectStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProUser]    Script Date: 02-Apr-18 10:57:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProUser](
	[userID] [int] IDENTITY(1,1) NOT NULL,
	[firstname] [varchar](255) NOT NULL,
	[lastname] [varchar](255) NULL,
	[email] [varchar](255) NOT NULL,
	[password] [varchar](50) NULL,
	[phoneNumber] [varchar](50) NULL,
	[dateCreated] [datetime2](7) NULL,
	[dateModified] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[userID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[report]    Script Date: 02-Apr-18 10:57:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[report](
	[reportID] [int] IDENTITY(1,1) NOT NULL,
	[taskName] [varchar](255) NULL,
	[reportDescription] [varchar](255) NULL,
	[createdBy] [varchar](255) NULL,
	[userID] [int] NOT NULL,
	[projectStatusID] [int] NULL,
	[taskID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[reportID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[task]    Script Date: 02-Apr-18 10:57:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[task](
	[taskID] [int] IDENTITY(1,1) NOT NULL,
	[task] [varchar](255) NOT NULL,
	[observedBy] [varchar](255) NULL,
	[deadlineDate] [varchar](255) NOT NULL,
	[dateCreated] [datetime2](7) NULL,
	[dateModified] [datetime2](7) NULL,
	[createdBy] [varchar](255) NULL,
	[userID] [int] NOT NULL,
	[projectStatusID] [int] NULL,
	[assignTo] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[taskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[project]  WITH CHECK ADD FOREIGN KEY([projectStatusID])
REFERENCES [dbo].[projectStatus] ([projectStatusID])
GO
ALTER TABLE [dbo].[project]  WITH CHECK ADD FOREIGN KEY([userID])
REFERENCES [dbo].[ProUser] ([userID])
GO
ALTER TABLE [dbo].[report]  WITH CHECK ADD FOREIGN KEY([projectStatusID])
REFERENCES [dbo].[projectStatus] ([projectStatusID])
GO
ALTER TABLE [dbo].[report]  WITH CHECK ADD FOREIGN KEY([taskID])
REFERENCES [dbo].[task] ([taskID])
GO
ALTER TABLE [dbo].[report]  WITH CHECK ADD FOREIGN KEY([userID])
REFERENCES [dbo].[ProUser] ([userID])
GO
ALTER TABLE [dbo].[task]  WITH CHECK ADD FOREIGN KEY([projectStatusID])
REFERENCES [dbo].[projectStatus] ([projectStatusID])
GO
ALTER TABLE [dbo].[task]  WITH CHECK ADD FOREIGN KEY([userID])
REFERENCES [dbo].[ProUser] ([userID])
GO
USE [master]
GO
ALTER DATABASE [ProWatchApp] SET  READ_WRITE 
GO
