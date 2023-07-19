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