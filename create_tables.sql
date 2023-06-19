USE [master]
GO
/****** Object:  Database [tcgct]    Script Date: 19/06/2023 10:06:36 PM ******/
CREATE DATABASE [tcgct]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'tcgct', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\tcgct.mdf' , SIZE = 139264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'tcgct_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\tcgct_log.ldf' , SIZE = 794624KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [tcgct] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [tcgct].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [tcgct] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [tcgct] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [tcgct] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [tcgct] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [tcgct] SET ARITHABORT OFF 
GO
ALTER DATABASE [tcgct] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [tcgct] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [tcgct] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [tcgct] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [tcgct] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [tcgct] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [tcgct] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [tcgct] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [tcgct] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [tcgct] SET  DISABLE_BROKER 
GO
ALTER DATABASE [tcgct] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [tcgct] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [tcgct] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [tcgct] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [tcgct] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [tcgct] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [tcgct] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [tcgct] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [tcgct] SET  MULTI_USER 
GO
ALTER DATABASE [tcgct] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [tcgct] SET DB_CHAINING OFF 
GO
ALTER DATABASE [tcgct] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [tcgct] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [tcgct] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [tcgct] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [tcgct] SET QUERY_STORE = ON
GO
ALTER DATABASE [tcgct] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [tcgct]
GO
/****** Object:  Schema [MTG]    Script Date: 19/06/2023 10:06:36 PM ******/
CREATE SCHEMA [MTG]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 19/06/2023 10:06:36 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [MTG].[Card]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [MTG].[Card](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [varchar](max) NOT NULL,
	[mana_cost] [varchar](max) NULL,
	[text] [varchar](max) NULL,
	[flavor] [varchar](max) NULL,
	[artist] [varchar](max) NULL,
	[collector_number] [nvarchar](25) NOT NULL,
	[power] [nvarchar](10) NULL,
	[toughness] [nvarchar](10) NULL,
	[card_set_id] [bigint] NOT NULL,
	[scryfall_id] [nvarchar](36) NOT NULL,
	[converted_cost] [int] NULL,
	[image] [varchar](max) NULL,
	[image_flipped] [varchar](max) NULL,
	[oracle_id] [nvarchar](36) NOT NULL,
	[rarity_id] [bigint] NOT NULL,
	[multi_face] [bit] NOT NULL,
 CONSTRAINT [PK__Card__3213E83F0A5FCB22] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [MTG].[CardFace]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [MTG].[CardFace](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CardID] [bigint] NOT NULL,
	[Object] [nchar](10) NOT NULL,
	[Name] [varchar](max) NOT NULL,
	[Image] [varchar](max) NULL,
	[Mana_Cost] [varchar](max) NULL,
	[Oracle_Text] [varchar](max) NULL,
	[ConvertedCost] [int] NULL,
	[FlavourText] [varchar](max) NULL,
	[Layout] [varchar](max) NULL,
	[Loyalty] [varchar](max) NULL,
	[OracleID] [varchar](max) NULL,
	[Power] [varchar](max) NULL,
	[Toughness] [varchar](max) NULL,
 CONSTRAINT [PK_CardFace] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [MTG].[CardPart]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [MTG].[CardPart](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CardID] [bigint] NULL,
	[Object] [varchar](25) NULL,
	[Component] [varchar](100) NULL,
	[RelatedCardID] [bigint] NULL,
 CONSTRAINT [PK_MTG.CardPart] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [MTG].[CardType]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [MTG].[CardType](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](300) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [MTG].[Collection]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [MTG].[Collection](
	[CardID] [varchar](max) NOT NULL,
	[UserID] [varchar](max) NOT NULL,
	[Count] [nchar](10) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [MTG].[Rarity]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [MTG].[Rarity](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [MTG].[Set]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [MTG].[Set](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[shorthand] [nvarchar](20) NOT NULL,
	[icon] [nvarchar](300) NOT NULL,
	[search_uri] [nvarchar](500) NOT NULL,
	[set_type_id] [bigint] NOT NULL,
	[scryfall_id] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK__Set__3213E83FB3D858EA] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [MTG].[SetType]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [MTG].[SetType](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [MTG].[TypeLine]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [MTG].[TypeLine](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[card_id] [bigint] NOT NULL,
	[type_id] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 19/06/2023 10:06:36 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 19/06/2023 10:06:36 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 19/06/2023 10:06:36 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 19/06/2023 10:06:36 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 19/06/2023 10:06:36 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 19/06/2023 10:06:36 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 19/06/2023 10:06:36 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [MTG].[Card]  WITH CHECK ADD  CONSTRAINT [collection_mtg_card_card_set_id_bb0fe297_fk_collection_mtg_set_id] FOREIGN KEY([card_set_id])
REFERENCES [MTG].[Set] ([id])
GO
ALTER TABLE [MTG].[Card] CHECK CONSTRAINT [collection_mtg_card_card_set_id_bb0fe297_fk_collection_mtg_set_id]
GO
ALTER TABLE [MTG].[Card]  WITH CHECK ADD  CONSTRAINT [collection_mtg_card_rarity_id_5835b005_fk_collection_mtg_rarity_id] FOREIGN KEY([rarity_id])
REFERENCES [MTG].[Rarity] ([id])
GO
ALTER TABLE [MTG].[Card] CHECK CONSTRAINT [collection_mtg_card_rarity_id_5835b005_fk_collection_mtg_rarity_id]
GO
ALTER TABLE [MTG].[CardFace]  WITH CHECK ADD  CONSTRAINT [FK_CardFace_Card] FOREIGN KEY([CardID])
REFERENCES [MTG].[Card] ([id])
GO
ALTER TABLE [MTG].[CardFace] CHECK CONSTRAINT [FK_CardFace_Card]
GO
ALTER TABLE [MTG].[CardFace]  WITH CHECK ADD  CONSTRAINT [FK_CardFace_CardFace] FOREIGN KEY([ID])
REFERENCES [MTG].[CardFace] ([ID])
GO
ALTER TABLE [MTG].[CardFace] CHECK CONSTRAINT [FK_CardFace_CardFace]
GO
ALTER TABLE [MTG].[Set]  WITH CHECK ADD  CONSTRAINT [collection_mtg_set_set_type_id_beaf3544_fk_collection_mtg_settype_id] FOREIGN KEY([set_type_id])
REFERENCES [MTG].[SetType] ([id])
GO
ALTER TABLE [MTG].[Set] CHECK CONSTRAINT [collection_mtg_set_set_type_id_beaf3544_fk_collection_mtg_settype_id]
GO
ALTER TABLE [MTG].[TypeLine]  WITH CHECK ADD  CONSTRAINT [collection_typeline_card_id_c1180624_fk_collection_mtg_card_id] FOREIGN KEY([card_id])
REFERENCES [MTG].[Card] ([id])
GO
ALTER TABLE [MTG].[TypeLine] CHECK CONSTRAINT [collection_typeline_card_id_c1180624_fk_collection_mtg_card_id]
GO
ALTER TABLE [MTG].[TypeLine]  WITH CHECK ADD  CONSTRAINT [collection_typeline_type_id_5e78fb45_fk_collection_mtg_cardtype_id] FOREIGN KEY([type_id])
REFERENCES [MTG].[CardType] ([id])
GO
ALTER TABLE [MTG].[TypeLine] CHECK CONSTRAINT [collection_typeline_type_id_5e78fb45_fk_collection_mtg_cardtype_id]
GO
/****** Object:  StoredProcedure [MTG].[TableCounts]    Script Date: 19/06/2023 10:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [MTG].[TableCounts]
AS
BEGIN
select 'Cards', count(*)
from MTG.[Card]

union all

select 'Card Faces', count(*)
from MTG.CardFace

union all

select 'Card Part', count(*)
from MTG.CardPart

union all

select 'Card Type', count(*)
from MTG.CardType

union all

select 'Rarity', count(*)
from MTG.Rarity

union all

select 'Set', count(*)
from MTG.[Set]

union all

select 'Set Type', count(*)
from MTG.SetType

union all

select 'TypeLine', count(*)
from MTG.TypeLine

union all

select 'Collected Cards', count(*)
from MTG.Collection
END
GO
USE [master]
GO
ALTER DATABASE [tcgct] SET  READ_WRITE 
GO
