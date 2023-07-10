CREATE TABLE [MTG].[Card] (
    [id]               BIGINT        IDENTITY (1, 1) NOT NULL,
    [name]             VARCHAR (MAX) NOT NULL,
    [mana_cost]        VARCHAR (MAX) NULL,
    [text]             VARCHAR (MAX) NULL,
    [flavor]           VARCHAR (MAX) NULL,
    [artist]           VARCHAR (MAX) NULL,
    [collector_number] NVARCHAR (25) NOT NULL,
    [power]            NVARCHAR (10) NULL,
    [toughness]        NVARCHAR (10) NULL,
    [card_set_id]      BIGINT        NOT NULL,
    [scryfall_id]      NVARCHAR (36) NOT NULL,
    [converted_cost]   INT           NULL,
    [image]            VARCHAR (MAX) NULL,
    [image_flipped]    VARCHAR (MAX) NULL,
    [oracle_id]        NVARCHAR (36) NOT NULL,
    [rarity_id]        BIGINT        NOT NULL,
    [multi_face]       BIT           NOT NULL
);
GO

ALTER TABLE [MTG].[Card]
    ADD CONSTRAINT [PK__Card__3213E83F0A5FCB22] PRIMARY KEY CLUSTERED ([id] ASC);
GO

ALTER TABLE [MTG].[Card]
    ADD CONSTRAINT [collection_mtg_card_card_set_id_bb0fe297_fk_collection_mtg_set_id] FOREIGN KEY ([card_set_id]) REFERENCES [MTG].[Set] ([id]);
GO

ALTER TABLE [MTG].[Card]
    ADD CONSTRAINT [collection_mtg_card_rarity_id_5835b005_fk_collection_mtg_rarity_id] FOREIGN KEY ([rarity_id]) REFERENCES [MTG].[Rarity] ([id]);
GO

