using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AnkiSentenceCardBuilder.Models
{
    public class Anki2Context : DbContext
    {
        //android_metadata
        //cards
        //col
        //config
        //deck_config
        public DbSet<Deck> Decks { get; protected set; }

        public string DbPath { get; }

        public Anki2Context(string dbPath)
        {
            DbPath = dbPath;
        }

        //Override OnConfiguring to use Sqlite
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }

    }

    public class Deck
    {
        public long Id { get; protected set; }
        public string Name { get; protected set; }
        [Column("mtime_secs")]
        public int MtimeSecs { get; protected set; }
        public byte[] Common { get; protected set; }
        public byte[] Kind { get; protected set; }
    }
}
