CREATE TABLE [MTG].[CollectionLog](
	[Time] [datetime2](7) NOT NULL,
	[Change] [int] NOT NULL,
	[CardID] [int] NOT NULL,
	[User] [int] NOT NULL
) ON [PRIMARY]