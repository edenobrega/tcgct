CREATE TABLE [MTG].[CardFace] (
    [ID]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [CardID]        BIGINT        NOT NULL,
    [Object]        NCHAR (10)    NOT NULL,
    [Name]          VARCHAR (MAX) NOT NULL,
    [Image]         VARCHAR (MAX) NULL,
    [Mana_Cost]     VARCHAR (MAX) NULL,
    [Oracle_Text]   VARCHAR (MAX) NULL,
    [ConvertedCost] INT           NULL,
    [FlavourText]   VARCHAR (MAX) NULL,
    [Layout]        VARCHAR (MAX) NULL,
    [Loyalty]       VARCHAR (MAX) NULL,
    [OracleID]      VARCHAR (MAX) NULL,
    [Power]         VARCHAR (MAX) NULL,
    [Toughness]     VARCHAR (MAX) NULL,
    CONSTRAINT [PK_CardFace] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CardFace_Card] FOREIGN KEY ([CardID]) REFERENCES [MTG].[Card] ([id]),
    CONSTRAINT [FK_CardFace_CardFace] FOREIGN KEY ([ID]) REFERENCES [MTG].[CardFace] ([ID])
);

