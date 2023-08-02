insert into MTG.[SetType] values ('Default Test Set Type')
insert into MTG.[Set] values ('Default Test Set', 'DTS', 'https://upload.wikimedia.org/wikipedia/commons/thumb/5/57/Circled_plus.svg/1200px-Circled_plus.svg.png', 'Test search uri', 1, '00001abcdedf')
insert into MTG.[CardType] values ('—'), ('One'), ('Two'), ('Three'), ('//')
insert into MTG.[Rarity] values ('Normal'), ('Uncommon')
insert into MTG.[Card] values 
('Card 1',          '5{B}{W}',      'This is the card text',          'This is flavour test', 'Artist',       '1', '4', '3',  1, 'card_1', 7, 'link to image here', 'link to flipped image here', 'oracle_id_1', 1, 0),
('Card 2',          '3{R}{R}',      'This is the SECOND card text',   null,                   'Artist Two',   '2', '5', '7',  1, 'card_2', 5, 'link to image here', null,                         'oracle_id_2', 2, 0),
('Card 3',          '1{G}',         'This is the third card text',    null,                   'Artist Two',   '3', '1', '1',  1, 'card_3', 2, 'link to image here', 'link to flipped image here', 'oracle_id_3', 1, 0),
('Card 4',          '{U}',          'This is the card text',          'This is flavour test', 'Artist',       '4', '2', '1',  1, 'card_4', 1, 'link to image here', 'link to flipped image here', 'oracle_id_4', 2, 0),
('Card 5',          '5{B}{W}',      'This is the card text',          'This is flavour test', 'Artist Three', '5', '4', '3',  1, 'card_5', 7, 'link to image here', 'link to flipped image here', 'oracle_id_1', 1, 0),
('Card 6',          '{B} // {R}',   null,                              null,                   'Artist',      '6',null, null, 1, 'card_4', 1, 'link to image here', 'link to flipped image here', 'oracle_id_4', 2, 1)
insert into MTG.[CardFace] values 
(6,'card_face', 'Face One', null, '{B}', null, 1, null, null, null, null, 1, 1),
(6,'card_face', 'Face Two', null, '{R}', null, 1, null, null, null, null, 2, 1)
insert into MTG.[CardPart] values
(1, 'related_card', 'token', 4),
(2, 'related_card', 'token', 3),
(2, 'related_card', 'token', 4)
insert into MTG.[TypeLine] VALUES
(1, 2, 1),
(1, 1, 2),
(1, 4, 3),
(3, 3, 1),
(4, 3, 1),
(2, 1, 1),
(2, 1, 2),
(5, 1, 1)

insert into MTG.[Collection](CardID, UserID, [Count]) values
(1,'0000000001',1),
(2,'0000000001',4)

insert into MTG.[PinnedSet] values
(1, '0000000001')
