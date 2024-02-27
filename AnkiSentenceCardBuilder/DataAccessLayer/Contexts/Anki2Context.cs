using AnkiJapaneseFlashcardManager.DataAccessLayer.Interfaces.Contexts;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts
{
    public class Anki2Context : DbContext, IAnki2Context
	{
        //android_metadata
        public DbSet<Card> Cards { get; protected set; }
        //col
        //config
        //deck_config
        public DbSet<Deck> Decks { get; protected set; }
        //fields
        //graves
        public DbSet<Note> Notes { get; protected set; }

        private string _dbPath { get; }

        public Anki2Context(string dbPath)
        {
            _dbPath = dbPath;
        }

        //Override OnConfiguring to use Sqlite
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={_dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Setup relationship between Deck and Note (one-to-many)
            //Intermediate table, Card, can contain multiple rows of the same DeckId and NoteId combiniations, which causes duplicate Notes, which we don't want


            //Setup foreign keys for Card
            modelBuilder.Entity<Card>()
                .HasOne(c => c.Note)
                .WithMany(n => n.Cards)
                .HasForeignKey(c => c.NoteId);

            modelBuilder.Entity<Card>()
                .HasOne(c => c.Deck)
                .WithMany(d => d.Cards)
                .HasForeignKey(c => c.DeckId);
        }

    }
}
