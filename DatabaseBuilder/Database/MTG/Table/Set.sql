CREATE TABLE [MTG].[Set] (
    [id]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [name]        NVARCHAR (100) NOT NULL,
    [shorthand]   NVARCHAR (20)  NOT NULL,
    [icon]        NVARCHAR (300) NOT NULL,
    [search_uri]  NVARCHAR (500) NOT NULL,
    [set_type_id] BIGINT         NOT NULL,
    [scryfall_id] NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK__Set__3213E83FB3D858EA] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [collection_mtg_set_set_type_id_beaf3544_fk_collection_mtg_settype_id] FOREIGN KEY ([set_type_id]) REFERENCES [MTG].[SetType] ([id])
);

