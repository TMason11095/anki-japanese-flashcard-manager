using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.ApplicationLayer.Helpers
{
	public static class NoteHelper
	{
		public static List<string> GetIdsFromTagList(IEnumerable<string> tagList, string tag)//Note?
		{
			return tagList.Where(t => t.StartsWith(tag))
						.Select(t => t.Substring(tag.Length))
						.ToList();
		}
	}
}
