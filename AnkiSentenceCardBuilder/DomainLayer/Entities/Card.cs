using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.DomainLayer.Entities
{
	public class Card
	{
		public long Id { get; protected set; }
		[Column("nid")]
		public long NoteId { get; protected set; }
		[Column("did")]
		public long DeckId { get; set; }
		[Column("ivl")]
		public int Interval { get; protected set; }

		//Navigation Properties
		public virtual Deck Deck { get; protected set; }
		public virtual Note Note { get; protected set; }

		//Override Equals
		public override bool Equals(object obj)
		{
			//Fail if not Card
			if (obj is null || !(obj is Card otherCard))
			{
				return false;
			}

			return Id == otherCard.Id
				&& NoteId == otherCard.NoteId
				&& DeckId == otherCard.DeckId
				&& Interval == otherCard.Interval;
		}
		//Override GetHashCode
		public override int GetHashCode()
		{
			return HashCode.Combine(Id, NoteId, DeckId, Interval);
		}
	}
}
