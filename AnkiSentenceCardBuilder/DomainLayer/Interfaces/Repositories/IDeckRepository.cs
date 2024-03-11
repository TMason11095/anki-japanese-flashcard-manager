using AnkiJapaneseFlashcardManager.DomainLayer.Entities;

namespace AnkiJapaneseFlashcardManager.DomainLayer.Interfaces.Repositories
{
	public interface IDeckRepository
	{
		IEnumerable<Deck> GetDecksByDescriptionContaining(string descriptionPart);
	}
}
