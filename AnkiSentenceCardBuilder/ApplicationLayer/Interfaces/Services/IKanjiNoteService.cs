using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.ApplicationLayer.Interfaces.Services
{
    public interface IKanjiNoteService
    {
        IEnumerable<Note> GetKanjiNotes(IEnumerable<Note> deckNotes);
        IEnumerable<string> GetSubKanjiIds(IEnumerable<Note> kanjiNotes);
        List<Note> PullAllSubKanjiNotesFromNoteList(ref List<Note> noteList, List<Note> originalKanjiNotes);
        IEnumerable<Note> GetTaggedNotes(IEnumerable<Note> deckNotes, string noteTagName);
        IEnumerable<Note> GetNotesByKanjiIds(IEnumerable<Note> kanjiNotes, IEnumerable<string> kanjiIds);
    }
}
