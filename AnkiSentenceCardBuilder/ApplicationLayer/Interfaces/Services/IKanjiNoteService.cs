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
        List<Note> PullAllSubKanjiNotesFromNoteList(ref List<Note> noteList, List<Note> originalKanjiNotes);
    }
}
