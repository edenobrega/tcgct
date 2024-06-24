using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcgct_services_framework.MTG.Models;

namespace tcgct_services_framework.MTG.Services
{
    public interface IMTGCardService
    {
        int CreateCard(Card card);
        int CreateCardFace(CardFace card);
        int CreateCardPart(CardPart cardPart);
        int CreateCardType(string name);
        IEnumerable<CardFace> GetAllCardFaces();
        IEnumerable<CardPart> GetAllCardParts();
        IEnumerable<Card> GetAllCards();
        IEnumerable<CardType> GetAllCardTypes();
        int CreateRarity(string name);
        void CreateTypeLine(int type_id, int card_id, int order);
        CardTypeLine GetCardTypeLine(int card_id);
        IEnumerable<Rarity> GetRarities();
        Task<IEnumerable<Card>> GetSetCardsAsync(int id);
    }
}
