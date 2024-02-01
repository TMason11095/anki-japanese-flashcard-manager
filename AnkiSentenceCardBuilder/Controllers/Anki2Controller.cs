using AnkiSentenceCardBuilder.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiSentenceCardBuilder.Controllers
{
    public class Anki2Controller
    {
        private readonly Anki2Context _context;

        public Anki2Controller(Anki2Context context)
        {
            _context = context;
        }

        public Anki2Controller(string dbPath) : this(new Anki2Context(dbPath))
        {
        }

        public List<T> GetTable<T>() where T : class
        {
            return _context.Set<T>().ToList();
        }
    }
}
