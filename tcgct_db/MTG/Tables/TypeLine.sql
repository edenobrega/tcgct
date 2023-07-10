CREATE TABLE [MTG].[TypeLine] (
    [id]      BIGINT IDENTITY (1, 1) NOT NULL,
    [card_id] BIGINT NOT NULL,
    [type_id] BIGINT NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);
GO

ALTER TABLE [MTG].[TypeLine]
    ADD CONSTRAINT [collection_typeline_card_id_c1180624_fk_collection_mtg_card_id] FOREIGN KEY ([card_id]) REFERENCES [MTG].[Card] ([id]);
GO

ALTER TABLE [MTG].[TypeLine]
    ADD CONSTRAINT [collection_typeline_type_id_5e78fb45_fk_collection_mtg_cardtype_id] FOREIGN KEY ([type_id]) REFERENCES [MTG].[CardType] ([id]);
GO

