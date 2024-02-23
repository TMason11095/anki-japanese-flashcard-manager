using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.DomainLayer.Entities
{
	public class Deck
	{
		public long Id { get; protected set; }
		public string Name { get; protected set; }
		[Column("mtime_secs")]
		public int MtimeSecs { get; protected set; }
		public byte[] Common { get; protected set; }
		public byte[] Kind { get; protected set; }

		//Navigation Properties
		public virtual ICollection<Card> Cards { get; protected set; }
	}
}
