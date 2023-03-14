USE [tcgct]
GO
/****** Object:  Table [dbo].[MTG_Card]    Script Date: 12/03/2023 15:49:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MTG_Card](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](200) NOT NULL,
	[mana_cost] [nvarchar](50) NULL,
	[text] [nvarchar](1000) NULL,
	[flavor] [nvarchar](1000) NULL,
	[artist] [nvarchar](100) NULL,
	[collector_number] [nvarchar](25) NOT NULL,
	[power] [nvarchar](10) NULL,
	[toughness] [nvarchar](10) NULL,
	[card_set_id] [bigint] NOT NULL,
	[scryfall_id] [nvarchar](36) NOT NULL,
	[converted_cost] [int] NULL,
	[image] [nvarchar](200) NULL,
	[image_flipped] [nvarchar](200) NULL,
	[oracle_id] [nvarchar](36) NOT NULL,
	[rarity_id] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MTG_CardType]    Script Date: 12/03/2023 15:49:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MTG_CardType](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](300) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MTG_Rarity]    Script Date: 12/03/2023 15:49:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MTG_Rarity](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MTG_Set]    Script Date: 12/03/2023 15:49:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MTG_Set](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[shorthand] [nvarchar](20) NOT NULL,
	[icon] [nvarchar](300) NULL,
	[search_uri] [nvarchar](500) NOT NULL,
	[set_type_id] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MTG_SetType]    Script Date: 12/03/2023 15:49:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MTG_SetType](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MTG_TypeLine]    Script Date: 12/03/2023 15:49:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MTG_TypeLine](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[card_id] [bigint] NOT NULL,
	[type_id] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MTG_Card]  WITH CHECK ADD  CONSTRAINT [collection_mtg_card_card_set_id_bb0fe297_fk_collection_mtg_set_id] FOREIGN KEY([card_set_id])
REFERENCES [dbo].[MTG_Set] ([id])
GO
ALTER TABLE [dbo].[MTG_Card] CHECK CONSTRAINT [collection_mtg_card_card_set_id_bb0fe297_fk_collection_mtg_set_id]
GO
ALTER TABLE [dbo].[MTG_Card]  WITH CHECK ADD  CONSTRAINT [collection_mtg_card_rarity_id_5835b005_fk_collection_mtg_rarity_id] FOREIGN KEY([rarity_id])
REFERENCES [dbo].[MTG_Rarity] ([id])
GO
ALTER TABLE [dbo].[MTG_Card] CHECK CONSTRAINT [collection_mtg_card_rarity_id_5835b005_fk_collection_mtg_rarity_id]
GO
ALTER TABLE [dbo].[MTG_Set]  WITH CHECK ADD  CONSTRAINT [collection_mtg_set_set_type_id_beaf3544_fk_collection_mtg_settype_id] FOREIGN KEY([set_type_id])
REFERENCES [dbo].[MTG_SetType] ([id])
GO
ALTER TABLE [dbo].[MTG_Set] CHECK CONSTRAINT [collection_mtg_set_set_type_id_beaf3544_fk_collection_mtg_settype_id]
GO
ALTER TABLE [dbo].[MTG_TypeLine]  WITH CHECK ADD  CONSTRAINT [collection_typeline_card_id_c1180624_fk_collection_mtg_card_id] FOREIGN KEY([card_id])
REFERENCES [dbo].[MTG_Card] ([id])
GO
ALTER TABLE [dbo].[MTG_TypeLine] CHECK CONSTRAINT [collection_typeline_card_id_c1180624_fk_collection_mtg_card_id]
GO
ALTER TABLE [dbo].[MTG_TypeLine]  WITH CHECK ADD  CONSTRAINT [collection_typeline_type_id_5e78fb45_fk_collection_mtg_cardtype_id] FOREIGN KEY([type_id])
REFERENCES [dbo].[MTG_CardType] ([id])
GO
ALTER TABLE [dbo].[MTG_TypeLine] CHECK CONSTRAINT [collection_typeline_type_id_5e78fb45_fk_collection_mtg_cardtype_id]
GO
