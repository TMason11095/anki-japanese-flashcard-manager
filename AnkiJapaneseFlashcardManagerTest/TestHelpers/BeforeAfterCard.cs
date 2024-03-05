using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.TestHelpers
{
	public class BeforeAfterCard
	{
		public Card BeforeCard { get; private set; }
		public Card AfterCard { get; private set; }

		public BeforeAfterCard(Card beforeCard, Card afterCard)
		{
			BeforeCard = beforeCard;
			AfterCard = afterCard;
		}

		public static IEnumerable<BeforeAfterCard> GetChangedCards(IEnumerable<Card> beforeCards, IEnumerable<Card> afterCards)
		{
			//Return changed cards
			return beforeCards
				.Join(afterCards,
					beforeCard => beforeCard.Id,
					afterCard => afterCard.Id,
					(beforeCard, afterCard) => new BeforeAfterCard(beforeCard, afterCard))//Join the two as a pair
				.Where(pair => !pair.BeforeCard.Equals(pair.AfterCard));//Filter out unchanged
		}
	}
}
