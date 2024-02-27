using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.DataAccessLayer.Interfaces.Contexts
{
	public interface IAnki2Context
	{
		public DbSet<Card> Cards { get; }
		public DbSet<Deck> Decks { get; }
		public DbSet<Note> Notes { get; }
	}
}
